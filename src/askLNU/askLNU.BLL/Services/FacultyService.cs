﻿using System;
using System.Collections.Generic;
using System.Text;
using askLNU.BLL.DTO;
using askLNU.DAL.Entities;
using askLNU.DAL.Interfaces;
using askLNU.BLL.Infrastructure;
using askLNU.BLL.Interfaces;
using AutoMapper;
using askLNU.BLL.Infrastructure.Exceptions;

namespace askLNU.BLL.Services
{
    public class FacultyService : IFacultyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FacultyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void CreateFaculty(FacultyDTO facultyDTO)
        {
            if (facultyDTO != null)
            {
                Faculty faculty = _mapper.Map<Faculty>(facultyDTO);
                _unitOfWork.Faculties.Create(faculty);
                _unitOfWork.Save();
            }
            else
            {
                throw new ArgumentNullException("facultyDTO");
            }
        }

        public IEnumerable<FacultyDTO> GetAll()
        {
            var faculties = _unitOfWork.Faculties.GetAll();
            return _mapper.Map<IEnumerable<FacultyDTO>>(faculties);
        }

        public FacultyDTO GetFaculty(int? id)
        {
            if (id != null)
            {
                var faculty = _unitOfWork.Faculties.Get(id.Value);
                if (faculty != null)
                {
                    return _mapper.Map<FacultyDTO>(faculty);
                }
                else
                {
                    throw new ItemNotFoundException($"Faculty not found.");
                }
            }
            else
            {
                throw new ArgumentNullException("id");
            }
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
