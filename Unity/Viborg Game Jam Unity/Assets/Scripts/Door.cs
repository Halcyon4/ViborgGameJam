using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public GameObject door;

    public Transform from;
    public Transform to;

    public float speed;
    public float progress;

    public bool opened;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(opened && progress < 1f)
        {
            progress += speed * Time.deltaTime;

            door.transform.position = Vector3.Lerp(from.position, to.position, progress);
        }
    }
}
