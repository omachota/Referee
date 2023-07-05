using System;
using Referee.Infrastructure;

namespace Referee.Models;

public class Person : AbstractNotifyPropertyChanged, IPerson
{
	private string _firstName;
	private string _lastName;
	private DateTime _birthDate;
	private string _address;
	private string _city;
	private bool _isSelected;
	private int? _reward;
	
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
	public string FullName => $"{FirstName} {LastName}";
	public string FullNameInverted => $"{LastName} {FirstName}";
}
