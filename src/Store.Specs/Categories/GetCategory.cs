using FluentAssertions;
using Store.Entities;
using Store.Infrastracture.Application;
using Store.Infrastracture.Tests;
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
    public class GetCategory : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _context;
        UnitOfWork _unitOfWork;
        CategoryRepository categoryRepository;
        CategoryService _Sut;
        private List<Category> categories;
        Action expect;
        public GetCategory(ConfigurationFixture configuration) : base(configuration)
        {
            _context = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_context);
            categoryRepository = new EFCategoryRepository(_context);
            _Sut = new CategoryAppService(categoryRepository, _unitOfWork);
        }

        [Given("دسته بندی هایی در سیستم وجود دارد")]
        private void GivenGetAll()
        {
            categories = new()
            {
                new Category { Title = "لبنیات" },
                new Category { Title = "خشکبار" },
                new Category { Title = "نان" }
            };
            _context.Manipulate(_ => _.AddRange(categories));
        }
        [When("درخواست نمایش اطلاعات ارسال می شود")]
        private void WhenGellAll()
        {

            _Sut.GetAll();
        }
        [Then("فهرستی از دسته بندی  نمایش داده می شود")]
        private void ThenGetAll()
        {
            var expect = _context.Categories.ToList();
            //expect.Should().HaveCount(3);
            expect.Should().Contain(_ => _.Title == categories[0].Title);
            expect.Should().Contain(_ => _.Title == categories[1].Title);
            expect.Should().Contain(_ => _.Title == categories[2].Title);

        }
        [Fact]
        private void Run()
        {
            GivenGetAll();
            WhenGellAll();
            ThenGetAll();
        }
        [Given("دسته بندی در سیستم وجود ندارد")]
        private void NotHaveGiven()
        {

        }

        [When("درخواست نمایش اطلاعات ارسال می شود")]
        private void NotHaveWhen()
        {

            expect = () => _Sut.GetAll();
        }
        [Then(" خطایی با عنوان 'اطلاعاتی جهت نمایش وجود ندارد' در سیستم رخ می دهد")]
        private void NotHaveThen()
        {
            expect.Should().ThrowExactly<ThereIsnoInformationToDisplay>();
        }
        [Fact]
        private void NotHaveRun()
        {
            Runner.RunScenario(
            _ => NotHaveGiven(),
            _ =>NotHaveWhen(),
            _=>NotHaveThen()
            );
        }
    }
}
