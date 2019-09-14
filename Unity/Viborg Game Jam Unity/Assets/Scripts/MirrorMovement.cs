using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorMovement : MonoBehaviour
{

    public Transform player;
    public float initialPos;

    public float reach = 3f;

    public GameObject mirrorCarrier;

    public float progress = 0f;
    public float initialProgress;

    public float trackLength = 1f;
    public float tempTrackLength = 1f;

    public bool z;
    public bool useTemp;

    public bool stopTemp;

    public Transform from;
    public Transform tempTo;
    public Transform to;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if((player.transform.position - mirrorCarrier.transform.position).magnitude <= reach)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (z)
                {
                    initialPos = player.transform.position.z;
                }
                else
                {
                    initialPos = player.transform.position.x;
                }

                initialProgress = progress;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                if(useTemp)
                {
                    if (z)
                    {
                        progress = initialProgress + (player.transform.position.z - initialPos) / tempTrackLength;
                    }
                    else
                    {
                        progress = initialProgress + (player.transform.position.x - initialPos) / tempTrackLength;
                    }
                }
                else
                {
                    if (z)
                    {
                        progress = initialProgress + (player.transform.position.z - initialPos) / trackLength;
                    }
                    else
                    {
                        progress = initialProgress + (player.transform.position.x - initialPos) / trackLength;
                    }
                }
            }
        }
        
        progress = Mathf.Clamp(progress, 0f, 1f);

        if(useTemp)
        {
            mirrorCarrier.transform.position = Vector3.Lerp(from.position, tempTo.position, progress);
        }
        else
        {
            mirrorCarrier.transform.position = Vector3.Lerp(from.position, to.position, progress);
        }

        if(stopTemp)
        {
            if(useTemp)
            {
                StopTemp();
            }

            stopTemp = false;
        }
    }

    void StopTemp()
    {
        progress = progress / (trackLength / tempTrackLength);
        useTemp = false;
    }
}
