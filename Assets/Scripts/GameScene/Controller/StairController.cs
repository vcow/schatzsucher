using System.Collections.Generic;
using GameObjectBounds;
using GameScene.Game;
using GameScene.Signals;
using UnityEngine;
using Zenject;

namespace GameScene.Controller
{
	/// <summary>
	/// Контроллер лестницы.
	/// </summary>
	[DisallowMultipleComponent, RequireComponent(typeof(GameObjectBounds3D))]
	public class StairController : MonoBehaviour, IStair
	{
		private Bounds _bounds;

		private readonly HashSet<string> _intersectedCharacters = new HashSet<string>();

#pragma warning disable 649
		[Inject] private readonly List<IStair> _stairs;
		[Inject] private readonly List<ICharacter> _characters;
		[Inject] private readonly SignalBus _signalBus;
#pragma warning restore 649

		private void Start()
		{
			_stairs.Add(this);
			_bounds = GetComponent<GameObjectBounds3D>().InnerBounds;
			_bounds.center = transform.position;
		}

		private void OnDestroy()
		{
			_stairs.Remove(this);
		}

		// IStair
		public Bounds Bounds => _bounds;
		// \IStair

		private void Update()
		{
			_characters.ForEach(character =>
			{
				if (_intersectedCharacters.Contains(character.Id))
				{
					if (!Bounds.Intersects(character.Bounds))
					{
						_intersectedCharacters.Remove(character.Id);
						_signalBus.Fire(new ExitFromStairSignal {CharacterId = character.Id});
					}
				}
				else
				{
					if (Bounds.Intersects(character.Bounds))
					{
						_intersectedCharacters.Add(character.Id);
						_signalBus.Fire(new EnterToStairSignal {CharacterId = character.Id});
					}
				}
			});
		}
	}
}