using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Cmd.Api.DTOs;
using Post.Commen.DTOs;

namespace Post.Cmd.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly ILogger<PostController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public PostController(ILogger<PostController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<ActionResult> NewPostAsync(NewPostCommand command)
        {
            var id = Guid.NewGuid();
            try
            {
                command.Id = id;

                await _commandDispatcher.SendAsync(command);

                return StatusCode(StatusCodes.Status201Created, new NewPostResponse
                {
                    Message = "New post created"
                });
            }
            catch (InvalidOperationException ex)
            {

                _logger.Log(LogLevel.Warning, ex, "client made a bad request!");
                return BadRequest(new BaseReponse
                {
                    Message = ex.Message,
                });
            }
            catch (Exception ex)
            {
                const string Safe_Error_Message = "Error while processing request to create a new post!";
                _logger.Log(LogLevel.Error, Safe_Error_Message, ex);

                return StatusCode(StatusCodes.Status500InternalServerError, new NewPostResponse
                {
                    Id = id,
                    Message = Safe_Error_Message
                });
            }
        }

        [HttpPut("addcomment/id")]
        public async Task<ActionResult> AddCommentAsync(Guid id, AddCommentCommand command)
        {
            try
            {
                command.Id = id;

                await _commandDispatcher.SendAsync(command);

                return StatusCode(StatusCodes.Status201Created, new NewPostResponse
                {
                    Message = "Comment has been added"
                });
            }
            catch (Exception ex)
            {
                const string Safe_Error_Message = "Error while processing request, to add comment!";
                _logger.Log(LogLevel.Error, Safe_Error_Message, ex);

                return StatusCode(StatusCodes.Status500InternalServerError, new NewPostResponse
                {
                    Id = id,
                    Message = Safe_Error_Message
                });
            }
        }

        [HttpPut("likePost/id")]
        public async Task<ActionResult> LikePost(Guid id, LikePostCommand command)
        {
            try
            {
                command.Id = id;

                await _commandDispatcher.SendAsync(command);

                return StatusCode(StatusCodes.Status201Created, new NewPostResponse
                {
                    Message = "Post has been liked"
                });
            }
            catch (Exception ex)
            {
                const string Safe_Error_Message = "Error while processing request, to like post!";
                _logger.Log(LogLevel.Error, Safe_Error_Message, ex);

                return StatusCode(StatusCodes.Status500InternalServerError, new NewPostResponse
                {
                    Id = id,
                    Message = Safe_Error_Message
                });
            }
        }
    }
}
