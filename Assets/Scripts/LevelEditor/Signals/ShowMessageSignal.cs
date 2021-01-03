using System;

namespace LevelEditor.Signals
{
	[Flags]
	public enum MessageBoxButton
	{
		None = 0x00,
		Ok = 0x01,
		Cancel = 0x02,
		Yes = 0x04,
		No = 0x08
	}

	public delegate void MessageBoxCallback(MessageBoxButton result);
	
	public class ShowMessageSignal
	{
		public string Message;
		public MessageBoxButton Buttons = MessageBoxButton.None;
		public MessageBoxCallback Callback;
	}
}