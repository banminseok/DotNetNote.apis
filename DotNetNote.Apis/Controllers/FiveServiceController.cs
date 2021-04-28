using DotNetNote.Apis.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetNote.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FiveServiceController : Controller
    {
        private readonly IFiveRepository _repository;

        public FiveServiceController(IFiveRepository repository)
        {
            _repository = repository;
        }
        public IActionResult Get()
        {
            try
            {
                IEnumerable<FiveViewModel> fives = _repository.GetAll();
                if (fives == null)
                {
                    return NotFound($"아무런 데이터가 없습니다.");
                }
                return Ok(fives);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Produces("application/json", Type = typeof(FiveViewModel))]
        [Consumes("application/json")]  // application/xml
        public IActionResult Post([FromBody] FiveViewModel model)
        {
            try
            {
                if (model.Note == null || model.Note.Length < 1)
                {
                    ModelState.AddModelError("Note", "노트를 입력해야 합니다.");
                }
                //모델 유효성 검사
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var m = _repository.Add(model);

                if (DateTime.Now.Second % 2 == 0) //[!] 둘 중 원하는 방식 사용
                {
                    // GetById 액션 이름 사용해서 입력된 데이터 반환 
                    //return CreatedAtAction("GetById", new { id = m.Id }, m);
                    return CreatedAtRoute("GetFiveById", new { id = m.Id }, m); // Status: 201 Created
                }
                else
                {
                    var uri = Url.Link("GetFiveById", new { id = m.Id });
                    return Created(uri, m); // 201 Created
                }
                //return Ok(m); //200
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
