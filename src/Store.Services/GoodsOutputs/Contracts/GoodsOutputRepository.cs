using Store.Entities;
using Store.Infrastracture.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.GoodsOutputs.Contracts
{
    public interface GoodsOutputRepository:Repository
    {
        void Add(GoodsOutput goodsOutput);
        void Delete(GoodsOutput goodsOutput);
        GoodsOutput GetById(int id);
        ShowGoodsOutputDTO GetOne(int id);
        HashSet<ShowGoodsOutputDTO> GetAll();
    }
}
