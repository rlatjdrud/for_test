using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distance : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject sphere;
    void Start()
    {
        sphere = GameObject.Find("test1");

    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Errors()
    {
        if (sphere != null)
        {
            // Transform ���� ��������
            Transform sphereTransform = sphere.transform;

            // Transform ���� ���
            Debug.Log("Position: " + sphereTransform.position);
            Debug.Log("Rotation: " + sphereTransform.rotation);
            Debug.Log("Scale: " + sphereTransform.localScale);
        }
        else
        {
            Debug.LogError("Sphere1�̶�� �̸��� ��ü�� ã�� �� �����ϴ�.");
        }
    }

}

