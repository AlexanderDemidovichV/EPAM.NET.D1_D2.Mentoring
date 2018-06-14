using System;
using System.Data.Entity.Core.Objects;
using AutoMapper;
using Task.DB;

namespace Task.Infrastructure
{
    public static class MapperConfigurationsHelper
    {
        public static TDestination Map<TSource, TDestination>(
            TSource source, TDestination destination, MapperConfiguration config
        )
        {
            var mapper = config.CreateMapper();
            return (TDestination)mapper.Map(source, destination,
                typeof(TSource), typeof(TDestination));
        }

        public static MapperConfiguration GetMapperConfiguration(Customer obj, Type objType, Type destinationType)
        {
            return new MapperConfiguration(conf =>
            {
                conf.CreateMap(objType, destinationType)
                    .ForMember("Orders", opt => opt.Ignore())
                    .ForMember("CustomerDemographics", opt => opt.Ignore());
            });
        }
        public static MapperConfiguration GetMapperConfiguration(Employee obj, Type objType, Type destinationType)
        {
            return new MapperConfiguration(conf =>
            {
                conf.CreateMap(objType, destinationType)
                    .ForMember("Employees1", opt => opt.Ignore())
                    .ForMember("Orders", opt => opt.Ignore())
                    .ForMember("Territories", opt => opt.Ignore())
                    .ForMember("Employee1", opt => opt.Ignore());
            });
        }

        public static MapperConfiguration GetMapperConfiguration(Order_Detail obj, Type objType, Type destinationType)
        {
            return new MapperConfiguration(conf =>
            {
                conf.CreateMap(objType, destinationType)
                    .ForMember("Order", opt => opt.Ignore())
                    .ForMember("Product", opt => opt.Ignore());
            });
        }

        public static MapperConfiguration GetMapperConfiguration(Shipper obj, Type objType, Type destinationType)
        {
            return new MapperConfiguration(conf =>
            {
                conf.CreateMap(objType, destinationType)
                    .ForMember("Orders", opt => opt.Ignore());
            });
        }

        public static MapperConfiguration GetMapperConfiguration(object obj, Type objType, Type destinationType)
        {
            return new MapperConfiguration(conf =>
            {
                conf.CreateMap(objType, destinationType);
            });
        }
    }
}
