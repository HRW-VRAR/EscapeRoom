using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotWireScript : MonoBehaviour
{
    public GameObject handle;
    public float shortCircuitTime = 5f;
    public float cooldownMultiplier = 0.2f;

    private Dictionary<Transform, Color> transformToColorMap = new Dictionary<Transform, Color>();
    private float collisionTime = 0f;
    private bool isColliding = false;
    private bool isFinished = false;
    private Color initialColor;
    private Color targetColor = new Color(1f, 0f, 0f, 0.75f);

    private GameObject newHandle;

    // Start is called before the first frame update
    void Start()
    {
        initialColor = GetComponent<Renderer>().material.color;

        newHandle = Instantiate(handle, handle.transform.parent);
        newHandle.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFinished)
        {
            return;
        }

        if (isColliding)
        {
            // Update collision time
            collisionTime += Time.deltaTime;
            collisionTime = Mathf.Min(collisionTime, shortCircuitTime);

            // Update renderer material color
            GetComponent<Renderer>().material.color = Color.Lerp(initialColor, targetColor, collisionTime / shortCircuitTime);

            if (collisionTime >= shortCircuitTime)
            {
                Reset();
            }
        } else
        {
            // Update collision time
            collisionTime -= Time.deltaTime * cooldownMultiplier;
            collisionTime = Mathf.Max(collisionTime, 0f);

            // Update renderer material color
            GetComponent<Renderer>().material.color = Color.Lerp(initialColor, targetColor, collisionTime / shortCircuitTime);
        }
    }

    private void Reset()
    {
        Destroy(handle);
        handle = newHandle;
        newHandle = Instantiate(handle, handle.transform.parent);
        handle.SetActive(true);

        isColliding = false;
        collisionTime = 0f;
        GetComponent<Renderer>().material.color = initialColor;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isFinished)
        {
            return;
        }

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
        if (isFinished)
        {
            return;
        }

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

    public void Finish()
    {
        isFinished = true;
        Color color = Color.green;
        color.a = 0.75f;
        GetComponent<Renderer>().material.color = color;
        // TODO
    }
}
