namespace MvdEndPoint.UnitTest.UI
{
    using CloudIn.Domain.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    [Subject(typeof(Message.Property))]
    public class When_save_Message_Property : SpecWithPersistenceSpecification<Message.Property>
    {
        It should_be_verify = () => persistenceSpecification.VerifyMappingAndSchema();
    }
}