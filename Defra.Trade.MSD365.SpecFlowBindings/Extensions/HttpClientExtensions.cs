// <copyright file="HttpClientExtensions.cs" company="DEFRA">
// Copyright (c) DEFRA. All rights reserved.
// </copyright>

namespace Defra.Trade.Plants.SpecFlowBindings.Extensions;

using System.Net.Http;
using System.Threading.Tasks;

/// <summary>
/// Extensions for the HttpClient class.
/// </summary>
public static class HttpClientExtensions
{
    /// <summary>
    /// This is missing in .Net 4 (present in 5 so can be deleted at that stage).
    /// </summary>
    /// <param name="client">HTTPClient object.</param>
    /// <param name="requestUri">The request URI to perform the patch against.</param>
    /// <param name="content">The content to send in the request.</param>
    /// <returns>Returns a HttpResponseMessage asynchronously.</returns>
    public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content)
    {
        var method = new HttpMethod("PATCH");

        var request = new HttpRequestMessage(method, requestUri)
        {
            Content = content,
        };

        return await client.SendAsync(request);
    }
}