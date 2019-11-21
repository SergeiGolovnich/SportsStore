﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Moq;
using SportsStore.Components;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void Саn_Select_Categories()
        {
            //Arrage
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new Product[]
                {
                    new Product{ProductId = 1, Name = "P1", Category = "Apples" },
                    new Product{ProductId = 2, Name = "P2", Category = "Apples" },
                    new Product{ProductId = 3, Name = "P3", Category = "Plums" },
                    new Product{ProductId = 4, Name = "P4", Category = "Oranges" }
                });
            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);

            //Act
            string[] results = ((IEnumerable<string>)(target.Invoke() as ViewViewComponentResult).ViewData.Model).ToArray();

            //Assert
            Assert.True(Enumerable.SequenceEqual(new string[] { "Apples", "Oranges", "Plums" }, results));
        }
        [Fact]
        public void Indicates_Selected_Category()
        {
            //Arrage
            string categoryToSelect = "Apples";
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new Product[]
                {
                    new Product{ProductId = 1, Name = "P1", Category = "Apples" },
                    new Product{ProductId = 4, Name = "P2", Category = "Oranges" }
                });
            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);
            target.ViewComponentContext = new ViewComponentContext
            {
                ViewContext = new ViewContext
                {
                    RouteData = new RouteData()
                }
            };
            target.RouteData.Values["category"] = categoryToSelect;

            //Act
            string result = (string)(target.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"];

            //Assert
            Assert.Equal(categoryToSelect, result);
        }
    }
}