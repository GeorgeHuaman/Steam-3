using SpatialSys.UnitySDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesactivateImage : MonoBehaviour
{
    public GameObject imageWorld;
    float timer;
    bool isActive;

    void Update()
    {
        if (isActive)
        {
            if (timer >= 0.5f)
            {
                if (Input.anyKeyDown)
                {
                    imageWorld.SetActive(false);
                    timer = 0;
                    isActive = false;
                }

                if (Input.touchCount > 0)
                {
                    imageWorld.SetActive(false);
                    timer = 0;
                    isActive = false;
                }

                Debug.Log(Vector3.Distance(SpatialBridge.actorService.localActor.avatar.position, gameObject.transform.position));
                if (Vector3.Distance(SpatialBridge.actorService.localActor.avatar.position, gameObject.transform.position) >= 6f)
                {
                    imageWorld.SetActive(false);
                    timer = 0;
                    isActive = false;
                }
            }
            else
            {
                timer += Time.deltaTime;
            }
        }

    }

    public void Activate()
    {
        isActive = true;
    }
}
