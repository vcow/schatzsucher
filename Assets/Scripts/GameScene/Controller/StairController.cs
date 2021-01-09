using System.Collections.Generic;
using GameObjectBounds;
using GameScene.Game;
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

#pragma warning disable 649
		[Inject] private readonly List<IStair> _stairs;
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
	}
}