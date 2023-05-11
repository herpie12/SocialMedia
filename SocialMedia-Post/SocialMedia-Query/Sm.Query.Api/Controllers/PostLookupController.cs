using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Commen.DTOs;
using Sm.Query.Api.DTOs;
using Sm.Query.Api.Queries;
using Sm.Query.Domain.Entities;

namespace Sm.Query.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PostLookupController : ControllerBase
    {
        private readonly ILogger<PostLookupController> _logger;
        private IQueryDispatcher<PostEntity> _queryDispatcher;

        public PostLookupController(ILogger<PostLookupController> logger, IQueryDispatcher<PostEntity> queryDispatcher)
        {
            _logger = logger;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllPostAsync()
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindAllPostsQuery());

                if (posts == null || !posts.Any())
                {
                    return NoContent();
                }

                var count = posts.Count();

                return Ok(new PostLookupResponse
                {
                    Posts = posts,
                    Message = $"Successfully returned {count} post{(count > 1 ? "s" : String.Empty)}!"
                });
            }
            catch (Exception ex)
            {

                const string errmsg = "Error while processing request, retrieve all posts!";
                _logger.LogError(ex, errmsg);

                return StatusCode(500, new BaseReponse
                {
                    Message = errmsg,
                });
            }
        }

        [HttpGet("postId")]
        public async Task<ActionResult> GetPostByIdAsync(Guid postId)
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostByIdQuery { Id = postId });

                if (posts == null || !posts.Any())
                {
                    return NoContent();
                }

                return Ok(new PostLookupResponse
                {
                    Posts = posts,
                    Message = $"Successfully returned post!"
                });

            }
            catch (Exception ex)
            {

                const string errmsg = "Error while processing request, find post by Id!";
                _logger.LogError(ex, errmsg);

                return StatusCode(500, new BaseReponse
                {
                    Message = errmsg,
                });
            }
        }

        [HttpGet("getPostsWithComments")]
        public async Task<ActionResult> GetPostsWithCommentsAsync()
        {
            try
            {
                var postsWithComments = await _queryDispatcher.SendAsync(new FindPostsWithCommentsQuery());

                if (postsWithComments == null || !postsWithComments.Any())
                {
                    return NoContent();
                }

                return Ok(new PostLookupResponse
                {
                    Posts = postsWithComments,
                    Message = $"Successfully returned post with comments"
                });

            }
            catch (Exception ex)
            {

                const string errmsg = "Error while processing request, find posts with comments!";
                _logger.LogError(ex, errmsg);

                return StatusCode(500, new BaseReponse
                {
                    Message = errmsg,
                });
            }
        }

        [HttpGet("getPostsWithLikes")]
        public async Task<ActionResult> GetPostsWithLikes(int numbersOfLikes)
        {
            try
            {
                var postWithLikes = await _queryDispatcher.SendAsync(new FindPostsWithLikesQuery());

                if (postWithLikes == null || !postWithLikes.Any())
                {
                    return NoContent();
                }

                return Ok(new PostLookupResponse
                {
                    Posts = postWithLikes,
                    Message = $"Successfully returned posts with likes"
                });

            }
            catch (Exception ex)
            {

                const string errmsg = "Error while processing request, find posts with likes!";
                _logger.LogError(ex, errmsg);

                return StatusCode(500, new BaseReponse
                {
                    Message = errmsg,
                });
            }
        }
    }
}
