using Microsoft.EntityFrameworkCore;
using Store.Entities;
using Store.Services.Goodses;
using Store.Services.Goodses.Contracts;
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

        public void Add(Goods goods)
        {
            _context.Add(goods);
        }

        public void Delete(Goods goods)
        {
            _context.Remove(goods);
        }

        public HashSet<ShowgoodsDTO> GetAll()
        {
            return _context.Goodses.Select(_ => new ShowgoodsDTO
            {
                CategoryName = _.Category.Title,
                Cost = _.Cost,
                GoodsCode = _.GoodsCode,
                Inventory = _.Inventory,
                MaxInventory = _.MaxInventory,
                MinInventory = _.MinInventory,
                Name = _.Name
            }).ToHashSet();
        }

        public Goods GetbyId(int goodsCode)
        {
            return _context.Goodses.FirstOrDefault(_ => _.GoodsCode.Equals(goodsCode));
        } 
        public Goods GetByName(string goodsName)
        {
            return _context.Goodses.FirstOrDefault(_ => _.Name.Equals(goodsName));
        }

        public ShowgoodsDTO GetOne(int GoodsCode)
        {
            return _context.Goodses.Select(_ => new ShowgoodsDTO
            {
                CategoryName = _.Category.Title,
                Cost = _.Cost,
                GoodsCode = _.GoodsCode,
                Inventory = _.Inventory,
                MaxInventory = _.MaxInventory,
                MinInventory = _.MinInventory,
                Name = _.Name
            }).FirstOrDefault(_=>_.GoodsCode.Equals(GoodsCode));
        }
    }
}
