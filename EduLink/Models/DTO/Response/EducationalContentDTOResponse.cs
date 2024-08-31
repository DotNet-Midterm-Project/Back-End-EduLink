namespace EduLink.Models.DTO.Response
{
    public class EducationalContentDTOResponse
    {
        public string ContentType { get; set; }
        public string ContentDescription { get; set; }
    }
    public class GetEducationalContentResponseDTO
    {
        public List<EducationalContentDTOResponse> EducationalContents { get; set; }
    }
}
