using System.Text;
using AiNewsBot_APILib.Endpoints;
using AiNewsBot_Backend.API.Models;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace AiNewsBot_APILib;

public class AiNewsApiClient
{
    private readonly HttpClient _client;
    private const string BaseUrl = "http://localhost:3232/";

    // Endpoints
    public AiGatewayEndpoint AiGatewayEndpoint { get; set; }

    public AiNewsApiClient()
    {
        AiGatewayEndpoint = new AiGatewayEndpoint(this, "ai-gateway");
        _client = new HttpClient();
    }

    private static StringContent? BuildContent(object? data)
    {
        if (data == null)
            return null;

        string json = JsonConvert.SerializeObject(data);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    private static string BuildUri(string url, Dictionary<string, string?>? queryParams)
    {
        if (queryParams != null)
        {
            queryParams = queryParams.Where(kv => kv.Value != null).ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        return queryParams != null ? QueryHelpers.AddQueryString(url, queryParams) : url;
    }

    private async Task<APIResponse> SendRequestAsync(HttpMethod method, string url, object? data = null,
        Dictionary<string, string?>? queryParams = null)
    {
        string uri = BuildUri(url, queryParams);
        StringContent? requestContent = BuildContent(data);

        using var request = new HttpRequestMessage(method, uri)
        {
            Content = requestContent
        };

        using var response = await _client.SendAsync(request, HttpCompletionOption.ResponseContentRead);
        string content = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<APIResponse>(content);
        if (result is null)
            throw new JsonException($"Ответ пустой или не распознан ({uri}).");

        return result;
    }

    public async Task<APIResponse> GetAsync(string endpoint, Dictionary<string, string?>? query = null) =>
        await SendRequestAsync(HttpMethod.Get, BaseUrl + endpoint, null, query);

    public async Task<APIResponse> PostAsync(string endpoint, object? data) =>
        await SendRequestAsync(HttpMethod.Post, BaseUrl + endpoint, data, null);
}