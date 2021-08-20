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

        [HttpGet]
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

        [HttpGet("{id:int}", Name="GetById")]   // GetById Web API 이름추가 (post에서 (GetById로) 호출 가능)
        public IActionResult Get(int id)
        {
            ////https://localhost:44367/api/FiveService/1111
            ////https://localhost:44367/api/GetById
            try
            {
                FiveViewModel model = _repository.GetById(id);
                if (model == null)
                {
                    return NotFound($"아무런 데이터가 없습니다. ({id}번)");
                }
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// 공식화된 Post 코드
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json", Type = typeof(FiveViewModel))] // 받는 인자 정의
        [Consumes("application/json")]  // application/xml   // 출력 포멧 정의
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
                    return BadRequest(ModelState);  // 400 에러 출력.
                }

                var m = _repository.Add(model);

                if (DateTime.Now.Second % 2 == 0) //[!] 둘 중 원하는 방식 사용
                {
                    // GetById 액션 이름 사용해서 입력된 데이터 반환 
                    //return CreatedAtAction("GetById", new { id = m.Id }, m); // Status: 201 Created
                    return CreatedAtRoute("GetById", new { id = m.Id }, m); // Status: 201 Created
                }
                else
                {
                    var uri = Url.Link("GetById", new { id = m.Id });
                    return Created(uri, m); // 201 Created
                }
                //return Ok(m); //200
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// https://localhost:44367/api/FiveService/1033
        /// Put --> Body : {    "Id": 1033,    "Note": "수정 - 21년 8월 17일 복습입니다.( 6 )"}
        /// </summary>
        [HttpPut("{id:int}")]   //HttpPatch=== 부분 업데이트
        public IActionResult Put(int id, [FromBody] FiveViewModel model) 
        {
            if (model==null)
            {
                return BadRequest();
            }
            try
            {
                var oldModel = _repository.GetById(id);
                if(oldModel ==null)
                {
                    return NotFound($"{id}번 데이터가 없습니다.");
                }
                model.Id = id;
                _repository.Update(model);
                //return Ok(model);
                return NoContent(); //204 No Content  이미 던져준 정보에 모든 값을 가지고 있어서 ...
            }
            catch (Exception)
            {

                return BadRequest("데이터가 업데이트 되지 않았습니다."); 
            }
        }

        [HttpDelete("{id:int}")]  //데코레이터 특성
        public IActionResult Delete(int id)
        {
            try
            {
                var oldModel = _repository.GetById(id);
                if (oldModel == null)
                {
                    return NotFound($"{id}번 데이터가 없습니다.");
                }
                _repository.Remove(id);
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest("데이터가 삭제 되지 않았습니다.");
                //NotFound
            }
        }

        [HttpGet("page/{pageNumber=1}/{pageSize=10}")] //기본값
        [HttpGet("page/{pageNumber:int}/{pageSize:int}")]  // 이름추가 
        public IActionResult Get(int pageNumber=1, int pageSize = 10)
        {
            //https://localhost:44367/api/FiveService/page/2/5
            try
            {
                var fives = _repository.GetAllWithPaging(pageNumber-1, pageSize);
                if(fives == null)
                {
                    return NotFound($"아무런 데이터가 없습니다.");
                }

                //응답 헤더에 총 레코드 수를 담아서 출력
                Response.Headers.Add("X-TotalRecordCount", _repository.GetRecordCount().ToString());
                return Ok(fives); //200
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
