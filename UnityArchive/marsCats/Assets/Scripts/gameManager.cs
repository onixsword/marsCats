using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    private Transform player;

    [SerializeField]private float timeToStorm;
    private Transform storm;
    private bool stormIsActive;

    private Transform[] edifications;

  

    private void Awake()
    {
        if (gameManager.instance != null && gameManager.instance != this) GameObject.Destroy(gameObject);
        else gameManager.instance = this;
    }

    private void Start()
    {
        StartCoroutine(stormTimer(timeToStorm));
    }

    private IEnumerator stormTimer(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            StormIsActive = !StormIsActive;
        }
    }

    private void stormChangeState()
    {
        storm.gameObject.SetActive(StormIsActive);
        storm.position = player.position; 
    }

    public Transform Player { get => player; set => player = value; }
    public bool StormIsActive {
        get
        {
            return stormIsActive;
        }
        set
        {
            stormIsActive = value;
            stormChangeState();
        }
    }
}
