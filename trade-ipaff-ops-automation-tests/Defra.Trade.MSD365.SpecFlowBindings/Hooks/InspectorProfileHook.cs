namespace Defra.Trade.Plants.SpecFlowBindings.Hooks;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.Model;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using Reqnroll;

/// <summary>
/// Hook for deactivating existing inspector profiles before a test, and activating them after a test.
/// This is important as some test environments may have had manual profiles set up and we don't want to delete them.
/// However, there are rules in place to ensure an inspector can only have 1 active profile at a time. Without this, some tests will fail.
/// </summary>
[Binding]
[Scope(Tag = "UsesInspectorProfiles")]
public class InspectorProfileHook : PowerAppsStepDefiner
{
    private readonly SessionContext ctx;
    private List<trd_inspectorprofile> deactivatedProfiles = new List<trd_inspectorprofile>();
    private bool profilesProcessed = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="InspectorProfileHook"/> class.
    /// </summary>
    /// <param name="ctx">The context.</param>
    public InspectorProfileHook(SessionContext ctx)
    {
        this.ctx = ctx;
    }

    /// <summary>
    /// Deactivates any existing profiles for the executing user. Runs after step as we need to have logged into Dynamics.
    /// </summary>
    [AfterStep]
    public void DeactivateExistingProfilesAtTestStart()
    {
        if (!this.profilesProcessed)
        {
            this.DeactivateExistingProfiles();
        }
    }

    /// <summary>
    /// Deactivates all profiles including test profiles.
    /// </summary>
    [When(@"I deactivate any inspector profiles owned by me")]
    public void IDeactivateAnyInspectorProfilesOwnedByMe()
    {
        this.DeactivateExistingProfiles(true);
    }

    /// <summary>
    /// Reactivates any existing profiles for the executing user.
    /// </summary>
    [AfterScenario]
    public void ReactivateExistingProfiles()
    {
        if (this.deactivatedProfiles.Any())
        {
            this.SetInspectorProfileState(this.deactivatedProfiles, trd_inspectorprofileState.Active, trd_inspectorprofile_statuscode.Active);
        }
    }

    /// <summary>
    /// Deletes any test profiles at the end of the test run.
    /// </summary>
    [AfterFeature]
    public static void RemoveAnyTestProfiles()
    {
        using (var serviceClient = SessionContext.GetServiceClient())
        {
            using (var context = new PlantsContext(serviceClient))
            {
                var profilesToDelete = context.trd_inspectorprofileSet
                    .Where(p => p.trd_name.StartsWith("INSPECTORACCTEST"))
                    .Select(p => new trd_inspectorprofile
                    {
                        Id = p.Id,
                    })
                    .ToList();

                var request = new ExecuteMultipleRequest()
                {
                    Requests = new OrganizationRequestCollection(),
                    Settings = new ExecuteMultipleSettings()
                    {
                        ContinueOnError = false,
                        ReturnResponses = true,
                    },
                };

                profilesToDelete.ForEach(profile =>
                {
                    request.Requests.Add(new DeleteRequest { Target = new EntityReference(trd_inspectorprofile.EntityLogicalName, profile.Id) });
                });

                var response = context.Execute(request) as ExecuteMultipleResponse;

                if (response.IsFaulted)
                {
                    var faults = string.Join(",", response.Responses.Where(r => r.Fault != null).Select(r => r.Fault));
                    throw new Exception($"Failed to delete the test inspector profile record(s) with the fault(s): {faults}");
                }
            }
        }
    }

    private void DeactivateExistingProfiles(bool deactivateTestProfiles = false)
    {
        using (var serviceClient = SessionContext.GetServiceClient())
        {
            using (var context = new PlantsContext(serviceClient))
            {
                var request = new ExecuteMultipleRequest()
                {
                    Requests = new OrganizationRequestCollection(),
                    Settings = new ExecuteMultipleSettings()
                    {
                        ContinueOnError = false,
                        ReturnResponses = true,
                    },
                };

                var profilesToDeactivate = context.trd_inspectorprofileSet
                    .Where(p => p.statecode == trd_inspectorprofileState.Active && p.statuscode == trd_inspectorprofile_statuscode.Active && p.OwnerId.Id == this.ctx.UserId)
                    .Select(p => new trd_inspectorprofile
                    {
                        Id = p.Id,
                        trd_name = p.trd_name != null ? p.trd_name : string.Empty,
                    })
                    .ToList();

                if (profilesToDeactivate.Any())
                {
                    this.deactivatedProfiles.AddRange(this.SetInspectorProfileState(profilesToDeactivate, trd_inspectorprofileState.Inactive, trd_inspectorprofile_statuscode.Inactive, deactivateTestProfiles));
                }

                this.profilesProcessed = true;
            }
        }
    }

    private List<trd_inspectorprofile> SetInspectorProfileState(List<trd_inspectorprofile> profiles, trd_inspectorprofileState state, trd_inspectorprofile_statuscode status, bool deactivateTestProfiles = false)
    {
        var serviceClient = SessionContext.GetServiceClient();
        var updatedProfiles = new List<trd_inspectorprofile>();

        using (var context = new PlantsContext(serviceClient))
        {
            var request = new ExecuteMultipleRequest()
            {
                Requests = new OrganizationRequestCollection(),
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = false,
                    ReturnResponses = true,
                },
            };

            profiles.ForEach(profile =>
            {
                // We want to ensure we don't deactivate profiles that might be part of an in-flight test
                var profileToUpdate = new trd_inspectorprofile { Id = profile.Id, statecode = state, statuscode = status };

                if (!string.IsNullOrEmpty(profile.trd_name) && profile.trd_name.StartsWith("INSPECTORACCTEST") && !deactivateTestProfiles)
                {
                    return;
                }
                else
                {
                    request.Requests.Add(new UpdateRequest { Target = profileToUpdate });
                    updatedProfiles.Add(profileToUpdate);
                }
            });

            var response = context.Execute(request) as ExecuteMultipleResponse;

            if (response.IsFaulted)
            {
                var faults = string.Join(",", response.Responses.Where(r => r.Fault != null).Select(r => r.Fault));
                throw new Exception($"Failed to change the state of the inspector profile record(s) with the fault(s): {faults}");
            }

            return updatedProfiles;
        }
    }
}