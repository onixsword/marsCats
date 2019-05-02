using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemDetectors : MonoBehaviour
{
    [SerializeField] private Color color;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float distance;
    [SerializeField] private LayerMask activeLayers;

    public bool isDetecting()
    {
        return Physics.Raycast(transform.position, direction.normalized, distance, activeLayers);
    }

    public Transform whatDetected()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, direction.normalized, out hit, distance, activeLayers);
        return hit.transform;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawLine(transform.position, transform.position + (direction.normalized * distance));
    }
}
