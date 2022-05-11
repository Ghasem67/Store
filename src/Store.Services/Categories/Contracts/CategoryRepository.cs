using Store.Entities;
using Store.Infrastracture.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Categories.Contracts
{
    public interface CategoryRepository: Repository
    {
        void Add(Category category);
        void Delete(Category category);
        Category GetById(int id);
        ShowCategoryDTO GetOne(int id);
        Category GetByTitle(string title);
        HashSet<ShowCategoryDTO> GetAll();

    }
}
