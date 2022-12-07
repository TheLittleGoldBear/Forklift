using General.Input;
using Physic;
using UnityEngine;

namespace General
{
	public class ForkliftSystem : MonoBehaviour
	{
		#region SerializeFields

		[SerializeField] private MotorController m_motorController;
		[SerializeField] private LiftingController m_liftingController;

		#endregion

		#region PrivateFields

		private IInputService m_inputService;

		#endregion

		#region UnityMethods

		private void Awake()
		{
			m_inputService = new InputService();

			m_inputService.Initialize();

			m_motorController.Inject(m_inputService);
			m_liftingController.Inject(m_inputService);
		}

		private void OnDestroy()
		{
			m_inputService.TearDown();
		}

		#endregion
	}
}
