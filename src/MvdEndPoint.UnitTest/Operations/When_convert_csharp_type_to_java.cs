namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(ConvertCSharpTypeToJavaQuery))]
    public class When_convert_csharp_type_to_java
    {
        #region Establish value

        static void Compare(Type csharp, string java)
        {
            var query = Pleasure.Generator.Invent<ConvertCSharpTypeToJavaQuery>(dsl => dsl.Tuning(r => r.Type, csharp));
            query.Execute();
            query.Result.ShouldEqual(java);
        }

        #endregion

        It should_be_bool = () => Compare(typeof(bool), "Boolean");

        It should_be_bool_nullable = () => Compare(typeof(bool?), "java.lang.Boolean");

        It should_be_byte = () => Compare(typeof(byte), "byte");

        It should_be_sbyte = () => Compare(typeof(sbyte), "byte");
    }
}