using System;
using UniRx;
using UnityEngine;

namespace GameScene.Input
{
	/// <summary>
	/// Интерфейс пользовательского ввода для управления персонажем.
	/// </summary>
	public interface IInput
	{
		/// <summary>
		/// Текущее направление движения.
		/// </summary>
		IReadOnlyReactiveProperty<Vector2> MoveDirection { get; }
		
		/// <summary>
		/// Выстрел.
		/// </summary>
		IObservable<bool> Fire { get; }
	}
}