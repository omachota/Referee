using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Referee.Infrastructure
{
	/// /// <summary>
	/// Base class from DynamicData library for implementing notify property changes
	/// </summary>
	public abstract class AbstractNotifyPropertyChanged : INotifyPropertyChanged
	{
		/// <summary>
		/// Occurs when a property value has changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Invokes on property changed
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>
		/// If the value has changed, sets referenced backing field and raise notify property changed
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="backingField">The backing field.</param>
		/// <param name="newValue">The new value.</param>
		/// <param name="propertyName">Name of the property.</param>
		protected virtual bool SetAndRaise<T>(ref T backingField, T newValue, [CallerMemberName] string propertyName = null)
		{
			// ReSharper disable once ExplicitCallerInfoArgument
			return SetAndRaise(ref backingField, newValue, EqualityComparer<T>.Default, propertyName);
		}

		/// <summary>
		/// If the value has changed, sets referenced backing field and raise notify property changed
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="backingField">The backing field.</param>
		/// <param name="newValue">The new value.</param>
		/// <param name="comparer">The comparer.</param>
		/// <param name="propertyName">Name of the property.</param>
		protected virtual bool SetAndRaise<T>(ref T backingField, T newValue, IEqualityComparer<T> comparer,
		                                      [CallerMemberName] string propertyName = null)
		{
			comparer ??= EqualityComparer<T>.Default;
			if (comparer.Equals(backingField, newValue))
			{
				return false;
			}

			backingField = newValue;
			OnPropertyChanged(propertyName);
			return true;
		}
	}
}
