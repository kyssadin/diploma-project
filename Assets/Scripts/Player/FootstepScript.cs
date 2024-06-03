using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsScript : MonoBehaviour
{
    public GameObject footstep;

    private List<KeyCode> keys = new List<KeyCode>() { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D };

    // Start is called before the first frame update
    void Start()
    {
        footstep.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            footstep.SetActive(true);
        }
        foreach (var key in keys)
        {
            if (Input.GetKeyDown(key))
            {
                footstep.SetActive(true);
            }
            if (Input.GetKeyUp(key))
            {
                footstep.SetActive(false);
            }

        }
        
    }
}
