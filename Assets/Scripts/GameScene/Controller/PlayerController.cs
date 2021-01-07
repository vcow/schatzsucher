using System;
using GameScene.Input;
using Model.Character;
using UnityEngine;
using Zenject;

namespace GameScene.Controller
{
	/// <summary>
	/// Контроллер персонажа игрока.
	/// </summary>
	[DisallowMultipleComponent, RequireComponent(typeof(Collider), typeof(Rigidbody))]
	public class PlayerController : MonoBehaviour
	{
		private IPlayerCharacter _characterModel;
		private IInput _input;

		private Rigidbody _rigidbody;

#pragma warning disable 649
		[SerializeField] private Transform _character;
#pragma warning restore 649

		[Inject]
		private void Construct(IInput input, IPlayerCharacter characterModel)
		{
			_input = input;
			_characterModel = characterModel;
		}

		private void Start()
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		private void Update()
		{
			_rigidbody.velocity = _input.MoveDirection.Value;
		}
	}
}