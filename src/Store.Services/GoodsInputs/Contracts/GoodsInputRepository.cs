using Store.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.GoodsInputs.Contracts
{
    public interface GoodsInputRepository
    {
        void Add(GoodsInput goodsInput);
        void Delete(GoodsInput goodsInput);
        GoodsInput GetById(int id);
        HashSet<ShowGoodsInputDTO> GetAll();
        ShowGoodsInputDTO GetOneGoodsInput();

    }
}
