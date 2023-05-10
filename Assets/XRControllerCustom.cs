using UnityEngine.SpatialTracking;

#if LIH_PRESENT
using UnityEngine.Experimental.XR.Interaction;
#endif

namespace UnityEngine.XR.Interaction.Toolkit
{
    /// <summary>
    /// Interprets feature values on a tracked input controller device from the XR input subsystem
    /// into XR Interaction states, such as Select. Additionally, it applies the current Pose value
    /// of a tracked device to the transform of the <see cref="GameObject"/>.
    /// </summary>
    /// <remarks>
    /// It is recommended to use <see cref="ActionBasedController"/> instead of this behavior.
    /// This behavior does not need as much initial setup as compared to <see cref="ActionBasedController"/>,
    /// however input processing is less customizable and the <see cref="Inputs.Simulation.XRDeviceSimulator"/> cannot be used to drive
    /// this behavior.
    /// </remarks>
    /// <seealso cref="XRBaseController"/>
    /// <seealso cref="ActionBasedController"/>
    public class XRControllerCustom : XRController
    {
        [HideInInspector]
        public bool m_bInCam = false;

        /// <inheritdoc />
        protected override void UpdateTrackingInput(XRControllerState controllerState)
        {
            base.UpdateTrackingInput(controllerState);
            if (controllerState == null)
                return;

            if (m_bInCam)
                controllerState.position.y = -0.3f;
        }

        /// <inheritdoc />
        protected override void UpdateInput(XRControllerState controllerState)
        {
            base.UpdateInput(controllerState);
            if (controllerState == null)
                return;
        }
    }
}
