using askLNU.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace askLNU.BLL.Interfaces
{
    public interface IQuestionService
    {
        public void CreateQuestion(QuestionDTO qDto);
        public QuestionDTO GetQuestion(int? id);
        public IEnumerable<QuestionDTO> GetAll();
        public void Dispose();

        public IEnumerable<FacultyDTO> GetAllFaculties();
        public IEnumerable<string> GetTagsByQuestionID(int? id);
        public void AddToFavorites(string userId, int questionId);
        public bool IsQuestionFavorite(string userId, int questionId);
        public void RemoveFromFavorites(string userId, int questionId);
    }
}
