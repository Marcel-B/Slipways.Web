﻿using AutoMapper;
using com.b_velop.Slipways.Data.Dtos;
using com.b_velop.Slipways.Data.Helper;
using com.b_velop.Slipways.Data.Models;
using com.b_velop.Slipways.Web.Contracts;
using com.b_velop.Slipways.Web.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;

namespace com.b_velop.Slipways.Web.Data
{
    public class PortStore : DataStore<Port, PortDto>, IPortStore
    {
        public PortStore(
            IMemoryCache cache,
            IMapper mapper,
            IPortService service,
            IGraphQLService graphQLService) : base(cache, mapper, service, graphQLService)
        {
            Key = Cache.Ports;
            Method = Queries.Ports.Item1;
            Query = Queries.Ports.Item2;
        }

        public override IEnumerable<Port> Sort(IEnumerable<Port> set)
            => set.OrderBy(_ => _.Name);

    }
}
