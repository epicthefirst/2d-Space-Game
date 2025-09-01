using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public Material dottedLineMaterial;
    private void Start()
    {
        dottedLine(new Vector2(0, 0), new Vector2(50, 0), gameObject);
    }
    void dottedLine(Vector2 startPoint, Vector2 endPoint, GameObject linkedObject)
    {
        GameObject dottedLine = new GameObject("dottedLine");   
        LineRenderer lineMaker = dottedLine.AddComponent<LineRenderer>();
        lineMaker.material = dottedLineMaterial;
        lineMaker.material.mainTextureScale = new Vector2(1f, 1.0f);
        lineMaker.startWidth = 1;
        lineMaker.endWidth = 1;

        lineMaker.textureMode = LineTextureMode.Tile;
        /*        lineMaker.material.mainTexture.wrapMode = TextureWrapMode.Repeat;*/
        //lineRenderer.material.mainTextureScale = new Vector2(1f / width, 1.0f);

        lineMaker.positionCount = 2;

        lineMaker.SetPosition(0, startPoint);
        lineMaker.SetPosition(1, endPoint);

    }

}
