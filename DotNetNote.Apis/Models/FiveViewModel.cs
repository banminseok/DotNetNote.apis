using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Dapper;
using System.Linq;

namespace DotNetNote.Apis.Models
{
    /// <summary>
    /// 모델 클래서 Fives Table 매칭
    /// </summary>
    public class FiveViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Note { get; set; }
    }

    public interface IFiveRepository
    {
        FiveViewModel Add(FiveViewModel model);
        List<FiveViewModel> GetAll();
        FiveViewModel GetById(int id);
        FiveViewModel Update(FiveViewModel model);
        void Remove(int id);

        List<FiveViewModel> GetAllWithPaging(int pageIndex, int pageSize =10);
    }

    /// <summary>
    /// Repository Class 구현
    /// </summary>
    public class FiveRepository : IFiveRepository
    {
        private readonly IDbConnection _db;

        public FiveRepository(string connectionString)
        {
            _db = new SqlConnection(connectionString);
        }

        /// <summary>
        /// 입력 메서드
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FiveViewModel Add(FiveViewModel model)
        {
            string sql = @"
                Insert into Fives (Note) Values (@Note); 
                Select Cast (SCOPE_IDENTITY() As Int);
            ";
            var id = _db.Query<int>(sql, new { Note = model.Note }).SingleOrDefault();
            model.Id = id;
            return model;
        }

        /// <summary>
        /// 출력 메서드
        /// </summary>
        /// <returns></returns>
        public List<FiveViewModel> GetAll()
        {
            string sql = $"Select Id, Note From Fives Order By Id Desc";
            return _db.Query<FiveViewModel>(sql).ToList();
        }

        public List<FiveViewModel> GetAllWithPaging(int pageIndex, int pageSize = 10)
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// 상세보기
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FiveViewModel GetById(int id)
        {
            string sql = "Select * From Fives Where Id=@Id";
            return _db.Query<FiveViewModel>(sql, new { Id = id }).SingleOrDefault();
        }

        public void Remove(int id)
        {
            throw new System.NotImplementedException();
        }

        public FiveViewModel Update(FiveViewModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}
