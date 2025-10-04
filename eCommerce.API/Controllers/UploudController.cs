using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        public UploadController(IWebHostEnvironment env) => _env = env;

      
        public class UploadDto
        {
            public IFormFile? image { get; set; }
        }

        [HttpPost("image")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(10_000_000)] // 10 MB
        public async Task<IActionResult> UploadImage([FromForm] UploadDto file)
        {
            if (file?.image == null || file.image.Length == 0)
                return BadRequest(new { error = "No file uploaded" });

            var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var ext = Path.GetExtension(file.image.FileName).ToLowerInvariant();
            if (!allowed.Contains(ext))
                return BadRequest(new { error = "Invalid file type" });


            var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadDir = Path.Combine(webRoot, "uploads");
            Directory.CreateDirectory(uploadDir);


            var fileName = $"{Guid.NewGuid():N}{ext}";
            var physicalPath = Path.Combine(uploadDir, fileName);

            await using (var stream = System.IO.File.Create(physicalPath))
                await file.image.CopyToAsync(stream);


            var scheme = Request.Headers["X-Forwarded-Proto"].FirstOrDefault() ?? Request.Scheme;
            var host = Request.Headers["X-Forwarded-Host"].FirstOrDefault() ?? Request.Host.Value;

            var relativePath = $"/uploads/{fileName}";
            var url = $"{scheme}://{host}{relativePath}";


            return Ok(new
            {
                url,                
                relativePath,       
                fileName,
                contentType = file.image.ContentType,
                size = file.image.Length
              
            });
        }
    }
}
