using System;
using UnityEngine;

namespace Model.Environment
{
	public class EnvironmentItem : IEnvironmentItem, IEquatable<IEnvironmentItem>
	{
		public EnvironmentItemType Type;
		public Vector2Int Position;

		EnvironmentItemType IEnvironmentItem.Type => Type;

		Vector2Int IEnvironmentItem.Position => Position;

		public bool Equals(IEnvironmentItem other)
		{
			return other != null
			       && Type == other.Type
			       && Position == other.Position;
		}
	}
}