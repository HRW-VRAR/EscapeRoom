using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;

public class CharacterScript : MonoBehaviour
{

    public GameObject XROrigin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activateCamera()
    {
        var XROriginParent = transform.GetChild(0);

        XROrigin.transform.SetParent(XROriginParent, false);

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

        // Remove layer and tag from previously active cctv camera
        GameObject[] activeCCTVCams = GameObject.FindGameObjectsWithTag("ActiveCCTVCam");
        for (int i = 0; i < activeCCTVCams.Length; i++)
        {
            var cam = activeCCTVCams[i];
            cam.layer = 0;
            cam.tag = "Untagged";
        }
    }

    /**
     * Event handler that is called when a cctv camera is activated.
     **/
    public void onCCTVActivated()
    {
        var canvas = Camera.main.transform.Find("Canvas").gameObject;
        canvas.SetActive(true);
    }
}
