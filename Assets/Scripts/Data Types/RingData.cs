using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingData
{
    public GameObject originalStar;
    public GameObject star;
    public Vector2 position;
    public int ring;
    public int orderInRing;
    public float distance;
    public RingData(GameObject star, Vector2 position, int ring, int orderInRing)
    {
        this.originalStar = star;
        this.star = star;
        this.position = position;
        this.ring = ring;
        this.orderInRing = orderInRing;
    }
}
