using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models
{
    public class ApplicationDbContex:DbContext
    {
        public ApplicationDbContex(DbContextOptions<ApplicationDbContex> options): base(options) { }
        public DbSet<Product> Products { get; set; }
    }
}
