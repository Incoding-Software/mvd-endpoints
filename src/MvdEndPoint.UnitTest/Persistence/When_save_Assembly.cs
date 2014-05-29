namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(Assembly))]
    public class When_save_Assembly : SpecWithPersistenceSpecification<Assembly>
    {
        Because of = () => persistenceSpecification
                                   .CheckProperty(r => r.Domain, Pleasure.Generator.String())
                                   .CheckProperty(r => r.File, Pleasure.Generator.Bytes());

        It should_be_verify = () => persistenceSpecification.VerifyMappingAndSchema();
    }
}