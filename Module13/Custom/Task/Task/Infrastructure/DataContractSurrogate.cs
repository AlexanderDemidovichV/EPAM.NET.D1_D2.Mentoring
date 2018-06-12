using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Task.DB;

namespace Task.Infrastructure
{
    public class DataContractSurrogate: IDataContractSurrogate
    {
        public Type GetDataContractType(Type type)
        {
            if (ObjectContext.GetObjectType(type) == typeof(Order))
                return typeof(Order);


            return type;
        }

        public object GetObjectToSerialize(object obj, Type targetType)
        {
            var objType = obj.GetType();
            if (objType.Namespace == "System.Data.Entity.DynamicProxies")
            {
                var destinationType = ObjectContext.GetObjectType(obj.GetType());
                MapperConfiguration config = null;
                if (destinationType == typeof(Order))
                {
                    config = new MapperConfiguration(conf =>
                    {
                        conf.CreateMap(obj.GetType(), destinationType)
                            .ForMember("Customer", opt => opt.Ignore())
                            .ForMember("Employee", opt => opt.Ignore())
                            .ForMember("Order_Details", opt => opt.Ignore())
                            .ForMember("Shipper", opt => opt.Ignore());
                    });
                }

                var newObj = Activator.CreateInstance(targetType);
                NorthwindEntityMapper.Map(obj, newObj, config);
                
                return newObj;
            }

            return obj;
        }

        public object GetDeserializedObject(object obj, Type targetType)
        {
            return obj;
        }

        public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
        {
            throw new NotImplementedException();
        }

        public object GetCustomDataToExport(Type clrType, Type dataContractType)
        {
            throw new NotImplementedException();
        }

        public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
        {
            throw new NotImplementedException();
        }

        public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
        {
            throw new NotImplementedException();
        }

        public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
        {
            throw new NotImplementedException();
        }
    }
}
