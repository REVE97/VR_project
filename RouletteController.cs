using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteControl : MonoBehaviour
{
    float rotSpeed = 0;
    
    
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.rotSpeed = 10000;
        }

        transform.Rotate(0, 0, this.rotSpeed);

        this.rotSpeed *= Random.Range(0.5f, 0.9f);
    }
}
