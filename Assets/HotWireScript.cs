using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotWireScript : MonoBehaviour
{
    public GameObject handle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision);
        Debug.Log(collision.gameObject);
        Debug.Log(collision.transform);
        Debug.Log(collision.transform.parent);
        Debug.Log(collision.transform.parent.gameObject);
        if (collision.gameObject == handle || collision.transform.parent.gameObject == handle)
        {
            handle.GetComponent<Renderer>().material.color = Color.red;
        }
    }
}
