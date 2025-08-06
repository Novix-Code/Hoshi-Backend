using Hoshi.Data;
using Hoshi.Data.ChatDTOs;
using Hoshi.Models.ChatModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hoshi.Hubs;

/// <summary>
/// SignalR Hub for real-time chat functionality
/// </summary>
[Authorize]
public class ChatHub :Hub
{
    private readonly HoshiDbContext _context;

    public ChatHub(HoshiDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Called when a client connects to the hub
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        try
        {
            var userId = GetUserId();
            if (!userId.HasValue)
            {
                Context.Abort();
                return;
            }

            // Save connection to database
            var connection = new Connection
            {
                UserId = userId.Value,
                ConnectionId = Context.ConnectionId,
                ConnectedAt = DateTime.Now
            };

            // Remove any existing connections for this user
            var existingConnection = await _context.Connections
                .Where(c => c.UserId == userId.Value)
                .ToListAsync();

            _context.Connections.RemoveRange(existingConnection);

            // Add new connection
            await _context.Connections.AddAsync(connection);
            await _context.SaveChangesAsync();

            // Send welcome message
            await Clients.Caller.SendAsync("ReceiveMessage", new MessageChatDTO
            {
                Id = 0,
                SenderId = 0,
                ReceiverId = userId ?? 0,
                Content = $"Welcome to the chat!",
                Timestamp = DateTime.Now,
                IsRead = true
            });

            await SendUnreadMessages(userId);
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Called when a client disconnects from the hub
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        try
        {
            var userId = GetUserId();
            if (userId.HasValue)
            {
                // Remove connection from database
                var connection = await _context.Connections
                    .FirstOrDefaultAsync(c => c.ConnectionId == Context.ConnectionId);

                if (connection != null)
                {
                    _context.Connections.Remove(connection);
                    await _context.SaveChangesAsync();
                }
            }
        }
        catch (Exception){}

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Send a message to another user
    /// </summary>
    /// <param name="receiverId">The ID of the user to send the message to</param>
    /// <param name="message">The message content</param>
    public async Task SendMessage(int? receiverId, string message)
    {
        try
        {
            var senderId = GetUserId();
            if (!senderId.HasValue)
            {
                await Clients.Caller.SendAsync("Error", "User not authenticated");
                return;
            }

            if (!receiverId.HasValue)
            {
                await Clients.Caller.SendAsync("Error", "Receiver ID is required");
                return;
            }

            if (string.IsNullOrEmpty(message))
            {
                await Clients.Caller.SendAsync("Error", "Message content is required");
                return;
            }

            // Check if receiver exists 
            var receiverExists = await _context.Users
                .AnyAsync(u => u.Id == receiverId.Value);

            if (!receiverExists)
            {
                await Clients.Caller.SendAsync("Error", "Receiver does not exist");
                return;
            }

            // Create and save the message
            var chatMessage = new Message
            {
                SenderId = senderId ?? 0,
                ReceiverId = receiverId ?? 0,
                Content = message,
                Timestamp = DateTime.Now,
                IsRead = false
            };

            await _context.Messages.AddAsync(chatMessage);
            await _context.SaveChangesAsync();

            // Check if receiver is online
            var receiverConnection = await _context.Connections
                .FirstOrDefaultAsync(c => c.UserId == receiverId);

            var messageDto = new MessageChatDTO
            {
                Id = chatMessage.Id,
                SenderId = chatMessage.SenderId,
                ReceiverId = chatMessage.ReceiverId,
                Content = chatMessage.Content,
                Timestamp = chatMessage.Timestamp,
                IsRead = receiverConnection != null // Mark as read if receiver is online
            };

            if (receiverConnection != null)
            {
                // Receiver is online, send message immediately and mark as read
                await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", messageDto);
                chatMessage.IsRead = true;
                await _context.SaveChangesAsync();
            }
            // Else !!
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", "Failed to send message");
        }
    }

    /// <summary>
    /// Get chat history with a specific user
    /// </summary>
    /// <param name="otherUserId">The ID of the other user</param>
    public async Task GetChatHistory(int otherUserId)
    {
        try
        {
            var currentUserId = GetUserId();
            if (!currentUserId.HasValue)
            {
                await Clients.Caller.SendAsync("Error", "User not authenticated");
                return;
            }

            var messages = await _context.Messages
                .Where(m => (m.SenderId == currentUserId && m.ReceiverId == otherUserId) ||
                           (m.SenderId == otherUserId && m.ReceiverId == currentUserId))
                .OrderBy(m => m.Timestamp)
                .Select(m => new MessageChatDTO
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    Content = m.Content,
                    Timestamp = m.Timestamp,
                    IsRead = m.IsRead
                })
                .ToListAsync();

            await Clients.Caller.SendAsync("ChatHistory", messages);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", "Failed to get chat history");
        }
    }


    /// <summary>
    /// Send unread messages to the connected user
    /// </summary>
    /// <param name="userId">The user ID</param>
    private async Task SendUnreadMessages(int? userId)
    {
        try
        {
            var unreadMessage = await _context.Messages
                .Where(m => m.ReceiverId == userId && !m.IsRead)
                .ToListAsync();

            if (unreadMessage.Any())
            {
                var unreadMessagesDto = unreadMessage.Select(m => new MessageChatDTO
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    Content = m.Content,
                    Timestamp = m.Timestamp,
                    IsRead = m.IsRead
                }).ToList();

                await Clients.Caller.SendAsync("ReceiveUnreadMessages", unreadMessagesDto);

                // Mark messages as read
                foreach (var message in unreadMessagesDto)
                {
                    message.IsRead = true;
                }
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }


    /// <summary>
    /// Get the user ID from the JWT claims
    /// </summary>
    /// <returns>The user ID or null if not found</returns>

    private int? GetUserId()
    {
        var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null ? int.Parse(userIdClaim.Value) : (int?)null;
    }
}
