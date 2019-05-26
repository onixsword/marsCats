using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : NPC
{
    private bool haveBeacon = false;
    [SerializeField] private float distanceToEraseBeacon;
    private GameObject beacon;

    private void Update()
    {
        if (navAgent.remainingDistance < distanceToEraseBeacon)
        {
            GameObject.Destroy(beacon);
        }
    }

    public void callMachine(GameObject beacon)
    {
        this.beacon = beacon;
        walkTo(beacon.transform.position);
    }
}
