using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotWireScript : MonoBehaviour
{
    public GameObject handle;
    public float shortCircuitTime = 5f;

    private Dictionary<Transform, Color> transformToColorMap = new Dictionary<Transform, Color>();
    private float collisionTime = 0f;
    private bool isColliding = false;
    private Color initialColor;
    private Color targetColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        initialColor = GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (isColliding)
        {
            // Update collision time
            collisionTime += Time.deltaTime;

            // Update renderer material color
            GetComponent<Renderer>().material.color = Color.Lerp(initialColor, targetColor, collisionTime / shortCircuitTime);

            if (collisionTime >= shortCircuitTime)
            {
                // TODO
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == handle || collision.transform.parent?.gameObject == handle)
        {
            isColliding = true;

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
            isColliding = false;

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
