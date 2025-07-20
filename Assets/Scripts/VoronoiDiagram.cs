using UnityEngine;
using VoronatorSharp;
using csDelaunay;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class VoronoiDiagram : MonoBehaviour
{
    private int counter = 0;
    [SerializeField] OwnerColourScript ownerColourScript;
    public Dictionary<int, Material[]> materialDictionary;
    
    public void Init(Dictionary<GameObject, int> dictionary)
    {
        materialDictionary = ownerColourScript.GetMaterialDictionary();
        List<GameObject> starList = new List<GameObject>();
        List<Vector2> pointList = new List<Vector2>();

        foreach (KeyValuePair<GameObject, int> pair in dictionary)
        {
            starList.Add(pair.Key);
            pointList.Add(pair.Key.transform.position);
        }
        VoronatorSharp.Voronator v = new VoronatorSharp.Voronator(pointList);
        for (int i = 0; i < pointList.Count; i++)
        {
            List<Vector2> vertices = v.GetClippedPolygon(i);
            GameObject insidePolygon = CreatePolygonMesh(CreateInwardPolygon(vertices, 1f), starList[i], dictionary[starList[i]], true, "insidePolygon");
            GameObject borderPolygon = CreatePolygonMesh(vertices, starList[i], dictionary[starList[i]], false, "borderPolygon");
            //if (insidePolygon != null)
            //{
            //    Debug.Log("insidePolygon");
            //}
            //if (borderPolygon != null)
            //{
            //    Debug.Log("borderPolygon");
            //}
            //if (materialDictionary == null)
            //{
            //    Debug.Log("materialDictionary is null");
            //}
            //else
            //{
            //    Debug.Log(dictionary);
            //}
            starList[i].GetComponent<StarScript>().SendPolygon(insidePolygon, borderPolygon, materialDictionary);
        }
    }
    GameObject CreatePolygonMesh(List<Vector2> polygonVertices, GameObject parentStar, int borderOwner, bool isBorder, string name)
    {
        Material currentMaterial = null;
        // Create the mesh
        Mesh mesh = new Mesh();
        List<Vector3> meshVertices = new List<Vector3>();

        // Convert the 2D polygon vertices into 3D space for Unity
        foreach (Vector2 v in polygonVertices)
        {
            meshVertices.Add(new Vector3(v.x, v.y, 0)); // Convert Vector2 to Vector3 (z=0 for 2D)
        }

        mesh.SetVertices(meshVertices);

        List<int> triangles = new List<int>();

        for (int i = 1; i < polygonVertices.Count - 1; i++)
        {
            triangles.Add(0); // Center vertex (index 0)
            triangles.Add(i); // Second vertex
            triangles.Add(i + 1); // Third vertex
        }

        mesh.SetTriangles(triangles, 0);


        // Create a GameObject to display the mesh
        GameObject polygonObject = new GameObject(name);
        polygonObject.transform.parent = parentStar.transform;
        polygonObject.AddComponent<MeshFilter>().mesh = mesh;
        

        MeshRenderer meshRenderer = polygonObject.AddComponent<MeshRenderer>();

        switch (borderOwner)
        {
            case 0:
                currentMaterial = materialDictionary[0][1];
                if (isBorder) currentMaterial = materialDictionary[0][0];
                break;
            case 1:
                currentMaterial = materialDictionary[1][1];
                if (isBorder) currentMaterial = materialDictionary[1][0];
                break;
            case 2:
                currentMaterial = materialDictionary[2][1];
                if (isBorder) currentMaterial = materialDictionary[2][0];
                break;

        }
        meshRenderer.material = currentMaterial;

        return polygonObject;
    }

    public static List<Vector2> CreateInwardPolygon(List<Vector2> polygon, float borderThickness)
    {
        List<Vector2> newPolygon = new List<Vector2>();

        for (int i = 0; i < polygon.Count; i++)
        {
            Vector2 current = polygon[i];
            Vector2 next = polygon[(i + 1) % polygon.Count];

            // Calculate the edge vector
            Vector2 edge = next - current;

            // Calculate the normal vector to this edge (perpendicular)
            Vector2 normal = new Vector2(-edge.y, edge.x).normalized;

            // Offset the vertex inward along the normal direction
            Vector2 newVertex = current + normal * borderThickness;

            newPolygon.Add(newVertex);
        }

        return newPolygon;
    }
}