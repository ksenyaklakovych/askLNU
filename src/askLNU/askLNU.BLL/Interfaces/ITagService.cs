using askLNU.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace askLNU.BLL.Interfaces
{
    public interface ITagService
    {
        void CreateTag(TagDTO Dto);
        IEnumerable<TagDTO> GetAll();
        void Dispose();
        int FindOrCreate(string tagText);
        List<string> FindTags(string searchString);
    }
}
