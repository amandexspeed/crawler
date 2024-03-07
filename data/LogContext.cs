using Microsoft.EntityFrameworkCore;
using Raspagem_API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspagem_API.data
{
    public class LogContext : DbContext
    {

        public DbSet<Log> Logrobo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=SQL9001.site4now.net;" +
            "Initial Catalog=db_aa5b20_apialmoxarifado;" +
            "User id=db_aa5b20_apialmoxarifado_admin;" +
            "Password=master@123");
        }

        
    }
}
