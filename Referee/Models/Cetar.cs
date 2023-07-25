using System;

namespace Referee.Models
{
	public class Cetar : Person
	{
		public Cetar()
		{
		}

		private Cetar(int id, string firstName, string lastName, DateTime birthDate, string address, string city)
		{
			Id = id;
			FirstName = firstName;
			LastName = lastName;
			BirthDate = birthDate;
			Address = address;
			City = city;
			IsSelected = false;
		}
		
		public Cetar(Cetar cetar) : this(cetar.Id, cetar.FirstName, cetar.LastName, cetar.BirthDate, cetar.Address,
			cetar.City)
		{
		}

		public static Cetar CreateEmpty()
		{
			return new Cetar(0, "", "", DateTime.Now, "", "");
		}
	}
}
