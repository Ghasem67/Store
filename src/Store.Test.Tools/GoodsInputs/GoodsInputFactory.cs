using Store.Entities;
using Store.Services.GoodsInputs.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Test.Tools.GoodsInputs
{
    public static class GoodsInputFactory
    {
        public static GoodsInput CreateGoodsInput(int goodsCode,int number)
        {
            return new GoodsInput
            {
                Count = 1,
                Date = DateTime.Now.Date,
                GoodsCode= goodsCode,
                Number= number,
                Price=1000
            };
        }
        public static AddGoodsInputDTO CreateAddGoodsInputDTO(int goodsCode, int number)
        {
            return new AddGoodsInputDTO
            {
                Count = 1,
                Date = DateTime.Now.Date.ToShortDateString(),
                GoodsCode = goodsCode,
                Number = number,
                Price = 1000
            };
        }
        public static UpdateGoodsInputDTO CreateUpdateGoodsInputDTO(int goodsCode)
        {
            return new UpdateGoodsInputDTO
            {
                Count = 1,
                Date = DateTime.Now.Date.ToShortDateString(),
                GoodsCode = goodsCode,
                Price = 1000
            };
        }
    }
}
