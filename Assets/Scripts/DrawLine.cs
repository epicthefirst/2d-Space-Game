using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class DrawLine : MonoBehaviour
{
    public Material dottedLineMaterial;
    public Material transparentDottedLineMaterial;
    public Dictionary<ShipController, GameObject> linePathDictionary = new Dictionary<ShipController, GameObject>();
    public UIManager uIManager;

    private void Start()
    {
/*        uIManager.NewTick += thisNewTick;*/
    }
    public GameObject makeTempPathObject()
    {
        GameObject dottedPath = new GameObject("tempDottedPath");
        LineRenderer lineMaker = dottedPath.AddComponent<LineRenderer>();

        lineMaker.material = dottedLineMaterial;
        lineMaker.material.color -= new Color(0, 0, 0, 0.5f);

        lineMaker.material.mainTextureScale = new Vector2(1.0f, 1.0f);
        lineMaker.startWidth = 1;
        lineMaker.endWidth = 1;

        lineMaker.textureMode = LineTextureMode.Tile;
        lineMaker.material.mainTexture.wrapMode = TextureWrapMode.Repeat;
        lineMaker.numCornerVertices = 1;

        //lineMaker.positionCount = pointList.Count;

        //for (int i = 0; i < pointList.Count; i++)
        //{
        //    lineMaker.SetPosition(i, pointList[i]);
        //}
        //Debug.Log("Made path");
        return dottedPath;
    }
    public GameObject drawLinePath(List<Vector2> pointList)
    {
        GameObject dottedPath = new GameObject("dottedPath");
        LineRenderer lineMaker = dottedPath.AddComponent<LineRenderer>();
        lineMaker.material = dottedLineMaterial;
        lineMaker.material.mainTextureScale = new Vector2(1.0f, 1.0f);
        lineMaker.startWidth = 1;
        lineMaker.endWidth = 1;

        lineMaker.textureMode = LineTextureMode.Tile;
        lineMaker.material.mainTexture.wrapMode = TextureWrapMode.Repeat;
        lineMaker.numCornerVertices = 1;
        
        lineMaker.positionCount = pointList.Count;

        for (int i = 0; i < pointList.Count; i++)
        {
            lineMaker.SetPosition(i, pointList[i]);
        }
        Debug.Log("Made path");
        return dottedPath;
    }
    public void addCarrierPath(ShipController linkedController)
    {
        linePathDictionary.Remove(linkedController);
        List<Vector2> pointList = new List<Vector2>();
        pointList.Add(linkedController.gameObject.transform.position);
        for (int i = 0; i < linkedController.starWaypoints.Count; i++)
        {
            pointList.Add(linkedController.starWaypoints[i].transform.position);
        }
        linePathDictionary.Add(linkedController, drawLinePath(pointList));
        Debug.LogWarning("Made addCarrierPath");
    }
    public void removeCarrierPath(ShipController controller)
    {
        linePathDictionary.Remove(controller);
    }
    private void thisNewTick(object sender, CycleEvent e)
    {
        /*        foreach (ShipController thing in linePathDictionary.Keys)
                {
                    List<Vector2> tempVectors = new List<Vector2>();
                    tempVectors.Add(thing.gameObject.transform.position);
                    List<GameObject> waypoints = thing.GetWaypoints();
                    if (waypoints.Count == 0)
                    {
                        linePathDictionary.Remove(thing);
                    }
                    foreach (GameObject obj in waypoints)
                    {
                        tempVectors.Add(obj.transform.position);
                    }

                }*/

        for (int i = 0; i < linePathDictionary.Count; i++) {
            ShipController thing = linePathDictionary.ElementAt(i).Key;
            if (thing.starWaypoints.Count > 0)
            {
                List<Vector2> pointList = new List<Vector2>();
                pointList.Add(thing.gameObject.transform.position);
                foreach (GameObject obj in thing.starWaypoints)
                {
                    pointList.Add(obj.transform.position);
                }
                reDrawLine(linePathDictionary[thing], pointList);
            }
        }
    }
    public void updateCarrier(ShipController controller)
    {
        if (controller.starWaypoints.Count > 0)
        {
            List<Vector2> pointList = new List<Vector2>();
            pointList.Add(controller.gameObject.transform.position);
            foreach (GameObject obj in controller.starWaypoints)
            {
                pointList.Add(obj.transform.position);
            }
            reDrawLine(linePathDictionary[controller], pointList);
        }
    }
    public void reDrawLine(GameObject line, List<Vector2> vectors)
    {
        LineRenderer lr = line.GetComponent<LineRenderer>();
        lr.positionCount = vectors.Count;
        for (int i = 0;i < vectors.Count;i++)
        {
            lr.SetPosition(i, vectors[i]);
        }
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
