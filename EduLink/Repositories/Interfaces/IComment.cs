using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;

namespace EduLink.Repositories.Interfaces
{
    public interface IComment
    {
        Task<IEnumerable<CommentResDto>> GetAllCommentsAsync(int articleId);
        Task<CommentResDto> GetCommentByIdAsync(int commentId);
        Task<CommentResDto> AddCommentAsync(AddCommentReqDto dto, string userId);
        Task UpdateCommentAsync(UpdateCommentReqDto dto);
        Task DeleteCommentAsync(int commentId);

        Task LikeArticleAsync(int articleId, string userId);

    }
}
