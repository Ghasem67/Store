using FluentAssertions;
using Store.Entities;
using Store.Infrastracture.Application;
using Store.Persistence.EF;
using Store.Persistence.EF.Categories;
using Store.Services.Categories;
using Store.Services.Categories.Contracts;
using Store.Services.Categories.Exceptions;
using Store.Specs.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Store.Specs.BDDHelper;

namespace Store.Specs.Categories
{
    [Scenario("نمایش دسته بندی کالا")]
    [Feature("",
          AsA = "فروشنده",
          IWantTo = "مدیریت دسته بندی کالا داشته باشم",
          InOrderTo = "تا بتوانم کالا را نمایش بدهم")]
    public class GetCategoryNoInformationForDisplay : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _context;
        UnitOfWork _unitOfWork;
        CategoryRepository categoryRepository;
        CategoryService _Sut;
        Action expect;
        public GetCategoryNoInformationForDisplay(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_context);
            categoryRepository = new EFCategoryRepository(_context);
            _Sut = new CategoryAppService(categoryRepository, _unitOfWork);
        }
        [Given("دسته بندی در سیستم وجود ندارد")]
        private void Given()
        {

        }

        [When("درخواست نمایش اطلاعات ارسال می شود")]
        private void When()
        {

            expect = () => _Sut.GetAll();
        }
        [Then(" خطایی با عنوان 'اطلاعاتی جهت نمایش وجود ندارد' در سیستم رخ می دهد")]
        private void Then()
        {
            expect.Should().ThrowExactly<ThereIsnoInformationToDisplay>();
        }
        [Fact]
        private void Run()
        {
            Runner.RunScenario(
            _ => Given(),
            _ => When(),
            _ => Then()
            );
        }
    }
}
