using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerSpawner : MonoBehaviour
{
    void OnJoinedRoom()
    {
        CreatePlayerObject();
    }

    void CreatePlayerObject()
    {
        Vector3 position = new Vector3(0f, 1f, 0f);

        GameObject newPlayerObject = PhotonNetwork.Instantiate("Astrocat", position, Quaternion.identity, 0);
    }
}
