namespace MvdEndPoint.Domain
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public enum TestOfEnum
    {
        Value,

        Value2,

        Value3
    }

    public class TestCommand : CommandBase
    {
        #region Properties

        public string Prop { get; set; }

        public bool PropBool { get; set; }

        public TestOfEnum PropEnum { get; set; }

        #endregion

        public override void Execute()
        {
            Result = "<h3>Prop:{0},PropBool:{1},PropEnum{2}</h3>".F(Prop, PropBool, PropEnum);
        }
    }
}