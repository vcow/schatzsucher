using System;
using System.Collections.Generic;
using GameScene.Controller;
using Model.Character;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace GameScene.Game
{
	/// <summary>
	/// Главный игровой контроллер.
	/// </summary>
	public class Game : IGame, IDisposable
	{
		private const string PlayerPrefabPath = "Characters/PlayerCharacter";

		private readonly BoolReactiveProperty _isActive = new BoolReactiveProperty(false);
		private readonly CompositeDisposable _disposables = new CompositeDisposable();

		private bool _isStarted;
		private List<PlayerCharacter> _players;

#pragma warning disable 649
		[Inject] private readonly DiContainer _container;
#pragma warning restore 649

		// IGame
		public IReadOnlyReactiveProperty<bool> IsActive => _isActive;

		public void Start()
		{
			if (_isStarted)
			{
				Debug.LogError("Game is already started.");
				return;
			}

			if (_players == null) _players = _container.ResolveAll<PlayerCharacter>();
			if (_players.Count <= 0)
			{
				Debug.LogError("There are no one players injected in the Game.");
				return;
			}

			_isStarted = true;

			// TODO: Prepare game
			Observable.Timer(TimeSpan.FromSeconds(1), Scheduler.MainThread)
				.Subscribe(l => DoStart()).AddTo(_disposables);
		}

		public void Stop()
		{
			throw new NotImplementedException();
		}
		// \IGame

		// IDisposable
		void IDisposable.Dispose()
		{
			_players.Clear();

			_disposables.Dispose();
			_isActive.Dispose();
		}
		// \IDisposable

		private void DoStart()
		{
			if (_isActive.Value) return;

			_players.ForEach(SpawnPlayer);
			_isActive.SetValueAndForceNotify(true);
		}

		private void SpawnPlayer(PlayerCharacter character)
		{
			if (character.Health.Value <= 0)
			{
				// Игрок убит. Забрать жизнь.
				character.NumLives.SetValueAndForceNotify(Mathf.Max(0, character.NumLives.Value - 1));
			}

			if (character.NumLives.Value <= 0)
			{
				// Игрок исчерпал свои жизни.
				return;
			}

			var enters = _container.ResolveAll<IEnter>();
			if (enters.Count <= 0)
			{
				Debug.LogError("There is no Enter for players in the scene.");
				return;
			}

			// Выбрать точку входа.
			var enter = enters.Count > 1 ? enters[Random.Range(0, enters.Count)] : enters[0];
			var player = _container.InstantiatePrefabResourceForComponent<PlayerController>(
				PlayerPrefabPath, new object[] {character.Input, character});
			enter.Spawn(player);
		}
	}
}