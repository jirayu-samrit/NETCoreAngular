using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NETCoreAngular.API.DTOs;
using NETCoreAngular.API.Entities;
using NETCoreAngular.API.Extensions;
using NETCoreAngular.API.Heplers;
using NETCoreAngular.API.Interfaces;

namespace NETCoreAngular.API.Controllers;
public class MessagesController : BaseApiController
{
    private readonly IUserReposioty _userReposioty;
    private readonly IMessageRepository _messageRepository;
    private readonly IMapper _mapper;

    public MessagesController(IUserReposioty userReposioty, IMessageRepository messageRepository
        , IMapper mapper)
    {
        _userReposioty = userReposioty;
        _messageRepository = messageRepository;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
    {
        var username = User.GetUsername();
        if (username == createMessageDto.RecipientUsername.ToLower())
            return BadRequest("You Cannot send messages to yourself");

        var sender = await _userReposioty.GetUserByUsernameAsync(username);
        var recipient = await _userReposioty.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

        if (recipient == null) return NotFound();

        var message = new Message
        {
            Sender = sender,
            Recipient = recipient,
            SenderUsername = username,
            RecipientUsername = recipient.UserName,
            Content = createMessageDto.Content
        };

        _messageRepository.AddMessage(message);

        if (await _messageRepository.SaveAllAsync())
            return Ok(_mapper.Map<MessageDto>(message));
        else return BadRequest("Fail to send message");
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser([FromQuery] MessageParams messageParams)
    {
        messageParams.Username = User.GetUsername();

        var message = await _messageRepository.GetMessagesForUser(messageParams);

        Response.AddPaginationHeader(new PaginationHeader(message.CurrentPage, message.PageSize, message.TotalCount, message.TotalPages));

        return message;
    }

    [HttpGet("thread/{username}")]
    public async Task<ActionResult<MessageDto>> GetMessageThread(string username)
    {
        var currentUserName = User.GetUsername();

        return Ok(await _messageRepository.GetMessageThread(currentUserName, username));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMessage(int id)
    {
        var username = User.GetUsername();
        var message = await _messageRepository.GetMessage(id);

        if(message.SenderUsername != username && message.RecipientUsername != username)
        {
            return Unauthorized();
        }

        if(message.SenderUsername == username)
        {
            message.SenderDeleted = true;
        }
        if(message.RecipientUsername == username)
        {
            message.RecipientDeleted = true;
        }

        if(message.SenderDeleted && message.RecipientDeleted)
        {
            _messageRepository.DeleteMessage(message);
        }

        if(await _messageRepository.SaveAllAsync()) return Ok();
        return BadRequest("Problem deleting the message");
    }
}