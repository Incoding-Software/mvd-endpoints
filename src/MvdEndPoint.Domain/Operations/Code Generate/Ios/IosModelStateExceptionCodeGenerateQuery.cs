namespace Incoding.Endpoint
{
    using System;
    using Incoding.CQRS;

    public class IosModelStateExceptionCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public FileOfIos File { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            switch (this.File)
            {
                case FileOfIos.H:
                    var tmplH = new Ios_ModelStateExcpetion_h();                    
                    return tmplH.TransformText();
                case FileOfIos.M:
                    var tmplM = new Ios_ModelStatreExcpetion_m();                    
                    return tmplM.TransformText();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}