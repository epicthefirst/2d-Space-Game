using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public Material dottedLineMaterial;
    public Dictionary<ShipController, GameObject> linePathDictionary = new Dictionary<ShipController, GameObject>();
    public UIManager uIManager;

    private void Start()
    {
        uIManager.NewTick += thisNewTick;
    }
    public void drawLinePath(List<Vector2> pointList, ShipController linkedController)
    {
        GameObject dottedPath = new GameObject("dottedPath");
        LineRenderer lineMaker = dottedPath.AddComponent<LineRenderer>();
        lineMaker.material = dottedLineMaterial;
        lineMaker.material.mainTextureScale = new Vector2(1.0f, 1.0f);
        lineMaker.startWidth = 1;
        lineMaker.endWidth = 1;

        lineMaker.textureMode = LineTextureMode.Tile;
        lineMaker.material.mainTexture.wrapMode = TextureWrapMode.MirrorOnce;
        lineMaker.positionCount = pointList.Count;

        for (int i = 0; i < pointList.Count; i++)
        {
            lineMaker.SetPosition(i, pointList[i]);
        }
        linePathDictionary.Add(linkedController, dottedPath);
        Debug.Log("Made path");
    }
    private void thisNewTick(object sender, CycleEvent e)
    {
        foreach (GameObject thing in linePathDictionary.Values)
        {
            Destroy(thing);
        }
        linePathDictionary.Clear();
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
