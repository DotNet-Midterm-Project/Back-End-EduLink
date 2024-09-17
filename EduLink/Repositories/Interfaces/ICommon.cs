﻿using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;

namespace EduLink.Repositories.Interfaces
{
    public interface ICommon
    {
        Task<List<WorkshopResponseDTO>> GetAllWorkshopsAsync();
        Task<ArticlesResDTO> GetAllArticlesAsync();
        //Implement this:
        Task<ArticleDetailsResDTO> GetArticleByIdAsync(int id);
        Task<ArticlesResDTO> GetArticlesByVolunteerIdAsync(int volunteerId);
        Task<List<EventContentResDTO>> GetEventContentByEventIdAsync(int eventId);
        Task<List<EventResDTO>> GetEventsByVolunteerAndCourseAsync(int volunteerId, int courseId);
        Task<List<SessionResponseDTO>> GetSessionsByEventAsync(int eventId);
        Task<MessageResDTO> EditProfile(UpdateUserReqDto user , int studentId );
    }

}
