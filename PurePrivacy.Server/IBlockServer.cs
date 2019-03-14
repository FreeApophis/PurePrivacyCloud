namespace PurePrivacy.Server
{
    public interface IBlockServer
    {
        int Port { get; }

        void Run();
    }
}