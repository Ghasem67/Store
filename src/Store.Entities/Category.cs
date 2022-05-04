using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Entities
{
    public class Category:Entity
    {
        public string Title { get; set; }
        public HashSet<Goods> Goodses { get; set; }
  
    }
}
