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
            var isArray = Property.PropertyType.EndsWith("[]");
            var clearType = isArray ? Property.PropertyType.Replace("[]","") : Property.PropertyType;
            var propertyType = Dispatcher.Query(new CreateByTypeQuery.FindTypeByName() { Type = clearType });
            if (!string.IsNullOrWhiteSpace(Property.GenericType))
                propertyType = propertyType.MakeGenericType(Dispatcher.Query(new CreateByTypeQuery.FindTypeByName() { Type = Property.GenericType }));

            return propertyType;
        }
    }
}