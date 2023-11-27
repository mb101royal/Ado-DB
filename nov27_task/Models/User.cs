using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nov27_task.Models
{
	public class User
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Surname { get; set; }
		public string Username { get; set; }
		public byte[] Password { get; set; }
		public byte[] Salt { get; set; }
	}
}
