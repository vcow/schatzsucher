using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using GameScene.Controller.Utility;
using GameScene.Game;
using GameScene.Input;
using Model.Character;
using Settings.Character;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
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
		private RopeAttitude _ropeAttitude;

		private CinemachineBrain _cinemachineBrain;

		private readonly CompositeDisposable _disposables = new CompositeDisposable();


		private readonly HashSet<Collider> _collisions = new HashSet<Collider>();

		private bool _ios, _if, _ih;
		private CharacterDirection _lastMoveDirection = CharacterDirection.None;
		private Tween _turnTween;
		private float _maxFallSpeed;

		private static readonly int FallSpeedKey = Animator.StringToHash("FallSpeed");
		private static readonly int MoveKey = Animator.StringToHash("Move");
		private static readonly int OnStairKey = Animator.StringToHash("OnStair");
		private static readonly int HangKey = Animator.StringToHash("Hang");

#pragma warning disable 649
		[SerializeField] private Transform _character;
		[SerializeField] private GameObject _hangCollider;
		[SerializeField] private Animator _animator;

		[Inject] private readonly DiContainer _container;
		[Inject] private readonly PlayerVCamController _playerVCamController;
		[Inject] private readonly List<ICharacter> _characters;
		[Inject] private readonly IReadOnlyList<IRope> _rops;
		[Inject] private readonly IPlayerCharacterSettings _playerCharacterSettings;
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

		private bool IsOnStair
		{
			get => _ios;
			set
			{
				if (value == _ios) return;
				_ios = value;
				_rigidbody.useGravity = !_ios;
				if (_collisions.Count <= 0 && !_ios) IsFalling = true;
				if (_ios) LookAt(Direction.Front);
				_animator.SetBool(OnStairKey, _ios);
			}
		}

		private bool IsFalling
		{
			get => _if;
			set
			{
				if (value == _if) return;
				_if = value;
				if (!_if) _animator.SetFloat(FallSpeedKey, 0);
				if (_lastMoveDirection != CharacterDirection.None)
				{
					_lastMoveDirection = CharacterDirection.None;
					_animator.SetInteger(MoveKey, (int) CharacterDirection.None);
				}
			}
		}

		private bool IsHanging
		{
			get => _ih;
			set
			{
				if (value == _ih) return;
				_ih = value;
				_ropeAttitude.enabled = _ih;
				_hangCollider.gameObject.SetActive(_ih);
				if (_ih) LookAt(Direction.Front);
				_animator.SetBool(HangKey, _ih);
			}
		}

		private bool IsInRopeItem => _rops.Any(rope => rope.Bounds.Intersects(Bounds));

		private void Start()
		{
			_characters.Add(this);

			_rigidbody = GetComponent<Rigidbody>();
			_collider = GetComponent<Collider>();

			_container.InstantiateComponent<StairAttitude>(gameObject)
				.IsOnComponent.Subscribe(b => IsOnStair = b).AddTo(_disposables);

			_ropeAttitude = _container.InstantiateComponent<RopeAttitude>(gameObject);
			_ropeAttitude.IsOnComponent.Where(b => !b).Subscribe(b => IsHanging = false).AddTo(_disposables);
			_ropeAttitude.enabled = false;

			_maxFallSpeed = _playerCharacterSettings.SafeFallSpeed + Mathf.Abs(
				(_playerCharacterSettings.DeadlyFallSpeed - _playerCharacterSettings.SafeFallSpeed) * 0.3f);
			Assert.IsTrue(_maxFallSpeed > 0);

			var cam = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<Camera>();
			_cinemachineBrain = cam != null ? cam.GetComponent<CinemachineBrain>() : null;

			if (_cinemachineBrain && _characterModel.IsMainPlayer)
			{
				_playerVCamController.VirtualCamera.Follow = transform;
				_playerVCamController.VirtualCamera.MoveToTopOfPrioritySubqueue();
			}

			LookAt(Direction.Back, true);
		}

		private void OnDestroy()
		{
			_turnTween?.Kill();
			_disposables.Dispose();
		}

		private void Update()
		{
			if (IsFalling)
			{
				_animator.SetFloat(FallSpeedKey, Mathf.Clamp01(Mathf.Abs(_rigidbody.velocity.y / _maxFallSpeed)));
				return;
			}

			Vector3 velocity;
			CharacterDirection direction;
			if (IsOnStair)
			{
				velocity = new Vector3(_input.MoveDirection.x, _input.MoveDirection.y) *
				           _playerCharacterSettings.DefaultSpeed * _playerCharacterSettings.StairSpeedModifier;
				direction = velocity == Vector3.zero
					? CharacterDirection.None
					: Mathf.Abs(velocity.x) < Mathf.Abs(velocity.y)
						? velocity.y > 0
							? CharacterDirection.Up
							: CharacterDirection.Down
						: velocity.x > 0
							? CharacterDirection.Right
							: CharacterDirection.Left;
			}
			else
			{
				var vel = _rigidbody.velocity;
				velocity = IsHanging
					? new Vector3(_input.MoveDirection.x * _playerCharacterSettings.DefaultSpeed *
					              _playerCharacterSettings.RopeSpeedModifier, vel.y)
					: new Vector3(_input.MoveDirection.x * _playerCharacterSettings.DefaultSpeed, vel.y);

				if (Mathf.Abs(_input.MoveDirection.y) > 0)
				{
					if (IsHanging && _input.MoveDirection.y <= -0.5f)
					{
						IsHanging = false;
					}
					else if (!IsHanging && _input.MoveDirection.y >= 0.5f && IsInRopeItem)
					{
						IsHanging = true;
					}
				}

				if (velocity.x > 0)
				{
					direction = CharacterDirection.Right;
					if (!IsHanging) LookAt(Direction.Right);
				}
				else if (velocity.x < 0)
				{
					direction = CharacterDirection.Left;
					if (!IsHanging) LookAt(Direction.Left);
				}
				else
				{
					direction = CharacterDirection.None;
				}
			}

			_rigidbody.velocity = velocity;
			if (direction != _lastMoveDirection)
			{
				_lastMoveDirection = direction;
				_animator.SetInteger(MoveKey, (int) direction);
			}
		}

		private void OnCollisionEnter(Collision other)
		{
			if (_collisions.Count <= 0)
			{
				IsFalling = false;
			}

			_collisions.Add(other.collider);
		}

		private void OnCollisionExit(Collision other)
		{
			_collisions.Remove(other.collider);
			if (_collisions.Count <= 0 && !IsOnStair) IsFalling = true;
		}

		private enum Direction
		{
			Front,
			Back,
			Left,
			Right
		}

		private readonly Dictionary<Direction, Vector3> _angels = new Dictionary<Direction, Vector3>
		{
			[Direction.Front] = new Vector3(0f, 0f, 0f),
			[Direction.Back] = new Vector3(0f, 180f, 0f),
			[Direction.Left] = new Vector3(0f, 270f, 0f),
			[Direction.Right] = new Vector3(0f, 90f, 0f)
		};

		private Direction _lastLookAtDirection = Direction.Front;

		private void LookAt(Direction direction, bool immediately = false)
		{
			if (direction == _lastLookAtDirection) return;
			_lastLookAtDirection = direction;

			const float turnTime = 0.5f;
			var t = _character.transform;
			var targetRotation = _angels[direction];
			t.localRotation = Quaternion.Euler(targetRotation);
		}
	}
}