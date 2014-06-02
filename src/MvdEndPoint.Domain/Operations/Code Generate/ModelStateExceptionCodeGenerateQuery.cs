namespace MvdEndPoint.Domain
{
    #region << Using >>

    using Incoding.CQRS;
    using MvdEndPoint.Domain.Operations.Code_Generate;

    #endregion

    public class ModelStateExceptionCodeGenerateQuery : QueryBase<string>
    {
        protected override string ExecuteResult()
        {
            var template = new Android_ModelStateException();
            return template.TransformText();
        }
    }
}