using General.Input;
using UnityEngine;
using View;

namespace Physics
{
	public class MotorController : MonoBehaviour
	{
		#region SerializeFields

		[SerializeField] private Rigidbody m_mainRigidbodyBody;
		[SerializeField] private GroundContactController m_groundContactController;
		[SerializeField] private SteeringView m_steeringView;
		[SerializeField] private Transform m_centerOfMass;
		[SerializeField] private float m_maxVelocity;
		[SerializeField] private float m_forwardForce;
		[SerializeField] private float m_steeringTorque;
		[SerializeField] private float m_sidewayForce;

		#endregion

		#region PrivateFields

		private IInputService m_inputService;
		private float m_forwardForceInput;
		private float m_steeringInput;
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

		private void Update()
		{
			m_steeringView.UpdateView(m_steeringInput);
		}

		private void FixedUpdate()
		{
			if (!m_groundContactController.CheckTouchGround())
			{
				Debug.LogWarning($"{nameof(MotorController)} there is not contact with the ground. Not able to apply forces");

				return;
			}

			ApplyTorque(Time.deltaTime);
			ApplyForwardForce(Time.deltaTime);
			ApplySidewayForce(Time.deltaTime);
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
			m_inputService.SteeringInput += OnSteering;

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
			m_forwardForceInput = input;
		}

		private void OnSteering(float input)
		{
			m_steeringInput = input;
		}

		private void ApplyForwardForce(float deltaTime)
		{
			float value = deltaTime * m_forwardForceInput * m_forwardForce;
			float velocity = m_mainRigidbodyBody.velocity.magnitude;

			if (velocity <= m_maxVelocity)
			{
				Vector3 forwardDirection = m_mainRigidbodyBody.transform.forward;

				m_mainRigidbodyBody.AddForce(value * forwardDirection);
			}
		}

		private void ApplyTorque(float deltaTime)
		{
			m_mainRigidbodyBody.AddTorque(
				deltaTime * m_steeringTorque * m_forwardForceInput * m_steeringInput * m_mainRigidbodyBody.transform.up
			);
		}

		private void ApplySidewayForce(float deltaTime)
		{
			Vector3 velocity = m_mainRigidbodyBody.velocity;
			Vector3 rightDirection = m_mainRigidbodyBody.transform.right;
			float value = Vector3.Dot(velocity, rightDirection);

			m_mainRigidbodyBody.AddForce(-value * deltaTime * m_sidewayForce * rightDirection);
		}

		#endregion
	}
}
