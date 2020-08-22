using System;

namespace Referee.Infrastructure
{
	public static class DateHelper
	{
		public static DateTime Tomorrow => DateTime.Today.AddDays(1);
	}
}
