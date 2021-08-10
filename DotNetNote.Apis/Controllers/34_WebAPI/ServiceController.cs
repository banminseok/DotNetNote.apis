using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DotNetNote.Apis.Controllers
{
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
        public string Get([FromRoute]int id, string query)
        {
            return $"넘어온 값 : {id}, {query}";
        }

        // POST api/<ServiceController>
        [HttpPost]
        public void Post([FromBody] JsonFormVal value)
        {
            if (!ModelState.IsValid)
            {
                throw new System.InvalidOperationException("잘못된 값");
            }
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
