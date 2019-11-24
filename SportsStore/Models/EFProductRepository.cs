using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class EFProductRepository : IProductRepository
    {
        private ApplicationDbContext context;
        public EFProductRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IEnumerable<Product> Products => context.Products;
        public void SaveProduct(Product product)
        {
            if(product.ProductId == 0)
            {
                context.Products.Add(product);
            }
            else
            {
                Product dbEntity = context.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
                if(dbEntity != null)
                {
                    dbEntity.Name = product.Name;
                    dbEntity.Description = product.Description;
                    dbEntity.Price = product.Price;
                    dbEntity.Category = product.Category;
                }
            }
            context.SaveChanges();
        }
        public Product DeleteProduct(int productId)
        {
            Product dbEntity = context.Products.FirstOrDefault(p => p.ProductId == productId);
            if(dbEntity != null)
            {
                context.Products.Remove(dbEntity);
                context.SaveChanges();
            }
            return dbEntity;
        }
    }
}
