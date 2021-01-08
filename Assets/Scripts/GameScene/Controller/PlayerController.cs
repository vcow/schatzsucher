using System.Collections.Generic;
using Cinemachine;
using GameScene.Game;
using GameScene.Input;
using GameScene.Signals;
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

		private readonly Subject<bool> _isOnStairStream = new Subject<bool>();
		private readonly CompositeDisposable _disposables = new CompositeDisposable();

#pragma warning disable 649
		[SerializeField] private Transform _character;

		[Inject] private readonly PlayerVCamController _playerVCamController;
		[Inject] private readonly List<ICharacter> _characters;
		[Inject] private readonly SignalBus _signalBus;
#pragma warning restore 649

		public PlayerController()
		{
			IsOnStair = _isOnStairStream.ThrottleFrame(2).DistinctUntilChanged()
				.ToReadOnlyReactiveProperty(false);
		}

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

		public IReadOnlyReactiveProperty<bool> IsOnStair { get; }

		private void Start()
		{
			_characters.Add(this);

			_rigidbody = GetComponent<Rigidbody>();
			_collider = GetComponent<Collider>();

			_signalBus.Subscribe<EnterToStairSignal>(OnEnterToStair);
			_signalBus.Subscribe<ExitFromStairSignal>(OnExitFromStair);

			IsOnStair.Subscribe(b =>
			{
				if (b)
				{
					Debug.Log("Enter to stair.");
				}
				else
				{
					Debug.Log("Exit from stair.");
				}
			}).AddTo(_disposables);

			var cam = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<Camera>();
			_cinemachineBrain = cam != null ? cam.GetComponent<CinemachineBrain>() : null;

			if (_cinemachineBrain && _characterModel.IsMainPlayer)
			{
				_playerVCamController.VirtualCamera.Follow = transform;
				_playerVCamController.VirtualCamera.MoveToTopOfPrioritySubqueue();
			}
		}

		private void OnExitFromStair(ExitFromStairSignal signal)
		{
			if (signal.CharacterId != Id) return;
			_isOnStairStream.OnNext(false);
		}

		private void OnEnterToStair(EnterToStairSignal signal)
		{
			if (signal.CharacterId != Id) return;
			_isOnStairStream.OnNext(true);
		}

		private void OnDestroy()
		{
			_disposables.Dispose();

			_signalBus.Unsubscribe<EnterToStairSignal>(OnEnterToStair);
			_signalBus.Unsubscribe<ExitFromStairSignal>(OnExitFromStair);

			_isOnStairStream.OnCompleted();
			_isOnStairStream.Dispose();

			_characters.Remove(this);
		}

		private void Update()
		{
			_rigidbody.velocity = new Vector3(_input.MoveDirection.Value.x, _rigidbody.velocity.y);
		}
	}
}