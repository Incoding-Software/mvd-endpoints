namespace MvdEndPoint.Domain
{
    #region << Using >>

    using Incoding;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class TestModelStateCommand : CommandBase
    {
        #region Properties

        public string Prop { get; set; }

        #endregion

        public override void Execute()
        {
            throw IncWebException.For<TestModelStateCommand>(r => r.Prop, "Value {0} is wrong".F(Prop));
        }
    }
}