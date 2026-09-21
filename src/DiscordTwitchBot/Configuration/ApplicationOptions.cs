using System.ComponentModel.DataAnnotations;

namespace DiscordTwitchBot.Configuration;

public class ApplicationOptions
{
    [Required (ErrorMessage = "Application name is required.")]
    public string? Name { get; set; }

    [Required (ErrorMessage = "Environment is required.")]
    public string? Environment { get; set; }
}