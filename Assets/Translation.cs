using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translation : MonoBehaviour
{
    public bool CanMove = false;
    public GameObject f_points;
    private OnOff onffscript;
    Vector3 points_position = Vector3.zero;
    Transform childTransform;
    private float unit = 0.001f;
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move_X_UP()
    {
        if (f_points != null)
        {

            for (int i = 0; i < transform.childCount; i++)
            {
                childTransform = transform.GetChild(i);
                onffscript = childTransform.gameObject.GetComponent<OnOff>();
                if (onffscript.CanMove)
                {
                    points_position = childTransform.transform.position;
                    points_position.x += unit;
                    childTransform.transform.position = points_position;

                }
            }
        }
    }

    public void Move_X_DOWN()
    {
        if (f_points != null)
        {

            for (int i = 0; i < transform.childCount; i++)
            {
                childTransform = transform.GetChild(i);
                onffscript = childTransform.gameObject.GetComponent<OnOff>();
                if (onffscript.CanMove)
                {
                    points_position = childTransform.transform.position;
                    points_position.x -= unit;
                    childTransform.transform.position = points_position;

                }
            }
        }
    }

    public void Move_Y_UP()
    {
        if (f_points != null)
        {

            for (int i = 0; i < transform.childCount; i++)
            {
                childTransform = transform.GetChild(i);
                onffscript = childTransform.gameObject.GetComponent<OnOff>();
                if (onffscript.CanMove)
                {
                    points_position = childTransform.transform.position;
                    points_position.y += unit;
                    childTransform.transform.position = points_position;

                }
            }
        }
    }

    public void Move_Y_DOWN()
    {
        if (f_points != null)
        {

            for (int i = 0; i < transform.childCount; i++)
            {
                childTransform = transform.GetChild(i);
                onffscript = childTransform.gameObject.GetComponent<OnOff>();
                if (onffscript.CanMove)
                {
                    points_position = childTransform.transform.position;
                    points_position.y -= unit;
                    childTransform.transform.position = points_position;

                }
            }
        }
    }

    public void Move_Z_UP()
    {
        if (f_points != null)
        {

            for (int i = 0; i < transform.childCount; i++)
            {
                childTransform = transform.GetChild(i);
                onffscript = childTransform.gameObject.GetComponent<OnOff>();
                if (onffscript.CanMove)
                {
                    points_position = childTransform.transform.position;
                    points_position.z += unit;
                    childTransform.transform.position = points_position;

                }
            }
        }
    }

    public void Move_Z_DOWN()
    {
        if (f_points != null)
        {

            for (int i = 0; i < transform.childCount; i++)
            {
                childTransform = transform.GetChild(i);
                onffscript = childTransform.gameObject.GetComponent<OnOff>();
                if (onffscript.CanMove)
                {
                    points_position = childTransform.transform.position;
                    points_position.z -= unit;
                    childTransform.transform.position = points_position;

                }
            }
        }
    }

    public void MM1()
    {
        unit = 0.001f;
    }

    public void MM0_1()
    {
        unit = 0.0001f;
    }

    public void MM0_01()
    {
        unit = 0.00001f;
    }

}
