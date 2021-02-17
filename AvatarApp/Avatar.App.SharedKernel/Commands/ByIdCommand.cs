namespace Avatar.App.SharedKernel.Commands
{
    public abstract class ByIdCommand
    {
        public long Id { get; }

        protected ByIdCommand(long id)
        {
            Id = id;
        }
    }
}
