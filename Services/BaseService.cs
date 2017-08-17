using DataAccess.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public abstract class BaseService<TEntity, Repository, UnitOfWork>
        where TEntity : BaseEntity
        where Repository : BaseRepository<TEntity>
        where UnitOfWork : IUnitOfWork
    {
        #region Constructors and fields
        protected IValidationDictionary validationDictionary;
        protected UnitOfWork unitOfWork;
        protected Repository repository;
        public BaseService(IValidationDictionary validationDictionary, Repository repository, UnitOfWork unitOfWork)
        {
            this.validationDictionary = validationDictionary;
            this.unitOfWork = unitOfWork;
            this.repository = repository;
        }
        #endregion

        public bool PreValidate()
        {
            if (!validationDictionary.isValid)
            {
                return false;
            }
            return true;
        }

        public List<TEntity> GetAll(Func<TEntity, bool> filter = null)
        {
            return repository.GetAll(filter);
        }

        public bool Save(TEntity entity)
        {
            repository.Save(entity);
            return unitOfWork.Save() > 0;
        }

        public bool DeleteById(int id)
        {
            repository.DeleteById(id);
            return unitOfWork.Save() > 0;
        }
    }
}
