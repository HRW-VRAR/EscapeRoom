using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotWireScript : MonoBehaviour
{
    public GameObject handle;

    private Dictionary<Transform, Color> transformToColorMap = new Dictionary<Transform, Color>();

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
        if (collision.gameObject == handle || collision.transform.parent?.gameObject == handle)
        {
            for (int i = 0; i < handle.transform.childCount; i++)
            {
                var child = handle.transform.GetChild(i);
                var childMaterial = child.GetComponent<Renderer>().material;
                transformToColorMap.Add(child, childMaterial.color);
                childMaterial.color = Color.red;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == handle || collision.transform.parent?.gameObject == handle)
        {
            for (int i = 0; i < handle.transform.childCount; i++)
            {
                var child = handle.transform.GetChild(i);
                child.GetComponent<Renderer>().material.color = transformToColorMap.Get(child);
            }
        }
    }
}
