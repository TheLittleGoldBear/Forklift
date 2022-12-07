using General.Input;
using UnityEngine;

namespace Physics
{
	public class LiftingController : MonoBehaviour
	{
		#region SerializeFields

		[SerializeField] private Rigidbody m_liftingRigidbody;
		[SerializeField] private Rigidbody m_mainRigidbody;
		[SerializeField] private float m_liftingDistanceDelta;
		[SerializeField] private float m_minHeight;
		[SerializeField] private float m_maxHeight;

		#endregion

		#region PrivateFields

		private IInputService m_inputService;
		private Vector3 m_positionOffset;
		private Vector3 m_upVector;
		private bool m_registerToEvents;
		private float m_liftUp;

		#endregion

		#region UnityMethods

		private void Start()
		{
			Transform mainRigidbodyTransform = m_mainRigidbody.transform;
			m_positionOffset = m_liftingRigidbody.transform.position - mainRigidbodyTransform.position;
			m_positionOffset = mainRigidbodyTransform.InverseTransformVector(m_positionOffset);
			m_upVector = mainRigidbodyTransform.up;

			RegisterToEvents();
		}

		private void OnDestroy()
		{
			UnregisterFromEvents();
		}

		private void FixedUpdate()
		{
			Lift(Time.deltaTime);
			UpdateTransform();
		}

		#endregion

		#region PublicMethods

		public void Inject(IInputService inputService)
		{
			m_inputService = inputService;
		}

		#endregion

		#region PrivateMethods

		private void UpdateTransform()
		{
			m_liftingRigidbody.MovePosition(
				m_mainRigidbody.transform.position
				+ m_mainRigidbody.transform.TransformVector(m_positionOffset)
			);

			m_liftingRigidbody.MoveRotation(m_mainRigidbody.transform.rotation);
		}

		private void RegisterToEvents()
		{
			if (m_registerToEvents)
			{
				return;
			}

			m_inputService.LiftingInput += OnLifting;

			m_registerToEvents = true;
		}

		private void UnregisterFromEvents()
		{
			if (!m_registerToEvents)
			{
				return;
			}

			m_inputService.LiftingInput -= OnLifting;

			m_registerToEvents = false;
		}

		private void OnLifting(float input)
		{
			m_liftUp = input;
		}

		private void Lift(float deltaTime)
		{
			Vector3 newPositionOffset = m_positionOffset + deltaTime * m_liftUp * m_liftingDistanceDelta * m_upVector;

			if (
				newPositionOffset.y <= m_maxHeight
				&& newPositionOffset.y >= m_minHeight)
			{
				m_positionOffset = newPositionOffset;
			}
		}

		#endregion
	}
}
