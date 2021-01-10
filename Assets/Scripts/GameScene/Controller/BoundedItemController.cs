using System;
using System.Collections.Generic;
using GameObjectBounds;
using GameScene.Game;
using UnityEngine;

namespace GameScene.Controller
{
	/// <summary>
	/// Базовый класс контроллера элемента сцены, имеющего заданные границы. 
	/// </summary>
	[DisallowMultipleComponent, RequireComponent(typeof(GameObjectBounds3D))]
	public abstract class BoundedItemController<T> : MonoBehaviour, IBoundedItem where T : IBoundedItem
	{
		private Transform _transform;
		private Bounds _bounds;

		/// <summary>
		/// Флаг, указывающий на то, что элемент динамически меняет свое метоположение.
		/// </summary>
		protected abstract bool IsDynamic { get; }

		/// <summary>
		/// Общий список элементов.
		/// </summary>
		protected abstract List<T> ElementsCollection { get; }

		protected virtual void Start()
		{
			if (this is T element)
			{
				ElementsCollection.Add(element);
				_transform = transform;
				_bounds = GetComponent<GameObjectBounds3D>().InnerBounds;
				if (!IsDynamic) _bounds.center = _transform.position;
			}
			else
			{
				throw new Exception($"Derived class must implement {typeof(T).FullName}.");
			}
		}

		protected virtual void OnDestroy()
		{
			if (this is T element) ElementsCollection.Remove(element);
		}

		// IStair
		public Bounds Bounds => IsDynamic
			? new Bounds(_transform.position, _bounds.size)
			: _bounds;

		// \IStair
	}
}