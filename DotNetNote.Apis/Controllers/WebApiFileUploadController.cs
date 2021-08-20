using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DotNetNote.Apis.Controllers
{
    [Route("api/[controller]")]
    public class WebApiFileUploadController : ControllerBase
    {
        private IWebHostEnvironment _environment;   // webroot folder 추출

        public WebApiFileUploadController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        /// <summary>
        /// files 매개변수 이름은 <input type="files" name="files"/>
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/json", "multipart/form-data")]
        public async Task<IActionResult> Post(List<IFormFile> files) //Microsoft.AspNetCore.Http;
        {
            //파일 업로드 폴더
            //var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "");
            var uploadFolder = Path.Combine(_environment.WebRootPath, "files");
            foreach (var file in files)
            {
                if (file.Length>0)
                {
                    //Trim('"')-->  " 문자를 제거
                    var fileName = Path.Combine(ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"'));
                    using (var fileStream = new FileStream(Path.Combine(uploadFolder, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }
            return Ok(new { message = "OK" });
        }

        
    }
}
