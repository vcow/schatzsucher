namespace Settings.Character
{
	/// <summary>
	/// Интерфейс настроек персонажа игрока.
	/// </summary>
	public interface IPlayerCharacterSettings
	{
		/// <summary>
		/// Скорость перемещения по умолчанию.
		/// </summary>
		float DefaultSpeed { get; }
		
		/// <summary>
		/// Коэффициент изменения скорости при нахождении персонажа на веревке.
		/// </summary>
		float RopeSpeedModifier { get; }
		
		/// <summary>
		/// Коэффициент изменения скорости при нахождении персонажа на лестнице.
		/// </summary>
		float StairSpeedModifier { get; }
		
		/// <summary>
		/// Скорость падения персонажа, при которой он не получает никаких увечий.
		/// </summary>
		float SafeFallSpeed { get; }
		
		/// <summary>
		/// Скорость падения персонажа, при которой он разбивается насмерть.
		/// </summary>
		float DeadlyFallSpeed { get; }
	}
}