using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;

public class CharacterScript : MonoBehaviour
{

    public GameObject XROrigin;

    private bool isInCam = false;

    // Start is called before the first frame update
    void Start()
    {
        
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
    }

    public void activateCamera()
    {
        var XROriginParent = transform.GetChild(0);

        XROrigin.transform.SetParent(XROriginParent, false);
        XROrigin.transform.localPosition = Vector3.zero; //TODO: is this needed/wanted?
        XROrigin.transform.hasChanged = false;

        isInCam = false;

        var camOffsetTransform = XROrigin.transform.GetChild(0);
        var trd = camOffsetTransform.GetChild(0).GetComponent<TrackedPoseDriver>();
        trd.trackingType = TrackedPoseDriver.TrackingType.RotationAndPosition;
        for (int i = 0; i < camOffsetTransform.childCount; i++)
        {
            var child = camOffsetTransform.GetChild(i);

            var xrController = child.GetComponent<UnityEngine.XR.Interaction.Toolkit.XRControllerCustom>();
            if (xrController != null)
            {
                xrController.m_bInCam = false;
            }
        }

        var canvas = Camera.main.transform.Find("Canvas").gameObject;
        canvas.SetActive(false);

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
    public void onCCTVActivated()
    {
        isInCam = true;

        var canvas = Camera.main.transform.Find("Canvas").gameObject;
        canvas.SetActive(true);

        Camera.main.cullingMask |= (1 << LayerMask.NameToLayer("Character Model"));
    }
}
