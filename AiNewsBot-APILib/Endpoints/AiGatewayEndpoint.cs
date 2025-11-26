using AiNewsBot_APILib.Models;
using AiNewsBot_Backend.API.Models;

namespace AiNewsBot_APILib.Endpoints;

public class AiGatewayEndpoint : ApiEndpoint
{
    public AiGatewayEndpoint(AiNewsApiClient apiClient, string baseEndpoint) : base(apiClient, baseEndpoint)
    {
    }

    public async Task<List<string>> GetAllPostIdsAsync()
    {
        APIResponse response = await ApiClient.GetAsync(BaseEndpoint + "/posts/ids");

        if (response.Data == null)
        {
            throw new HttpRequestException("Ответ от сервера равен null");
        }
        
        return response.GetData<List<string>>()!;
    }

    public async Task<JobIdData> SummarizePostAsync(PostCreateInfo postCreateInfo)
    {
        APIResponse response = await ApiClient.PostAsync(BaseEndpoint + "/summarize-post", postCreateInfo);
        JobIdData? jobIdData = response.GetData<JobIdData>();
        
        if (jobIdData == null)
            throw new HttpRequestException("Ответ от сервера равен null");
        
        return jobIdData;
    }
}