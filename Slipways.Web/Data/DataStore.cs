﻿using com.b_velop.Slipways.Web.Data.Models;
using com.b_velop.Slipways.Web.Services;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.b_velop.Slipways.Web.Data
{
    public static class Cache
    {
        public const string Waters = "Waters";
        public const string Slipways = "Slipways";
        public const string Services = "Services";
        public const string Manufacturers = "Manufacturers";
        public const string Extras = "Extras";
    }

    public abstract class DataStore<T, DTO> : IDataStore<T, DTO>
        where T : class, IEntity
        where DTO : class, IEntity
    {
        private IMemoryCache _cache;
        private IGraphQLService _graphQLService;
        private ITokenService<DTO> _service;

        protected string Key { get; set; }
        protected string Query { get; set; }
        protected string Method { get; set; }

        public DataStore(
            IMemoryCache cache,
            ITokenService<DTO> service,
            IGraphQLService graphQLService)
        {
            _cache = cache;
            _service = service;
            _graphQLService = graphQLService;
        }

        public async Task<HashSet<T>> GetValuesAsync()
        {
            if (!_cache.TryGetValue(Key, out HashSet<T> entities))
            {
                var values = await _graphQLService.GetValuesAsync<IEnumerable<T>>(Method, Query);
                entities = values.ToHashSet();
                _cache.Set(Key, entities);
            }
            return entities;
        }

        protected async Task<DTO> InsertAsync(
            DTO item)
            => await _service.InsertAsync(item);

        public async Task<HashSet<T>> AddAsync(
            T item)
        {
            var dto = ToDto(item);
            var result = await _service.InsertAsync(dto);

            if (result == null)
                return null;

            item.Id = result.Id;
            var values = await GetValuesAsync();
            values.Add(item);
            _cache.Set(Key, values);
            return values;
        }

        public abstract DTO ToDto(T item);
        public abstract T ToClass(DTO item);

        public async Task<HashSet<T>> RemoveAsync(
            Guid id)
        {
            var result = await _service.DeleteAsync(id);
            if (result == null)
                return null;
            var entities = await GetValuesAsync();
            entities.RemoveWhere(_ => _.Id == id);
            _cache.Set(Key, entities);
            return entities;
        }

        public async Task<HashSet<T>> UpdateAsync(
            T item, Guid id)
        {
            var dto = ToDto(item);

            var result = await _service.UpdateAsync(id, dto);
            if (result == null)
                return null;

            var entities = await GetValuesAsync();
            entities.RemoveWhere(_ => _.Id == id);
            entities.Add(item);
            _cache.Set(Key, entities);
            return entities;
        }
    }
}
