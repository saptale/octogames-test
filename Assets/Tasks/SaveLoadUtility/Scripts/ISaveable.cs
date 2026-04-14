namespace EvgeniiMaklaev.SaveSystem
{
    public interface ISaveable
    {
        public abstract string SaveKey { get; }
        object SaveHandle();
        void LoadHandle(object state);
    }
}
