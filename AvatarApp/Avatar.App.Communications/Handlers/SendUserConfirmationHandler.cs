using System;
using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Authentication.Commands;
using Avatar.App.Communications.Commands;
using MediatR;
using MimeKit;

namespace Avatar.App.Communications.Handlers
{
    internal abstract class SendUserConfirmationHandler<TCommand>: IRequestHandler<TCommand> where TCommand: SendUserConfirmation
    {
        protected IMediator Mediator;
        protected abstract string ConfirmSubject { get; }

        protected SendUserConfirmationHandler(IMediator mediator)
        {
            Mediator = mediator;
        }

        public async Task<Unit> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var user = request.User;
            var body = CreateBody(user.ConfirmationCode, user.Guid);
            await Mediator.Send(new SendEmail(user.Email, ConfirmSubject, body), cancellationToken);
            return Unit.Value;
        }

        protected abstract MimeEntity CreateBody(string confirmCode, Guid guid);
    }
}
