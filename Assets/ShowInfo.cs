using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowInfo : MonoBehaviour
{
    public GameObject textPrefab; // 3D TextMeshPro �������� �����մϴ�.
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

        // �浹 ������ ����մϴ�.
        if (other.transform.name == "Tip")
        {

            collisionPoint = other.ClosestPoint(transform.position);
            //Debug.Log("CollisionPoint : " + collisionPoint);
            isColliding = true;
            collisionPoint.x += 0.05f;
            collisionPoint.y += 0.05f;
            // �ؽ�Ʈ ��ġ�� ������ ������Ʈ�մϴ�.
            textPlate = Instantiate(textPrefab, collisionPoint, Head.transform.rotation);
            textPlate.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
            for (int i = 0; i < textPlate.transform.childCount; i++)
            {
                childchildTransform = textPlate.transform.GetChild(i);

                childchildTransform.name = "Childs";
                TextMeshPro tmpComponent = childchildTransform.GetComponent<TextMeshPro>();

                // TextMeshPro�� �ִ� ���� ������Ʈ�� Ȱ��ȭ �Ǵ� ��Ȱ��ȭ�ϱ�
                if (tmpComponent != null)
                {
                    tmpComponent.text = transform.name;
                }

               

            }
        }

        //tmpComponent.text = transform.name;
        //textPrefab.SetActive(true); // �ؽ�Ʈ ǥ��


    }

    void OnTriggerExit(Collider other)
    {
        
            isColliding = false;
            Destroy(textPlate);
            //textPrefab.SetActive(false); // �浹�� ������ �ؽ�Ʈ ����
        
    }


}
