using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Gameplay")]
    [SerializeField] private MainPlayer player;
    [SerializeField] private float cameraFollowThreshold = 2.0f;

    private Vector3 playerPosition = Vector3.zero;
    private float cameraGap = 0f;

    // Update is called once per frame
    void Update()
    {
        playerPosition = player.GetPlayerPosition();

        cameraGap = (playerPosition.x - transform.position.x);

        if (Mathf.Abs(cameraGap) >  cameraFollowThreshold)
        {
            transform.position += (new Vector3(cameraGap, 0, 0)) * Time.deltaTime;
        }
    }
}
