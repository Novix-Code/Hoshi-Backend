using System.ComponentModel.DataAnnotations;

namespace Hoshi.Models.ChatModels;

/// <summary>
/// Represents a user's SignalR connection
/// </summary>
public class Connection
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The SignalR connection ID
    /// </summary>
    [Required]
    [StringLength(450)]
    public string ConnectionId { get; set; } = string.Empty;

    /// <summary>
    /// The ID of the connected user
    /// </summary>
    [Required]
    public int UserId { get; set; }

    /// <summary>
    /// When the connection was established
    /// </summary>
    public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;
}
