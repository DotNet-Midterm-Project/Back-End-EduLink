namespace EduLink.Models.DTO.Response
{
    public class EducationalContentDtoResponse
    {
        public int CourseID { get; set; }
        public int VolunteerID { get; set; }
        public string ContentType { get; set; }
        public string ContentDescription { get; set; }
       
    }
    public class GetEducationalContentResponseDTO
    {
        public List<EducationalContentDtoResponse> EducationalContents { get; set; }
    }
    
}
