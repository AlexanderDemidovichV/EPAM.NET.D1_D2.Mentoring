using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Task.DB;

namespace Task.Infrastructure
{
    public static class NorthwindEntityMapper
    {
        public static TDestination Map<TSource, TDestination>(
            TSource source, TDestination destination, MapperConfiguration config
        )
        {
            var mapper = config.CreateMapper();
            return (TDestination)mapper.Map(source, destination,
                typeof(TSource), typeof(TDestination));
        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<Order, Order>()
            //    .ForMember("OrderID", );


        }
    }
}
