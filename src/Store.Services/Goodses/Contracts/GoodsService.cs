using Store.Infrastracture.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Goodses.Contracts
{
    public interface GoodsService: Service
    {
        void Add(AddGoodsDTO addGoodsDTO);
        void Update(UpdateGoodsDTO updateGoodsDTO,int id);
        void Delete(int id);
        ShowgoodsDTO GetOneGoods(int id);
        HashSet<ShowgoodsDTO> GetAll();

    }
}
