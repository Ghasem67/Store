using Microsoft.EntityFrameworkCore;
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
         private readonly  DbSet<GoodsInput> _goodsInput;

        public EFGoodsInputRepository(EFDataContext context)
        {
            _goodsInput = context.Set<GoodsInput>();
        }

        public void Add(GoodsInput goodsInput)
        {
            _goodsInput.Add(goodsInput);
        }

        public void Delete(GoodsInput goodsInput)
        {
            _goodsInput.Remove(goodsInput);
        }

        public HashSet<ShowGoodsInputDTO> GetAll()
        {
            return _goodsInput.Select(_ => new ShowGoodsInputDTO
            {
                Price = _.Price,
                Count = _.Count,
                Date = _.Date.ToShortDateString(),
                Number = _.Number,
                GoodsCode = _.GoodsCode,
            }).ToHashSet();
            
        }

        public GoodsInput GetById(int number)
        {
            return _goodsInput.FirstOrDefault(_=>_.Number.Equals(number));
        }

        public ShowGoodsInputDTO GetOneGoodsInput(int number)
        {
          
            return _goodsInput.Select(_ => new ShowGoodsInputDTO
            {
                Count = _.Count,
                Date = _.Date.ToShortDateString(),
                GoodsCode = _.GoodsCode,
                GoodsName = _.Goods.Name,
                Number = _.Number,
                Price = _.Price
            }).FirstOrDefault(_ => _.Number.Equals(number));
        }

    }
}
