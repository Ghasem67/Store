using FluentAssertions;
using Store.Entities;
using Store.Infrastracture.Application;
using Store.Persistence.EF;
using Store.Persistence.EF.GoodsOutputs;
using Store.Services.GoodsOutputs;
using Store.Services.GoodsOutputs.Contracts;
using Store.Services.GoodsOutputs.Exceptions;
using Store.Specs.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Store.Specs.BDDHelper;

namespace Store.Specs.GoodsOutputs
{
    public class GetGoodsOutputNoInformationForDisplay: EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _context;
        UnitOfWork _unitOfWork;
        GoodsOutputRepository goodsOutputRepository;
        GoodsOutputService _sut;
        Action expect;

        public GetGoodsOutputNoInformationForDisplay(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_context);
            goodsOutputRepository = new EFGoodsOutPutRepository(_context);
            _sut = new GoodsOutputAppService(_unitOfWork, goodsOutputRepository);
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
