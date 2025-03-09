using System.ComponentModel.DataAnnotations;

namespace OdysseyWebServer.Models.ViewModels;

public class SendPlayerViewModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Player name is required")]
    public string? Player { get; set; } = "*";

    [Required(AllowEmptyStrings = false, ErrorMessage = "Stage name is required")]
    public string? Stage { get; set; } = "HomeShipInsideStage";

    [Required(AllowEmptyStrings = false, ErrorMessage = "Scenario is required")]
    public int? Scenario { get; set; } = -1;
    [Required(AllowEmptyStrings = false, ErrorMessage = "Id is required")]
    public int? Id { get; set; } = 0;
}