using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOff : MonoBehaviour
{
    // Start is called before the first frame update
    public bool CanMove = false; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void On()
    {
        CanMove = true;
    }

    public void off()
    {
        CanMove = false;
    }
}
