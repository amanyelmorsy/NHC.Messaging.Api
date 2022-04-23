using NHC.Messaging.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHC.Messaging.Application.Crud
{
    public interface ICrudService<TEntity, TDto>
        where TEntity : Entity
        where TDto : class
    {
        Task<IEnumerable<TDto>> GetAll();
        Task<TDto> GetById(int id);
        Task<TDto> Add(TDto item);
        Task Update(TDto item);
        Task Delete(int id);
        Task Delete(TEntity entity);
    }
}
