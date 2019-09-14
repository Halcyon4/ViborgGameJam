using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] private GameObject door;
    [SerializeField] private Vector3 from;
    [SerializeField] private Vector3 to;
    [SerializeField] private float speed;

    private float progress;
    [HideInInspector] public bool opened;

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

            door.transform.position = Vector3.Lerp(new Vector3(0,from.y,0), new Vector3(0, to.y, 0), progress);
        }
    }
}
