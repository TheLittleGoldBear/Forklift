using UnityEngine;

namespace Physics
{
	public class GroundContactController : MonoBehaviour
	{
		#region SerializeFields

		[SerializeField] private Rigidbody m_mainRigidbody;
		[SerializeField] private Rigidbody m_backLeftWheelRigidbody;
		[SerializeField] private Rigidbody m_backRightWheelRigidbody;
		[SerializeField] private LayerMask m_layerMask;
		[SerializeField] private float m_rayLength;

		#endregion

		#region PublicMethods

		public bool CheckTouchGround()
		{
			Vector3 upDirection = m_mainRigidbody.transform.up;

			return UnityEngine.Physics.Raycast(m_backLeftWheelRigidbody.transform.position, -upDirection, m_rayLength, m_layerMask)
				&& UnityEngine.Physics.Raycast(m_backRightWheelRigidbody.transform.position, -upDirection, m_rayLength, m_layerMask);
		}

		#endregion
	}
}
