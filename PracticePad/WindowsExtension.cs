using System.Threading;
using System.Threading.Tasks;

namespace System.Windows.Controls {
	public static class WindowsExtension {

		public static async void PerformClick(this Button button) {
			PressDown(button);
			await Task.Delay(100);
			PressUp(button);
		}

		public static void PressDown(this Button button) {
			typeof(Button).GetMethod("set_IsPressed", Reflection.BindingFlags.Instance | Reflection.BindingFlags.NonPublic).Invoke(button, new object[] { true });
		}

		public static void PressUp(this Button button) {
			typeof(Button).GetMethod("set_IsPressed", Reflection.BindingFlags.Instance | Reflection.BindingFlags.NonPublic).Invoke(button, new object[] { false });
			button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
		}
	}
}
