using Store.Entities;
using Store.Infrastracture.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.GoodsInputs.Contracts
{
    public interface GoodsInputRepository:Repository
    {
        void Add(GoodsInput goodsInput);
        void Delete(GoodsInput goodsInput);
        GoodsInput GetById(int id);
        HashSet<ShowGoodsInputDTO> GetAll();
        ShowGoodsInputDTO GetOneGoodsInput(int id);

    }
}
