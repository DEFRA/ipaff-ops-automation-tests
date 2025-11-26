namespace Defra.Trade.Plants.SpecFlowBindings.Hooks;

using Capgemini.PowerApps.SpecFlowBindings;
using Defra.Trade.Plants.Model;
using Defra.Trade.Plants.SpecFlowBindings.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Loads and caches a list of available model-driven apps.
/// </summary>
public class ModelDrivenAppHelper : PowerAppsStepDefiner
{
    private static IList<AppModule> apps;
    private static object appsLock = new object();

    /// <summary>
    /// Gets the List of available model-driven apps.
    /// </summary>
    public static IList<AppModule> Apps
    {
        get
        {
            lock (appsLock)
            {
                if (apps != null && apps.Any())
                {
                    return apps;
                }

                using (var service = new CdsWebApiService(TestConfig.GetTestUrl(), AccessToken))
                {
                    var newApps = new List<AppModule>();
                    var jsonObject = service.Get("appmodules", "name,appmoduleid,uniquename");
                    var appsJson = (JArray)jsonObject["value"];

                    appsJson.ToList().ForEach(entity =>
                    {
                        var app = new AppModule
                        {
                            Name = entity["name"].ToString(),
                            UniqueName = entity["uniquename"].ToString(),
                            AppModuleId = Guid.Parse(entity["appmoduleid"].ToString()),
                        };
                        newApps.Add(app);
                    });
                    apps = newApps;
                }
            }

            return apps;
        }
    }
}
