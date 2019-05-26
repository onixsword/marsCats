using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemDetectors : MonoBehaviour
{
    private enum directions {front, up, down, left, right, back, noneRelative};
    [SerializeField] private directions activeDirection;
    [SerializeField] private Color color;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float distance;
    [SerializeField] private LayerMask activeLayers;

    public bool isDetecting()
    {
        switch (activeDirection)
        {
            case directions.front:
                return Physics.Raycast(transform.position, transform.forward, distance, activeLayers);
            case directions.up:
                return Physics.Raycast(transform.position, transform.up, distance, activeLayers);
            case directions.down:
                return Physics.Raycast(transform.position, -transform.up, distance, activeLayers);
            case directions.left:
                return Physics.Raycast(transform.position, -transform.right, distance, activeLayers);
            case directions.right:
                return Physics.Raycast(transform.position, transform.right, distance, activeLayers);
            case directions.back:
                return Physics.Raycast(transform.position, -transform.forward, distance, activeLayers);
            case directions.noneRelative:
                return Physics.Raycast(transform.position, direction.normalized, distance, activeLayers);
        }
        return false;
    }

    private RaycastHit gethit()
    {
        RaycastHit hit = new RaycastHit();
        switch (activeDirection)
        {
            case directions.front:
                Physics.Raycast(transform.position, transform.forward, out hit, distance, activeLayers);
                break;
            case directions.up:
                Physics.Raycast(transform.position, transform.up, out hit, distance, activeLayers);
                break;
            case directions.down:
                Physics.Raycast(transform.position, -transform.up, out hit, distance, activeLayers);
                break;
            case directions.left:
                Physics.Raycast(transform.position, -transform.right, out hit, distance, activeLayers);
                break;
            case directions.right:
                Physics.Raycast(transform.position, transform.right, out hit, distance, activeLayers);
                break;
            case directions.back:
                Physics.Raycast(transform.position, -transform.forward, out hit, distance, activeLayers);
                break;
            case directions.noneRelative:
                Physics.Raycast(transform.position, direction.normalized, out hit, distance, activeLayers);
                break;
        }
        return hit;
    }

    public Transform whatDetected()
    {
        return gethit().transform;
    }

    public Vector3 whereHit()
    {
        return gethit().point;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = color;

        switch (activeDirection)
        {
            case directions.front:
                Gizmos.DrawLine(transform.position, transform.position + (transform.forward * distance));
                break;
            case directions.up:
                Gizmos.DrawLine(transform.position, transform.position + (transform.up * distance));
                break;
            case directions.down:
                Gizmos.DrawLine(transform.position, transform.position + (-transform.up * distance));
                break;
            case directions.left:
                Gizmos.DrawLine(transform.position, transform.position + (-transform.right * distance));
                break;
            case directions.right:
                Gizmos.DrawLine(transform.position, transform.position + (transform.right * distance));
                break;
            case directions.back:
                Gizmos.DrawLine(transform.position, transform.position + (-transform.forward * distance));
                break;
            case directions.noneRelative:
                Gizmos.DrawLine(transform.position, transform.position + (direction.normalized * distance));
                break;
        }
    }
}
