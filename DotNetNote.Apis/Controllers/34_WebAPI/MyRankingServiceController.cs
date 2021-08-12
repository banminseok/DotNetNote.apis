using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetNote.Apis.Controllers
{
    [Route("api/[controller]")]
    public class MyRankingServiceController : Controller
    {
        //[1] 객체전달
        //[HttpGet]
        //public Subject Get()
        //{
        //    return new Subject { Kor=95, Eng=80, Total=195 };
        //}

        //[2] 컬렉션형태전달
        //[HttpGet]
        //public List<Student> Get()
        //{
        //    var students = new List<Student> {
        //        new Student {Id=1, Name="홍길동", Score=3},
        //        new Student {Id=2, Name="임덕표", Score=2},
        //        new Student {Id=3, Name="황현희", Score=1},
        //    };

        //    return students;
        //}


        //[3] 복합형식(complex Type : 하나 이상의 다른 개체를 포함) 으로 전달
        [HttpGet]
        public MyRankingDto Get()
        {
            var subject = new Subject { Kor = 95, Eng = 80, Total = 195 };
            var students = new List<Student> {
                new Student {Id=1, Name="홍길동", Score=3},
                new Student {Id=2, Name="임덕표", Score=2},
                new Student {Id=3, Name="황현희", Score=1},
            };

            return new MyRankingDto { Subject = subject, Students = students };
        }
        
    }


    /// <summary>
    /// 과목
    /// </summary>
    public class Subject
    {
        public int Kor { get; set; }
        public int Eng { get; set; }
        public int Total { get; set; }
    }

    /// <summary>
    /// 학생
    /// </summary>
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
    }

    /// <summary>
    /// 과목+학생들
    /// </summary>
    public class MyRankingDto
    {
        public Subject Subject { get; set; }
        public List<Student> Students { get; set; }
    }

    public class MyRankingServiceTestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
