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
        int FindOrCreate(string tagText);
        List<string> FindTags(string searchString);
        TagDTO GetTag(int id);
    }
}
