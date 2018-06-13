using System;
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

        public static MapperConfiguration GetMapperConfiguration(Type sourceType, Type destinationType)
        {
            MapperConfiguration mapperConfiguration;

            if (destinationType == typeof(Customer)) {
                mapperConfiguration = new MapperConfiguration(conf =>
                {
                    conf.CreateMap(sourceType, destinationType)
                        .ForMember("Orders", opt => opt.Ignore())
                        .ForMember("CustomerDemographics", opt => opt.Ignore());
                });
            } else if (destinationType == typeof(Employee)) {
                mapperConfiguration = new MapperConfiguration(conf =>
                {
                    conf.CreateMap(sourceType, destinationType)
                        .ForMember("Employees1", opt => opt.Ignore())
                        .ForMember("Orders", opt => opt.Ignore())
                        .ForMember("Territories", opt => opt.Ignore())
                        .ForMember("Employee1", opt => opt.Ignore());
                });
            } else if (destinationType == typeof(Order_Detail)) {
                mapperConfiguration = new MapperConfiguration(conf =>
                {
                    conf.CreateMap(sourceType, destinationType)
                        .ForMember("Order", opt => opt.Ignore())
                        .ForMember("Product", opt => opt.Ignore());
                });
            } else if (destinationType == typeof(Shipper))
            {
                mapperConfiguration = new MapperConfiguration(conf =>
                {
                    conf.CreateMap(sourceType, destinationType)
                        .ForMember("Orders", opt => opt.Ignore());
                });
            } else {
                mapperConfiguration = new MapperConfiguration(conf =>
                {
                    conf.CreateMap(sourceType, destinationType);
                });
            }

            return mapperConfiguration;
        }
    }


}
