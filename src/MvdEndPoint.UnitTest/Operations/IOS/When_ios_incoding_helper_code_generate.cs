namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.IO;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using System.Linq;
    using Incoding.Endpoint;

    #endregion

    [Subject(typeof(IosIncodingHelperCodeGenerateQuery))]
    public class When_ios_incoding_helper_code_generate
    {
        #region Establish value

        static void Verify(FileOfIos file)
        {
            var query = Pleasure.Generator.Invent<IosIncodingHelperCodeGenerateQuery>(dsl => dsl.Tuning(r => r.BaseUrl, "http://localhost:48801/")
                                                                                                .Tuning(r => r.Imports, new[] { "File", "File2" }.ToList())
                                                                                                .Tuning(r => r.File, file));
            string expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, typeof(When_ios_incoding_helper_code_generate).Name + "_" + file.ToString().ToUpper()));

            var mockQuery = MockQuery<IosIncodingHelperCodeGenerateQuery, string>
                    .When(query);

            mockQuery.Execute();
            mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
        }

        #endregion

        It should_be_h = () => Verify(FileOfIos.H);

        It should_be_m = () => Verify(FileOfIos.M);
    }
}