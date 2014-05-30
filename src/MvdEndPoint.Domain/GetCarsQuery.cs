namespace MvdEndPoint.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.CQRS;
    using Incoding.Quality;
    using JetBrains.Annotations;

    [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, false), ExcludeFromCodeCoverage]
    public class GetCarsQuery : QueryBase<List<GetCarsQuery.Response>>
    {
        #region Nested classes

        public class Response
        {
            #region Properties

            public string Brand { get; set; }

            public string Model { get; set; }

            public int Seating { get; set; }

            public string Id { get; set; }

            #endregion
        }

        #endregion

        protected override List<Response> ExecuteResult()
        {
            return new List<Response>
                       {
                               new Response { Brand = "Audi", Model = "A4", Seating = 4 },
                               new Response { Brand = "Audi", Model = "Q7", Seating = 6 },
                       };
        }
    }
}