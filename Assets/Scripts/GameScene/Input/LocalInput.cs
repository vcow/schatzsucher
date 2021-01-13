using System;
using UniRx;
using UnityEngine;

namespace GameScene.Input
{
	/// <summary>
	/// Локальный ввод для управления персонажем.
	/// </summary>
	public class LocalInput : IInput, IDisposable
	{
		private readonly IDisposable _updateHandler;

		public LocalInput()
		{
			_updateHandler = Observable.EveryUpdate().Subscribe(l =>
			{
				MoveDirection = new Vector2(
					UnityEngine.Input.GetAxis("Horizontal"),
					UnityEngine.Input.GetAxis("Vertical"));
				Fire = UnityEngine.Input.GetButton("Fire1");
			});
		}

		// IInput
		public Vector2 MoveDirection { get; private set; } = Vector2.zero;

		public bool Fire { get; private set; }
		// \IInput

		void IDisposable.Dispose()
		{
			_updateHandler.Dispose();
		}
	}
}