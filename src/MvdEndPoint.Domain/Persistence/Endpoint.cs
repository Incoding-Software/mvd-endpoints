namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.Data;
    using Incoding.Quality;
    using JetBrains.Annotations;

    #endregion

    public class Endpoint : IncEntityBase
    {
        #region Properties

        public string AssemblyQualifiedName { get; set; }

        #endregion

        #region Nested classes

        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class Map : NHibernateEntityMap<Endpoint>
        {
            ////ncrunch: no coverage start
            #region Constructors

            protected Map()
            {
                MapEscaping(r => r.AssemblyQualifiedName);
            }

            #endregion

            ////ncrunch: no coverage end        
        }

        #endregion
    }
}