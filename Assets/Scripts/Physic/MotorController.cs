using General.Input;
using UnityEngine;

namespace Physic
{
	public class MotorController : MonoBehaviour
	{
		#region SerializeFields

		[SerializeField] private Rigidbody m_mainRigidbodyBody;
		[SerializeField] private Transform m_rightForceTransform;
		[SerializeField] private Transform m_leftForceTransform;
		[SerializeField] private Transform m_centerOfMass;
		[SerializeField] private float m_maxVelocity;
		[SerializeField] private float m_force;

		#endregion

		#region PrivateFields

		private IInputService m_inputService;
		private float m_forwardForce;
		private bool m_registerToEvents;

		#endregion

		#region UnityMethods

		private void Start()
		{
			m_mainRigidbodyBody.centerOfMass = m_centerOfMass.localPosition;
			RegisterToEvents();
		}

		private void OnDestroy()
		{
			UnregisterFromEvents();
		}

		private void FixedUpdate()
		{
			ApplyForwardForce(Time.deltaTime);
		}

		#endregion

		#region PublicMethods

		public void Inject(IInputService inputService)
		{
			m_inputService = inputService;
		}

		#endregion

		#region PrivateMethods

		private void RegisterToEvents()
		{
			if (m_registerToEvents)
			{
				return;
			}

			m_inputService.ForwardMovementInput += OnForwardMovement;

			m_registerToEvents = true;
		}

		private void UnregisterFromEvents()
		{
			if (!m_registerToEvents)
			{
				return;
			}

			m_inputService.ForwardMovementInput -= OnForwardMovement;

			m_registerToEvents = false;
		}

		private void OnForwardMovement(float input)
		{
			m_forwardForce = input;
		}

		private void ApplyForwardForce(float deltaTime)
		{
			float value = deltaTime * m_forwardForce * m_force;
			float velocity = m_mainRigidbodyBody.velocity.magnitude;

			if (velocity <= m_maxVelocity)
			{
				Vector3 forwardDirection = m_mainRigidbodyBody.transform.forward;

				m_mainRigidbodyBody.AddForceAtPosition(value * forwardDirection, m_leftForceTransform.position);
				m_mainRigidbodyBody.AddForceAtPosition(value * forwardDirection, m_rightForceTransform.position);
			}
		}

		#endregion
	}
}
