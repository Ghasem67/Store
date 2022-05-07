using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.GoodsInputs.Contracts
{
    public class ShowGoodsInputDTO
    {
        public int Number { get; set; }
        public string Date { get; set; }
        public int GoodsCode { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public string GoodsName { get; set; }
    }
}
