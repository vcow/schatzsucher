using System;
using UnityEngine;

namespace Model.Environment
{
	/// <summary>
	/// Элемент игрового уровня.
	/// </summary>
	public class EnvironmentItem : IEnvironmentItem, IEquatable<IEnvironmentItem>
	{
		public EnvironmentItemType Type { get; set; }
		public Vector2Int Position { get; set; }

		// IEnvironmentItem
		EnvironmentItemType IEnvironmentItem.Type => Type;

		Vector2Int IEnvironmentItem.Position => Position;
		// \IEnvironmentItem

		public bool Equals(IEnvironmentItem other)
		{
			return other != null
			       && Type == other.Type
			       && Position == other.Position;
		}
	}
}