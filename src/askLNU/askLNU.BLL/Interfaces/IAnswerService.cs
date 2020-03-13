using askLNU.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace askLNU.BLL.Interfaces
{
    public interface IAnswerService
    {
        public void CreateAnswer(AnswerDTO answerDto);
        public AnswerDTO GetAnswer(int? id);
        public IEnumerable<AnswerDTO> GetAll();
        public void Dispose();
    }
}
