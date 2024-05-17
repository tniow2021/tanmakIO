namespace sex
{
    public enum IsSuccess
    {
        Success,
        failure
    }
    public interface Machine
    {
        public void Init();
        public IsSuccess Start();
        public void Stop();
    }
}
