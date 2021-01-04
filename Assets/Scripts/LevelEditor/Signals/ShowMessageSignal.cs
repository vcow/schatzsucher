using System;

namespace LevelEditor.Signals
{
	/// <summary>
	/// Флаги отображаемых в окне сообщений кнопок.
	/// </summary>
	[Flags]
	public enum MessageBoxButton
	{
		None = 0x00,
		Ok = 0x01,
		Cancel = 0x02,
		Yes = 0x04,
		No = 0x08
	}

	/// <summary>
	/// Делегат ответа окна сообщений.
	/// </summary>
	/// <param name="result">Флаг выбранной пользователем кнопки в окне сообщений.</param>
	public delegate void MessageBoxCallback(MessageBoxButton result);
	
	/// <summary>
	/// Сигнал показа окна сообщений.
	/// </summary>
	public class ShowMessageSignal
	{
		public string Message; /// Текст сообщения.
		public MessageBoxButton Buttons = MessageBoxButton.None; /// Набор отображаемых кнопок.
		public MessageBoxCallback Callback; /// Коллбек для получения ответа окна сообщений.
	}
}