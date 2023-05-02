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
        /// <inheritdoc />
        protected override void UpdateTrackingInput(XRControllerState controllerState)
        {
            base.UpdateTrackingInput(controllerState);
            if (controllerState == null)
                return;

            controllerState.position.y = -0.3f;

            /*
            controllerState.inputTrackingState = InputTrackingState.None;
#if LIH_PRESENT_V1API
            if (m_PoseProvider != null)
            {
                if (m_PoseProvider.TryGetPoseFromProvider(out var poseProviderPose))
                {
                    controllerState.position = poseProviderPose.position;
                    controllerState.rotation = poseProviderPose.rotation;
                    controllerState.inputTrackingState = InputTrackingState.Position | InputTrackingState.Rotation;
                }
            }
            else
#elif LIH_PRESENT_V2API
            if (m_PoseProvider != null)
            {
                var retFlags = m_PoseProvider.GetPoseFromProvider(out var poseProviderPose);
                if ((retFlags & PoseDataFlags.Position) != 0)
                {
                    controllerState.position = poseProviderPose.position;
                    controllerState.inputTrackingState |= InputTrackingState.Position;
                }
                if ((retFlags & PoseDataFlags.Rotation) != 0)
                {
                    controllerState.rotation = poseProviderPose.rotation;
                    controllerState.inputTrackingState |= InputTrackingState.Rotation;
                }
            }
            else
#endif
            {
                if (inputDevice.TryGetFeatureValue(CommonUsages.trackingState, out var trackingState))
                {
                    controllerState.inputTrackingState = trackingState;
                    
                    if ((trackingState & InputTrackingState.Position) != 0 &&
                        inputDevice.TryGetFeatureValue(CommonUsages.devicePosition, out var devicePosition))
                    {
                        controllerState.position = devicePosition;
                    }

                    if ((trackingState & InputTrackingState.Rotation) != 0 &&
                        inputDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out var deviceRotation))
                    {
                        controllerState.rotation = deviceRotation;
                    }
                }
            }

            controllerState.position.y = -0.5f; //*/
        }

        /// <inheritdoc />
        protected override void UpdateInput(XRControllerState controllerState)
        {
            base.UpdateInput(controllerState);
            if (controllerState == null)
                return;

            /*
            controllerState.ResetFrameDependentStates();
            controllerState.selectInteractionState.SetFrameState(IsPressed(m_SelectUsage), ReadValue(m_SelectUsage));
            controllerState.activateInteractionState.SetFrameState(IsPressed(m_ActivateUsage), ReadValue(m_ActivateUsage));
            controllerState.uiPressInteractionState.SetFrameState(IsPressed(m_UIPressUsage), ReadValue(m_UIPressUsage)); //*/
        }

        /*

        /// <summary>
        /// Evaluates whether the button is considered pressed.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <returns>Returns <see langword="true"/> when the button is considered pressed. Otherwise, returns <see langword="false"/>.</returns>
        protected virtual bool IsPressed(InputHelpers.Button button)
        {
            inputDevice.IsPressed(button, out var pressed, m_AxisToPressThreshold);
            return pressed;
        }

        /// <summary>
        /// Reads and returns the given action value.
        /// </summary>
        /// <param name="button">The button to read the value from.</param>
        /// <returns>Returns the button value.</returns>
        protected virtual float ReadValue(InputHelpers.Button button)
        {
            inputDevice.TryReadSingleValue(button, out var value);
            return value;
        }

        /// <inheritdoc />
        public override bool SendHapticImpulse(float amplitude, float duration)
        {
            if (inputDevice.TryGetHapticCapabilities(out var capabilities) &&
                capabilities.supportsImpulse)
            {
                return inputDevice.SendHapticImpulse(0u, amplitude, duration);
            }
            return false;
        } //*/
    }
}
