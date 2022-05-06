using Store.Entities;
using Store.Services.GoodsInputs.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Persistence.EF.GoodsInputs
{
    public class EFGoodsInputRepository : GoodsInputRepository
    {
         private readonly  EFDataContext _context;

        public EFGoodsInputRepository(EFDataContext context)
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

        public HashSet<ShowGoodsInputDTO> GetAll()
        {
            return _context.GoodsInputs.Select(_ => new ShowGoodsInputDTO { 
                Price = _.Price,
                Count = _.Count,
                Date = _.Date.ToShortDateString(),
                Number=_.Number }).ToHashSet();
        }

        public GoodsInput GetById(int id)
        {
            return _context.GoodsInputs.FirstOrDefault();
        }
    }
}
