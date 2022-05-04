using Store.Entities;
using Store.Services.Goodses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Persistence.EF.Goodses
{
    public class EFGoodsRepository : GoodsRepository
    {
        private readonly EFDataContext _context;

        public EFGoodsRepository(EFDataContext context)
        {
            _context = context;
        }

        public void AddGoods(Goods goods)
        {
            _context.Add(goods);
        }

        public void DeleteGoods(Goods goods)
        {
           _context.Remove(goods);
        }

        public HashSet<ShowgoodsDTO> GetAll()
        {
         return _context.Goods.Select(_=>new ShowgoodsDTO {CategoryName=_.Category.Title,
             Cost=_.Cost,
             GoodsCode=_.GoodsCode,
             Inventory=_.Inventory,
             MaxInventory=_.MaxInventory,
             MinInventory=_.MinInventory,
             Name=_.Name }).ToHashSet();
        }

        public Goods GetbyId(int id)
        {
            return _context.Goods.FirstOrDefault(_ => _.Id.Equals(id));
        }

      
    }
}
