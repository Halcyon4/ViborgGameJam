using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarPanel : MonoBehaviour
{

    public bool powered;

    //public GameObject sphere;

    public Door door;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (door) { door.opened = powered; }
    }
}
