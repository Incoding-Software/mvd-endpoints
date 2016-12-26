namespace Incoding.Endpoint
{
    #region << Using >>

    using System.Linq;
    using System.Web.Mvc;
    using CloudIn.Domain.Endpoint;
    using Incoding.CQRS;

    #endregion

    public class MessageCommand
    {
        public class UpdateNameCommand : CommandBase
        {
            public string Id { get; set; }

            public string Name { get; set; }

            protected override void Execute()
            {
                var message = Repository.GetById<Message>(Id);
                message.Name = Name;
            }
        }

        public class UpdateGroupNameCommand : CommandBase
        {
            public string Id { get; set; }

            public string Name { get; set; }

            protected override void Execute()
            {
                var message = Repository.GetById<Message.Group>(Id);
                message.Name = Name;
            }
        }

        public class UpdateGroupDescriptionCommand : CommandBase
        {
            public string Id { get; set; }

            [AllowHtml]
            public string Description { get; set; }

            protected override void Execute()
            {
                var message = Repository.GetById<Message.Group>(Id);
                message.Description = Description;
            }
        }

        public class UpdateDescriptionCommand : CommandBase
        {
            public string Id { get; set; }

            [AllowHtml]
            public string Description { get; set; }

            protected override void Execute()
            {
                var message = Repository.GetById<Message>(Id);
                message.Description = Description;
            }
        }

        public class JoinToGroupCommand : CommandBase
        {
            public string MessageId { get; set; }

            public string Group { get; set; }

            protected override void Execute()
            {
                if (string.IsNullOrWhiteSpace(Group))
                    return;

                var message = Repository.GetById<Message>(MessageId);
                message.GroupKey = Repository.Query(whereSpecification: new Message.Group.Where.ByName(Group))
                                             .FirstOrDefault() ?? new Message.Group()
                                                                  {
                                                                          Name = Group
                                                                  };
                Repository.SaveOrUpdate(message);
            }
        }

        public class UpdatePropertyDescriptionCommand : CommandBase
        {
            public string Id { get; set; }

            [AllowHtml]
            public string Description { get; set; }

            protected override void Execute()
            {
                var message = Repository.GetById<Message.Property>(Id);
                message.Description = Description;
            }
        }

        public class TogglePropertyOptionalCommand : CommandBase
        {
            public string Id { get; set; }

            protected override void Execute()
            {
                var message = Repository.GetById<Message.Property>(Id);
                message.IsRequired = !message.IsRequired;
            }
        }

    }
}