using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_camera : MonoBehaviour
{
    public Transform cameraPosition;

    // Update is called once per frame
    private void Update()
    {
        // keeps camera to player's "CameraPos" position
        transform.position = cameraPosition.position;
    }
}
