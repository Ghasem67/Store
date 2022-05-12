using Store.Entities;
using Store.Infrastracture.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Goodses.Contracts
{
    public interface GoodsRepository:Repository
    {
        void Add(Goods goods);
        void Delete(Goods goods);
        Goods GetbyId(int GoodsCode);
        Goods GetByName(string goodsName);
        ShowgoodsDTO GetOne(int id);
        HashSet<ShowgoodsDTO> GetAll();

    }
}
