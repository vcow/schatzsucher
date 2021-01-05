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
		private readonly ReactiveProperty<Vector2> _moveDirection = new ReactiveProperty<Vector2>(Vector2.zero);
		private readonly Subject<bool> _fire = new Subject<bool>();
		private readonly IDisposable _updateHandler;

		private bool _lastFire;

		public LocalInput()
		{
			_updateHandler = Observable.EveryUpdate().Subscribe(l =>
			{
				var horizontal = UnityEngine.Input.GetAxis("Horizontal");
				var vertical = UnityEngine.Input.GetAxis("Vertical");
				var fire = UnityEngine.Input.GetButton("Fire1");

				if (!MoveDirection.Value.x.Equals(horizontal) || !MoveDirection.Value.y.Equals(vertical))
				{
					_moveDirection.SetValueAndForceNotify(new Vector2(horizontal, vertical));
				}

				if (fire && !_lastFire)
				{
					_fire.OnNext(true);
				}

				_lastFire = fire;
			});
		}

		// IInput
		public IReadOnlyReactiveProperty<Vector2> MoveDirection => _moveDirection;

		public IObservable<bool> Fire => _fire;
		// \IInput

		void IDisposable.Dispose()
		{
			_updateHandler.Dispose();
			_moveDirection.Dispose();

			_fire.OnCompleted();
			_fire.Dispose();
		}
	}
}