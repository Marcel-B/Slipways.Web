using com.b_velop.Slipways.Web.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Data
{
    public interface IDataStore<T, DTO> where T : class, IEntity where DTO : class, IEntity
    {
        Task<HashSet<T>> GetValuesAsync();
        Task<HashSet<T>> AddAsync(T item);
        Task<HashSet<T>> RemoveAsync(Guid id);
    }
}