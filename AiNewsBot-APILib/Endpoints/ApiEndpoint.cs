namespace AiNewsBot_APILib.Endpoints;

public abstract class ApiEndpoint
{
    protected readonly AiNewsApiClient ApiClient;
    protected readonly string BaseEndpoint;

    public ApiEndpoint(AiNewsApiClient apiClient, string baseEndpoint)
    {
        if (baseEndpoint.StartsWith("/"))
            baseEndpoint = baseEndpoint.Substring(1);

        BaseEndpoint = baseEndpoint;
        ApiClient = apiClient;
    }
}