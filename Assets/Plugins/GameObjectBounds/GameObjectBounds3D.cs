using UnityEngine;

namespace GameObjectBounds
{
	public class GameObjectBounds3D : GameObjectBounds
	{
#pragma warning disable 649
		[SerializeField] private bool _hasOuterBounds = true;
		[SerializeField] private Vector3 _innerLeftBottomFront = Vector3.one * -0.5f;
		[SerializeField] private Vector3 _innerRightTopBack = Vector3.one * 0.5f;
		[SerializeField] private Vector3 _outerLeftBottomFront = Vector3.one * -0.75f;
		[SerializeField] private Vector3 _outerRightTopBack = Vector3.one * 0.75f;
#pragma warning restore 649

		public bool HasOuterBounds => _hasOuterBounds;

		public Bounds InnerBounds => new Bounds
		{
			min = _innerLeftBottomFront,
			max = _innerRightTopBack
		};

		public Bounds OuterBounds => new Bounds
		{
			min = _outerLeftBottomFront,
			max = _outerRightTopBack
		};
	}
}