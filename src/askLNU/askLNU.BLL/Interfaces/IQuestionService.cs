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

        public int GetIdByFacutyName(string name);
        public IEnumerable<FacultyDTO> GetAllFaculties();
        public IEnumerable<string> GetTagsByQuestionID(int? id);
        void AddTag(int questionId, int tagId);
        int VoteUp(string userId, int questionId);
        int VoteDown(string userId, int questionId);
        void AddAnswer(int questionId, AnswerDTO answer);
    }
}
