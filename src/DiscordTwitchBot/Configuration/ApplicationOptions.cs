using System.ComponentModel.DataAnnotations;

namespace DiscordTwitchBot.Configuration;

public class ApplicationOptions
{
    [Required (ErrorMessage = "Application name is required.")]
    public string? Name { get; set; }
}