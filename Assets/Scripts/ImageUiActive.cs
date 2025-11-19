using SpatialSys.UnitySDK;
using SpatialSys.UnitySDK.Internal;
using UnityEngine;

public class ImageUiActive : MonoBehaviour
{
    public GameObject imageWorld;
    float timer;
    void Update()
    {
        if (timer >= 0.5f)
        {
            if (Input.anyKeyDown)
            {
                gameObject.SetActive(false);
                timer = 0;
            }

            if (Input.touchCount > 0)
            {
                gameObject.SetActive(false);
                timer = 0;
            }

            Debug.Log(Vector3.Distance(SpatialBridge.actorService.localActor.avatar.position, imageWorld.transform.position));
            if (Vector3.Distance(SpatialBridge.actorService.localActor.avatar.position, imageWorld.transform.position) >= 6f)
            {
                gameObject.SetActive(false);
                timer = 0;
            }
        }
        else
        {
            timer += Time.deltaTime;
        }


    }

}
