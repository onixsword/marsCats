using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{

    public static gameManager instance;

    private void Awake()
    {
        if (gameManager.instance != null && gameManager.instance != this) GameObject.Destroy(gameObject);
        else gameManager.instance = this;
    }

}
