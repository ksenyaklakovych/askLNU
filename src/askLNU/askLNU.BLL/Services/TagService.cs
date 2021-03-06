﻿using askLNU.BLL.DTO;
using askLNU.BLL.Interfaces;
using askLNU.DAL.Entities;
using askLNU.DAL.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;


namespace askLNU.BLL.Services
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<TagService> _logger;

        public TagService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TagService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public void CreateTag(TagDTO tagDTO)
        {
            if (tagDTO != null)
            {
                var tag = _mapper.Map<Tag>(tagDTO);
                _unitOfWork.Tags.Create(tag);
                _unitOfWork.Save();
                _logger.LogInformation("Created Tag");
            }
            else
            {
                _logger.LogWarning("Error creating tag");
                throw new ArgumentNullException("tagDTO");
            }
        }

        public int FindOrCreate(string tagText)
        {
            var tag =_unitOfWork.Tags.Find(tag => tag.Text == tagText.ToLower()).FirstOrDefault();
            if (tag == null)
            {
                tag = new Tag { Text = tagText.ToLower() };
                _unitOfWork.Tags.Create(tag);
                _unitOfWork.Save();
            }

            return tag.Id;
        }

        public List<string> FindTags(string searchString)
        {
            return _unitOfWork.Tags.Find(tag => tag.Text.Contains(searchString))
                .Select(tag => tag.Text).ToList();
        }

        public IEnumerable<TagDTO> GetAll()
        {
            var tags = _unitOfWork.Tags.GetAll();
            return _mapper.Map<IEnumerable<TagDTO>>(tags);
        }

        public TagDTO GetTag(int id)
        {
            var tag = _unitOfWork.Tags.Get(id);
            return _mapper.Map<TagDTO>(tag);
        }

        public void Dispose(int id)
        {
            _logger.LogInformation("Deleted tag");
            _unitOfWork.Tags.Delete(id);
            _unitOfWork.Save();
        }
    }
}
