namespace MvdEndPoint.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.IO;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Machine.Specifications.Annotations;
    using MvdEndPoint.Domain;

    #endregion

    [Subject(typeof(IosTaskCodeGenerateQuery))]
    public class When_ios_task_code_generate
    {
        #region Fake classes

        class FakeQuery : QueryBase<FakeQuery.Response>
        {
            #region Properties

            [UsedImplicitly]
            public string Id { get; set; }

            #endregion

            #region Nested classes

            public class Response
            {
                #region Properties

                [UsedImplicitly]
                public string Name { get; set; }

                #endregion
            }

            #endregion

            protected override Response ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Establish value

        static void Verify(FileOfIos fileOfIos)
        {
            var query = Pleasure.Generator.Invent<IosTaskCodeGenerateQuery>(dsl => dsl.Tuning(r => r.File, fileOfIos)
                                                                                      .Tuning(r => r.Type, typeof(FakeQuery)));
            string expected = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, typeof(When_ios_task_code_generate).Name + "_" + fileOfIos.ToString().ToLower()));

            Func<GetNameFromTypeQuery.ModeOf, GetNameFromTypeQuery> createByName = modeOf => Pleasure.Generator.Invent<GetNameFromTypeQuery>(dsl => dsl.Tuning(r => r.Mode, modeOf)
                                                                                                                                                       .Tuning(r => r.Type, query.Type));

            var mockQuery = MockQuery<IosTaskCodeGenerateQuery, string>
                    .When(query)
                    .StubQuery(createByName(GetNameFromTypeQuery.ModeOf.Listener), "IFakeOn")
                    .StubQuery(createByName(GetNameFromTypeQuery.ModeOf.Request), "FakeRequest")
                    .StubQuery(createByName(GetNameFromTypeQuery.ModeOf.Response), "Response")
                    .StubQuery(createByName(GetNameFromTypeQuery.ModeOf.Task), "FakeTask")
                    .StubQuery(Pleasure.Generator.Invent<GetPropertiesByTypeQuery>(dsl => dsl.Tuning(r => r.Device, DeviceOfType.Ios)
                                                                                             .Tuning(r => r.Type, typeof(FakeQuery))), new List<GetPropertiesByTypeQuery.Response> { Pleasure.Generator.Invent<GetPropertiesByTypeQuery.Response>() });

            mockQuery.Original.Execute();
            mockQuery.ShouldBeIsResult(s => s.ShouldEqual(expected));
        }

        #endregion

        It should_be_h = () => Verify(FileOfIos.H);

        It should_be_m = () => Verify(FileOfIos.M);
    }
}