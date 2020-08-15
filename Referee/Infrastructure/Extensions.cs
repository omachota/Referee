using System;
using Referee.Models;

namespace Referee.Infrastructure
{
	public static class Extensions
	{
		public static bool ValidateRozhodci(Rozhodci rozhodci)
		{
			if (rozhodci == null)
				return false;
			if (rozhodci.FirstName == "")
				return false;
			if (rozhodci.LastName == "")
				return false;
			if (rozhodci.Address == "")
				return false;
			if (rozhodci.City == "")
				return false;
			if (rozhodci.BirthDate >= DateTime.Today)
				return false;
			return true;
		}

		public static void CopyValuesFrom(this Rozhodci rozhodci, Rozhodci rozhodciCache)
		{
			rozhodci.FirstName = rozhodciCache.FirstName;
			rozhodci.LastName = rozhodciCache.LastName;
			rozhodci.BirthDate = rozhodciCache.BirthDate;
			rozhodci.Address = rozhodciCache.Address;
			rozhodci.City = rozhodciCache.City;
		}
	}
}
