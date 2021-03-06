﻿namespace Incoding.Endpoint
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
            Guard.NotNull("Type", Type);
            Guard.IsConditional("Type", Type.IsEnum, errorMessage: "{0} provided must be an Enum".F(Type.Name));

            return Enum.GetValues(Type)
                       .Cast<Enum>()
                       .Select(r =>
                               {
                                   var localization = r.ToLocalization();
                                   var asString = r.ToString();
                                   return new Response()
                                          {
                                                  AsInt = r.ToString("d"),
                                                  AsString = asString,
                                                  Display = localization == asString ? string.Empty : localization
                                          };
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