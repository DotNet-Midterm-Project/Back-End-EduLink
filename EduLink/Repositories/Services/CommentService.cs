using EduLink.Data;
using EduLink.Models;
using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;
using EduLink.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduLink.Repositories.Services
{
    public class CommentService : IComment
    {
        private readonly EduLinkDbContext _context;


        public CommentService(EduLinkDbContext context)
        {
            _context = context;
        }
        public async Task<CommentResDto> AddCommentAsync(AddCommentReqDto dto, string userId)
        {
            var comment = new Comment()
            {
                CommentText = dto.CommentText,
                CreatedAt = DateTime.Now,
                ArticleID = dto.ArticleID,
                UserID = userId
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return new CommentResDto
            {

                CommentID = comment.CommentID,
                UserId = userId,
                ArticleID = comment.ArticleID,
                CommentText = comment.CommentText,
                CreatedAt = comment.CreatedAt
            };
        }
        


        public async Task<IEnumerable<CommentResDto>> GetAllCommentsAsync(int articleId)
        {
            return await _context.Comments
                .Where(c => c.ArticleID == articleId)
                .Select(c => new CommentResDto
                {
                    CommentID = c.CommentID,
                    ArticleID = c.ArticleID,
                    UserId = c.UserID,
                    CommentText = c.CommentText,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();
        }
        public async Task<CommentResDto> GetCommentByIdAsync(int commentId)
        {
            
                
                var comment = await _context.Comments
                .Where(c => c.CommentID == commentId)
                .Select(c => new CommentResDto
                {
                    CommentID = c.CommentID,
                    ArticleID = c.ArticleID,
                    UserId = c.UserID,
                    CommentText = c.CommentText,
                    CreatedAt = c.CreatedAt
                })
                .FirstOrDefaultAsync();

            return comment;
        }
        public async Task UpdateCommentAsync(UpdateCommentReqDto dto)
        {
            var comment = await _context.Comments.FindAsync(dto.CommentId);

            if (comment == null)
            {
                throw new KeyNotFoundException("Comment not found.");
            }

            comment.CommentText = dto.CommentText;

            _context.Entry(comment).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
        public async Task DeleteCommentAsync(int commentId)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.CommentID == commentId);

            _context.Comments.Remove(comment);

            await _context.SaveChangesAsync();
        }

        
    }
}
