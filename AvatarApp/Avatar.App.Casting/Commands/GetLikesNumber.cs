using MediatR;

namespace Avatar.App.Casting.Commands
{
    public class GetLikesNumber: IRequest<int>
    {
        public GetLikesNumber(long userId)
        {
            UserId = userId;
        }

        public long UserId { get; }
    }
}
