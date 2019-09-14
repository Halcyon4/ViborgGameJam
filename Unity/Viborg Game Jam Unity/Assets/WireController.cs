using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;

[RequireComponent(typeof(WireMeshCreator))]
public class WireController : MonoBehaviour
{

    [SerializeField] public Material mat_off;
    [SerializeField] public Material mat_on;

    private WireMeshCreator wireMeshCreator;
    
    private void Start()
    {
        TurnOff();
    }

    public void TurnOn ()
    {
        AssignTextures(mat_on);
    }

    public void TurnOff()
    {
        AssignTextures(mat_off);
    }

    private void AssignTextures(Material mat)
    {
        GetComponent<MeshRenderer>().material = mat;
        GetComponent<MeshRenderer>().material = mat;
    }

}
