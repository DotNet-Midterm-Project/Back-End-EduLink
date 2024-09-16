using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Repositories.Interfaces;
using EduLink.Repositories.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduLink.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentAndLikeController : ControllerBase
    {
        private readonly IComment _comment;

        public CommentAndLikeController(IComment commentService)
        {
            _comment = commentService;
        }

        // POST: api/Comment/add-comment
        [Authorize]
        [HttpPost("add-comment")]
        public async Task<ActionResult<CommentResDto>> AddComment(AddCommentReqDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }
            // Get the user ID from the token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            var createdComment = await _comment.AddCommentAsync(dto, userId);

            return Ok(createdComment);
        }
        [Authorize]
        [HttpPost("add-like/{articleId}")]
        public async Task<IActionResult> LikeArticle(int articleId)
        {
            // Get the userId from the claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            // Call the service method with the articleId and userId
            await _comment.LikeArticleAsync(articleId, userId);
            return Ok("Like added successfully");
        }
        // GET: api/Comment/get-comment-by-id/{commentId}
        [Authorize]
        [HttpGet("get-comment-by-id/{commentId}")]
        public async Task<ActionResult<CommentResDto>> GetCommentById(int commentId)
        {
            var comment = await _comment.GetCommentByIdAsync(commentId);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }

        // GET: api/Comments/get-comments-by-article/{articleId}
        [Authorize]
        [HttpGet("get-comments-by-article/{articleId}")]
        public async Task<ActionResult<IEnumerable<CommentResDto>>> GetCommentsByArticle(int articleId)
        {
            var comments = await _comment.GetAllCommentsAsync(articleId);
            return Ok(comments);
        }

        // PUT: api/Comment/update-comment
        [Authorize]
        [HttpPut("update-comment")]
        public async Task<IActionResult> UpdateComment(UpdateCommentReqDto dto)
        {
            await _comment.UpdateCommentAsync(dto);
            return Ok(dto);
        }

        // DELETE: api/Comment/delete-comment/5
        [Authorize]
        [HttpDelete("delete-comment/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        { 
            await _comment.DeleteCommentAsync(commentId);
            return Ok("Comment deleted successfully");
        }



       
    }
}
