using System.Collections.Generic;
using Cinemachine;
using GameScene.Controller.Utility;
using GameScene.Game;
using GameScene.Input;
using Model.Character;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameScene.Controller
{
	/// <summary>
	/// Контроллер персонажа игрока.
	/// </summary>
	[DisallowMultipleComponent, RequireComponent(typeof(Collider), typeof(Rigidbody))]
	public class PlayerController : MonoBehaviour, ICharacter
	{
		private IPlayerCharacter _characterModel;
		private IInput _input;

		private Rigidbody _rigidbody;
		private Collider _collider;

		private CinemachineBrain _cinemachineBrain;

		private readonly CompositeDisposable _disposables = new CompositeDisposable();

		private bool _isOnStair;
		private bool _isFalling;

		private readonly HashSet<Collider> _collisions = new HashSet<Collider>();

#pragma warning disable 649
		[SerializeField] private Transform _character;

		[Inject] private readonly DiContainer _container;
		[Inject] private readonly PlayerVCamController _playerVCamController;
		[Inject] private readonly List<ICharacter> _characters;
#pragma warning restore 649

		[Inject]
		private void Construct(IInput input, IPlayerCharacter characterModel)
		{
			_input = input;
			_characterModel = characterModel;
		}

		// ICharacter
		public string Id => _characterModel.Id;

		public Bounds Bounds => _collider.bounds;
		// \ICharacter

		private void Start()
		{
			_characters.Add(this);

			_rigidbody = GetComponent<Rigidbody>();
			_collider = GetComponent<Collider>();

			_container.InstantiateComponent<StairAttitude>(gameObject)
				.IsOnStair.Subscribe(b =>
				{
					_isOnStair = b;
					_rigidbody.useGravity = !b;
					if (_collisions.Count <= 0 && !_isOnStair) _isFalling = true;
				}).AddTo(_disposables);

			var cam = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<Camera>();
			_cinemachineBrain = cam != null ? cam.GetComponent<CinemachineBrain>() : null;

			if (_cinemachineBrain && _characterModel.IsMainPlayer)
			{
				_playerVCamController.VirtualCamera.Follow = transform;
				_playerVCamController.VirtualCamera.MoveToTopOfPrioritySubqueue();
			}
		}

		private void Update()
		{
			if (_isFalling) return;

			_rigidbody.velocity = _isOnStair
				? new Vector3(_input.MoveDirection.Value.x, _input.MoveDirection.Value.y)
				: new Vector3(_input.MoveDirection.Value.x, _rigidbody.velocity.y);
		}

		private void OnCollisionEnter(Collision other)
		{
			if (_collisions.Count <= 0) _isFalling = false;
			_collisions.Add(other.collider);
		}

		private void OnCollisionExit(Collision other)
		{
			_collisions.Remove(other.collider);
			if (_collisions.Count <= 0 && !_isOnStair) _isFalling = true;
		}
	}
}