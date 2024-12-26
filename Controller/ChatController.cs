using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

public class ChatMessage
{

    public string User { get; set; }
    public string Message { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IHubContext<ChatHub> _chatHubContext;
    public ChatController(IHubContext<ChatHub> chatHubContext)
    {
        _chatHubContext = chatHubContext;
    }
    [HttpPost]
    [Route("send")]
    public async Task<IActionResult> SendMessage(ChatMessage chatMessage)
    {
        if (string.IsNullOrWhiteSpace(chatMessage.Message) || string.IsNullOrEmpty(chatMessage.User))
        {
            return BadRequest("Invalid message or username");
        }
        await _chatHubContext.Clients.All.SendAsync("ReceiveMessage", chatMessage.User, chatMessage.Message);
        return Ok();

    }
}