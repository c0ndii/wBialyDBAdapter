namespace wBialyDBAdapter.Model.User
{
    public class LoginAttemptResultDto
    {
        public bool IsSuccess { get; set; }
        public UserGetDto? User { get; set; }
        public string? Message { get; set; }
        public int? RetryAfterSeconds { get; set; }
    }
}
