using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Repositories.Interfaces
{
    public interface IRepositoryBase<T>: IRepository
    {
        List<T> List();
        T Save(T entity);
        List<T> FilterBy(object param);
        T FindBy(object param);
        T FindById(int id);
        int DeleteBy(object param);
        int DeleteById(int id);
    }
}
