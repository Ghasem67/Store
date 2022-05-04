using Microsoft.EntityFrameworkCore;
using Store.Entities;
using Store.Persistence.EF.Goodses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Persistence.EF
{
    public class EFDataContext : DbContext
    {
        public EFDataContext(string connectionString)
            : base(new DbContextOptionsBuilder().UseSqlServer(connectionString).Options)
        { }
        public EFDataContext(DbContextOptions options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GoodsEntityMap).Assembly);
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Goods> Goods { get; set; }
        public DbSet<GoodsInput> GoodsInputs { get; set; }
        public DbSet<GoodsOutput> GoodsOutputs { get; set; }
    }
}
