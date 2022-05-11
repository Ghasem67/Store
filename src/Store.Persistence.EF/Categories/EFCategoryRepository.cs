using Microsoft.EntityFrameworkCore;
using Store.Entities;
using Store.Services.Categories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Persistence.EF.Categories
{
    public class EFCategoryRepository : CategoryRepository
    {
        private readonly DbSet<Category> _context;

        public EFCategoryRepository(EFDataContext context)
        {
            _context = context.Set<Category>();
        }

        public void Add(Category category)
        {
           _context.Add(category);
        }

        public void Delete(Category category)
        {
           _context.Remove(category);
        }

        public HashSet<ShowCategoryDTO> GetAll()
        {
           return _context.Select(_ => new ShowCategoryDTO {Id=_.Id,Title=_.Title }).ToHashSet();
        }

        public Category GetById(int id)
        {
            return _context.FirstOrDefault(x=>x.Id.Equals(id));
        }

        public Category GetByTitle(string title)
        {
            return _context.FirstOrDefault(_ => _.Title.Equals(title));
        }

        public ShowCategoryDTO GetOne(int id)
        {
            return _context.Select(_ => new ShowCategoryDTO { Id = _.Id, Title = _.Title }).FirstOrDefault(_=>_.Id.Equals(id));
        }
    }
}
