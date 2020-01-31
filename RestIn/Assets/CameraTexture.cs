using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTexture : MonoBehaviour
{
    static WebCamTexture cameraTexture;

    void Start()
    {

        var webCamDevices = WebCamTexture.devices;
        string frontCamName = null;
        foreach (var camDevice in webCamDevices)
        {
            if (camDevice.isFrontFacing)
            {
                frontCamName = camDevice.name;
                break;
            }
        }

        if (cameraTexture == null)
        {
            if (frontCamName != null)
            {
                cameraTexture = new WebCamTexture(frontCamName);

                GetComponent<Renderer>().material.mainTexture = cameraTexture;

            }
        }

        if (!cameraTexture.isPlaying)
        {
            cameraTexture.Play();
        }

    }

    void Update()
    {

    }
}
