using System.Text.Json.Serialization;

namespace ePOS.Shared.ValueObjects;

public class APIResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("statusCode")]
    public int StatusCode { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    public APIResponse IsSuccess(string? message = null) => new ()
    {
        Success = true,
        StatusCode = 200,
        Message = message
    };
}

public class APIResponse<TData> : APIResponse
{
    [JsonPropertyName("data")]
    public TData Data { get; set; } = default!;
    
    public APIResponse<TData> IsSuccess(TData data, string? message = null) => new ()
    {
        Success = true,
        StatusCode = 200,
        Message = message,
        Data = data
    };
}