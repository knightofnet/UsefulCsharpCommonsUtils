namespace UsefulCsharpCommonsUtils.ui.linker
{

    public interface IUiLinker<T> where T : class
    {
        void LoadsWith(T obj);

        T UpdateObj(T enviro);
    }


}
