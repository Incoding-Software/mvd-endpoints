namespace MvdEndPoint.Domain
{
    using System;
    using System.Web;
    using Incoding.CQRS;
    using Incoding.Extensions;

    public class SignCommand : CommandBase
    {
        public string Id { get; set; }

        public override void Execute()
        {
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("UserId", Id)
                                                         {
                                                                 Expires = DateTime.Now.AddYears(1)
                                                         });
        }
    }
}