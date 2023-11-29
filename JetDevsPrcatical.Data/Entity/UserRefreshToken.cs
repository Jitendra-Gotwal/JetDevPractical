using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetDevsPrcatical.Data.Entity
{
    public class UserRefreshToken
    {
		public int RefreshTokenId { get; set; }
		public string UserName { get; set; }
		public string RefreshToken { get; set; }
		public string AccessToken { get; set; }
		public DateTime RefreshTokenExpiryTime { get; set; }
		public long InsertedBy { get; set; }
		public long UpdatedBy { get; set; }
		public DateTime InsertedDate { get; set; }
		public DateTime UpdatedDate { get; set; }
	}
}
