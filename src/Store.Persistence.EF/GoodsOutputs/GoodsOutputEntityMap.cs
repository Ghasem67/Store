using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Persistence.EF.GoodsOutputs
{
    public class GoodsOutputEntityMap : IEntityTypeConfiguration<GoodsOutput>
    {
        public void Configure(EntityTypeBuilder<GoodsOutput> builder)
        {
            builder.HasKey(_=>_.Number);

            builder.HasOne(_ => _.Goods)
                 .WithMany(_ => _.GoodsOutputs)
                 .HasForeignKey(_ => _.GoodsCode);
              
        }
    }
}
