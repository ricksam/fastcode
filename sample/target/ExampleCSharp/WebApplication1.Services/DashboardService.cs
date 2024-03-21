using WebApplication1.Repositories;
using WebApplication1.Repositories.Models;
using WebApplication1.Services.Interfaces;
using System.Collections.Generic;

namespace WebApplication1.Services
{
    public class DashboardService :IService
    {
        public DashboardService(DashboardRepository repository) 
        {
            this.repository = repository;
        }
        DashboardRepository repository {  get; set; }

        public List<DashboardItem> List() {
            return this.repository.List();
            
        }
    }
}

