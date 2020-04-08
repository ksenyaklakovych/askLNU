using askLNU.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace askLNU.BLL.Interfaces
{
    public interface IFacultyService
    {
        void CreateFaculty(FacultyDTO Dto);
        FacultyDTO GetFaculty(int? id);
        IEnumerable<FacultyDTO> GetAll();
        void Dispose(int id);
        public int GetFacultyIdByName(string name);

    }
}
