using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{

    public Camera cam;
    public float maxZoom = 0;
    public float minZoom = 50;
    public float sensitivity = 1;
    public float defaultSpeed = 30;
    public float speed;
    public float zoomTarget1;
    float targetZoom;
    // Start is called before the first frame update
    void Start()
    {
        targetZoom = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        speed = cam.orthographicSize * sensitivity;
        targetZoom -= Input.mouseScrollDelta.y * sensitivity * cam.orthographicSize/25;
        targetZoom = Mathf.Clamp(targetZoom, maxZoom, minZoom);
        float newSize = Mathf.MoveTowards(cam.orthographicSize, targetZoom, speed * Time.deltaTime);
        cam.orthographicSize = newSize;

        if (cam.orthographicSize > zoomTarget1)
        {

        }
    }
}
