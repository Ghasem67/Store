using Store.Infrastracture.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Categories.Contracts
{
    public interface CategoryService:Service
    {
        void Add(AddCategoryDTO addCategoryDTO);
        void Update(UpdateCategoryDTO updateCategoryDTO,int id);
        void Delete(int id);
        //ShowCategoryDTO Get(int id);
        HashSet<ShowCategoryDTO> GetAll();
    }
}
