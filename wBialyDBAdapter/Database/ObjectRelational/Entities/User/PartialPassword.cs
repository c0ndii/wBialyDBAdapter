namespace wBialyDBAdapter.Database.ObjectRelational.Entities.User
{
    public class PartialPassword
    {
        public int PartialPasswordId { get; set; }
        public int UserSecurityProfileId { get; set; }
        public UserSecurityProfile UserSecurityProfile { get; set; } = null!;
        public int SlotNumber { get; set; }
        public int PasswordLength { get; set; }
        public string RequiredPositions { get; set; } = string.Empty;
        public string CharacterHashes { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
    }
}
