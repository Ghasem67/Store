using Microsoft.EntityFrameworkCore;
using Store.Entities;
using Store.Services.GoodsOutputs.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Persistence.EF.GoodsOutputs
{
    public class EFGoodsOutPutRepository : GoodsOutputRepository
    {
        private readonly DbSet<GoodsOutput> _goodsOutputs;

        public EFGoodsOutPutRepository(EFDataContext context)
        {
            _goodsOutputs = context.Set<GoodsOutput>();
        }

        public void Add(GoodsOutput goodsOutput)
        {
            _goodsOutputs.Add(goodsOutput);
        }

        public void Delete(GoodsOutput goodsOutput)
        {
            _goodsOutputs.Remove(goodsOutput);
        }

        public HashSet<ShowGoodsOutputDTO> GetAll()
        {
          return  _goodsOutputs.Select(_ => new ShowGoodsOutputDTO {
              Count = _.Count,
              Date = _.Date.ToShortDateString(),
              GoodsCode = _.GoodsCode,
              GoodsName = _.Goods.Name,
              Number = _.Number,
              Price = _.Price }).ToHashSet();
        }

        public GoodsOutput GetById(int id)
        {
            return _goodsOutputs.FirstOrDefault();
        }

        public ShowGoodsOutputDTO GetOne(int id)
        {
            return _goodsOutputs.Select(_ => new ShowGoodsOutputDTO { 
                Count = _.Count,
                Date = _.Date.ToShortDateString(),
                GoodsCode = _.GoodsCode, 
                GoodsName = _.Goods.Name,
                Number = _.Number,
                Price = _.Price
            }).FirstOrDefault(_=>_.GoodsCode.Equals(id));
        }
    }
}
