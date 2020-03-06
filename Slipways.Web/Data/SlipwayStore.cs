﻿using AutoMapper;
using com.b_velop.Slipways.Data.Dtos;
using com.b_velop.Slipways.Data.Extensions;
using com.b_velop.Slipways.Data.Helper;
using com.b_velop.Slipways.Data.Models;
using com.b_velop.Slipways.Web.Contracts;
using com.b_velop.Slipways.Web.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;

namespace com.b_velop.Slipways.Web.Data
{
    public class SlipwayStore : DataStore<Slipway, SlipwayDto>, ISlipwayStore
    {
        public SlipwayStore(
            IMemoryCache cache,
            IMapper mapper,
            ISlipwayService service,
            IGraphQLService graphQLService) : base(cache, mapper, service, graphQLService)
        {
            Key = Cache.Slipways;
            Method = Queries.Slipways.Item1;
            Query = Queries.Slipways.Item2;
        }

        public override IEnumerable<Slipway> Sort(IEnumerable<Slipway> set)
            => set.OrderBy(_ => _.Name);
    }
}
