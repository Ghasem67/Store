using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Migrations
{
    [Migration(2022521120)]
    public class _2022521120Init : Migration
    {
        public override void Up()
        {
            CreateCategory();
            CreateGoods();
            CreateGoodsInput();
            CreateGoodsOutput();
        }
        public override void Down()
        {
            Delete.Table("GoodsInputs");
            Delete.Table("GoodsOutputs");
            Delete.Table("Goodses");
            Delete.Table("Categories");

        }

        private void CreateGoods()
        {
            Create.Table("Goodses")
                .WithColumn("GoodsCode").AsInt32().PrimaryKey()
              .WithColumn("Name").AsString().NotNullable()
              .WithColumn("Cost").AsInt32().NotNullable()
              .WithColumn("Inventory").AsInt32().NotNullable()
              .WithColumn("MinInventory").AsInt32().NotNullable()
              .WithColumn("MaxInventory").AsInt32().NotNullable()
              .WithColumn("CategoryId").AsInt32().NotNullable().ForeignKey("FK_Goodses_Categories", "Categories","Id")
              .OnDelete(System.Data.Rule.None);
             
        }
        private void CreateCategory()
        {
            Create.Table("Categories").WithColumn("Id").AsInt32().PrimaryKey().NotNullable().Identity()
              .WithColumn("Title").AsString().NotNullable();
        }
        private void CreateGoodsInput()
        {
            Create.Table("GoodsInputs").WithColumn("Number").AsInt32().PrimaryKey().NotNullable()
             .WithColumn("Date").AsDateTime2().NotNullable()
             .WithColumn("Count").AsInt32().NotNullable()
             .WithColumn("Price").AsInt32().NotNullable()
             .WithColumn("GoodsCode").AsInt32().NotNullable().ForeignKey("FK_GoodsInputs_Goodses", "Goodses", "GoodsCode")
             .OnDelete(System.Data.Rule.Cascade);
        }
        private void CreateGoodsOutput()
        {
            Create.Table("GoodsOutputs").WithColumn("Number").AsInt32().PrimaryKey().NotNullable()
             .WithColumn("Date").AsDateTime2().NotNullable()
             .WithColumn("Count").AsInt32().NotNullable()
             .WithColumn("Price").AsInt32().NotNullable()
             .WithColumn("GoodsCode").AsInt32().NotNullable().ForeignKey("FK_GoodsOutputs_Goodses", "Goodses", "GoodsCode")
             .OnDelete(System.Data.Rule.Cascade); 
        }
      
    }
}
