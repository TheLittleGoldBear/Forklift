using UnityEngine;

namespace View
{
	public class SteeringView : MonoBehaviour
	{
		#region SerializeFields

		[Header("Wheels")] [SerializeField] private Rigidbody m_mainRigidbody;
		[SerializeField] private Rigidbody m_backLeftWheelRigidbody;
		[SerializeField] private Rigidbody m_backRightWheelRigidbody;
		[SerializeField] private Transform m_backLeftWheel;
		[SerializeField] private Transform m_backRightWheel;
		[SerializeField] private float m_maxWheelAngle;

		[Header("Steering wheel")] [SerializeField] private Transform m_steeringWheel;
		[SerializeField] private float m_maxSteeringWheelAngle;

		#endregion

		#region PrivateFields

		private float m_steeringWheelZAngle;
		private float m_wheelYAngle;

		#endregion

		#region UnityMethods

		private void Update()
		{
			UpdateWheelRotation();
			UpdateSteeringWheelRotation();
		}

		#endregion

		#region PublicMethods

		public void UpdateView(float steeringValue)
		{
			m_wheelYAngle = -steeringValue * m_maxWheelAngle;
			m_steeringWheelZAngle = steeringValue * m_maxSteeringWheelAngle;
		}

		#endregion

		#region PrivateMethods

		private void UpdateWheelRotation()
		{
			Vector3 mainRigidbodyUp = m_mainRigidbody.transform.up;
			m_backLeftWheel.rotation = Quaternion.AngleAxis(m_wheelYAngle, mainRigidbodyUp) * m_backLeftWheelRigidbody.transform.rotation;
			m_backRightWheel.rotation = Quaternion.AngleAxis(m_wheelYAngle, mainRigidbodyUp) * m_backRightWheelRigidbody.transform.rotation;
		}

		private void UpdateSteeringWheelRotation()
		{
			Quaternion steeringWheelRotation = m_steeringWheel.rotation;
			m_steeringWheel.localRotation = Quaternion.Euler(steeringWheelRotation.x, steeringWheelRotation.y, m_steeringWheelZAngle);
		}

		#endregion
	}
}
