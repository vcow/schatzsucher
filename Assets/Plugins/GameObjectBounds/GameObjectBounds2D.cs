using UnityEngine;

namespace GameObjectBounds
{
	public class GameObjectBounds2D : GameObjectBounds
	{
#pragma warning disable 649
		[SerializeField] private bool _hasOuterBounds = true;
		[SerializeField] private Rect _innerBounds = new Rect(Vector2.one * -0.5f, Vector2.one);
		[SerializeField] private Rect _outerBounds = new Rect(Vector2.one * -0.75f, Vector2.one * 1.5f);
#pragma warning restore 649

		public bool HasOuterBounds => _hasOuterBounds;

		public Rect InnerBounds => _innerBounds;

		public Rect OuterBounds => _outerBounds;
	}
}