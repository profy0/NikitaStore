using Domain.Abstract;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.Controllers;
using WebUI.Models;

using WebUI.HtmlHelpers;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod]
        public void Can_Paginate()
        {
            // Организация (arrange)
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product { Id = 1, Name = "Игра1"},
                new Product { Id = 2, Name = "Игра2"},
                new Product { Id = 3, Name = "Игра3"},
                new Product { Id = 4, Name = "Игра4"},
                new Product { Id = 5, Name = "Игра5"}
            });
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            // Действие (act)
            IEnumerable<Product> result = (ProductsListViewModel)controller.List(2).Model;

            // Утверждение (assert)
            List<Product> Products = result.ToList();
            Assert.IsTrue(Products.Count == 2);
            Assert.AreEqual(Products[0].Name, "Игра4");
            Assert.AreEqual(Products[1].Name, "Игра5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {

            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product { Id = 1, Name = "Игра1"},
                new Product { Id = 2, Name = "Игра2"},
                new Product { Id = 3, Name = "Игра3"},
                new Product { Id = 4, Name = "Игра4"},
                new Product { Id = 5, Name = "Игра5"}
            });
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            // Act
            ProductsListViewModel result
                = (ProductsListViewModel)controller.List(2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

    }
}
