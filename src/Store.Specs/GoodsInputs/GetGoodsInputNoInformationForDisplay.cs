using FluentAssertions;
using Store.Entities;
using Store.Infrastracture.Application;
using Store.Persistence.EF;
using Store.Persistence.EF.GoodsInputs;
using Store.Services.GoodsInputs;
using Store.Services.GoodsInputs.Contracts;
using Store.Services.GoodsInputs.Exceptions;
using Store.Specs.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Store.Specs.BDDHelper;

namespace Store.Specs.GoodsInputs
{
    [Scenario("نمایش ورودی کالا")]
    [Feature("",
          AsA = "فروشنده",
          IWantTo = "مدیریت   کالا داشته باشم",
          InOrderTo = "تا بتوانیم ورودی  کالا را نمایش دهیم")]
    public class GetGoodsInputNoInformationForDisplay: EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _context;
        UnitOfWork _unitOfWork;
        GoodsInputRepository goodsInputRepository;
        GoodsInputService _sut;
        Action expect;
        public GetGoodsInputNoInformationForDisplay(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_context);
            goodsInputRepository = new EFGoodsInputRepository(_context);
            _sut = new GoodsInputAppService(_unitOfWork, goodsInputRepository);
        }

        [Given("ورود کالایی در سیستم وجود ندارد")]
        private void Given()
        {

        }

        [When("درخواست نمایش اطلاعات ارسال می شود")]
        private void When()
        {

            expect = () => _sut.GetAll();
        }
        [Then(" خطایی با عنوان 'اطلاعاتی جهت نمایش وجود ندارد' در سیستم رخ می دهد")]
        private void Then()
        {
            expect.Should().ThrowExactly<ThereIsnotInformationToDisplay>();
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
