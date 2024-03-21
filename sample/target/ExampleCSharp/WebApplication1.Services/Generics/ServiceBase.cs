using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Repositories.Interfaces;
using WebApplication1.Services.Models;

namespace WebApplication1.Services.Generics
{
    public class ServiceBase<T> where T : IEntity
    {
        public ServiceBase(IRepositoryBase<T> repository)
        {
            this.repository = repository;
        }

        protected IRepositoryBase<T> repository { get; set; }

        public virtual T Get(int id)
        {
            return this.repository.FindById(id);
        }

        public List<T> List()
        {
            return this.repository.List();
        }

        public virtual DefaultResponse Save(T entity)
        {
            try
            {
                entity = this.repository.Save(entity);
                return new DefaultResponse();
            }
            catch (Exception ex)
            {
                return new DefaultResponse(false, ex);
            }
        }

        public virtual DefaultResponse Delete(int id)
        {
            try
            {
                this.repository.DeleteById(id);
                return new DefaultResponse();
            }
            catch (Exception ex)
            {
                return new DefaultResponse(false, ex);
            }
        }

        public virtual bool EntityRepeated(IEntity valid)
        {
            int validId = valid != null && valid.Id != 0 ? valid.Id : 0;
            return valid != null && validId != 0;
        }

        public virtual bool EntityRepeated(IEntity valid, IEntity entity)
        {
            int validId = valid != null && valid.Id != 0 ? valid.Id : 0;
            int entityId = entity != null && entity.Id != 0 ? entity.Id : 0;
            return valid != null && validId != entityId;
        }
    }
}
