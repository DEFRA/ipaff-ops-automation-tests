// <copyright file="DataSteps.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Steps;

using Capgemini.PowerApps.SpecFlowBindings;
using Capgemini.PowerApps.SpecFlowBindings.Configuration;
using Defra.Trade.Plants.BusinessLogic.ReferenceData;
using Defra.Trade.Plants.Model;
using Defra.Trade.Plants.SpecFlowBindings.Context;
using Defra.Trade.Plants.SpecFlowBindings.Extensions;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using Reqnroll;
using System.ServiceModel;

/// <summary>
/// Steps related to test data set up.
/// </summary>
[Binding]
public class DataSteps : PowerAppsStepDefiner
{
    private readonly SessionContext ctx;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataSteps"/> class.
    /// </summary>
    /// <param name="ctx">The test session context object.</param>
    public DataSteps(SessionContext ctx)
    {
        this.ctx = ctx;
    }

    [Given(@"'(.*)' has the (.*) commodities of type (.*)")]
    public void GivenHasTheCommodities(string dataFileName, int numberOfCommodities, string applicationType)
    {
        dynamic jsonObject = JsonConvert.DeserializeObject(TestDataRepository.GetTestData(dataFileName));

        if (jsonObject == null)
        {
            throw new Exception("error in deserialisation");
        }

        var entityRecord = (JArray)jsonObject["trd_exportapplication_trd_consignment_exportapplicationid"];

        while (entityRecord.Count <= numberOfCommodities)
        {
            entityRecord.Add(entityRecord[0]);
        }

        switch (applicationType)
        {
            case "Plants":
                SetPlantProduceConsignment(numberOfCommodities, entityRecord);
                break;
            case "UFM":
                SetUfmConsignment(numberOfCommodities, entityRecord);
                break;
            case "Plant Products":
                SetPlantProductsConsignment(numberOfCommodities, entityRecord);
                break;
            default:
                throw new NotImplementedException("Unexpected type");
        }

        string output = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
        this.ctx.SetVariable(dataFileName, output);
    }

    [Scope(Tag = "Trade")]
    [Given(@"'(.*)' has created '(.*)'")]
    public void GivenIHaveCreated(string alias, string fileName)
    {
        Driver.WaitForTransaction();

        Policy
            .Handle<WebDriverException>(ex => ex.Message.Contains("Cannot read properties of undefined (reading 'loadTestDataAsUser')"))
            .Retry((ex, i) =>
            {
                TestDriver.InjectOnPage(TestConfig.ApplicationUser != null ? AccessToken : null);
            })
            .Execute(() =>
            {
                var testData = TestDataRepository.GetTestData(fileName);
                TestDriver.LoadTestDataAsUser(
                    testData,
                    TestConfig.GetUser(alias).Username);
                this.AddAliasToCache(testData);
                this.PostProcess(testData);
            });
    }

    private void AddAliasToCache(string json)
    {
        var parsedJson = JObject.Parse(json);
        var aliases = new List<string>();
        GetNestedAliases(parsedJson, aliases);
        aliases.ForEach(alias => this.ctx.GetEntityReference(alias));
    }

    private static void GetNestedAliases(JToken jToken, List<string> aliases)
    {
        if (jToken.Type == JTokenType.Object)
        {
            aliases.AddRange((jToken as JObject).Properties().Where(p => p.Name == "@alias").Select(p => p.Value.ToString()).ToList());
        }

        if (jToken.Type == JTokenType.Object || jToken.Type == JTokenType.Array)
        {
            IEnumerable<JToken> children = null;
            if (jToken.Root == jToken)
            {
                children = (jToken as JContainer).Children().Children();
            }
            else
            {
                children = (jToken as JContainer).Children();
            }

            var childObjects = children.Where(c => c.Type == JTokenType.Object).Cast<JObject>().ToList();
            if (childObjects.Any())
            {
                foreach (var childObject in childObjects)
                {
                    GetNestedAliases(childObject, aliases);
                }
            }

            var childArrays = children.Where(c => c.Type == JTokenType.Array).Cast<JArray>().ToList();
            if (childArrays.Any())
            {
                foreach (var childArray in childArrays)
                {
                    GetNestedAliases(childArray, aliases);
                }
            }
        }
    }

    [Scope(Tag = "Trade")]
    [Given(@"I have created '(.*)'")]
    public void GivenIHaveCreated(string fileName)
    {
        Policy
            .Handle<WebDriverException>()
            .WaitAndRetry(2, retryAttempt => TimeSpan.FromSeconds(15))
            .Execute(() =>
            {
                var testData = TestDataRepository.GetTestData(fileName);
                TestDriver.LoadTestData(testData);
                this.AddAliasToCache(testData);
                this.PostProcess(testData);
            });

    }

    [Given(@"'(.*)' has the following setup for '(.*)'")]
    public void GivenHasTheFollowingSetupFor(string dataFileName, string entityName, Table table)
    {
        dynamic jsonObject = JsonConvert.DeserializeObject(TestDataRepository.GetTestData(dataFileName));

        if (jsonObject == null)
        {
            throw new Exception("error in deserialisation");
        }

        var entityRecord = (JArray)jsonObject[entityName];
        var recordIndexNeeded = table.Rows.ToList().Select(p => Convert.ToInt32(p["Record"])).ToList();

        while (entityRecord.Count <= recordIndexNeeded.Count)
        {
            entityRecord.Add(entityRecord[0]);
        }

        foreach (var tableRow in table.Rows)
        {
            var recordPosition = Convert.ToInt32(tableRow["Record"]) - 1;
            foreach (var header in table.Header.Where(p => p.ToLower() != "record"))
            {
                var record = entityRecord[recordPosition];
                record[header] = tableRow[header];
            }
        }

        // Clear the default records if not requested
        for (var recordPosition = entityRecord.Count; recordPosition >= 1; recordPosition--)
        {
            if (recordIndexNeeded.Contains(recordPosition))
            {
                continue;
            }

            entityRecord.Remove(entityRecord[recordPosition - 1]);
        }

        string output = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
        this.ctx.SetVariable(dataFileName, output);
    }

    [Given(@"a privileged user has created the following records")]
    public void GivenAUserHasCreatedSomethingICannot(Table items)
    {
        foreach (var itemsRow in items.Rows)
        {
            this.CreateTestData(itemsRow[0]);
        }
    }

    [Given(@"'(.*)' creates the following records")]
    public void GivenAUserCreatesSomethingICannot(string privilegedUser, Table items)
    {
        foreach (var itemsRow in items.Rows)
        {
            Policy
            .Handle<WebDriverException>(ex => ex.Message.Contains("Cannot read properties of undefined (reading 'loadTestDataAsUser')"))
            .Retry((ex, i) =>
            {
                TestDriver.InjectOnPage(TestConfig.ApplicationUser != null ? AccessToken : null);
            })
            .Execute(() =>
            {
                var testData = TestDataRepository.GetTestData(itemsRow[0]);
                TestDriver.LoadTestDataAsUser(
                    testData,
                    TestConfig.GetUser(privilegedUser).Username);
                this.AddAliasToCache(testData);
                this.PostProcess(testData);
            });
        }
    }

    [Given(@"I have opened '(.*)' record")]
    [When(@"I have opened '(.*)' record")]
    public void GivenIHaveOpened(string name)
    {
        var ents = this.ctx.GetEntityReference(name);
        XrmApp.Entity.OpenEntity(ents.LogicalName, ents.Id);
        Driver.WaitForTransaction();
    }

    // This is posed as a generic method to allow creation of additional related data
    // Ideally should be reworked to actually be generic or be renamed to indicate it only works for HMI related entities.
    [Given(@"'(.*)' has entered '(.*)' for '(.*)'")]
    public void GivenHasEnteredFor(string userAlias, string relatedDataAlias, string applicationAlias)
    {
        var entityHolder = this.ctx.GetEntity("WorkOrder");

        using (var serviceClient = SessionContext.GetServiceClient())
        {
            using (var context = new PlantsContext(serviceClient))
            {
                var workOrder = entityHolder.Id.ToEntity<msdyn_workorder>();
                var serviceTasks = workOrder.GetServiceTasks(context);

                var serviceTaskType = msdyn_servicetasktype.FindTaskTypeBy("Exports HMI Check", context);
                var serviceTask = serviceTasks.FirstOrDefault(t => t.msdyn_TaskType.Id == serviceTaskType.Id);

                List<trd_HMIResult> hmiResults = null;

                Policy
                    .Handle<Exception>()
                    .WaitAndRetry(5, retryCount => 5.Seconds())
                    .Execute(() =>
                    {
                        // Retrieve a collection of HMI Results with a risk rating of 'red'
                        hmiResults = serviceTask.ResultsWithRiskRating(trd_hmiriskrating.Red, context).ToList();
                        if (hmiResults.Count == 0)
                        {
                            throw new Exception("No HMI Result found");
                        }
                    });

                hmiResults.ForEach((hmiResult) =>
                {
                    // Create associated records
                    dynamic jsonObject = JsonConvert.DeserializeObject(TestDataRepository.GetTestData(relatedDataAlias));
                    var entities = (JArray)jsonObject["value"];

                    string attributeName = DataFileConstants.HMIResultRelations[relatedDataAlias];

                    if (string.IsNullOrEmpty(attributeName))
                    {
                        throw new ArgumentException($"Cannot find a attribute name for {relatedDataAlias}.");
                    }

                    entities.ToList().ForEach(entity =>
                    {
                        if (entity.Type == JTokenType.Object)
                        {
                            this.ReplaceAliasesWithEntityReference(entity as JObject);
                        }

                        entity[attributeName] = string.Format((string)entity[attributeName], hmiResult.Id.ToString());
                        TestDriver.LoadTestDataAsUser(
                            JsonConvert.SerializeObject(entity),
                            TestConfig.GetUser(userAlias).Username);
                    });
                });
            }
        }
    }

    private void ReplaceAliasesWithEntityReference(JObject entity)
    {
        var unmappedAliases = entity.Properties().Where(p => p.Name.Contains("@alias")).ToList();
        foreach (var unmappedAlias in unmappedAliases)
        {
            var alias = unmappedAlias.Name.Substring(unmappedAlias.Name.IndexOf("(") + 1, unmappedAlias.Name.Length - unmappedAlias.Name.IndexOf("(") - 2);
            var entityReferenceKey = this.ctx.EntityReferences.Keys.Where(k => k.Contains(alias)).FirstOrDefault();
            if (string.IsNullOrEmpty(entityReferenceKey))
            {
                throw new ArgumentException($"No entity reference can be found for the alias {alias}");
            }

            var key = unmappedAlias.Name.Substring(0, unmappedAlias.Name.IndexOf("@alias"));
            var value = this.ctx.EntityReferences[entityReferenceKey];
            unmappedAlias.AddAfterSelf(new JProperty(key, string.Format((string)unmappedAlias.Value, value.Id)));
            unmappedAlias.Remove();
        }
    }

    [Given(@"the system has calculated the overall risk rating for '(.*)'")]
    public void GivenTheSystemHasCalculatedTheOverallRiskRatingFor(string applicationAlias)
    {
        var entityHolder = this.ctx.GetEntity("WorkOrder");

        using (var serviceClient = SessionContext.GetServiceClient())
        {
            using (var context = new PlantsContext(serviceClient))
            {
                context.MergeOption = MergeOption.NoTracking;

                var workOrder = entityHolder.Id.ToEntity<msdyn_workorder>();
                var serviceTaskType = msdyn_servicetasktype.FindTaskTypeBy("Exports HMI Check", context);

                Policy
                    .Handle<Exception>()
                    .WaitAndRetry(5, retryCount => 5.Seconds())
                    .Execute(() =>
                    {
                        var serviceTasks = workOrder.GetServiceTasks(context);
                        var serviceTask = serviceTasks.FirstOrDefault(t => t.msdyn_TaskType.Id == serviceTaskType.Id);

                        if (!serviceTask.trd_OverallRiskRating.HasValue)
                        {
                            throw new Exception($"Service Task '{serviceTask.msdyn_name}' does not have an Overall Risk Rating.");
                        }
                    });
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userAlias"></param>
    /// <param name="applicationAlias"></param>
    [Scope(Tag = "FailedClassification")]
    [Given(@"'(.*)' has failed an inspection for '(.*)'")]
    public void GivenHasFailedAnInspectionFor(string userAlias, string applicationAlias)
    {
        var entityHolder = this.ctx.GetEntity("WorkOrder");

        using (var serviceClient = SessionContext.GetServiceClient())
        {
            using (var context = new PlantsContext(serviceClient))
            {
                var hmiResult = this.GetHMIResults(context)
                    .ToArray()
                    .OrderBy(r => r.trd_Commodity.Name)
                    .First();

                var defectClass = trd_DefectClass.FindByName("SMS non compliant, GMS non compliant", context);
                hmiResult.FailedToMeetStandard(defectClass.ToEntityReference(), context);
            }
        }
    }

    [Scope(Tag = "FailedOtherResult")]
    [Given(@"'(.*)' has failed an inspection for '(.*)'")]
    public void FailedInspectionOtherResult(string userAlias, string applicationAlias)
    {
        using (var serviceClient = SessionContext.GetServiceClient())
        {
            using (var context = new PlantsContext(serviceClient))
            {
                var hmiResult = this.GetHMIResults(context)
                    .ToArray()
                    .OrderBy(r => r.trd_Commodity.Name)
                    .First();

                hmiResult.FailedToMeetStandard(context);
            }
        }
    }

    [Scope(Tag = "HMI")]
    [Given(@"'(.*)' has reinspected a failed inspection")]
    public void GivenHasReinspectedAFailedInspection(string userAlias)
    {
        using (var serviceClient = SessionContext.GetServiceClient())
        {
            using (var context = new PlantsContext(serviceClient))
            {
                context.MergeOption = MergeOption.NoTracking;

                IQueryable<trd_HMIResult> hmiResults = null;

                // Wait for inspection lines to be created
                Policy.Handle<ArgumentNullException>()
                    .WaitAndRetry(3, retryCount => TimeSpan.FromSeconds(5))
                    .Execute(() =>
                    {
                        hmiResults = this.GetHMIResults(context)
                        .ToArray()
                        .OrderBy(r => r.trd_Commodity.Name)
                        .AsQueryable();
                    });

                // Clone the HMI Result
                var hmiResult = hmiResults.First();
                hmiResult.CloneForReinspection(context);
            }
        }
    }

    [When(@"I wait for the system to submit '(.*)' time recording records")]
    public void WhenIWaitForTheSystemToSubmitTimeRecordingRecords(int count)
    {
        using (var serviceClient = SessionContext.GetServiceClient())
        {
            using (var plantsContext = new PlantsContext(serviceClient))
            {
                var workOrder = this.ctx.GetEntity("WorkOrder").Id.ToEntity<msdyn_workorder>();
                var serviceTask = workOrder.GetServiceTasks(plantsContext)
                    .ToList()
                    .Where(t => t.msdyn_TaskType.Id == ServiceTaskType.ExportsHMICheck)
                    .First();

                Policy.Handle<Exception>()

                    .WaitAndRetry(4, retryCount => TimeSpan.FromSeconds(5))
                    .Execute(() =>
                    {
                        var timeRecordings = serviceTask.LoggedTime(plantsContext)
                            .ToArray()
                            .Where(tr => tr.statuscode == trd_timerecording_statuscode.Submitted)
                            .ToArray();

                        if (timeRecordings.Length != count)
                        {
                            throw new Exception($"Query results contained {timeRecordings.Length} 'Submitted' time recording records but expected {count}");
                        }
                    });
            }
        }
    }

    /// <summary>
    /// Retrieves a collection of related trd_HMIResult instances.
    /// </summary>
    /// <returns>Collection of trd_HMIResult.</returns>
    private IQueryable<trd_HMIResult> GetHMIResults(PlantsContext context)
    {
        var entityHolder = this.ctx.GetEntity("WorkOrder");
        var workOrder = entityHolder.Id.ToEntity<msdyn_workorder>();
        var serviceTaskType = msdyn_servicetasktype.FindTaskTypeBy("Exports HMI Check", context);
        var serviceTasks = workOrder.GetServiceTasks(context);
        var serviceTask = serviceTasks.FirstOrDefault(t => t.msdyn_TaskType.Id == serviceTaskType.Id);
        return serviceTask.GetHMIResults(context);
    }

    private UserConfiguration FetchUserConfig(string privilegedUser)
    {
        var user = TestConfig.GetUser(privilegedUser);
        this.ctx.UserName = user.Username;
        this.ctx.Password = user.Password;
        return user;
    }

    private void CreateTestData(string fileName)
    {
        var json = (this.ctx.GetVariable<string>(fileName) ?? TestDataRepository.GetTestData(fileName))
                .TokeniseText()
                .TransformAliasToGuid(this.ctx);

        var jsonAsDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

        string logicalName = jsonAsDictionary
            .Where(prop => prop.Key == "@logicalName")
            .Select(pp => pp.Value)
            .First().ToString();

        string alias = jsonAsDictionary
            .Where(prop => prop.Key == "@alias")
            .Select(pp => pp.Value)
            .First().ToString();

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12;

        this.ctx.SessionToken = AccessToken;
        using (HttpClient httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.ctx.SessionToken);

            var callerId = this.ctx.GetVariable<string>("AdminUserCallerId");
            if (string.IsNullOrEmpty(callerId))
            {
                var user = this.FetchUserConfig("an admin");
                var res = httpClient.GetStringAsync(
                    $"{TestConfig.GetTestUrl()}api/data/v9.0/" +
                    $"systemusers?" +
                    $"$filter=internalemailaddress eq '{user.Username}'&" +
                    $"$select=azureactivedirectoryobjectid").Result;
                var usersJson = JObject.Parse(res);
                callerId = usersJson["value"][0]["azureactivedirectoryobjectid"].Value<string>();
                this.ctx.SetVariable("AdminUserCallerId", callerId);
            }

            httpClient.DefaultRequestHeaders.Add("CallerObjectId", callerId);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var metadataName = httpClient.GetAsync($"{TestConfig.GetTestUrl()}api/data/v9.0/EntityDefinitions(LogicalName='{logicalName}')?$select=LogicalCollectionName").Result;
            var collectionName = JsonConvert.DeserializeObject<Dictionary<string, object>>(metadataName.Content.ReadAsStringAsync().Result).Where(prop => prop.Key == "LogicalCollectionName").Select(pp => pp.Value).First().ToString();

            var response = httpClient.PostAsync($"{TestConfig.GetTestUrl()}api/data/v9.0/{collectionName}", content).Result;
            if (response.IsSuccessStatusCode)
            {
                Regex rx = new Regex(@"\(([^)]+)\)");
                Match match = rx.Match(response.Headers.GetValues("OData-EntityId").First());
                string entityID = match.Value;

                this.ctx.Entities.Add($"{fileName}{this.ctx.SessionId}", new EntityHolder
                {
                    Alias = alias,
                    EntityName = logicalName,
                    EntityCollectionName = collectionName,
                    Id = Guid.Parse(entityID),
                });
            }
            else
            {
                throw new Exception($"Unable to create record: status {response.StatusCode} for entity : {logicalName} Error: {response.Content.ReadAsStringAsync().Result}");
            }
        }

        this.PostProcess(json);
    }

    private static void SetUfmConsignment(int numberOfCommodities, JArray entityRecord)
    {
        for (var i = 0; i < numberOfCommodities; i++)
        {
            var record = entityRecord[i];
            record["trd_name"] = $"consignment - {i}";
            record["trd_valueamount"] = 12345.68;
            record["trd_pointofentry"] = "Dover";
            record["trd_transportmode"] = 167440000;
            record["trd_consigneename"] = "consignee name";
            record["trd_consigneeaddress1"] = $"consignee address {i}";
            record["trd_importpermitnumber"] = "111222333";
            record["trd_consigneecountryid@odata.bind"] = "trd_countries(trd_consolidatedisocode='CN')";
            var subRecord = record["trd_trd_consignment_trd_consignmentitem_consignmentid"];
            subRecord[0].Remove();
            subRecord[0]["trd_id"] = "12345";
            subRecord[0]["trd_farmmachineryuniqueid"] = "123456";
            subRecord[0]["trd_farmmachinerytype"] = $"Farm Type - {i}";
            subRecord[0]["trd_farmmachinerymake"] = $"Farm - {i}";
            subRecord[0]["trd_farmmachinerymodel"] = $"Farm Model Tractor - {i}";
            subRecord[0]["trd_primarycountryoforigin@odata.bind"] = "trd_countries(trd_consolidatedisocode='CN')";
        }
    }

    private static void SetPlantProduceConsignment(int numberOfCommodities, JArray entityRecord)
    {
        for (var i = 0; i < numberOfCommodities; i++)
        {
            var record = entityRecord[i];
            record["trd_name"] = $"consignment - {i}";
            record["trd_valueamount"] = 12345.67;
            record["trd_pointofentry"] = "Dover";
            record["trd_transportmode"] = 167440000;
            record["trd_consigneename"] = "consignee name";
            record["trd_consigneeaddress1"] = $"consignee address {i}";
            record["trd_importpermitnumber"] = "111222333";
            record["trd_consigneecountryid@odata.bind"] = "trd_countries(trd_consolidatedisocode='PT')";
            var subRecord = record["trd_trd_consignment_trd_consignmentitem_consignmentid"];
            subRecord[0]["trd_id"] = "123456";
            subRecord[0]["trd_botanicalgenus"] = "Acrocomia";
            subRecord[0]["trd_botanicalspecies"] = "Mexicana";
            subRecord[0]["trd_commodityfamilyname"] = "Testus";
            subRecord[0]["trd_distinguishingmarks"] = "container numbers";
            subRecord[0]["trd_numberofpackages"] = 1.00;
            subRecord[0]["trd_packagingmaterialused"] = "Stainless steel";
            subRecord[0]["trd_packagingtype"] = "Bag";
            subRecord[0]["trd_packagingtypecode"] = "BG";
            subRecord[0]["trd_quantity"] = 100.00;
            subRecord[0]["trd_unitofmeasurement"] = "Gram";
            subRecord[0]["trd_eppocode"] = "EP1";
            subRecord[0]["trd_variety"] = "Variety";
            subRecord[0]["trd_primarycountryoforigin@odata.bind"] = "trd_countries(trd_consolidatedisocode='GB')";
            subRecord[0]["trd_additionalcountriesoforigin"] = "test1;test2";
            subRecord[0]["trd_samplereference"] = "sample reference";
        }
    }

    private static void SetPlantProductsConsignment(int numberOfCommodities, JArray entityRecord)
    {
        for (var i = 0; i < numberOfCommodities; i++)
        {
            var record = entityRecord[i];
            record["trd_name"] = $"consignment - {i}";
            record["trd_valueamount"] = 12345.67;
            record["trd_pointofentry"] = "Dover";
            record["trd_transportmode"] = 167440000;
            record["trd_consigneename"] = "consignee name";
            record["trd_consigneeaddress1"] = $"consignee address {i}";
            record["trd_importpermitnumber"] = "111222333";
            record["trd_consigneecountryid@odata.bind"] = "trd_countries(trd_consolidatedisocode='PT')";
            var subRecord = record["trd_trd_consignment_trd_consignmentitem_consignmentid"];
            subRecord[0]["trd_id"] = "123456";
            subRecord[0]["trd_botanicalgenus"] = "Acrocomia";
            subRecord[0]["trd_botanicalspecies"] = "Mexicana";
            subRecord[0]["trd_commodityfamilyname"] = "Testus";
            subRecord[0]["trd_distinguishingmarks"] = "container numbers";
            subRecord[0]["trd_numberofpackages"] = 1.00;
            subRecord[0]["trd_packagingmaterialused"] = "Stainless steel";
            subRecord[0]["trd_packagingtype"] = "Bag";
            subRecord[0]["trd_packagingtypecode"] = "BG";
            subRecord[0]["trd_quantity"] = 100.00;
            subRecord[0]["trd_unitofmeasurement"] = "Gram";
            subRecord[0]["trd_eppocode"] = "EP1";
            subRecord[0]["trd_variety"] = "Variety";
            subRecord[0]["trd_primarycountryoforigin@odata.bind"] = "trd_countries(trd_consolidatedisocode='GB')";
            subRecord[0]["trd_additionalcountriesoforigin"] = "test1;test2";
            subRecord[0]["trd_samplereference"] = "sample reference";
        }
    }

    private void PostProcess(string json)
    {
        const string autoStatusKey = "@autoSetStatusCodeAfterCreate";

        if (json.Contains(autoStatusKey))
        {
            dynamic dynamicJson = JsonConvert.DeserializeObject(json);
            var alias = dynamicJson["@alias"]?.ToString();
            var recordId = this.ctx.GetEntityReference(alias);
            var statusCode = Convert.ToInt32(dynamicJson[autoStatusKey]);
            Policy
                .Handle<FaultException<OrganizationServiceFault>>(e => e.Message == "Geocode address internal server error.")
                .Retry(3)
                .Execute(() =>
                {
                    var updRecord = new Entity(recordId.LogicalName, recordId.Id);
                    updRecord["statuscode"] = new OptionSetValue(statusCode);
                    SessionContext.GetServiceClient().Update(updRecord);
                });
        }
    }
}
