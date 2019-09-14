using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] private float to;
    [SerializeField] private float speed;

    private float progress;
    [HideInInspector] public bool opened;

    // Update is called once per frame
    void Update()
    {
        if(opened && progress < 1f)
        {
            progress += speed * Time.deltaTime;

            transform.position = Vector3.Lerp(new Vector3(transform.position.x,transform.position.y, transform.position.z), new Vector3(transform.position.x, to, transform.position.z), progress);
        }
    }
}
