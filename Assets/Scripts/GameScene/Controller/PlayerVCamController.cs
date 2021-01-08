using System;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace GameScene.Controller
{
	[DisallowMultipleComponent, RequireComponent(typeof(CinemachineVirtualCameraBase))]
	public class PlayerVCamController : MonoInstaller<PlayerVCamController>
	{
		private readonly Lazy<CinemachineVirtualCameraBase> _virtualCamera;

		public PlayerVCamController()
		{
			_virtualCamera = new Lazy<CinemachineVirtualCameraBase>(GetComponent<CinemachineVirtualCameraBase>);
		}

		public override void InstallBindings()
		{
			Container.Bind<PlayerVCamController>().FromInstance(this).AsSingle();
		}

		public CinemachineVirtualCameraBase VirtualCamera => _virtualCamera.Value;
	}
}