using Store.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Test.Tools.Categories
{
    public static class CategoryFactory
    {
        public static Category CreateCategory(string Title)
        {
            return new Category
            {
                Title = Title,
            };
        }
    }
}
