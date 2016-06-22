namespace MvdEndPoint.UnitTest.UI
{
    #region << Using >>

    using CloudIn.Domain.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(Message.Group))]
    public class When_save_MessageGroup : SpecWithPersistenceSpecification<Message.Group>
    {
        It should_be_verify = () => persistenceSpecification.VerifyMappingAndSchema();
    }
}