using System;
using Referee.Infrastructure;

namespace Referee.Models
{
	public class Rozhodci : AbstractNotifyPropertyChanged, IPerson
	{
		private string _firstName;
		private string _lastName;
		private DateTime _birthDate;
		private string _address;
		private string _city;
		private string _email;
		private string _class;
		private string _telephoneNumber;
		private string _registrationNumber;
		private string _bankAccountNumber;
		private bool _isSelected;
		private int? _reward;

		public Rozhodci()
		{
		}

		public Rozhodci(int id, string firstName, string lastName, DateTime birthDate, string address, string city)
		{
			Id = id;
			_firstName = firstName;
			_lastName = lastName;
			_birthDate = birthDate;
			_address = address;
			_city = city;
			_isSelected = false;
		}

		public Rozhodci(Rozhodci rozhodci) : this(rozhodci.Id, rozhodci.FirstName, rozhodci.LastName, rozhodci.BirthDate, rozhodci.Address,
			rozhodci.City)
		{
		}

		public static Rozhodci CreateEmpty()
		{
			return new Rozhodci(0, "", "", DateTime.Now, "", "");
		}

		public int Id { get; set; }

		public string FirstName
		{
			get => _firstName;
			set => SetAndRaise(ref _firstName, value);
		}

		public string LastName
		{
			get => _lastName;
			set => SetAndRaise(ref _lastName, value);
		}

		public DateTime BirthDate
		{
			get => _birthDate;
			set => SetAndRaise(ref _birthDate, value);
		}

		public string Address
		{
			get => _address;
			set => SetAndRaise(ref _address, value);
		}

		public string City
		{
			get => _city;
			set => SetAndRaise(ref _city, value);
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

		public bool IsSelected
		{
			get => _isSelected;
			set => SetAndRaise(ref _isSelected, value);
		}

		public int? Reward
		{
			get => _reward;
			set => SetAndRaise(ref _reward, value);
		}

		public string FullName => $"{_firstName} {_lastName}";
	}
}
