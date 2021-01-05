using System;

namespace Model.Character
{
	/// <summary>
	/// Флаги проходимости.
	/// </summary>
	[Serializable, Flags]
	public enum Pass
	{
		None = 0x00,
		Walk = 0x01,
		Hanging = 0x02,
		Fire = 0x04,
		Toxic = 0x08,
		Glue = 0x10,
		Thorns = 0x20
	}
}