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

        public void Add(GoodsInput goodsInput)
        {
            _context.Add(goodsInput);
        }

        public void Delete(GoodsInput goodsInput)
        {
            _context.Remove(goodsInput);
        }

        public HashSet<ShowGoodsInputDTO> GetAll()
        {
            return _context.GoodsInputs.Select(_ => new ShowGoodsInputDTO
            {
                Price = _.Price,
                Count = _.Count,
                Date = _.Date.ToShortDateString(),
                Number = _.Number,
                GoodsCode = _.GoodsCode,
            }).ToHashSet();
            
        }

        public GoodsInput GetById(int id)
        {
            return _context.GoodsInputs.FirstOrDefault();
        }

        public ShowGoodsInputDTO GetOneGoodsInput(int id)
        {
            return _context.GoodsInputs.Select(_ => new ShowGoodsInputDTO
            {
                Count = _.Count,
                Date = _.Date.ToShortDateString(),
                GoodsCode = _.GoodsCode,
                GoodsName = _.Goods.Name
            }).FirstOrDefault();
        }

    }
}
