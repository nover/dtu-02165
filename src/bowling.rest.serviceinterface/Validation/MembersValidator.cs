using Bowling.Rest.Service.Model.Operations;
using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Rest.Service.Interface.Validation
{
    public class MembersValidator : AbstractValidator<Members>
    {
        public IEmailValidator EmailValidator { get; set; }

        public MembersValidator()
        {
            RuleSet(ServiceStack.ServiceInterface.ApplyTo.Post, () =>
                {
                    RuleFor(r => r.Member.Name).NotEmpty();
                    RuleFor(r => r.Member.Password).NotEmpty();
                    RuleFor(r => r.Member.Email).Must(x => EmailValidator.IsValid(x));
                    RuleFor(r => r.Member.ReceiveNewsLetter).NotEmpty();
                });
        }
    }
}
