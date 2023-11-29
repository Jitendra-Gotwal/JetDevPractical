namespace JetDevsPrcatical.Data.Entity
{
    /// <summary>
    /// User Entity
    /// </summary>
    public class Users
    {
        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// FirstName
        /// </summary>
        public required string FirstName { get; set; }

        /// <summary>
        /// LastName
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// UserPhone
        /// </summary>
        public string? UserPhone { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string? Token { get; set; }
        
    }
}
