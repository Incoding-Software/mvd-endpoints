namespace MvdEndPoint.Domain
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.MvcContrib;

    #endregion

    public class TestModelStateCommand : CommandBase
    {
        #region Properties

        public string Prop { get; set; }

        #endregion

        public override void Execute()
        {
            throw IncWebException.For<TestModelStateCommand>(r => r.Prop, "Test validation");
        }
    }
}