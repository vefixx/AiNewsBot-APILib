using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace AiNewsBot_APILib;

public class AiNewsApiClient
{
    private readonly HttpClient _client;
    private const string BaseUrl = "http://localhost:3232/";

    // Endpoints


    public AiNewsApiClient()
    {
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

    private async Task<T> SendRequestAsync<T>(HttpMethod method, string url, object? data = null,
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

        var result = JsonConvert.DeserializeObject<T>(content);
        if (result is null)
            throw new JsonException($"Ответ пустой или не распознан ({uri}).");

        return result;
    }

    public async Task<T> GetAsync<T>(string endpoint, Dictionary<string, string?>? query = null) =>
        await SendRequestAsync<T>(HttpMethod.Get, BaseUrl + endpoint, null, query);
}