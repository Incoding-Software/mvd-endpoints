namespace MvdEndPoint.Domain
{
    #region << Using >>

    using Incoding.CQRS;
    using MvdEndPoint.Domain.Operations.Code_Generate;

    #endregion

    public class IncodingHelperCodeGenerateQuery : QueryBase<string>
    {
        protected override string ExecuteResult()
        {
            var template = new Android_IncodingHelper();
            return template.TransformText();
        }
    }
}