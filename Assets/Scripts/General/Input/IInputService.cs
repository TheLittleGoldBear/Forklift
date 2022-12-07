using System;

namespace General.Input
{
	public interface IInputService
	{
		#region Events

		event Action<float> ForwardMovementInput;
		event Action<float> LiftingInput;

		#endregion

		#region PublicMethods

		void Initialize();
		void TearDown();

		#endregion
	}
}
