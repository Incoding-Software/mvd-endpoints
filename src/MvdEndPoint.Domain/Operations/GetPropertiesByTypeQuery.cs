namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Incoding.CQRS;

    #endregion

    public class GetPropertiesByTypeQuery : QueryBase<string>
    {
        public string Type { get; set; }

        protected override string ExecuteResult()
        {
            System.Type.GetType(Type).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                  .Where(r => r.CanWrite)
                  .Select(r =>
                              {
                                  string javaType = Dispatcher.Query(new ConvertCSharpTypeToJavaQuery
                                                                         {
                                                                                 Type = r.PropertyType.Name
                                                                         });
                                  return new KeyValuePair<string, string>(javaType, r.Name);
                              })
                  .ToList();
        }
    }
}