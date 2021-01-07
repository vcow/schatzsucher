using System;
using System.Collections.Generic;
using Common.Controller;
using GameScene.Game;
using Model.Character;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameScene.Controller
{
	/// <summary>
	/// Контроллер стартовой точки игрока.
	/// </summary>
	[DisallowMultipleComponent,
	 RequireComponent(typeof(EnterEnvironmentItemController), typeof(Collider))]
	public class EnterController : MonoBehaviour, IEnter
	{
		private readonly Queue<PlayerCharacter> _spawnQueue = new Queue<PlayerCharacter>();
		private readonly BoolReactiveProperty _isBusy = new BoolReactiveProperty(false);

#pragma warning disable 649
		[Inject] private readonly List<IEnter> _enters;
#pragma warning restore 649

		private void Start()
		{
			_enters.Add(this);
		}

		// IEnter
		public IReadOnlyReactiveProperty<bool> IsBusy => _isBusy;

		public void Spawn(PlayerController player)
		{
			player.transform.position = transform.position;
		}
		// \IEnter

		private void OnCollisionEnter(Collision other)
		{
			Debug.Log("On Enter");
		}

		private void OnCollisionExit(Collision other)
		{
			Debug.Log("On Exit");
		}
	}
}