using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator anim;
    bool isOpened;

    private void Awake()
    {
        anim = transform.GetComponent<Animator>();
    }

    public bool IsOpened
    {
        get{
            return isOpened;
        }
        set
        {
            isOpened = value;
            if(isOpened) anim.SetTrigger("open");
            else anim.SetTrigger("close");
        }
    }
}
