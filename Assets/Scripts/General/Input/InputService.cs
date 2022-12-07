using System;
using UnityEngine.InputSystem;

namespace General.Input
{
	public class InputService : IInputService
	{
		#region Events

		public event Action<float> ForwardMovementInput;
		public event Action<float> SteeringInput;
		public event Action<float> LiftingInput;

		#endregion

		#region PrivateFields

		private InputManager m_inputManager;
		private bool m_registeredToEvents;

		#endregion

		#region InterfaceImplementations

		public void Initialize()
		{
			m_inputManager = new InputManager();

			m_inputManager.Enable();
			RegisterToEvents();
		}

		public void TearDown()
		{
			UnregisterFromEvents();
			m_inputManager.Disable();
		}

		#endregion

		#region PrivateMethods

		private void RegisterToEvents()
		{
			if (m_registeredToEvents)
			{
				return;
			}

			m_inputManager.Keyboard.ForwardMovementInput.performed += OnForwardMovement;
			m_inputManager.Keyboard.SteeringInput.performed += OnSteering;
			m_inputManager.Keyboard.LiftingInput.performed += OnLifting;

			m_registeredToEvents = true;
		}

		private void UnregisterFromEvents()
		{
			if (!m_registeredToEvents)
			{
				return;
			}

			m_inputManager.Keyboard.ForwardMovementInput.performed -= OnForwardMovement;
			m_inputManager.Keyboard.SteeringInput.performed -= OnSteering;
			m_inputManager.Keyboard.LiftingInput.performed -= OnLifting;

			m_registeredToEvents = false;
		}

		private void OnForwardMovement(InputAction.CallbackContext ctx)
		{
			float inputValue = ctx.ReadValue<float>();

			ForwardMovementInput?.Invoke(inputValue);
		}

		private void OnSteering(InputAction.CallbackContext ctx)
		{
			float inputValue = ctx.ReadValue<float>();

			SteeringInput?.Invoke(inputValue);
		}

		private void OnLifting(InputAction.CallbackContext ctx)
		{
			float inputValue = ctx.ReadValue<float>();

			LiftingInput?.Invoke(inputValue);
		}

		#endregion
	}
}
