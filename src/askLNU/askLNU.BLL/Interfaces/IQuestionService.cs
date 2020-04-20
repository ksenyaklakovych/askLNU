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
        
        public void Dispose(int questionId);
        
        public IEnumerable<string> GetTagsByQuestionID(int? id);
        
        public void AddToFavorites(string userId, int questionId);
        
        public bool IsQuestionFavorite(string userId, int questionId);
        
        public void RemoveFromFavorites(string userId, int questionId);
        
        void AddTag(int questionId, int tagId);
        
        int VoteUp(string userId, int questionId);
        
        int VoteDown(string userId, int questionId);
        
        void AddAnswer(int questionId, AnswerDTO answer);
    }
}
