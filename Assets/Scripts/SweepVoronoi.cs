//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;




//// Define a structure for a point in 2D space
//public struct Point
//{
//    public double x, y;
//}

//// Define a structure for a Voronoi region
//public struct Region
//{
//    public Point site; // The site that this region is based on
//    public List<Point> vertices; // The vertices of this region
//}

//public class SweepVoronoi : MonoBehaviour
//{
//    // Define a list of points
//    List<Point> points = new List<Point> {};

//    // Function to generate Voronoi regions for a given set of points
//    public static List<Region> VoronoiSweepLine(List<Point> points)
//    {
//        int n = points.Count;
//        List<Region> regions = new List<Region>(new Region[n]);

//        // For each point, create a region with the site at (0, 0)
//        for (int i = 0; i < n; ++i)
//        {
//            Region region = new Region { site = new Point { x = 0, y = 0 }, vertices = new List<Point>() };
//            regions[i] = region;
//        }

//        return regions;
//    }

//    public void Init(List<Vector2> pointList)
//    {
//        foreach (Vector2 z in pointList)
//        {
//            points.Add(new Point { x = z.x, y = z.y });
//        }

//        // Generate Voronoi regions for the points
//        List<Region> voronoiRegions = VoronoiSweepLine(points);

//        // Print out the Voronoi regions
//        for (int i = 0; i < voronoiRegions.Count; ++i)
//        {
//            Debug.Log("Voronoi Region #" + (i + 1) + ": Site (" + voronoiRegions[i].site.x + ", " + voronoiRegions[i].site.y + ")");
//            foreach (Point vertex in voronoiRegions[i].vertices)
//            {
//                Debug.Log("Vertex (" + vertex.x + ", " + vertex.y + ")");
//            }
//        }
//    }
//}


using System.Collections.Generic;
using UnityEngine;

public struct Point
{
    public float x, y;

    public Point(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
}

public struct Region
{
    public Point site;
    public List<Vector2> vertices;
}

public class SweepVoronoi : MonoBehaviour
{
    public List<Point> sites;  // The list of sites to generate the Voronoi diagram
    public float mapSize = 10f;  // Size of the map

    public void Init(List<Vector2> pointList)
    {
        foreach (Vector2 z in pointList)
        {
            sites.Add(new Point { x = z.x, y = z.y });
        }

        // Generate Voronoi regions
        List<Region> voronoiRegions = GenerateVoronoi(sites);

        // Create the Voronoi diagram in Unity
        CreateVoronoiMesh(voronoiRegions);
    }

    private List<Region> GenerateVoronoi(List<Point> sites)
    {
        List<Region> regions = new List<Region>();

        // Create regions for each site with empty vertices (this is a simplification)
        foreach (Point site in sites)
        {
            Region region = new Region { site = site, vertices = new List<Vector2>() };
            regions.Add(region);
        }

        // Note: Here we are simplifying the Voronoi region generation.
        // A full Voronoi algorithm (like Fortune's Algorithm) is needed to calculate the vertices.

        // For now, we will just create random vertices for each region.
        foreach (Region region in regions)
        {
            // Generating random points for each region (to be replaced with actual Voronoi calculation)
            for (int i = 0; i < 5; i++) // Random vertices per region
            {
                region.vertices.Add(new Vector2(Random.Range(0f, mapSize), Random.Range(0f, mapSize)));
            }
        }

        return regions;
    }

    private void CreateVoronoiMesh(List<Region> regions)
    {
        foreach (Region region in regions)
        {
            // Create a GameObject to hold the mesh
            GameObject regionObject = new GameObject("Region");
            regionObject.transform.parent = transform;

            // Create a mesh for the region
            Mesh mesh = new Mesh();
            regionObject.AddComponent<MeshFilter>().mesh = mesh;
            regionObject.AddComponent<MeshRenderer>();

            // Assign the vertices
            List<Vector3> vertices = new List<Vector3>();
            foreach (var vertex in region.vertices)
            {
                vertices.Add(new Vector3(vertex.x, vertex.y, 0f)); // Create 3D positions
            }

            mesh.SetVertices(vertices);

            // Define triangles (for simplicity, this assumes convex shapes for now)
            List<int> triangles = new List<int>();
            for (int i = 1; i < vertices.Count - 1; i++)
            {
                triangles.Add(0); // Center point
                triangles.Add(i); // The i-th point
                triangles.Add(i + 1); // Next point
            }

            mesh.SetTriangles(triangles, 0);

            // Optionally: Assign a color to the region
            regionObject.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
        }
    }
}

