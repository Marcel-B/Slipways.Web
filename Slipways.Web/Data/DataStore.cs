using com.b_velop.Slipways.Web.Data.Dtos;
using com.b_velop.Slipways.Web.Data.Models;
using com.b_velop.Slipways.Web.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
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
    }

    public class DataStore : IDataStore
    {
        private IMemoryCache _cache;
        private IGraphQLService _graphQLService;
        private ISlipwayService _slipwayService;
        private IWaterService _waterService;
        private ILogger<DataStore> _logger;

        public DataStore(
            IMemoryCache cache,
            IGraphQLService graphQLService,
            ISlipwayService slipwayService,
            IWaterService waterService,
            ILogger<DataStore> logger)
        {
            _cache = cache;
            _graphQLService = graphQLService;
            _slipwayService = slipwayService;
            _waterService = waterService;
            _logger = logger;
        }

        public async Task<HashSet<Slipway>> GetSlipwaysAsync()
        {
            if (!_cache.TryGetValue(Cache.Slipways, out HashSet<Slipway> slipways))
            {
                var slipwayDtos = await _graphQLService.GetSlipwaysAsync();
                slipways = new HashSet<Slipway>();
                foreach (var slipwayDto in slipwayDtos)
                    slipways.Add(new Slipway(slipwayDto));
                _cache.Set(Cache.Slipways, slipways);
            }
            return slipways;
        }

        public async Task<HashSet<Slipway>> RemoveSlipwayAsync(
            Guid id)
        {
            var result = await _slipwayService.DeleteSlipwayAsync(id);
            var slipways = await GetSlipwaysAsync();
            slipways.RemoveWhere(_ => _.Id == result.Id);
            _cache.Set(Cache.Slipways, slipways);
            return slipways;
        }

        public async Task<HashSet<Water>> GetWatersAsync()
        {
            if (!_cache.TryGetValue(Cache.Waters, out HashSet<Water> waters))
            {
                var watersDtos = await _graphQLService.GetWatersAsync();
                waters = new HashSet<Water>();
                foreach (var waterDto in watersDtos)
                    waters.Add(new Water(waterDto));
                _cache.Set(Cache.Waters, waters);
            }
            return waters;
        }

        public async Task<HashSet<Slipway>> AddSlipwayAsync(
            Slipway slipway,
            IEnumerable<Guid> extras)
        {
            var slipwayDto = new SlipwayDto
            {
                Name = slipway.Name,
                City = slipway.City,
                Latitude = slipway.Latitude.Value,
                Longitude = slipway.Longitude.Value,
                Costs = slipway.Costs.Value,
                Rating = slipway.Rating.Value,
                Street = slipway.Street,
                Postalcode = slipway.Postalcode,
                WaterFk = Guid.Parse(slipway.Water),
                Pro = slipway.Pro,
                Contra = slipway.Contra,
                Comment = slipway.Comment,
                Created = DateTime.Now,
                Extras = extras
            };

            var result = await _slipwayService.InsertSlipway(slipwayDto);

            if (result == null)
                return null;

            var slipways = await GetSlipwaysAsync();
            slipway.Id = result.Id;
            slipways.Add(slipway);
            _cache.Set(Cache.Slipways, slipways);
            return slipways;
        }

        public async Task<HashSet<Water>> AddWaterAsync(
            Water water)
        {
            var waterDto = new WaterDto
            {
                Longname = water.Longname,
                Shortname = water.Shortname
            };
            var result = await _waterService.InsertAsync(waterDto);

            if (result == null)
                return null;

            var waters = await GetWatersAsync();
            water.Id = result.Id;
            waters.Add(water);
            _cache.Set(Cache.Waters, waters);

            return waters;
        }

        public async Task<HashSet<Water>> RemoveWaterAsync(
            Guid id)
        {
            if (_cache.TryGetValue(Cache.Waters, out HashSet<Water> waters))
            {
                waters.RemoveWhere(_ => _.Id == id);
                var water = await _waterService.DeleteWaterAsync(id);
                _cache.Set(Cache.Waters, waters);
            }
            return waters;
        }
    }
}
