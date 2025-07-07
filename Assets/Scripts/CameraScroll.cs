using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{

    public Camera cam;
    public float maxZoom = 0;
    public float minZoom = 50;
    public float sensitivity = 1;
    public float speed = 30;
    float targetZoom;
    // Start is called before the first frame update
    void Start()
    {
        targetZoom = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        targetZoom -= Input.mouseScrollDelta.y * sensitivity;
        targetZoom = Mathf.Clamp(targetZoom, maxZoom, minZoom);
        float newSize = Mathf.MoveTowards(cam.orthographicSize, targetZoom, speed * Time.deltaTime);
        cam.orthographicSize = newSize;
    }
}
