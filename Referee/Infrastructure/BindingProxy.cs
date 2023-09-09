using System.Windows;

namespace Referee.Infrastructure
{
	// https://learn.microsoft.com/en-us/answers/questions/30569/how-can-i-access-a-vm-object-declared-in-main-wind
	public class BindingProxy : Freezable
	{
		protected override Freezable CreateInstanceCore() => new BindingProxy();

		public object Data
		{
			get => GetValue(DataProperty);
			set => SetValue(DataProperty, value);
		}

		public static readonly DependencyProperty DataProperty =
			DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));
	}
}
