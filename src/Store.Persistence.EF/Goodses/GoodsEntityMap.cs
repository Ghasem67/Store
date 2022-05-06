using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Entities;


namespace Store.Persistence.EF.Goodses
{
    public class GoodsEntityMap : IEntityTypeConfiguration<Goods>
    {
        public void Configure(EntityTypeBuilder<Goods> builder)
        {
            builder.HasKey(_=>_.GoodsCode);
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(_ => _.Category)
                .WithMany(_ => _.Goodses)
                .HasForeignKey(e => e.CategoryId).OnDelete(DeleteBehavior.Cascade);
                
        }
    }
}
