using Store.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Categories.Contracts
{
    public interface CategoryRepository
    {
        void Add(Category category);
        void Delete(Category category);
        Category GetById(int id);
        HashSet<ShowCategoryDTO> GetAll();

    }
}
