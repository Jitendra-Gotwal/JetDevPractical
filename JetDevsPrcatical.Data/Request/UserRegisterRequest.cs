using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace JetDevsPrcatical.Data.Request
{
    /// <summary>
    /// User Register View Model
    /// </summary>
    public class UserRegisterRequest
    {

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
    }
}
