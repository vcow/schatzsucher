using UniRx;
using UnityEngine;

namespace Model.Character
{
	/// <summary>
	/// Интерфейс игрока.
	/// </summary>
	public interface IPlayerCharacter
	{
		/// <summary>
		/// Уникальный идентификатор игрока.
		/// </summary>
		string Id { get; }
		
		/// <summary>
		/// Количество жизней игрока.
		/// </summary>
		IReadOnlyReactiveProperty<int> NumLives { get; }
		
		/// <summary>
		/// Текущий уровень здоровья (0..1).
		/// </summary>
		IReadOnlyReactiveProperty<float> Health { get; }
		
		/// <summary>
		/// Текущая позиция на уровне.
		/// </summary>
		IReadOnlyReactiveProperty<Vector2Int> Position { get; }
		
		/// <summary>
		/// Признак активности персонажа.
		/// </summary>
		IReadOnlyReactiveProperty<bool> IsActive { get; }
	}
}