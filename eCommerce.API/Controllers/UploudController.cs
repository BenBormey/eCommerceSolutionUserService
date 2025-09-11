using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        public UploadController(IWebHostEnvironment env) => _env = env;

        // DTO: key ត្រូវតែ "image"
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

            // Ensure wwwroot/uploads exists
            var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadDir = Path.Combine(webRoot, "uploads");
            Directory.CreateDirectory(uploadDir);

            // Safe unique filename
            var fileName = $"{Guid.NewGuid():N}{ext}";
            var physicalPath = Path.Combine(uploadDir, fileName);

            // Save file
            await using (var stream = System.IO.File.Create(physicalPath))
                await file.image.CopyToAsync(stream);

            // Build public URL (respect reverse proxy headers if any)
            var scheme = Request.Headers["X-Forwarded-Proto"].FirstOrDefault() ?? Request.Scheme;
            var host = Request.Headers["X-Forwarded-Host"].FirstOrDefault() ?? Request.Host.Value;

            var relativePath = $"/uploads/{fileName}";
            var url = $"{scheme}://{host}{relativePath}";

            // 👉 FRONTEND គួររក្សាទុក 'url' ឬ 'relativePath' ប៉ុណ្ណោះ
            return Ok(new
            {
                url,                 // ex: http(s)://host/uploads/xxxx.png
                relativePath,        // ex: /uploads/xxxx.png
                fileName,
                contentType = file.image.ContentType,
                size = file.image.Length
                // physicalPath // ❌ កុំប្រើ/កុំរក្សាទុកក្នុង DB
            });
        }
    }
}
