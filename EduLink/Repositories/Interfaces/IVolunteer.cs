﻿using EduLink.Models.DTO.Request;
using EduLink.Models.DTO.Response;

namespace EduLink.Repositories.Interfaces
{
    public interface IVolunteer
    {

        Task<List<VolunteerCourseDTO>> GetVolunteerCoursesAsync(int volunteerID);
        Task<MessageResponseDTO> AddEducationalContentAsync(EducationalContentDtoReq dto);
        Task<GetEducationalContentResponseDTO> GetEducationalContentForEachCourseAsync(int volunteerID, int courseID);
        Task<MessageResponseDTO> AddArticleAsync(ArticleDTO dto);
        Task<MessageResponseDTO> DeleteArticleAsync(int volunteerId, int articleId);
        Task<ArticleResponseDTO> GetArticleByIdAsync(int volunteerId, int articleId);
        Task<MessageResponseDTO> AddReservationAsync(AddReservationRequestDTO request);

        //
        Task<List<ReservationDtoResponse>> GetAllReservationAsync(ReservationReqDTO reservationRequest);
        Task<MessageResponseDTO> DeleteReservationAsync(DeleteReservationDTO deleteReservationRequest);
        Task<MessageResponseDTO> UpdateReservationAsync(UpdateReservationReqDTO updateReservationRequest);
        Task<MessageResponseDTO> AddWorkshopAsync(AddWorkshopReqDTO addWorkshopRequest);
        Task<MessageResponseDTO> DeleteWorkshopAsync(DeleteWorkshopReqDTO deleteWorkshopRequest);
        Task<List<WorkshopResDTO>> GetAllWorkshopsAsync(GetAllWorkshopsReqDTO getAllWorkshopsRequest);
        Task<List<GetWorkshopNotificationRespDTO>> GetWorkshopNotificationsAsync();
    }
}
