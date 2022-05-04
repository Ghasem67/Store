using Store.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Goodses
{
    public interface GoodsRepository
    {
        void AddGoods(Goods goods);
        void DeleteGoods(Goods goods);
        Goods GetbyId(int id);
        HashSet<ShowgoodsDTO> GetAll();

    }
}
