using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarPanel : MonoBehaviour
{

    public bool powered;
    [SerializeField] WireController wireController;

    //public GameObject sphere;

    public Door door;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (door) { door.opened = powered; }
        if (wireController)
        {
            if(powered)
            {
                wireController.TurnOn();
            } else
            {
                wireController.TurnOff();
            }
        }
    }
}
