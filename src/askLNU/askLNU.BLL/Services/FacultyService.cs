using System;
using System.Collections.Generic;
using System.Text;
using askLNU.BLL.DTO;
using askLNU.DAL.Entities;
using askLNU.DAL.Interfaces;
using askLNU.BLL.Infrastructure;
using askLNU.BLL.Interfaces;
using AutoMapper;

namespace askLNU.BLL.Services
{
    public class FacultyService : IFacultyService
    {
        IUnitOfWork Database { get; set; }

        public FacultyService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public void CreateFaculty(FacultyDTO Dto)
        {
            Faculty answer = Database.Faculties.Get(Dto.Id);

            if (answer == null)
                throw new ValidationException("Answer not found", "");
            Faculty a = new Faculty
            {
                Id= Dto.Id,
                Title = Dto.Title,
            };
            Database.Faculties.Create(a);
            Database.Save();
        }

        public IEnumerable<FacultyDTO> GetAll()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Faculty, FacultyDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Faculty>, List<FacultyDTO>>(Database.Faculties.GetAll());
        }

        public FacultyDTO GetFaculty(int? id)
        {
            if (id == null)
                throw new ValidationException("Id not set", "");
            var faculty = Database.Faculties.Get(id.Value);
            if (faculty == null)
                throw new ValidationException("Faculty not found", "");

            return new FacultyDTO
            {
                Id = faculty.Id,
                Title = faculty.Title,
            };
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
