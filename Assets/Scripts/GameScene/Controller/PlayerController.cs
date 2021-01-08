using Cinemachine;
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

		private CinemachineBrain _cinemachineBrain;

#pragma warning disable 649
		[SerializeField] private Transform _character;

		[Inject] private readonly PlayerVCamController _playerVCamController;
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
			
			var cam = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<Camera>();
			_cinemachineBrain = cam ? cam.GetComponent<CinemachineBrain>() : null;

			if (_cinemachineBrain && _characterModel.IsMainPlayer)
			{
				_playerVCamController.VirtualCamera.Follow = transform;
				_playerVCamController.VirtualCamera.MoveToTopOfPrioritySubqueue();
			}

		}

		private void Update()
		{
			_rigidbody.velocity = new Vector3(_input.MoveDirection.Value.x, _rigidbody.velocity.y);
		}
	}
}