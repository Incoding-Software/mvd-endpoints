namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.Data;
    using Incoding.Quality;
    using JetBrains.Annotations;

    #endregion

    public class Assembly : IncEntityBase
    {
        #region Properties

        public virtual string Name { get; set; }

        public virtual byte[] File { get; set; }

        public virtual string Domain { get; set; }

        #endregion

        #region Nested classes

        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class Map : NHibernateEntityMap<Assembly>
        {
            #region Constructors

            protected Map()
            {
                IdGenerateByGuid(r => r.Id);
                MapEscaping(r => r.File);
            }

            #endregion
        }

        #endregion
    }
}