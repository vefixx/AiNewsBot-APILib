using System.ComponentModel.DataAnnotations;

namespace AiNewsBot_APILib.Models;

public class PostCreateInfoDTO
{
    public required string PostId { get; set; }
    public required string Text { get; set; }
}