# ChatHub Integration Guide

## Overview

The ChatHub is a SignalR-based real-time chat system that enables instant messaging between users in the application. It provides real-time message delivery, chat history retrieval, and connection management.

## Base URL

```
ws://your-domain/chathub
```

## Authentication

The ChatHub requires JWT authentication. Include your JWT token in the connection headers:

```javascript
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chathub", {
        accessTokenFactory: () => "your-jwt-token-here"
    })
    .build();
```

## Connection Events

### OnConnected
When a user successfully connects to the hub:
- A welcome message is sent to the caller
- Any unread messages are delivered to the user
- Previous connections for the same user are cleaned up

### OnDisconnected
When a user disconnects:
- The connection is removed from the database
- No specific action is required from the frontend

## Available Methods

### 1. SendMessage

**Purpose**: Send a message to another user

**Parameters**:
- `receiverId` (int): The ID of the user to send the message to
- `message` (string): The message content

**Example**:
```javascript
await connection.invoke("SendMessage", receiverId, "Hello, how are you?");
```

**Response Events**:
- `ReceiveMessage`: Message is delivered to the receiver (if online)
- `Error`: If there's an error (user not authenticated, receiver doesn't exist, etc.)

### 2. GetChatHistory

**Purpose**: Retrieve chat history with a specific user

**Parameters**:
- `otherUserId` (int): The ID of the other user in the conversation

**Example**:
```javascript
await connection.invoke("GetChatHistory", otherUserId);
```

**Response Event**:
- `ChatHistory`: Returns an array of messages

## Client-Side Event Handlers

### 1. ReceiveMessage
Triggered when a new message is received.

**Event Data Structure**:
```typescript
interface MessageChatDTO {
    id: number;
    senderId: number;
    receiverId: number;
    content: string;
    timestamp: Date;
    isRead: boolean;
}
```

**Example Handler**:
```javascript
connection.on("ReceiveMessage", (message) => {
    console.log("New message received:", message);
    // Handle the new message in your UI
});
```

### 2. ChatHistory
Triggered when chat history is retrieved.

**Event Data**: Array of `MessageChatDTO` objects

**Example Handler**:
```javascript
connection.on("ChatHistory", (messages) => {
    console.log("Chat history:", messages);
    // Display chat history in your UI
});
```

### 3. ReceiveUnreadMessages
Triggered when unread messages are delivered upon connection.

**Event Data**: Array of `MessageChatDTO` objects

**Example Handler**:
```javascript
connection.on("ReceiveUnreadMessages", (messages) => {
    console.log("Unread messages:", messages);
    // Handle unread messages in your UI
});
```

### 4. Error
Triggered when an error occurs.

**Event Data**: Error message string

**Example Handler**:
```javascript
connection.on("Error", (errorMessage) => {
    console.error("ChatHub error:", errorMessage);
    // Handle error in your UI
});
```

## Complete Frontend Integration Example

```javascript
// Establish connection
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chathub", {
        accessTokenFactory: () => localStorage.getItem("jwt-token")
    })
    .withAutomaticReconnect()
    .build();

// Set up event handlers
connection.on("ReceiveMessage", (message) => {
    // Add message to chat UI
    addMessageToChat(message);
});

connection.on("ChatHistory", (messages) => {
    // Load chat history into UI
    loadChatHistory(messages);
});

connection.on("ReceiveUnreadMessages", (messages) => {
    // Handle unread messages
    handleUnreadMessages(messages);
});

connection.on("Error", (errorMessage) => {
    // Show error to user
    showError(errorMessage);
});

// Start connection
async function startConnection() {
    try {
        await connection.start();
        console.log("Connected to ChatHub");
    } catch (err) {
        console.error("Failed to connect:", err);
    }
}

// Send a message
async function sendMessage(receiverId, messageContent) {
    try {
        await connection.invoke("SendMessage", receiverId, messageContent);
    } catch (err) {
        console.error("Failed to send message:", err);
    }
}

// Get chat history
async function getChatHistory(otherUserId) {
    try {
        await connection.invoke("GetChatHistory", otherUserId);
    } catch (err) {
        console.error("Failed to get chat history:", err);
    }
}

// Start the connection when the page loads
startConnection();
```

## Message Flow

1. **User connects**: User establishes connection with JWT token
2. **Welcome message**: System sends welcome message to the user
3. **Unread messages**: Any unread messages are delivered to the user
4. **Send message**: User can send messages to other users
5. **Real-time delivery**: If receiver is online, message is delivered immediately
6. **Offline handling**: Messages are stored and delivered when user comes online

## Error Handling

Common error scenarios:
- **User not authenticated**: Ensure JWT token is valid
- **Receiver doesn't exist**: Verify the receiver ID is correct
- **Message content required**: Ensure message is not empty
- **Connection issues**: Implement automatic reconnection

## Best Practices

1. **Always handle errors**: Set up error event handlers
2. **Implement reconnection**: Use `withAutomaticReconnect()` for better UX
3. **Validate input**: Check message content and receiver ID before sending
4. **Handle offline scenarios**: Messages are stored and delivered when users come online
5. **Update UI immediately**: Show sent messages in UI before server confirmation for better UX

## Data Models

### MessageChatDTO
```typescript
interface MessageChatDTO {
    id: number;           // Unique message identifier
    senderId: number;     // ID of the user who sent the message
    receiverId: number;   // ID of the user who should receive the message
    content: string;      // Message content
    timestamp: Date;      // When the message was sent
    isRead: boolean;      // Whether the message has been read
}
```

## Security Notes

- All connections require valid JWT authentication
- Users can only access messages they're involved in
- Connection IDs are managed server-side for security
- Messages are validated before storage

## Troubleshooting

1. **Connection fails**: Check JWT token validity
2. **Messages not received**: Verify receiver ID and user authentication
3. **Chat history empty**: Ensure both users exist and have conversation history
4. **Real-time issues**: Check network connectivity and SignalR configuration 