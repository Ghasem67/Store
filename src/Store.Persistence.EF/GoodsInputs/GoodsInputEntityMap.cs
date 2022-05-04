using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Persistence.EF.GoodsInputs
{
    public class GoodsInputEntityMap : IEntityTypeConfiguration<GoodsInput>
    {
        public void Configure(EntityTypeBuilder<GoodsInput> builder)
        {
            builder.HasKey(x => x.Number);
        }
    }
}
