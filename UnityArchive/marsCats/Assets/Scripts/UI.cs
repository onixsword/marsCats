using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private Slider h2O;
    [SerializeField] private Slider food;
    [SerializeField] private Slider O2;
    [SerializeField] private Slider Health;
    [SerializeField] private Slider Radiation;

    private Astrocat player;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(searchPlayer());
    }

    // Update is called once per frame
    void Update()
    {
        h2O.value = player.ActualWater / player.MaxWater * 100;
        food.value = player.ActualFood / player.MaxFood * 100;
        O2.value = player.ActualOxigen / player.MaxOxigen * 100;
        Health.value = player.ActualCharacterHealth / player.MaxCharacterHealth * 100;
        Radiation.value = player.ActualRadiation / player.MaxRadiationResistance * 100;
    }

    private IEnumerator searchPlayer()
    {
        yield return new WaitUntil(() => gameManager.instance.Player != null);
        player = gameManager.instance.Player.GetComponent<Astrocat>();

    }
}
