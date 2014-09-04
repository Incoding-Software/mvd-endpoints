namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using MvdEndPoint.Domain.Operations.Code_Generate.Ios;

    #endregion

    public class IosEnumCodeGenerateQuery : QueryBase<string>
    {
        #region Properties

        public Type Type { get; set; }

        public FileOfIos File { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            switch (File)
            {
                case FileOfIos.H:
                    var tmplH = new Ios_Enum_h();
                    tmplH.Initialize();
                    return tmplH.TransformText();
                case FileOfIos.M:
                    var tmplM = new Ios_Enum_m();
                    tmplM.Initialize();
                    return tmplM.TransformText();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}