using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    private enum disasterState { sandstorm, solarEruption, none };
    public static gameManager instance;
    private Transform player;

    [Header("Weather")]

    [SerializeField] private Vector2 mapSize;
    [SerializeField] private float timeToUpdateWeather;
    [SerializeField] private Vector2 disasterRangeSize;
    private Vector2 disasterPosition;
    private float disasterSize;
    disasterState activeDisaster;
    

    [SerializeField] private float sandstormProbability;
    [SerializeField] private float solarEruptionProbability;

    private List<Transform> edifications;
    private List<NPC> npcs;

    private void Awake()
    {
        if (gameManager.instance != null && gameManager.instance != this) GameObject.Destroy(gameObject);
        else gameManager.instance = this;

        edifications = new List<Transform>();
        npcs = new List<NPC>();

        activeDisaster = disasterState.none;
    }

    private void Start()
    {
        StartCoroutine(disasterTimer(timeToUpdateWeather));
    }

    private IEnumerator disasterTimer(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            activeDisaster = Random.Range(0, 100) < sandstormProbability ? disasterState.sandstorm : Random.Range(0, 100) < sandstormProbability + solarEruptionProbability ? disasterState.solarEruption : disasterState.none;
            if (activeDisaster != disasterState.none)
            {
                disasterSize = Random.Range(disasterRangeSize.x, disasterRangeSize.y);
                disasterPosition = new Vector2(Random.Range(-mapSize.x / 2 + disasterSize, mapSize.x / 2 - disasterSize), Random.Range(-mapSize.y / 2 + disasterSize, mapSize.y / 2 - disasterSize));
            }
        }
    }

    public Transform Player { get => player; set => player = value; }
    public List<Transform> Edifications { get => edifications; set => edifications = value; }
    public List<NPC> NPCs { get => npcs; set => npcs = value; }
}
