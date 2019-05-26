using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]

public abstract class NPC : MonoBehaviour
{
    protected NavMeshAgent navAgent;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        StartCoroutine(AddItselfToList());
    }

    protected void walkTo(Vector3 target)
    {
        navAgent.SetDestination(target);
    }

    private IEnumerator AddItselfToList()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            try
            {
                gameManager.instance.NPCs.Add(this);
                Debug.Log("Added to list");
                break;
            }
            catch (System.Exception ex)
            {
                Debug.Log("Retrying. Error: " + ex.Message);
            };
        }
        
    }

}
