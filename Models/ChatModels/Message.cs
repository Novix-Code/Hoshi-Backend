using System.ComponentModel.DataAnnotations;

namespace Hoshi.Models.ChatModels;

/// <summary>
/// Represents a chat message between users
/// </summary>
public class Message
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The ID of the user who sent the message
    /// </summary>
    [Required]
    public int SenderId { get; set; } 

    /// <summary>
    /// The ID of the user who should receive the message
    /// </summary>
    [Required]
    public int ReceiverId { get; set; }

    /// <summary>
    /// The content of the message
    /// </summary>
    [Required]
    [StringLength(2000)]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// When the message was sent
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Whether the message has been read by the receiver
    /// </summary>
    public bool IsRead { get; set; } = false;
}
