namespace Hoshi.Data.ChatDTOs;

/// <summary>
/// DTO for chat messages
/// </summary>
public class MessageChatDTO
{
    /// <summary>
    /// The message ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The ID of the user who sent the message
    /// </summary>
    public int SenderId { get; set; }

    /// <summary>
    /// The ID of the user who should receive the message
    /// </summary>
    public int ReceiverId { get; set; } 

    /// <summary>
    /// The content of the message
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// When the message was sent
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Whether the message has been read by the receiver
    /// </summary>
    public bool IsRead { get; set; }
}
