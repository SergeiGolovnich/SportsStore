using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using Xunit;

namespace SportsStore.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void Can_Paginate()
        {
            //Arrage
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new Product[]
                {
                    new Product{ProductId = 1, Name = "P1" },
                    new Product{ProductId = 2, Name = "P2" },
                    new Product{ProductId = 3, Name = "P3" },
                    new Product{ProductId = 4, Name = "P4" },
                    new Product{ProductId = 5, Name = "P5" }
                });
            ProductController controller = new ProductController(mock.Object)
            {
                PageSize = 3
            };

            //Act
            ProductsListViewModel result = controller.List(null, 2).ViewData.Model as ProductsListViewModel;

            //Assert
            Product[] prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P4", prodArray[0].Name);
            Assert.Equal("P5", prodArray[1].Name);
        }
        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            //Arrage
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new Product[]
                {
                    new Product{ProductId = 1, Name = "P1" },
                    new Product{ProductId = 2, Name = "P2" },
                    new Product{ProductId = 3, Name = "P3" },
                    new Product{ProductId = 4, Name = "P4" },
                    new Product{ProductId = 5, Name = "P5" }
                });
            ProductController controller = new ProductController(mock.Object)
            {
                PageSize = 3
            };

            //Act
            ProductsListViewModel result = controller.List(null, 2).ViewData.Model as ProductsListViewModel;

            //Assert
            PagingInfo pageinfo = result.PagingInfo;
            Assert.Equal(2, pageinfo.CurrentPage);
            Assert.Equal(3, pageinfo.ItemsPerPage);
            Assert.Equal(5, pageinfo.TotalItems);
            Assert.Equal(2, pageinfo.TotalPages);
        }
        [Fact]
        public void Can_Filter_Products()
        {
            //Arrage
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new Product[]
                {
                    new Product{ProductId = 1, Name = "P1", Category = "Cat1" },
                    new Product{ProductId = 2, Name = "P2", Category = "Cat2" },
                    new Product{ProductId = 3, Name = "P3", Category = "Cat1" },
                    new Product{ProductId = 4, Name = "P4", Category = "Cat2" },
                    new Product{ProductId = 5, Name = "P5", Category = "Cat3" }
                });
            ProductController controller = new ProductController(mock.Object)
            {
                PageSize = 3
            };

            //Act
            Product[] result = (controller.List("Cat2", 1).ViewData.Model as ProductsListViewModel).Products.ToArray();

            //Assert
            Assert.Equal(2, result.Length);
            Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.True(result[1].Name == "P4" && result[1].Category == "Cat2");
        }
        [Fact]
        public void Generate_Category_Specific_Product_Count()
        {
            //Arrage
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new Product[]
                {
                    new Product{ProductId = 1, Name = "P1", Category = "Cat1" },
                    new Product{ProductId = 2, Name = "P2", Category = "Cat2" },
                    new Product{ProductId = 3, Name = "P3", Category = "Cat1" },
                    new Product{ProductId = 4, Name = "P4", Category = "Cat2" },
                    new Product{ProductId = 5, Name = "P5", Category = "Cat3" }
                });
            ProductController controller = new ProductController(mock.Object)
            {
                PageSize = 3
            };
            Func<ViewResult, ProductsListViewModel> getModel = resultView =>
             resultView?.ViewData?.Model as ProductsListViewModel;

            //Act
            int? res1 = getModel(controller.List("Cat1"))?.PagingInfo.TotalItems;
            int? res2 = getModel(controller.List("Cat2"))?.PagingInfo.TotalItems;
            int? res3 = getModel(controller.List("Cat3"))?.PagingInfo.TotalItems;
            int? resAll = getModel(controller.List(null))?.PagingInfo.TotalItems;

            //Assert
            Assert.Equal(2, res1);
            Assert.Equal(2, res2);
            Assert.Equal(1, res3);
            Assert.Equal(5, resAll);
        }
    }
}
