namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using Incoding.CQRS;

    #endregion

    public class TestServerErrorCommand : CommandBase
    {
        public override void Execute()
        {
            throw new ArgumentException("Test");
        }
    }
}