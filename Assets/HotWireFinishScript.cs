using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotWireFinishScript : MonoBehaviour
{
    public HotWireScript hotWireScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (!this.enabled)
        {
            return;
        }

        if (GetComponent<Collider>().transform.parent.gameObject == hotWireScript.handle)
        {
            hotWireScript.Finish();
            this.enabled = false;
        }
    }
}
