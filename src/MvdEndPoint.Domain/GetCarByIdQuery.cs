namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.CQRS;
    using Incoding.Quality;
    using JetBrains.Annotations;

    #endregion

    [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, false), ExcludeFromCodeCoverage]
    public class GetCarByIdQuery : QueryBase<GetCarByIdQuery.Response>
    {
        #region Properties

        public string Id { get; set; }

        #endregion

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

        protected override Response ExecuteResult()
        {
            return new Response
                       {
                               Id = Id, 
                               Brand = "Audi", 
                               Model = "A4", 
                               Seating = 4
                       };
        }
    }
}