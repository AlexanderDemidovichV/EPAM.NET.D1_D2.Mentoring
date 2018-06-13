using System;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Objects;
using System.Reflection;
using System.Runtime.Serialization;
using Task.DB;

namespace Task.Infrastructure
{
    public class DataContractSurrogate: IDataContractSurrogate
    {
        public Type GetDataContractType(Type type)
        {
            if (ObjectContext.GetObjectType(type) == typeof(Order))
                return typeof(Order);
            if (ObjectContext.GetObjectType(type) == typeof(Customer))
                return typeof(Customer);
            if (ObjectContext.GetObjectType(type) == typeof(Shipper))
                return typeof(Shipper);
            if (ObjectContext.GetObjectType(type) == typeof(Order_Detail))
                return typeof(Order_Detail);
            if (ObjectContext.GetObjectType(type) == typeof(Employee))
                return typeof(Employee);

            return type;
        }

        public object GetObjectToSerialize(dynamic obj, Type targetType)
        {
            var objType = obj.GetType();
            if (objType.Namespace == "System.Data.Entity.DynamicProxies") {
                var config = MapperConfigurationsHelper.GetMapperConfiguration(obj);
                var newObj = Activator.CreateInstance(targetType);
                MapperConfigurationsHelper.Map(obj, newObj, config);
                
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
