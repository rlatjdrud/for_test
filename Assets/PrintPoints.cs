using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class PrintPoints : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
            
           
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("x y z : " + transform.position.ToString("F4"));
    }
}
