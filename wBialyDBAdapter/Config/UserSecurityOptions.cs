namespace wBialyDBAdapter.Config
{
    public class UserSecurityOptions
    {
        public int BaseDelaySeconds { get; set; } = 1;
        public int DelayStepSeconds { get; set; } = 1;
        public int MaxDelaySeconds { get; set; } = 60;
    }
}
