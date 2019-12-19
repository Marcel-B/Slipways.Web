﻿using com.b_velop.Slipways.Web.Data.Dtos;
using com.b_velop.Slipways.Web.Data.Models;
using com.b_velop.Slipways.Web.Infrastructure;
using com.b_velop.Slipways.Web.Services;
using Microsoft.Extensions.Caching.Memory;

namespace com.b_velop.Slipways.Web.Data
{
    public interface ISlipwayStore : IDataStore<Slipway, SlipwayDto> { }

    public class SlipwayStore : DataStore<Slipway, SlipwayDto>, ISlipwayStore
    {
        public SlipwayStore(
            IMemoryCache cache,
            ISlipwayService service,
            IGraphQLService graphQLService) : base(cache, service, graphQLService)
        {
            Key = Cache.Slipways;
            Method = Queries.Slipways.Item1;
            Query = Queries.Slipways.Item2;
        }

        public override Slipway ToClass(
            SlipwayDto item)
        {
            return new Slipway(item);
        }

        public override SlipwayDto ToDto(
            Slipway item)
        {
            return new SlipwayDto(item);
        }
    }
}