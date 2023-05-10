using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;

public class CharacterScript : MonoBehaviour
{

    public GameObject XROrigin;

    private bool isInCam = false;
    private CCTVCameraActivationData cctvCameraActivationData = null;

    private GameObject cachedCamera = null;
    private GameObject cachedCharacterModel = null;

    // Start is called before the first frame update
    void Start()
    {
        var camOffsetTransform = XROrigin.transform.GetChild(0);
        for (int i = 0; i < camOffsetTransform.childCount; i++)
        {
            var child = camOffsetTransform.GetChild(i);

            var cameraComponent = child.GetComponent<Camera>();
            if (cameraComponent != null)
            {
                cachedCamera = child.gameObject;
                break;
            }
        }

        int characterModelLayer = LayerMask.NameToLayer("Character Model");
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);

            if (child.gameObject.layer == characterModelLayer)
            {
                cachedCharacterModel = child.gameObject;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Transform xrOriginTransform = XROrigin.transform;
        if (xrOriginTransform.hasChanged && !isInCam)
        {
            transform.localPosition = transform.localPosition + xrOriginTransform.localPosition;
            xrOriginTransform.localPosition = Vector3.zero;
            xrOriginTransform.hasChanged = false;
        }
        
        if (!isInCam)
        {
            float yRotation = xrOriginTransform.localEulerAngles.y;
            if (cachedCamera != null)
            {
                yRotation += cachedCamera.transform.localEulerAngles.y;
            }

            if (cachedCharacterModel != null)
            {
                Vector3 characterRotation = cachedCharacterModel.transform.localEulerAngles;
                characterRotation.y = yRotation;
                cachedCharacterModel.transform.localEulerAngles = characterRotation;
            }
        }
    }

    /**
     * Moves the camera back to the character model. Restores the XR origin's position and rotation.
     **/
    public void activateCamera()
    {
        var XROriginParent = transform.GetChild(0);

        // Move XR Origin back to character
        XROrigin.transform.SetParent(XROriginParent, false);
        XROrigin.transform.localPosition = Vector3.zero;
        XROrigin.transform.hasChanged = false;

        // Restore XR Origin rotation
        Vector3 xrOriginRot = XROrigin.transform.localEulerAngles;
        xrOriginRot.y = cctvCameraActivationData.xrOriginYRotation;
        XROrigin.transform.localEulerAngles = xrOriginRot;

        // Update isInCam flag
        isInCam = false;

        var camOffsetTransform = XROrigin.transform.GetChild(0);

        // Restore position (height) tracking and restore Y position
        var trd = camOffsetTransform.GetChild(0).GetComponent<TrackedPoseDriver>();
        trd.trackingType = TrackedPoseDriver.TrackingType.RotationAndPosition;
        Vector3 camOffsetLP = camOffsetTransform.localPosition;
        camOffsetLP.y = cctvCameraActivationData.cameraOffsetYPosition;
        camOffsetTransform.localPosition = camOffsetLP;
        for (int i = 0; i < camOffsetTransform.childCount; i++)
        {
            var child = camOffsetTransform.GetChild(i);

            // Tell controllers we're not in a CCTV camera anymore
            var xrController = child.GetComponent<UnityEngine.XR.Interaction.Toolkit.XRControllerCustom>();
            if (xrController != null)
            {
                xrController.m_bInCam = false;
            }

            // Enable walking
            var xrRayInteractor = child.GetComponent<UnityEngine.XR.Interaction.Toolkit.XRRayInteractor>();
            if (xrRayInteractor != null)
            {
                xrRayInteractor.interactionLayers |= (1 << UnityEngine.XR.Interaction.Toolkit.InteractionLayerMask.NameToLayer("Walkable area"));
            }
        }

        // Enable snap turn provider
        var snapTurnProvider = XROrigin.GetComponent<UnityEngine.XR.Interaction.Toolkit.DeviceBasedSnapTurnProvider>();
        if (snapTurnProvider != null)
        {
            snapTurnProvider.enabled = true;
        }

        // Hide canvas containing camera exit button
        var canvas = Camera.main.transform.Find("Canvas").gameObject;
        canvas.SetActive(false);

        // Hide character model from main camera
        Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("Character Model"));

        // Remove layer and tag from previously active cctv camera
        GameObject[] activeCCTVCams = GameObject.FindGameObjectsWithTag("ActiveCCTVCam");
        for (int i = 0; i < activeCCTVCams.Length; i++)
        {
            var cam = activeCCTVCams[i];
            cam.layer = 0;
            cam.tag = "Untagged";
            RecursiveSetLayerIfTagged(cam.transform, 0, "VisibleCameraPart");
        }
    }

    private void RecursiveSetLayerIfTagged(Transform parent, int layer, string tag)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            var child = parent.GetChild(i);
            if (child.tag == tag)
            {
                child.gameObject.layer = layer;
            }
            RecursiveSetLayerIfTagged(child, layer, tag);
        }
    }

    /**
     * Event handler that is called when a cctv camera is activated.
     **/
    public void onCCTVActivated(CCTVCameraActivationData activationData)
    {
        if (!isInCam)
        {
            // Set isInCam flag and store cctv camera activation data
            isInCam = true;
            cctvCameraActivationData = activationData;
        }

        // Enable canvas containing camera exit button
        var canvas = Camera.main.transform.Find("Canvas").gameObject;
        canvas.SetActive(true);

        // Disable snap turn provider
        var snapTurnProvider = XROrigin.GetComponent<UnityEngine.XR.Interaction.Toolkit.DeviceBasedSnapTurnProvider>();
        if (snapTurnProvider != null)
        {
            snapTurnProvider.enabled = false;
        }

        // Show character model in main camera
        Camera.main.cullingMask |= (1 << LayerMask.NameToLayer("Character Model"));
    }
}
