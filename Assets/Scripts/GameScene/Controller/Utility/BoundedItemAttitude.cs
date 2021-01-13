using System;
using System.Collections.Generic;
using GameObjectBounds;
using GameScene.Game;
using UniRx;
using UnityEngine;

namespace GameScene.Controller.Utility
{
	/// <summary>
	/// Базовый класс вспомогательного компонента, определяющего отношение объекта к
	/// элементу сцены с определенными границами.
	/// </summary>
	[RequireComponent(typeof(GameObjectBounds3D))]
	public abstract class BoundedItemAttitude<T> : MonoBehaviour where T : IBoundedItem
	{
		private Transform _transform;
		private Bounds _innerBounds;
		private Bounds _outerBounds;

		private readonly HashSet<T> _intersectedStairs = new HashSet<T>();

		private readonly Subject<bool> _isOnStair = new Subject<bool>();

		protected abstract IReadOnlyList<T> Items { get; }

		public IObservable<bool> IsOnComponent => _isOnStair;

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

			foreach (var item in Items)
			{
				var stairIsIntersected = _intersectedStairs.Contains(item);
				if (item.Bounds.Intersects(_innerBounds) && !stairIsIntersected)
				{
					if (_intersectedStairs.Count <= 0) _isOnStair.OnNext(true);
					_intersectedStairs.Add(item);
				}
				else if (!item.Bounds.Intersects(_outerBounds) && stairIsIntersected)
				{
					_intersectedStairs.Remove(item);
					if (_intersectedStairs.Count <= 0) _isOnStair.OnNext(false);
				}
			}
		}
	}
}