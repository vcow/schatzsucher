using UnityEngine;

namespace Utils
{
	public class BoundsSettings : MonoBehaviour
	{
		[SerializeField] private bool _hasOuterBounds = true;
		[SerializeField] private Rect _innerBounds;
		[SerializeField] private Rect _outerBounds;

		public bool HasOuterBounds => _hasOuterBounds;

		public Rect InnerBounds => _innerBounds;

		public Rect OuterBounds => _outerBounds;
	}
}