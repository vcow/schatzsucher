using UnityEngine;
using Zenject;

namespace Settings.Character
{
	/// <summary>
	/// Настройки персонажа игрока.
	/// </summary>
	[CreateAssetMenu(fileName = "PlayerCharacterSettings", menuName = "Settings/Character/PlayerCharacterSettings")]
	public class PlayerCharacterSettings : ScriptableObjectInstaller<PlayerCharacterSettings>, IPlayerCharacterSettings
	{
#pragma warning disable 649
		[SerializeField, Header("Speed settings")]
		private float _defaultSpeed;

		[SerializeField] private float _ropeSpeedModifier;
		[SerializeField] private float _stairSpeedModifier;

		[SerializeField, Header("Fall settings")]
		private float _safeFallSpeed;

		[SerializeField] private float _deadlyFallSpeed;
#pragma warning restore 649

		public override void InstallBindings()
		{
			Container.Bind<IPlayerCharacterSettings>().FromInstance(this).AsSingle();
		}

		public float DefaultSpeed => _defaultSpeed;
		public float RopeSpeedModifier => _ropeSpeedModifier;
		public float StairSpeedModifier => _stairSpeedModifier;
		public float SafeFallSpeed => _safeFallSpeed;
		public float DeadlyFallSpeed => _deadlyFallSpeed;
	}
}