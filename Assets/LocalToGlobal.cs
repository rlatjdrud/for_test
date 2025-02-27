using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalToGlobal : MonoBehaviour
{
    private Transform child;
    private Vector3 global_Points;
    private Vector3 local_Points;
    public GameObject gameObject;
    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            child = transform.GetChild(i);
            local_Points = child.transform.position;
            //global_Points = child.TransformPoint(local_Points);
            local_Points *= 1000f;
            Debug.Log(transform.parent.name + " | "+ child.transform.name+ " : " + local_Points.ToString("F6"));


        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
