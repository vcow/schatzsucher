using System;
using UniRx;
using UnityEngine;

namespace GameScene.Input
{
	/// <summary>
	/// Ввод для второго персонажа с удаленного клиента.
	/// </summary>
	public class RemoteInput : IInput
	{
		public Vector2 MoveDirection => throw new NotImplementedException();
		public bool Fire => throw new NotImplementedException();
	}
}