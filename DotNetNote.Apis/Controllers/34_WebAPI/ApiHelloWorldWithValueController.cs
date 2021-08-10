using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DotNetNote.Apis.Controllers
{
    // [!]  애트리뷰트 라우팅
    [Route("api/[controller]")]
    [ApiController]
    public class ApiHelloWorldWithValueController : ControllerBase
    {
        // GET: api/<ApiHelloWorldController>
        // 보통 기본으로 전제Data를 반환
        [HttpGet]
        public IEnumerable<Value> Get()
        {
            //return new string[] { "test", "test2" };
            return new Value[] {
                new Value { Id = 1, Text = "안녕하세요" },
                new Value { Id = 2, Text = "반값습니다." },
                new Value { Id = 3, Text = "어서오세요" },
                new Value { Id = 4, Text = "고맙습니다." },
            };
        }

        // 라우트 변수
        // GET api/<ApiHelloWorldController>/5
        //[!]모델 바인딩 + 인라인 제약 조건 (:)
        //[HttpPut("{id}")]
        //[HttpPut("{id=1000}")] // 기본값셋팅
        //[HttpGet("{id?}")] //id 생략가능.
        [HttpGet("{id:int}")] //제약조건
        public IActionResult Get([FromRoute] int id, [FromQuery] string query)
        //public Value Get([FromRoute]int id, [FromQuery] string query)
        //public Value Get(int id)
        {
            //return $"넘어온 값은 {id}";
            //return new Value { Id = id, Text = $"넘어온 값은 {id}, query={query}" };
            return Ok(new Value { Id = id, Text = $"값 : {id}" });
        }

        //// POST api/<ApiHelloWorldController>
        //[HttpPost]
        ////[FromBody] Request 본문으로 받는다
        //[Produces("application/json", Type=typeof(Value))]
        //[Consumes("application/json")]
        //public IActionResult Post([FromBody] Value value)
        //{
        //    // 넘어온 데이터를 DB에 저장등등...
        //    // 완료값 리턴.
        //    return CreatedAtAction("Get", new { id=value.Id}, value);
        //}

        // POST api/<ApiHelloWorldController>
        [HttpPost]
        //[FromBody] Request 본문으로 받는다
        [Produces("application/json", Type = typeof(Value))]
        [Consumes("application/json")]  //응답은 Json 으로.
        public IActionResult Post([FromBody] Value value)
        {
            // 모델 유효성 검사
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); //400 에러 출력.
                //throw new System.InvalidOperationException("잘못되었습니다.");
            }

            // 넘어온 데이터를 DB에 저장등등...
            value.Id++;
            // 완료값 리턴.
            //return CreatedAtAction("Get", new { id = value.Id }, value);  //201 Created (추천)
            return CreatedAtAction("Get", new { id = value.Id, Value = "after Save, Return Identity Valeu(Id)" }, value);  //201 //Get Action Name
        }
    
    }

    // 포맷터 : JSON, XML
    public class Value
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Text 속성은 필수입력 값입니다.")]
        [MinLength(5)]
        public string Text { get; set; }
    }

    public class Dto
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }

    //public class Value
    //{
    //    public int Id { get; set; }
    //    public string Text { get; set; }
    //}  

    public class ApiHelloWorldDemoController : Controller
    {
        public IActionResult Index()
        {
            string html = @"
    <html>
    <head>
        <title>CORS</title>
    </head>
    <body>
        <h1>외부에 있는 Web API 호출</h1>
        <div id='print'></div>
        <script src='https://code.jquery.com/jquery-1.12.4.min.js'></script>
        <script>
            // CORS 설정 필요 (Startup.cs)
            //var API_URI = 'http://dotnetnote/....'
            var API_URI = '/api/ApiHelloWorldWithValue';
            $(function() {
                $.getJSON(API_URI, function(data) {
                    console.log(data);
                    var str = '<dl>';
                    $.each(data, function(index, entry){
                        str += '<dt>' + entry.id + '</dt>';
                        str += '<dd>' + entry.text + '</dd>';
                    });
                    str += '</dl>';
                    $('#print').html(str);
                });
                
            });
        </script>
    </body>
    </html>
    ";

            return new ContentResult()
            {
                Content = html,
                ContentType = "text/html; charset=utf-8"
            };
        }
    }
}
