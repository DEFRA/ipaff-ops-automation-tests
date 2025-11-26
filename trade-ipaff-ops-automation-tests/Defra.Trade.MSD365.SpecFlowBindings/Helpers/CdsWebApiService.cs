
namespace Defra.Trade.Plants.SpecFlowBindings.Helpers;

using Capgemini.PowerApps.SpecFlowBindings.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

/// <summary>
/// Class enabling the calling of the D365 Web API.
/// </summary>
public class CdsWebApiService : IDisposable
{
    private readonly HttpClient httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="CdsWebApiService"/> class.
    /// </summary>
    /// <param name="orgUrl">The D365 Organisation URL.</param>
    /// <param name="accessToken">Valid Access token for the API.</param>
    public CdsWebApiService(Uri orgUrl, string accessToken)
    {
        this.httpClient = new HttpClient()
        {
            BaseAddress = new Uri($"{orgUrl.AbsoluteUri}api/data/v9.1/"),
        };

        this.httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
        this.httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
        this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }

    /// <summary>
    /// Sends a GET request to the web api.
    /// </summary>
    /// <param name="entitySetName">The name of entity.</param>
    /// <param name="columns">The columns to return.</param>
    /// <param name="filter">The ODATA filter to apply.</param>
    /// <returns>API response.</returns>
    public JObject Get(string entitySetName, string columns, string filter = null)
    {
        return this.GetAsync(entitySetName, columns, filter).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Sends a GET request to the web api.
    /// </summary>
    /// <param name="entitySetName">The name of entity.</param>
    /// <param name="columns">The columns to return.</param>
    /// <param name="filter">The ODATA filter to apply.</param>
    /// <returns>Task returned containing API response.</returns>
    public async Task<JObject> GetAsync(string entitySetName, string columns, string filter = null)
    {
        string url = $"{this.httpClient.BaseAddress.OriginalString}{entitySetName}?$select={columns}";
        if (!string.IsNullOrEmpty(filter))
        {
            url += $"&$filter={filter}";
        }

        using (var message = new HttpRequestMessage(HttpMethod.Get, url))
        {
            var response = await this.httpClient.SendAsync(message);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = this.ParseError(response);
                throw new Exception(errorMessage);
            }

            var jsonObject = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
            return jsonObject;
        }
    }

    /// <summary>
    /// Sends a POST request to the web api.
    /// </summary>
    /// <param name="entitySetName">The name of entity.</param>
    /// <param name="body">The content to post.</param>
    public void Post(string entitySetName, object body)
    {
        this.PostAsync(entitySetName, body).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Sends a POST request to the web api.
    /// </summary>
    /// <param name="entitySetName">The name of entity.</param>
    /// <param name="body">The content to post.</param>
    /// <returns>Task returned.</returns>
    public async Task PostAsync(string entitySetName, object body)
    {
        using (var message = new HttpRequestMessage(HttpMethod.Post, $"{this.httpClient.BaseAddress.OriginalString}{entitySetName}"))
        {
            message.Content = new StringContent(JObject.FromObject(body).ToString());
            message.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            var response = await this.httpClient.SendAsync(message);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = this.ParseError(response);
                throw new Exception(errorMessage);
            }
        }
    }

    /// <summary>
    /// Sends a DELETE request to the web api.
    /// </summary>
    /// <param name="entitySetName">The name of entity.</param>
    /// <param name="entityId">Unique identifier of the entity.</param>
    public void Delete(string entitySetName, Guid entityId)
    {
        this.DeleteAsync(entitySetName, entityId).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Sends a DELETE request to the web api.
    /// </summary>
    /// <param name="entitySetName">The name of entity.</param>
    /// <param name="entityId">Unique identifier of the entity.</param>
    /// <returns>Task returned containing API response.</returns>
    public async Task DeleteAsync(string entitySetName, Guid entityId)
    {
        string url = $"{this.httpClient.BaseAddress.OriginalString}{entitySetName}({entityId})";
        var response = await this.httpClient.DeleteAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = this.ParseError(response);
            throw new Exception(errorMessage);
        }
    }

    /// <summary>
    /// Gets entity metadata.
    /// </summary>
    /// <param name="logicalName">Logical name of the entity.</param>
    /// <param name="columns">The columns to return.</param>
    /// <returns>API response.</returns>
    public JObject GetEntityDefinition(string logicalName, string columns)
    {
        return this.GetEntityDefinitionAsync(logicalName, columns).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Gets entity metadata.
    /// </summary>
    /// <param name="logicalName">Logical name of the entity.</param>
    /// <param name="columns">The columns to return.</param>
    /// <returns>Task returned containing API response.</returns>
    public async Task<JObject> GetEntityDefinitionAsync(string logicalName, string columns)
    {
        string url = $"{this.httpClient.BaseAddress.OriginalString}EntityDefinitions(LogicalName='{logicalName}')?$select={columns}";
        using (var message = new HttpRequestMessage(HttpMethod.Get, url))
        {
            var response = await this.httpClient.SendAsync(message);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = this.ParseError(response);
                throw new Exception(errorMessage);
            }

            var jsonObject = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
            return jsonObject;
        }
    }

    /// <summary>
    /// Dispose of the <see cref="HttpClient"/>.
    /// </summary>
    public void Dispose()
    {
        this.httpClient?.Dispose();
    }

    private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption httpCompletionOption = HttpCompletionOption.ResponseHeadersRead)
    {
        var response = await this.httpClient.SendAsync(request, httpCompletionOption);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"HttpRequest failed with response status code: {response.StatusCode}");
        }

        return response;
    }

    private string ParseError(HttpResponseMessage response)
    {
        var errorResponse = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
        var errorDetails = JsonConvert.DeserializeObject<JObject>(errorResponse.Property("error").Value.ToString());
        return errorDetails.Property("message").Value.ToString();
    }
}
