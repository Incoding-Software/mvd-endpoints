namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Incoding.CQRS;
    using MvdEndPoint.Domain.Operations;

    #endregion

    public class TaskCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public string Type { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            var queryType = System.Type.GetType(Type);

            var task = new Android_Task();
            task.Session = new Dictionary<string, object>
                               {
                                       { "Name", queryType.Name.Replace("Query", "Task") },
                                       { "Listener", "I" + queryType.Name.Replace("Query", "On") },
                                       { "Request", queryType.Name.Replace("Query", "Request") },
                                       { "Response", queryType.BaseType.GetGenericArguments()[0].Name.Replace("Query", "Response") },
                                       {
                                               "ToJson", queryType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                                                                  .Where(r => r.CanWrite)
                                                                  .Select(r =>
                                                                              {
                                                                                  string javaType = Dispatcher.Query(new GetPropertiesByTypeQuery
                                                                                                                         {
                                                                                                                                 Type = r.PropertyType.Name
                                                                                                                         });
                                                                                  return new KeyValuePair<string, string>(javaType, r.Name);
                                                                              })
                                                                  .ToList()
                                       }
                               };

            task.Initialize();
            return task.TransformText();
        }
    }
}