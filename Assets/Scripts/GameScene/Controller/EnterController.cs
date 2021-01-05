using System.Collections.Generic;
using Common.Controller;
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
	public class EnterController : MonoBehaviour
	{
		private const string PlayerPrefabPath = "Characters/PlayerCharacter.prefab";

		private readonly BoolReactiveProperty _isBusy = new BoolReactiveProperty(false);

		private IReadOnlyList<PlayerCharacter> _players;
		private DiContainer _container;

#pragma warning disable 649
#pragma warning restore 649

		[Inject]
		private void Construct(DiContainer container)
		{
			_container = container;
			_players = container.ResolveAll<PlayerCharacter>();
		}
	}
}