namespace UsefulCsharpCommonsUtils.work.backgroundworker
{
    public interface IGenericWorkBackground
    {
        bool IsCompleted { get; }
        void SendCancel();
    }
}