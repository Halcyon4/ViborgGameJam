using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameMenu : MonoBehaviour
{

    public GameObject ingameMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ingameMenu.SetActive(true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            Time.timeScale = 0;
        }
    }

    public void No()
    {
        ingameMenu.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1;
    }

    public void Yes()
    {
        Application.LoadLevel(0);
    }
}
