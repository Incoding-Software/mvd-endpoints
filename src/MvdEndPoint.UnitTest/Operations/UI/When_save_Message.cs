namespace MvdEndPoint.UnitTest.UI
{
    #region << Using >>

    using CloudIn.Domain.Endpoint;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(Message))]
    public class When_save_Message : SpecWithPersistenceSpecification<Message>
    {
        It should_be_verify = () => persistenceSpecification.VerifyMappingAndSchema();
    }
  
}