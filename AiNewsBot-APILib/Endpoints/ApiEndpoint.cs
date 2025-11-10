namespace AiNewsBot_APILib.Endpoints;

public abstract class ApiEndpoint
{
    protected readonly AiNewsApiClient _apiClient;

    public ApiEndpoint(AiNewsApiClient apiClient)
    {
        _apiClient = apiClient;
    }
}