using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Mover : MonoBehaviour
{
    private Vector3 startPos;
    private float domainX;
    private float domainY;
    
    public void StartMoving()
    {
        startPos = transform.localPosition;
    }
}
