namespace Incoding.Endpoint
{
    #region << Using >>

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using CloudIn.Domain.Endpoint;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class GetMessageDetailQuery : QueryBase<List<GetMessageDetailQuery.Response>>
    {
        public string Id { get; set; }

        protected override List<Response> ExecuteResult()
        {
            return Repository.Query(whereSpecification: new Message.Property.Where.ByMessage(Id)
                                            .And(new Message.Property.Where.ByType(Message.Property.TypeOf.Request)))
                             .ToList()
                             .Select(s =>
                                     {
                                         var type = Dispatcher.Query(new GetTypeFromPropertyQuery() { Property = s });
                                         return new Response()
                                                {
                                                        Name = s.Name,
                                                        IsArray = !type.IsAnyEquals(typeof(string), typeof(byte[])) && type.IsImplement<IEnumerable>(),
                                                        IsBool = type.IsAnyEquals(typeof(bool), typeof(bool?)),
                                                        IsFile = type.IsAnyEquals(typeof(byte[]), typeof(byte), typeof(HttpPostedFileBase), typeof(HttpPostedFile)),
                                                        IsDate = type.IsAnyEquals(typeof(DateTime), typeof(DateTime?)),
                                                        IsNumber = type.IsAnyEquals(typeof(int), typeof(int?), typeof(Decimal), typeof(Decimal?), typeof(float), typeof(float?), typeof(Int64), typeof(Int64?)),
                                                        IsEnum = type.IsEnum,
                                                        GUID = type.GUID
                                                };
                                     })
                             .ToList();
        }

        public class Response
        {
            public string Name { get; set; }

            public bool IsDate { get; set; }

            public bool IsNumber { get; set; }

            public bool IsBool { get; set; }

            public bool IsEnum { get; set; }

            public bool IsString { get { return !IsBool && !IsEnum && !IsFile && !IsDate && !IsNumber; } }

            public bool IsFile { get; set; }

            public Guid GUID { get; set; }

            public bool IsArray { get; set; }
        }
    }
}