namespace MvdEndPoint.Domain
{
    #region << Using >>

    using System.Collections.Generic;
    using System.IO;
    using Incoding.CQRS;
    using Ionic.Zip;

    #endregion

    public class ToZipQuery : QueryBase<byte[]>
    {
        #region Constructors

        public ToZipQuery()
        {
            Entries = new Dictionary<string, string>();
        }

        #endregion

        #region Properties

        public Dictionary<string, string> Entries { get; set; }

        #endregion

        protected override byte[] ExecuteResult()
        {
            using (var zip = new ZipFile())
            {                
                foreach (var pair in Entries)
                    zip.AddEntry(pair.Key, pair.Value);
                using (var stream = new MemoryStream())
                {
                    zip.Save(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}