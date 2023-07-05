using System;

namespace Referee.Models
{
	public class Rozhodci : Person
	{
		private string _email;
		private string _class;
		private string _telephoneNumber;
		private string _registrationNumber;
		private string _bankAccountNumber;

		public Rozhodci()
		{
		}

		public Rozhodci(int id, string firstName, string lastName, DateTime birthDate, string address, string city)
		{
			Id = id;
			FirstName = firstName;
			LastName = lastName;
			BirthDate = birthDate;
			Address = address;
			City = city;
			IsSelected = false;
		}

		public Rozhodci(int id, string firstName, string lastName, DateTime birthDate, string address, string city, string email, string @class,
		                string telephoneNumber, string registrationNumber, string bankAccountNumber)
		{
			Id = id;
			FirstName = firstName;
			LastName = lastName;
			BirthDate = birthDate;
			Address = address;
			City = city;
			_email = email;
			_class = @class;
			_telephoneNumber = telephoneNumber;
			_registrationNumber = registrationNumber;
			_bankAccountNumber = bankAccountNumber;
		}

		public Rozhodci(Rozhodci rozhodci) : this(rozhodci.Id, rozhodci.FirstName, rozhodci.LastName, rozhodci.BirthDate, rozhodci.Address,
			rozhodci.City, rozhodci.Email, rozhodci.Class, rozhodci.TelephoneNumber, rozhodci.RegistrationNumber, rozhodci.BankAccountNumber)
		{
		}

		public static Rozhodci CreateEmpty()
		{
			return new Rozhodci(0, "", "", DateTime.Now, "", "");
		}

		public string Email
		{
			get => _email;
			set => SetAndRaise(ref _email, value);
		}

		public string Class
		{
			get => _class;
			set => SetAndRaise(ref _class, value);
		}

		public string TelephoneNumber
		{
			get => _telephoneNumber;
			set => SetAndRaise(ref _telephoneNumber, value);
		}

		public string RegistrationNumber
		{
			get => _registrationNumber;
			set => SetAndRaise(ref _registrationNumber, value);
		}

		public string BankAccountNumber
		{
			get => _bankAccountNumber;
			set => SetAndRaise(ref _bankAccountNumber, value);
		}
	}
}
