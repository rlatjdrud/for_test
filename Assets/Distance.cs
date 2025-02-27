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
            // Transform 정보 가져오기
            Transform sphereTransform = sphere.transform;

            // Transform 정보 출력
            Debug.Log("Position: " + sphereTransform.position);
            Debug.Log("Rotation: " + sphereTransform.rotation);
            Debug.Log("Scale: " + sphereTransform.localScale);
        }
        else
        {
            Debug.LogError("Sphere1이라는 이름의 객체를 찾을 수 없습니다.");
        }
    }

}

