using System;

namespace Referee.Models
{
	public interface IPerson
	{
		int Id { get; }
		string FirstName { get; set; }
		string LastName { get; set; }
		DateTime BirthDate { get; set; }
		string Address { get; set; }
		string City { get; set; }
		bool IsSelected { get; set; }
		int? Reward { get; set; }
	}
}
