namespace CloudIn.Domain.Endpoint
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using Incoding;
    using Incoding.Data;
    using Incoding.Quality;
    using JetBrains.Annotations;

    #endregion

    public class Message : IncEntityBase
    {
        private IList<Property> properties = new List<Property>();

        [IgnoreCompare("Auto")]
        public new virtual string Id { get; protected set; }

        public virtual string Name { get; set; }

        public virtual string Type { get; set; }

        public virtual string Description { get; set; }

        public virtual IList<Property> Properties { get { return this.properties; } set { this.properties = value; } }

        public virtual Group GroupKey { get; set; }

        public virtual int? Jira { get; set; }

        public virtual string Result { get; set; }

        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class Map : NHibernateEntityMap<Message>
        {
            protected Map()
            {
                Table("Endpoint_Message_Tbl");
                IdGenerateByGuid(r => r.Id);
                MapEscaping(r => r.Name).Length(int.MaxValue);
                MapEscaping(r => r.Jira);
                MapEscaping(r => r.Description).Length(int.MaxValue);
                MapEscaping(r => r.Result);
                MapEscaping(r => r.Type);
                DefaultHasMany(r => r.Properties).Cascade.AllDeleteOrphan();
                DefaultReference(r => r.GroupKey);
            }
        }

        public class Property : IncEntityBase
        {
            public enum TypeOf
            {
                Response,

                Request
            }

            [IgnoreCompare("Auto")]
            public new virtual string Id { get; protected set; }

            public virtual string Name { get; set; }

            public virtual Message Message { get; set; }

            public virtual string Default { get; set; }

            public virtual string Description { get; set; }

            public virtual string PropertyType { get; set; }

            public virtual string GenericType { get; set; }

            public virtual string GroupKey { get; set; }

            public virtual bool IsRequired { get; set; }

            public virtual TypeOf Type { get; set; }

            public virtual Property Parent { get; set; }

            public virtual IList<Property> Childs { get; set; }

            [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
            public class Map : NHibernateEntityMap<Property>
            {
                protected Map()
                {
                    Table("Endpoint_Property_Tbl");
                    IdGenerateByGuid(r => r.Id);
                    DefaultReference(r => r.Message);
                    DefaultReference(r => r.Parent).Cascade.SaveUpdate();
                    HasMany(r => r.Childs).Cascade.DeleteOrphan();
                    MapEscaping(r => r.Name).Length(int.MaxValue);
                    MapEscaping(r => r.Type);
                    MapEscaping(r => r.Default);
                    MapEscaping(r => r.Description).Length(int.MaxValue);
                    MapEscaping(r => r.PropertyType);
                    MapEscaping(r => r.GenericType);
                    MapEscaping(r => r.GroupKey);
                    MapEscaping(r => r.IsRequired);
                }
            }

            public class Where
            {
                public class ByMessage : Specification<Property>
                {
                    private readonly string id;

                    public ByMessage(string id)
                    {
                        this.id = id;
                    }

                    public override Expression<Func<Property, bool>> IsSatisfiedBy()
                    {
                        return property => property.Message.Id == id;
                    }
                }

                public class ByType : Specification<Property>
                {
                    private readonly TypeOf request;

                    public ByType(TypeOf request)
                    {
                        this.request = request;
                    }

                    public override Expression<Func<Property, bool>> IsSatisfiedBy()
                    {
                        return property => property.Type == this.request;
                    }
                }
            }
        }

        public class Group : IncEntityBase
        {
            [IgnoreCompare("Auto")]
            public new virtual string Id { get; protected set; }

            public virtual string Name { get; set; }

            public virtual string Description { get; set; }

            [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
            public class Map : NHibernateEntityMap<Group>
            {
                public Map()
                {
                    Table("Endpoint_Group_Tbl");
                    IdGenerateByGuid(r => r.Id);
                    MapEscaping(r => r.Name);
                    MapEscaping(r => r.Description);
                }
            }

            public abstract class Where
            {
                public class ByName : Specification<Group>
                {
                    private readonly string name;

                    public ByName(string name)
                    {
                        this.name = name;
                    }

                    public override Expression<Func<Group, bool>> IsSatisfiedBy()
                    {
                        return group => group.Name == name;
                    }
                }
            }
        }

        public abstract class Where
        {
            public class ByFullName : Specification<Message>
            {
                private readonly string fullName;

                public ByFullName(string fullName)
                {
                    this.fullName = fullName;
                }

                public override Expression<Func<Message, bool>> IsSatisfiedBy()
                {
                    return endpoint => endpoint.Type == this.fullName;
                }
            }
        }

        public abstract class Order
        {
            public class Default : OrderSpecification<Message>
            {
                public override Action<AdHocOrderSpecification<Message>> SortedBy()
                {
                    return specification => specification.OrderByDescending(r => r.GroupKey.Name)
                                                         .OrderBy(r => r.Name);
                }
            }
        }
    }
}