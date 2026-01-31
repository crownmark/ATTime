using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using static CrownATTime.Server.Models.EmailMessage;

namespace CrownATTime.Server.Controllers
{
    public partial class UploadController : Controller
    {
        private readonly IWebHostEnvironment environment;

        public UploadController(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        // Single file upload
        [HttpPost("upload/single")]
        public IActionResult Single(IFormFile file)
        {
            try
            {
                // Put your code here
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Multiple files upload
        [HttpPost("upload/multiple")]
        public IActionResult Multiple(IFormFile[] files)
        {
            try
            {
                // Put your code here
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Multiple files upload with parameter
        [HttpPost("upload/{id}")]
        public IActionResult Post(IFormFile[] files, int id)
        {
            try
            {
                // Put your code here
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Image file upload (used by HtmlEditor components)
        [HttpPost("upload/image")]
        public IActionResult Image(IFormFile file)
        {
            try
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                using (var stream = new FileStream(Path.Combine($"{environment.WebRootPath}\\EmailImages\\", fileName), FileMode.Create))
                {
                    // Save the file
                    file.CopyTo(stream);

                    // Build absolute URL (works in IIS, HTTPS, reverse proxy, sub-path)
                    var request = HttpContext.Request;

                    var baseUrl =
                        $"{request.Scheme}://{request.Host}{request.PathBase}";

                    var imageUrl = $"{baseUrl}/EmailImages/{fileName}";

                    return Ok(new { Url = imageUrl });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Multiple files upload for email attachment
        [HttpPost("upload/EmailAttachments")]
        public List<IFormFileModel> EmailAttachments(IFormFile[] files)
        {
            var attachments = new List<IFormFileModel>();
            try
            {
                // Put your code here

                foreach (var file in files)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        var newFile = new IFormFileModel();
                        newFile.ByteArray = fileBytes;
                        newFile.ContentDisposition = file.ContentDisposition;
                        newFile.ContentType = file.ContentType;
                        newFile.FileName = file.FileName;
                        //newFile.Headers = (Models.Headers)file.Headers;
                        newFile.Length = file.Length;
                        newFile.Name = file.Name;
                        attachments.Add(newFile);
                    }
                }
                return attachments;
                //return StatusCode(200);
            }
            catch (Exception ex)
            {
                return attachments;
                //return StatusCode(500, ex.Message);
            }
        }
    }
}
