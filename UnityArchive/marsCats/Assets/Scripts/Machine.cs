using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : NPC
{
    private bool haveBeacon = false;
    private GameObject beacon;

    [Header("audios")]
    [SerializeField] private AudioClip act;
    [SerializeField] private AudioClip accelerate;
    [SerializeField] private AudioClip motor;
    private bool moving = false;

    private void Update()
    {
        if (navAgent.remainingDistance < navAgent.stoppingDistance + 0.1f)
        {
            GameObject.Destroy(beacon);
            moving = false;
        }
        else
        {
            moving = true;
        }

    }

    public bool Moving {
        get{
            return moving;
        }
        set
        {
            if (moving != value)
            {
                anim.SetBool("moving", moving);
                if (!moving)
                {
                    anim.SetTrigger("act");
                    aud.PlayOneShot(act);
                    Debug.Log("acted");
                } else
                {
                    aud.PlayOneShot(accelerate);
                    Debug.Log("accelerated");
                }
                moving = value;
            }
        }
    }

    public void callMachine(GameObject beacon)
    {
        this.beacon = beacon;
        walkTo(beacon.transform.position);
    }
}
