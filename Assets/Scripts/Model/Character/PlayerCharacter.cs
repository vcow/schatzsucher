using System;
using GameScene.Input;
using UniRx;
using UnityEngine;

namespace Model.Character
{
	public class PlayerCharacter : IPlayerCharacter, IDisposable
	{
		private readonly string _id;

		public readonly IntReactiveProperty NumLives;
		public readonly FloatReactiveProperty Health;
		public readonly ReactiveProperty<Vector2Int> Position;
		public readonly BoolReactiveProperty IsActive;
		
		public IInput Input { get; }

		public PlayerCharacter(IInput input, string id, int numLives = 1, float health = 1f,
			Vector2Int? position = null, bool isActive = false)
		{
			Input = input;
			_id = id;
			NumLives = new IntReactiveProperty(numLives);
			Health = new FloatReactiveProperty(health);
			Position = new ReactiveProperty<Vector2Int>(position ?? Vector2Int.zero);
			IsActive = new BoolReactiveProperty(isActive);
		}

		// IPlayerCharacter
		string IPlayerCharacter.Id => _id;

		IReadOnlyReactiveProperty<int> IPlayerCharacter.NumLives => NumLives;

		IReadOnlyReactiveProperty<float> IPlayerCharacter.Health => Health;

		IReadOnlyReactiveProperty<Vector2Int> IPlayerCharacter.Position => Position;

		IReadOnlyReactiveProperty<bool> IPlayerCharacter.IsActive => IsActive;
		// \IPlayerCharacter

		// IDisposable
		void IDisposable.Dispose()
		{
			NumLives.Dispose();
			Health.Dispose();
			Position.Dispose();
			IsActive.Dispose();
		}

		// \IDisposable
	}
}