﻿
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

namespace EduLink.Models.DTO.Request
{
    public class AddArticleReqDTO
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [MaxLength(2000)]
        public string ArticleContent { get; set; }

        [Required]
        public DateTime PublicationDate { get; set; }
        public IFormFile? UploadFile { get; set; }

    }
}
