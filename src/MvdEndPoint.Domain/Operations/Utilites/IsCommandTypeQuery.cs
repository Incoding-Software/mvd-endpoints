namespace Incoding.Endpoint
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.CQRS;
    using Incoding.Quality;
    using JetBrains.Annotations;

    #endregion

    public class IsCommandTypeQuery : QueryBase<bool>
    {
        [UsedImplicitly, Obsolete(ObsoleteMessage.SerializeConstructor, true), ExcludeFromCodeCoverage]
        public IsCommandTypeQuery() { }

        public IsCommandTypeQuery(Type type)
        {
            Type = type;
        }

        #region Properties

        public Type Type { get; set; }

        #endregion

        protected override bool ExecuteResult()
        {
            var baseType = Type.BaseType;
            while (baseType != typeof(object))
            {
                if (baseType == typeof(CommandBase))
                    return true;
                if (baseType.Name == "QueryBase`1")
                    return false;

                baseType = baseType.BaseType;
            }

            throw new ArgumentException("Type does not satisfied for message base", "Type");
        }
    }
}