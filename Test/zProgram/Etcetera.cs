namespace sex
{
    public enum IsSuccess
    {
        Success,
        failure
    }
    public interface Machine
    {
        public IsSuccess Start();
        public void Stop();
    }
}
