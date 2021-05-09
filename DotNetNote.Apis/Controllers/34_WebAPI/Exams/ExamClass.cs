using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Linq;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace DotNetNote.Apis.Models
{
    public class ExamClass
    {
    }

    /// <summary>
    /// 모델 클래스
    /// </summary>
    public class Question
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
    }


    /// <summary>
    /// 인터페이스
    /// </summary>
    public interface IQuestionRepository
    {
        Question Add(Question model);
        List<Question> GetAll();
        Question GetById(int id);
        Question Update(Question model);
        void Remove(int id);

        List<Question> GetAllWithPaging(int pageIndex, int pageSize = 10);
        int GetRecordCount();
    }


    /// <summary>
    /// 리포지토리 클래스
    /// </summary>
    public class QuestionRepository : IQuestionRepository
    {
        private IConfiguration _config;
        private IDbConnection _db;


        public QuestionRepository(IConfiguration config)
        {
            _config = config;
            _db = new SqlConnection(_config.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);
        }

        /// <summary>
        /// 입력 메서드 
        /// </summary>
        public Question Add(Question model)
        {
            string sql = @"
                Insert Into Questions (Title) Values (@Title);
                Select Cast(SCOPE_IDENTITY() As Int);
            ";
            var id = _db.Query<int>(sql, model).Single();
            model.Id = id;
            return model;
        }

        /// <summary>
        /// 출력 메서드: GetAll, GetQuestions 
        /// </summary>
        public List<Question> GetAll()
        {
            string sql = "Select * From Questions Order By Id Desc";
            return _db.Query<Question>(sql).ToList();
        }
               
        /// <summary>
        /// 수정 메서드 
        /// </summary>
        public Question Update(Question model)
        {
            var query =
                "Update Questions           " +
                "Set                        " +
                "   Title = @Title          " +
                "Where Id = @Id             ";
            _db.Execute(query, model);
            return model;
        }

        /// <summary>
        /// 삭제 메서드
        /// </summary>
        public void Remove(int id)
        {
            var query = "Delete From Questions Where Id = @Id";
            _db.Execute(query, new { Id = id });
        }

        /// <summary>
        /// 상세 메서드
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Question GetById(int id)
        {
            string query = "Select * From Questions Where Id = @Id";
            return _db.Query<Question>(query, new { Id = id }).Single();
        }

        /// <summary>
        /// 레코드 카운트 반환 메서드
        /// </summary>
        public int GetRecordCount()
        {
            string query = "Select Count(*) From Questions";
            return _db.Query<int>(query).FirstOrDefault();
        }


        /// <summary>
        /// 페이징 처리된 리스트 출력 메서드
        /// </summary>
        public List<Question> GetAllWithPaging(int pageIndex, int pageSize = 10)
        {
            string sql = @"
                Select Id, Title
                From 
                    (
                        Select Row_Number() Over (Order By Id Desc) As RowNumbers, Id, Title
                        From Questions
                    ) As TempRowTables
                Where 
                    RowNumbers
                        Between
                            (@PageIndex * @PageSize + 1)
                        And
                            (@PageIndex + 1) * @PageSize
            ";
            return _db.Query<Question>(sql, new { PageIndex = pageIndex, PageSize = pageSize }).ToList();
        }
    }


    /// <summary>
    /// DTO 클래스
    /// </summary>
    public class QuestionDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(4000, ErrorMessage ="문제는 4000자 이하로 입력하세요")]
        public string Title { get; set; }
    }

    /// <summary>
    /// WebAPI 컨트롤러 클래스
    /// 컨벤션 기반 라우팅 대신에 어튜리뷰트 라우팅 추천
    /// </summary>
    //[Route("api/questions")]
    [Route("api/[controller]")] //api/QuestionService
    public class QuestionServiceController : Controller
    {
        private IQuestionRepository _repository;

        public QuestionServiceController(IQuestionRepository repository)
        {
            _repository = repository;
        }

        // GET: /api/QuestionService 
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(List<Question>), 200)]
        public IActionResult Get()
        {
            // 500 에러 찍어보려면,
            // throw new Exception("인위적으로 에러 발생시켜 500에러 출력");
            try
            {
                var models = _repository.GetAll();
                if (models == null)
                {
                    return NotFound("아무런 데이터가 없습니다.");
                }
                return Ok(models); // 200 
            }
            catch (Exception ex)
            {
                //_logger.LogError($"에러 발생: {ex.Message}");
                return BadRequest();
            }
        }
    }
}
