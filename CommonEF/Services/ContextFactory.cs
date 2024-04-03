namespace CommonEF.Services
{
    public class ContextFactory : IContextFactory
    {
        public ICryogatt Create()
            => new Cryogatt();
    }
}
