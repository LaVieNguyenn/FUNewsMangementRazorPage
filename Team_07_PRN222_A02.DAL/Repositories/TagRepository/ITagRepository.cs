using System.Collections.Generic;
using System.Threading.Tasks;
using Team_07_PRN222_A02.DAL.Models;

namespace Team_07_PRN222_A02.DAL.Repositories.TagRepository
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllTagsAsync();
    }
}
