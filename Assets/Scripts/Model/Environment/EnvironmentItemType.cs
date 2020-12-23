using System;

namespace Model.Environment
{
	[Serializable]
	public enum EnvironmentItemType
	{
		None,
		// surfaces
		Wood = 1,
		Brick,
		Concrete,
		// special
		Stair = 100,
		Rope,
		// interactive
		Enter = 200,
		Exit,
		SpawnGuard,
		// traps
		Holography = 300,
		Glue,
		Fire,
		Toxic,
		Thorns
	}
}