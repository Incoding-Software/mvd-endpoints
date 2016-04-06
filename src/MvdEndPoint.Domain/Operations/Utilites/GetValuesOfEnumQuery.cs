namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class GetValuesOfEnumQuery : QueryBase<List<GetValuesOfEnumQuery.Response>>
    {
        public Type Type { get; set; }

        protected override List<Response> ExecuteResult()
        {
            return Enum.GetValues(Type)
                       .Cast<Enum>()
                       .Select(r => new Response()
                                    {
                                            AsInt = r.ToString("d"),
                                            AsString = r.ToString(),
                                            Display = r.ToLocalization()
                                    })
                       .ToList();
        }

        public class Response
        {
            public string AsInt { get; set; }

            public string AsString { get; set; }

            public string Display { get; set; }
        }
    }
}