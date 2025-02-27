using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowInfo : MonoBehaviour
{
    public GameObject textPrefab; // 3D TextMeshPro 프리팹을 연결합니다.
    public GameObject Head;

    private GameObject textPlate;
    private Vector3 collisionPoint;
    private bool isColliding = false;
    private Transform childchildTransform;
    
  

    void Start()
    {
        
       
    }

    void OnTriggerEnter(Collider other)
    {

        // 충돌 지점을 계산합니다.
        if (other.transform.name == "Tip")
        {

            collisionPoint = other.ClosestPoint(transform.position);
            //Debug.Log("CollisionPoint : " + collisionPoint);
            isColliding = true;
            collisionPoint.x += 0.05f;
            collisionPoint.y += 0.05f;
            // 텍스트 위치와 내용을 업데이트합니다.
            textPlate = Instantiate(textPrefab, collisionPoint, Head.transform.rotation);
            textPlate.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
            for (int i = 0; i < textPlate.transform.childCount; i++)
            {
                childchildTransform = textPlate.transform.GetChild(i);

                childchildTransform.name = "Childs";
                TextMeshPro tmpComponent = childchildTransform.GetComponent<TextMeshPro>();

                // TextMeshPro가 있는 게임 오브젝트를 활성화 또는 비활성화하기
                if (tmpComponent != null)
                {
                    tmpComponent.text = transform.name;
                }

               

            }
        }

        //tmpComponent.text = transform.name;
        //textPrefab.SetActive(true); // 텍스트 표시


    }

    void OnTriggerExit(Collider other)
    {
        
            isColliding = false;
            Destroy(textPlate);
            //textPrefab.SetActive(false); // 충돌이 끝나면 텍스트 숨김
        
    }


}
