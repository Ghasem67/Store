using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Entities
{
    public abstract class GoodsTurnover
    {
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public int GoodsId { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public Goods Goods { get; set; }

    }
}
