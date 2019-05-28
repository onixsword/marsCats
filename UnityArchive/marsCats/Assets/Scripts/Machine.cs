using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : NPC
{
    private bool haveBeacon = false;
    private GameObject beacon;

    private void Update()
    {
        if (navAgent.remainingDistance < navAgent.stoppingDistance + 0.1f)
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
