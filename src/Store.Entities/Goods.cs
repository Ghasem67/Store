using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Entities
{
    public class Goods
    {
        public int GoodsCode { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public int Inventory { get; set; }
        public int MinInventory { get; set; }
        public int MaxInventory { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public HashSet<GoodsInput> GoodsInputs { get; set; }
        public HashSet<GoodsOutput> GoodsOutputs { get; set; }

    }
}
