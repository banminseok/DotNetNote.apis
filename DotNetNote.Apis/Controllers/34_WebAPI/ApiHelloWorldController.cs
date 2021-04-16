using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DotNetNote.Apis.Controllers
{
    // [!]  애트리뷰트 라우팅
    [Route("api/[controller]")]
    [ApiController]
    public class ApiHelloWorldController : ControllerBase
    {
        // GET: api/<ApiHelloWorldController>
        // 보통 기본으로 전제Data를 반환
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "안녕!", "반가워" };            
        }

        // 라우트 변수
        // GET api/<ApiHelloWorldController>/5
        //1[HttpGet("{id}")]
        //[!]모델 바인딩 + 인라인 제약 조건 (:)
        [HttpGet("{id:int}")]
        public string Get(int id)
        {
            return $"넘어온 값은 {id}";
        }

        // POST api/<ApiHelloWorldController>
        [HttpPost]
        //public void Post([FromBody] string value)
        //[FromBody] Request 본문으로 받기
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ApiHelloWorldController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ApiHelloWorldController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
