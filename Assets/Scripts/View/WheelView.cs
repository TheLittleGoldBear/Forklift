using UnityEngine;

namespace View
{
	public class WheelView : MonoBehaviour
	{
		#region SerializeFields

		[SerializeField] private Rigidbody m_wheelRigidbody;

		#endregion

		#region UnityMethods

		private void LateUpdate()
		{
			transform.rotation = m_wheelRigidbody.transform.rotation;
		}

		#endregion
	}
}
