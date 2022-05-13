using Store.Entities;
using Store.Services.GoodsOutputs.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Test.Tools.GoodsOutputs
{
    public static class GoodsOutputFactory
    {
        public static GoodsOutput CreateGoodsOutput(int goodsCode, int number)
        {
            return new GoodsOutput
            {
                Count = 1,
                Date = DateTime.Now.Date,
                GoodsCode = goodsCode,
                Number = number,
                Price = 1000
            };
        }
        public static AddgoodsOutputDTO CreateAddGoodsOutputDTO(int goodsCode, int number)
        {
            return new AddgoodsOutputDTO
            {
                Count = 1,
                Date = DateTime.Now.Date.ToShortDateString(),
                GoodsCode = goodsCode,
                Number = number,
                Price = 1000
            };
        }
        public static UpdateGoodsOutputDTO CreateUpdateGoodsOutputDTO(int goodsCode)
        {
            return new UpdateGoodsOutputDTO
            {
                Count = 1,
                Date = DateTime.Now.Date.ToShortDateString(),
                GoodsCode = goodsCode,
                Price = 1000
            };
        }
    }
}
