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
                var childRenderer = child.GetComponent<Renderer>();
                // If child does not have a renderer, skip
                if (childRenderer == null)
                    continue;
                var childMaterial = childRenderer.material;
                // If child is already in map, skip
                if (transformToColorMap.ContainsKey(child))
                    continue;
                transformToColorMap[child] = childMaterial.color;
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
                var childRenderer = child.GetComponent<Renderer>();
                // If child does not have a renderer, skip
                if (childRenderer == null)
                    continue;
                // If child is not in map, skip
                if (!transformToColorMap.ContainsKey(child))
                    continue;
                childRenderer.material.color = transformToColorMap[child];
                transformToColorMap.Remove(child);
            }
        }
    }
}
