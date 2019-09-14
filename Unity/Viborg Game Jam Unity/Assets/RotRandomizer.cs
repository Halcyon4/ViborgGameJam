using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotRandomizer : MonoBehaviour
{
    void Start()
    {
        transform.eulerAngles += new Vector3(0,Random.Range(0, 16)*22.5f,0);

    }
}
