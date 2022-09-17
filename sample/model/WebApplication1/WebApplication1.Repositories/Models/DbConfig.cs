using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Repositories.Interfaces;

namespace WebApplication1.Repositories.Models
{
    public class DbConfig : IConfig
    {
        public static string staticConnectionString { get; set; }
        public string ConnectionString { get { return staticConnectionString; } set { staticConnectionString = value; } }
    }
}
