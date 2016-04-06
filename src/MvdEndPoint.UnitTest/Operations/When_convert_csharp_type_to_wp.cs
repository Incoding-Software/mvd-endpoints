namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(ConvertCSharpTypeToTargetQuery.ToWPQuery))]
    public class When_convert_csharp_type_to_wp
    {
        It should_be_nullable = () => { Compare(typeof(int?), "Int32?"); };

        It should_be_simple = () => { Compare(typeof(string), "String"); };

        static void Compare(Type csharp, string wp)
        {
            var query = Pleasure.Generator.Invent<ConvertCSharpTypeToTargetQuery.ToWPQuery>(dsl => dsl.Tuning(r => r.Type, csharp));
            var mock = MockQuery<ConvertCSharpTypeToTargetQuery.ToWPQuery, string>.When(query);
            mock.Execute();
            mock.ShouldBeIsResult(wp);
        }
    }
}