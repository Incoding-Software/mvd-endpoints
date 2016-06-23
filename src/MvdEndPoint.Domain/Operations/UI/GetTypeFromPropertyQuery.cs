namespace Incoding.Endpoint
{
    using System;
    using CloudIn.Domain.Endpoint;
    using Incoding.CQRS;
    using Incoding.MvcContrib.MVD;

    public class GetTypeFromPropertyQuery : QueryBase<Type>
    {
        public Message.Property Property { get; set; }

        protected override Type ExecuteResult()
        {
            var propertyType = Property.PropertyType == "System.Byte[]" ? typeof(byte[]) : Dispatcher.Query(new CreateByTypeQuery.FindTypeByName() { Type = Property.PropertyType });
            if (!string.IsNullOrWhiteSpace(Property.GenericType))
                propertyType = propertyType.MakeGenericType(Dispatcher.Query(new CreateByTypeQuery.FindTypeByName() { Type = Property.GenericType }));

            return propertyType;
        }
    }
}