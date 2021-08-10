using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DotNetNote.Apis.Controllers
{
    /// <summary>
    /// 응답 결과 상태 표시
    /// IActionResult( 혹은  Task<IActionResult>) 반환
    /// Ok (object) : 200 OK 성공
    /// BadRequest(ModelState) : 잘못된 요청
    /// CreatedAtAction ("Get", new {id=1234}) : 201, 새리소스 생성
    /// Content ("안녕하세요") : 텍스트 반환
    /// Json (object) : JSON 반환
    /// </summary>
    /// 
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ServiceController>/5
        //[HttpGet("{id}")]
        //[FromQuery] --> ?id=1222
        [HttpGet("{id=1000}")] //기본값
        [HttpGet("{id:int}")] //제약조건
        //public string Get([FromRoute]int id, string query)
        public IActionResult Get([FromRoute]int id, string query)
        {
            //return $"넘어온 값 : {id}, {query}";
            return Ok(new JsonFormVal { Id = id, Text = $"값 : {id} " });
        }


        // POST api/<ServiceController>
        [HttpPost]
        public IActionResult Post([FromBody] JsonFormVal value)
        {
            if (!ModelState.IsValid)
            {
                //throw new System.InvalidOperationException("잘못된 값");
                return BadRequest(ModelState); //400 BadRequest
            }
            value.Id++;
            // 데이터 저장 ,
            return CreatedAtAction("Get",new { id = value.Id }, value);
        }

       

        // PUT api/<ServiceController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] JsonFormVal value)
        {
        }

        // DELETE api/<ServiceController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    public class JsonFormVal
    {
        public int Id { get; set; }
        [MinLength(5)]
        public string Text { get; set; }
    }
}
