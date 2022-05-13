using Store.Entities;
using Store.Services.Goodses.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Test.Tools.Goodses
{
    public static class GoodsFactory
    {
        public static Goods CreateGoods(int goodsCode,string name,int categoryId)
        {
            return new Goods
            {
                GoodsCode = goodsCode,
                Name = name,
                CategoryId= categoryId,
                Inventory=11,
                MaxInventory=20,
                MinInventory=10,
                Cost=2000,
            };
        }
        public static UpdateGoodsDTO CreateGoodsDTO( string name, int categoryId)
        {
            return new UpdateGoodsDTO
            {
                CategoryId = categoryId,
                Cost = 1000,
                Inventory = 12,
                Name = name,
                MaxInventory = 30,
                MinInventory = 10
            };
        }
    }
}
