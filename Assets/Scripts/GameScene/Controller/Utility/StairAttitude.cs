using System;
using System.Collections.Generic;
using GameObjectBounds;
using GameScene.Game;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameScene.Controller.Utility
{
	/// <summary>
	/// Вспомогательный компонент, определяющий отношение объекта к
	/// элементу сцены "лестница".
	/// </summary>
	[DisallowMultipleComponent, RequireComponent(typeof(GameObjectBounds3D))]
	public class StairAttitude : MonoBehaviour
	{
		private Transform _transform;
		private Bounds _innerBounds;
		private Bounds _outerBounds;

		private readonly HashSet<IStair> _intersectedStairs = new HashSet<IStair>();

		private readonly Subject<bool> _isOnStair = new Subject<bool>();

#pragma warning disable 649
		[Inject] private readonly IReadOnlyList<IStair> _stairs;
#pragma warning restore 649

		public IObservable<bool> IsOnStair => _isOnStair;

		private void Start()
		{
			_transform = transform;

			var bounds = GetComponent<GameObjectBounds3D>();
			_innerBounds = bounds.InnerBounds;
			_outerBounds = bounds.OuterBounds;
		}

		private void OnDestroy()
		{
			_isOnStair.Dispose();
		}

		private void Update()
		{
			_innerBounds.center = _transform.position;
			_outerBounds.center = transform.position;

			foreach (var stair in _stairs)
			{
				var stairIsIntersected = _intersectedStairs.Contains(stair);
				if (stair.Bounds.Intersects(_innerBounds) && !stairIsIntersected)
				{
					if (_intersectedStairs.Count <= 0) _isOnStair.OnNext(true);
					_intersectedStairs.Add(stair);
				}
				else if (!stair.Bounds.Intersects(_outerBounds) && stairIsIntersected)
				{
					_intersectedStairs.Remove(stair);
					if (_intersectedStairs.Count <= 0) _isOnStair.OnNext(false);
				}
			}
		}
	}
}