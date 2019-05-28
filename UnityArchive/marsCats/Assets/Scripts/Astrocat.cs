﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.SystemControls;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]

public class Astrocat : MonoBehaviour
{
    [Header("Movement Variables")]
    //[SerializeField] private float moveSpeed;
    [SerializeField] private float advancementForce;
    [SerializeField, Tooltip("Angle in which the force vector of the movement is realized")] private float advancementAngle;
    [SerializeField] private float rotationSpeed;

    private PhotonView m_PhotonView;
    private PhotonTransformView m_TransformView;

    private Rigidbody rigid;
    private Animator anim;
    private SystemDetectors targetDetector;

    [Header("Survival Variables")]
    [SerializeField, Tooltip("Max Health of the character. If health reach 0, character dies")] private float maxCharacterHealth = 100;
    private float actualCharacterHealth;
    [SerializeField, Tooltip("Max value for water. If water reach 0, character lose health")] private float maxWater = 100;
    private float actualWater;
    [SerializeField, Tooltip("Max value for food. If food reach 0, character lose health")] private float maxFood = 100;
    private float actualFood;
    [SerializeField, Tooltip("Max value for oxigen. If oxigen reach 0, character dies")] private float maxOxigen = 100;
    private float actualOxigen;
    [SerializeField, Tooltip("Values of temperature in which character survives." +
        " If body temperature go out of values, character lose health")]
    private Vector2 temperatureRange = new Vector2(-20, 50);
    private float actualTemperature;
    [SerializeField, Tooltip("Max value for radiation resistance. " +
        "If radiation reach value, character dies")]
    private float maxRadiationResistance = 100;
    private float actualRadiation;
    [SerializeField, Tooltip("Max Health of the spacesuit. " +
        "If health reach 0, suits fails and character lose protection")]
    private float maxSuitHealth = 100;
    private float actualSuitHealth;
    [SerializeField, Tooltip("Max force that spacesuit tolerates before getting damaged")] private float maxForceTillDamageSuit;
    [SerializeField, Tooltip("Max force that character tolerates before getting damaged")] private float maxForceTillDamageCharacter;
    private bool spacesuitIsBroken = false;

    [Header("Ubications objects")]
    [SerializeField] private Transform head;
    [SerializeField] private SystemDetectors groundDetector;

    [Header("Resources")]
    [SerializeField] private GameObject beacon;
    [SerializeField] private GameObject edification;

    [Header("Animation Values")]
    private bool pasoIzquierdo = false;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        m_PhotonView = GetComponent<PhotonView>();
        m_TransformView = GetComponent<PhotonTransformView>();
        if (m_PhotonView.isMine)
        {
            Camera.main.transform.parent = head.transform;
            Camera.main.transform.position = head.position;
            targetDetector = Camera.main.transform.GetComponent<SystemDetectors>();
        }

        rigid.maxAngularVelocity = 0;

        //assign survival values
        ActualCharacterHealth = maxCharacterHealth;
        ActualWater = maxWater;
        ActualFood = maxFood;
        ActualOxigen = maxOxigen;
        ActualRadiation = 0;
        ActualTemperature = 37;
        ActualSuitHealth = maxSuitHealth;

        pasoIzquierdo = false;

        gameManager.instance.Player = transform;
    }

    private void FixedUpdate()
    {
        if (m_PhotonView.isMine)
        {
            Movement();
        }
    }

    private void Update()
    {
        updateSurvivalValues();
        if (Input.GetButtonDown("Fire1")) placeItem();
        selectConstruction();
    }

    private void Movement()
    {
        //displacement
        if (SystemControls.Axis.y != 0 && isGrounded)
        {
            Vector3 moveVector = transform.forward;
            moveVector = new Vector3(moveVector.x * Mathf.Cos(advancementAngle * Mathf.Deg2Rad), Mathf.Sin(advancementAngle * Mathf.Deg2Rad),
                moveVector.z * Mathf.Cos(advancementAngle * Mathf.Deg2Rad));
            moveVector = moveVector.normalized * advancementForce * SystemControls.Axis.y;
            if (SystemControls.Axis.y < 0) moveVector.y = -moveVector.y;
            rigid.AddForce(moveVector, ForceMode.Impulse);

            StartCoroutine(changeFeet());
        }

        transform.Rotate(transform.up * SystemControls.Axis.x * Time.deltaTime * rotationSpeed);

        anim.SetBool("walking", SystemControls.Axis.y != 0);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("velocity", rigid.velocity.y);
    }

    private void updateSurvivalValues()
    {
        ActualCharacterHealth += Time.deltaTime / 100;
        ActualOxigen -= spacesuitIsBroken ? Time.deltaTime * 10 : Time.deltaTime / 100;
        ActualWater -= Time.deltaTime / 100;
        ActualFood -= Time.deltaTime / 100;
        ActualRadiation += Time.deltaTime / 1000;
        ActualTemperature = 37;

    }

    private void placeItem()
    {
        if (targetDetector.isDetecting())
        {
            foreach (Machine searcher in gameManager.instance.NPCs)
            {
                if (searcher.name == "driller")
                {
                    searcher.callMachine(GameObject.Instantiate(beacon, targetDetector.whereHit(), Quaternion.identity));
                    break;
                }
            }
            anim.SetTrigger("place");
        }
    }

    private void selectConstruction()
    {
        if (Input.GetKeyDown(KeyCode.E))
            StartCoroutine(
                displayEdification(
                    GameObject.Instantiate(
                        edification, 
                        targetDetector.whereHit(), 
                        Quaternion.identity)
                    .transform
                )
            );
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.CompareTag("collidable"))
        {
            //**************************************corregir ecuaciones para ser mas realistas
            if (rigid.velocity.magnitude >= maxForceTillDamageSuit) ActualSuitHealth -= rigid.velocity.magnitude - maxForceTillDamageSuit;
            if (rigid.velocity.magnitude >= maxForceTillDamageCharacter) ActualCharacterHealth -= rigid.velocity.magnitude - maxForceTillDamageCharacter; ;
        }
    }

    private IEnumerator changeFeet()
    {
        anim.SetBool("pasoIzquierdo", pasoIzquierdo);
        pasoIzquierdo = !pasoIzquierdo;
        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator displayEdification(Transform item)
    {
        while (Input.GetKey(KeyCode.E))
        {
            yield return new WaitForFixedUpdate();
            item.position = targetDetector.whereHit();
        }
        anim.SetTrigger("place");
        Destroy(item.GetComponent<Preview>());
    }

    private void drink(float cuantityDrinked)
    {
        ActualWater += cuantityDrinked;
        anim.SetTrigger("drink");
    }

    private void Eat(float cuantityEaten)
    {
        ActualFood += cuantityEaten;
        anim.SetTrigger("eat");
    }

    private void openCloseDoor()
    {
        if (targetDetector.isDetecting())
        {
            Door interactingDoor = targetDetector.whatDetected().GetComponent<Door>();
            if (interactingDoor != null)
            {
                interactingDoor.IsOpened = !interactingDoor.IsOpened;
                anim.SetTrigger("open");
            }
            
        }
        
    }

    private bool isGrounded
    {
        get
        {
            return groundDetector.isDetecting();
        }
    }

    public float ActualSuitHealth
    {
        get
        {
            return actualSuitHealth;
        }
        set
        {
            actualSuitHealth = value;
            if (actualSuitHealth <= 0)
            {
                actualSuitHealth = 0;
                spacesuitIsBroken = true;
            }
            else
            {
                actualSuitHealth = actualSuitHealth >= maxSuitHealth ? maxSuitHealth : actualSuitHealth;
                spacesuitIsBroken = false;
            }
        }
    }

    public float ActualCharacterHealth
    {
        get
        {
            return actualCharacterHealth;
        }
        set
        {
            actualCharacterHealth = value;
            if (actualCharacterHealth <= 0) die();
            else if (actualCharacterHealth > 100) actualCharacterHealth = 100;
        }
    }

    public float ActualWater
    {
        get
        {
            return actualWater;
        }
        set
        {
            actualWater = value;
            if (actualWater < 0)
            {
                ActualCharacterHealth += actualWater;
                actualWater = 0;
            }
        }
    }

    public float ActualFood
    {
        get
        {
            return actualFood;
        }
        set
        {
            actualFood = value;
            if (actualFood < 0)
            {
                ActualCharacterHealth += actualFood;
                actualFood = 0;
            }
        }
    }

    public float ActualOxigen
    {
        get
        {
            return actualOxigen;
        }
        set
        {
            actualOxigen = value;
            if (actualOxigen < 0)
            {
                ActualCharacterHealth += actualOxigen * 10;
                actualOxigen = 0;
            }
        }
    }

    public float ActualTemperature
    {
        get
        {
            return actualTemperature;
        }
        set
        {
            actualTemperature = value;
            if (actualTemperature < temperatureRange.x) ActualCharacterHealth -= Mathf.Abs(Mathf.Abs(actualTemperature) - Mathf.Abs(temperatureRange.x));
            else if (actualTemperature > temperatureRange.y) ActualCharacterHealth -= Mathf.Abs(Mathf.Abs(actualTemperature) - Mathf.Abs(temperatureRange.y));
        }
    }

    public float ActualRadiation
    {
        get
        {
            return actualRadiation;
        }
        set
        {
            actualRadiation = value;
            if (actualRadiation >= maxRadiationResistance) die();
        }
    }

    private void die()
    {
        anim.SetTrigger("death");
    }
}