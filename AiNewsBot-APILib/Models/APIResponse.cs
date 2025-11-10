using Newtonsoft.Json;

namespace AiNewsBot_Backend.API.Models;

public class APIResponse
{
    public object? Data { get; set; }
    public string Message { get; set; } = "";

    public T? GetData<T>()
    {
        T? d = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(Data));
        return d;
    }
}