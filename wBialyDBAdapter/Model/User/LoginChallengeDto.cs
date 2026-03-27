namespace wBialyDBAdapter.Model.User
{
    public class LoginChallengeDto
    {
        public int PartialPasswordId { get; set; }
        public List<int> RequiredPositions { get; set; } = new();
        public int TotalPasswordLength { get; set; }
    }
}
