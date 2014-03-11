namespace MvdEndPoint.Domain
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class TestCommand : CommandBase
    {
        #region Properties

        public string Prop { get; set; }

        public bool PropBool { get; set; }

        #endregion

        public override void Execute()
        {
            Result = "<h3>{0}</h3>".F(Prop);
        }
    }
}