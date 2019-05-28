using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preview : MonoBehaviour
{

    [Header("Detectors")]
    [SerializeField] private SystemDetectors north;
    [SerializeField] private SystemDetectors south;
    [SerializeField] private SystemDetectors east;
    [SerializeField] private SystemDetectors west;
    [SerializeField] private SystemDetectors center;

    [SerializeField] private LayerMask allowedItemsToConstructOver;

    //resources
    [SerializeField] private GameObject beacon;

    private Collider col;
    private Material mat;
    private Renderer rend;
    private Transform constructor;
    private int nonAllowedItemsInTrigger;

    // Start is called before the first frame update
    void Awake()
    {
        col = GetComponent<Collider>();
        col.isTrigger = true;
        rend = GetComponent<Renderer>();
        mat = rend.material;
        mat.color = Color.blue;
    }

    // Update is called once per frame
    void Update()
    {
        mat.color = (isAbleToConstruct) ? Color.blue : Color.red;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != allowedItemsToConstructOver) nonAllowedItemsInTrigger++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != allowedItemsToConstructOver) nonAllowedItemsInTrigger--;
    }

    private bool inDistanceToBuild
    {
        get {
            return west.isDetecting() && north.isDetecting() && east.isDetecting() && south.isDetecting();
        }
    }

    private bool isAbleToConstruct
    {
        get
        {
            return nonAllowedItemsInTrigger <= 0 && inDistanceToBuild;
        }
    }

    public IEnumerator buildEdification(float timeToBuild)
    {
        foreach (Machine searcher in gameManager.instance.NPCs)
        {
            if (searcher.name == "constructor")
            {
                searcher.callMachine(GameObject.Instantiate(beacon, transform.position, Quaternion.identity));
                constructor = searcher.transform;
                break;
            }
        }
        rend.enabled = false;
        yield return new WaitUntil(() => Vector3.Distance(transform.position, constructor.position) <= Vector3.Distance(center.transform.position, north.transform.position) + 8);
        yield return new WaitForSeconds(timeToBuild);
        Destroy(this);
    }

    private void OnDestroy()
    {
        if (isAbleToConstruct)
        {
            //convert to actual edification
            GameObject.Destroy(north.gameObject);
            GameObject.Destroy(south.gameObject);
            GameObject.Destroy(east.gameObject);
            GameObject.Destroy(west.gameObject);
            GameObject.Destroy(center.gameObject);
            rend.enabled = true;
            col.isTrigger = false;
            mat.color = Color.white;
            gameObject.layer = 16;
            
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
    }
}
