using Mapster;
using Microsoft.EntityFrameworkCore;
using NHC.Messaging.Application.Exceptions;
using NHC.Messaging.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHC.Messaging.Application.Crud
{
    public class CrudService<TEntity, TDto> : ICrudService<TEntity, TDto>
        where TEntity : Entity
        where TDto : class
    {
         private readonly DbContext _dbContext;

        public CrudService(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public virtual async Task<IEnumerable<TDto>> GetAll()
        {
            return await GetQueryable().OrderByDescending(x => x.Id).ProjectToType<TDto>().ToListAsync();
        }

        public virtual Task<TDto> GetById(int id)
        {
            return GetQueryable().Where(x => x.Id == id).ProjectToType<TDto>().FirstOrDefaultAsync();
        }

        public virtual async Task<TDto> Add(TDto item)
        {
            await ValidateOnAdd(item);

            var entity = item.Adapt<TEntity>();

            await PrepareEntityForAdd(entity);
            _dbContext.Set<TEntity>().Add(entity);

            await BeforeSavingAddedEntity(entity);
            await _dbContext.SaveChangesAsync();
            await AfterSavingAddedEntity(entity);

            return entity.Adapt<TDto>();
        }

        protected virtual Task ValidateOnAdd(TDto item) => Task.CompletedTask;

        protected virtual async Task PrepareEntityForAdd(TEntity entity)
        {
            entity.CreatedBy = 1;
            entity.CreatedAt = DateTime.Now;
        }
        protected virtual Task BeforeSavingAddedEntity(TEntity entity) => Task.CompletedTask;
        protected virtual Task AfterSavingAddedEntity(TEntity entity) => Task.CompletedTask;

        public virtual async Task Update(TDto item)
        {
            await ValidateOnUpdate(item);
            await UpdateWithoutValidation(item);
        }

        protected virtual async Task UpdateWithoutValidation(TDto item)
        {
            var entity = item.Adapt<TEntity>();

            var originalEntity = await GetOriginalEntityForUpdate(entity);
            if (originalEntity == null)
                throw new EntityNotFoundException($"Original entity of type [{typeof(TEntity).Name}] with id #{entity.Id} not found");

            var originalEntityCopy = originalEntity.Adapt<TEntity>();

            await PrepareEntityForUpdate(entity, originalEntity);

           await BeforeSavingModifiedEntity(entity, originalEntityCopy);
            await _dbContext.SaveChangesAsync();

            await AfterSavingModifiedEntity(entity, originalEntityCopy);

        }

        protected virtual Task AfterSavingModifiedEntity(TEntity entity, TEntity originalEntity)
        {
            return Task.CompletedTask;
        }

        protected virtual Task ValidateOnUpdate(TDto item) => Task.CompletedTask;
        protected virtual Task<TEntity> GetOriginalEntityForUpdate(TEntity modifiedEntity)
        {
            return GetQueryable().FirstOrDefaultAsync(x => x.Id == modifiedEntity.Id);
        }

        protected virtual async Task PrepareEntityForUpdate(TEntity entity, TEntity originalEntity)
        {
            entity.Adapt(originalEntity);
        }


        protected virtual Task BeforeSavingModifiedEntity(TEntity entity, TEntity originalEntity) => Task.CompletedTask;

        public virtual async Task Delete(int id)
        {
            var entity = GetQueryable().FirstOrDefault(x => x.Id.Equals(id));
            if (entity == null)
                throw new EntityNotFoundException();

            await Delete(entity);
        }

        public virtual async Task Delete(TEntity entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
                _dbContext.Set<TEntity>().Attach(entity);

            _dbContext.Set<TEntity>().Remove(entity);
            await PrepareEntityForDelete(entity);
            await _dbContext.SaveChangesAsync();
        }

        protected virtual IQueryable<TEntity> GetQueryable()
        {
            var query = _dbContext.Set<TEntity>().AsQueryable();
            foreach (var navProp in _dbContext.Model.FindEntityType(typeof(TEntity)).GetNavigations().Select(x => x.Name).ToArray())
                query = query.Include(navProp);
            return query;
        }
        protected virtual Task PrepareEntityForDelete(TEntity entity) => Task.CompletedTask;
    }
}
