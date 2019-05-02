using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reticleController : MonoBehaviour
{

    [SerializeField] private Transform reticle;
    [SerializeField] private float maxDistance;
    [SerializeField] private LayerMask layer;

    //initial transform
    private Vector3 initialLocalPosition;
    private Vector3 initialScale;
    private Quaternion initialRotation;
    private RaycastHit hit;

    private bool isHitting;

    private void Start()
    {
        initialLocalPosition = reticle.localPosition;
        initialScale = reticle.localScale;
        initialRotation = reticle.localRotation;
        hit = new RaycastHit();
    }

    private void Update()
    {
        isHitting = Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, layer);

        if (isHitting)
        {
            reticle.position = hit.point;
            reticle.localScale = initialScale * hit.distance;
            reticle.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
        }
        else
        {
            reticle.localPosition = initialLocalPosition;
            reticle.localScale = initialScale;
            reticle.localRotation = initialRotation;
        }
    }

    public bool IsHitting
    {
        get
        {
            return isHitting;
        }
    }

    public RaycastHit Hit
    {
        get
        {
            return hit;
        }
    }
}
