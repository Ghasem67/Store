using FluentAssertions;
using Store.Entities;
using Store.Infrastracture.Application;
using Store.Persistence.EF;
using Store.Persistence.EF.Goodses;
using Store.Services.Goodses;
using Store.Services.Goodses.Contracts;
using Store.Services.Goodses.Exceptions;
using Store.Specs.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Store.Specs.BDDHelper;

namespace Store.Specs.Goodses
{
    [Scenario("نمایش کالا")]
    [Feature("",
          AsA = "فروشنده",
          IWantTo = "مدیریت کالا داشته باشیم",
          InOrderTo = "تا بتوانیم اطلاعات را نمایش دهیم")]
    public class GetGoodsNoInformationForDisplay : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        UnitOfWork unitOfWork;
        GoodsRepository goodsRepository;
        GoodsService _sut;
        private List<Goods> goodsList;
        Action Expect;
        private HashSet<ShowgoodsDTO> goodsHashset;
        public GetGoodsNoInformationForDisplay(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            unitOfWork = new EFUnitOfWork(_dataContext);
            goodsRepository = new EFGoodsRepository(_dataContext);
            _sut = new GoodsAppService(goodsRepository, unitOfWork);
        }
        [Given("کالایی در سیستم وجود ندارد")]
        private void Given()
        {

        }

        [When("درخواست نمایش اطلاعات ارسال می شود")]
        private void When()
        {

            Expect = () => _sut.GetAll();
        }
        [Then(" خطایی با عنوان 'اطلاعاتی جهت نمایش وجود ندارد' در سیستم رخ می دهد")]
        private void Then()
        {
            Expect.Should().ThrowExactly<ThereIsnotInformationToDisplay>();
        }
        [Fact]
        private void NotHaveRun()
        {
            Runner.RunScenario(
            _ => Given(),
            _ => When(),
            _ => Then()
            );
        }
    }
}
