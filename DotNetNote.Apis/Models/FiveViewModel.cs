using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Dapper;
using System.Linq;

namespace DotNetNote.Apis.Models
{
    /// <summary>
    /// 모델 클래스 Fives Table 매칭
    /// </summary>
    public class FiveViewModel
    {
        public int Id { get; set; }

        [Required] //Not Null
        public string Note { get; set; }
    }

    /// <summary>
    /// 인터페이스
    /// </summary>
    public interface IFiveRepository
    {
        //입력
        FiveViewModel Add(FiveViewModel model);
        //출력
        List<FiveViewModel> GetAll();
        FiveViewModel GetById(int id);
        //수정
        FiveViewModel Update(FiveViewModel model);
        //삭제
        void Remove(int id);

        // 페이징 처리 
        List<FiveViewModel> GetAllWithPaging(int pageIndex, int pageSize =10);
        int GetRecordCount();
    }

    /// <summary>
    /// Repository Class 구현
    /// </summary>
    public class FiveRepository : IFiveRepository
    {
        private readonly IDbConnection _db;

        public FiveRepository(string connectionString)
        {
            _db = new SqlConnection(connectionString); //Microsoft.Data.SqlClient
        }

        /// <summary>
        /// 입력 메서드
        /// </summary>
        /// <param name="model">FiveViewModel</param>
        /// <returns>FiveViewModel</returns>
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
            string sql = @"
                Select Id, Note 
                From (
                    Select Row_Number() Over (Order By Id Desc) As RowNumber, Id, Note
                    From Fives                    
                ) As TempRowTables
                Where 
                    RowNumber Between 
                        (@PageIndex * @PageSize +1)
                    And
                        (@PageIndex + ) * @PageSize
                Order By Id Desc
            ";
            sql = @"
                Select Row_Number() Over (Order By Id Desc) As RowNumber, Id, Note 
                From Fives
                Order By Id Desc
                offset (@PageIndex)*@PageSize rows
                fetch next @PageSize rows only  
               
            ";
            return _db.Query<FiveViewModel>(sql, new { pageIndex = pageIndex, pageSize=pageSize}).ToList();
        }

        /// <summary>
        /// 상세보기
        /// </summary>
        /// <returns></returns>
        public FiveViewModel GetById(int id)
        {
            string sql = "Select * From Fives Where Id=@Id";
            return _db.Query<FiveViewModel>(sql, new { Id = id }).SingleOrDefault();  //.Single()
        }

        /// <summary>
        /// 레코드 카운트
        /// </summary>
        /// <returns></returns>
        public int GetRecordCount()
        {
            string sql = @"Select Count(*) From Fives";
            return _db.Query<int>(sql).FirstOrDefault();
        }

        /// <summary>
        /// 삭제...
        /// </summary>
        public void Remove(int id)
        {
            var sql = @"
                Delete From Fives Where Id=@Id
            ";
            _db.Execute(sql, new { Id = id });
        }

        /// <summary>
        /// 수정
        /// </summary>
        public FiveViewModel Update(FiveViewModel model)
        {
            var sql = @"Update Fives 
                        Set Note= @Note
                        Where Id = @Id
            ";
            _db.Execute(sql, model);
            return model;
        }
    }
}
