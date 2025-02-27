
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Transformation : MonoBehaviour
{

    public Vector3 Rotation_first_row = new Vector3(1f,0,0);
    public Vector3 Rotation_second_row = new Vector3(0, 1f, 0);
    public Vector3 Rotation_third_row = new Vector3(0, 0, 1f);
    private MeshFilter mf;
    private Vector3[] origVerts;
    private Vector3[] newVerts;

    public Vector3 tvec = new Vector3(0f,0f,0f);

    Transform childTransform;
    Transform meshTransform;
    Transform childchildTransform;

    Quaternion Matrix4x4ToQuaternion(Matrix4x4 m)
    {
        Quaternion q = new Quaternion();
        //float Tm = m.m00 + m.m11 + m.m22;
        //float q0 = Mathf.Sqrt(Mathf.Max(0, (Tm + 1) / 4));
        //float q1 = Mathf.Sqrt(Mathf.Max(0, m.m00 / 2 + (1 - Tm) / 4));
        //float q2 = Mathf.Sqrt(Mathf.Max(0, m.m11 / 2 + (1 - Tm) / 4));
        //float q3 = Mathf.Sqrt(Mathf.Max(0, m.m22 / 2 + (1 - Tm) / 4));
        //q.w = q0;
        //q.x = q1;
        //q.y = q2;
        //q.z = q3;
        q.w = Mathf.Sqrt(Mathf.Max(0, 1 + m.m00 + m.m11 + m.m22)) / 2;
        q.x = Mathf.Sqrt(Mathf.Max(0, 1 + m.m00 - m.m11 - m.m22)) / 2;
        q.y = Mathf.Sqrt(Mathf.Max(0, 1 - m.m00 + m.m11 - m.m22)) / 2;
        q.z = Mathf.Sqrt(Mathf.Max(0, 1 - m.m00 - m.m11 + m.m22)) / 2;
        q.x *= Mathf.Sign(q.x * (m.m21 - m.m12));
        q.y *= Mathf.Sign(q.y * (m.m02 - m.m20));
        q.z *= Mathf.Sign(q.z * (m.m10 - m.m01));
        return q;
    }
    void Rotation_Skull(Transform gameobject_transform, Vector3 Rotation_1_row, Vector3 Rotation_2_row, Vector3 Rotation_3_row, Vector3 Tvec)
    {
        Matrix4x4 matrix4x4 = Matrix4x4.identity;
        matrix4x4.m00 = -Rotation_1_row.x;
        matrix4x4.m01 = Rotation_1_row.y;
        matrix4x4.m02 = Rotation_1_row.z;
        matrix4x4.m10 = -Rotation_2_row.x;
        matrix4x4.m11 = Rotation_2_row.y;
        matrix4x4.m12 = Rotation_2_row.z;
        matrix4x4.m20 = -Rotation_3_row.x;
        matrix4x4.m21 = Rotation_3_row.y;
        matrix4x4.m22 = Rotation_3_row.z;

        //Matrix4x4 matrix4x4 = Matrix4x4.identity;
        //matrix4x4.m00 = 0.2500000f;
        //matrix4x4.m01 = -0.4330127f;
        //matrix4x4.m02 = 0.8660254f;
        //matrix4x4.m10 = 0.8080127f;
        //matrix4x4.m11 = -0.3995191f;
        //matrix4x4.m12 = -0.4330127f;
        //matrix4x4.m20 = 0.5334936f;
        //matrix4x4.m21 = 0.8080127f;
        //matrix4x4.m22 = 0.2500000f;

        Vector4 basis1 = new Vector4(1f, 0, 0, 0);
        Vector4 basis2 = new Vector4(0, 0, 1f, 0);
        Vector4 basis3 = new Vector4(0, 1f, 0, 0);
        Vector4 basis4 = new Vector4(0, 0, 0, 0);
        Matrix4x4 change_basis = new Matrix4x4();

        change_basis.SetColumn(0, basis1);
        change_basis.SetColumn(1, basis2);
        change_basis.SetColumn(2, basis3);
        change_basis.SetColumn(3, basis4);
        change_basis = change_basis.inverse;

        Matrix4x4 final_mat =  matrix4x4.inverse;
        Quaternion quaternion = Matrix4x4ToQuaternion(matrix4x4.inverse);
        Debug.Log("quater:" + quaternion);
        Debug.Log("b :" + quaternion.eulerAngles);


        Quaternion a = Quaternion.Euler(new Vector3(0,0,180));
        //Debug.Log("a :" + a);
        //float current_x = -(90-quaternion.eulerAngles.x);
        //Quaternion b = Quaternion.Euler(new Vector3(current_x, 0 , 0));
        //Quaternion c = Quaternion.Euler(new Vector3(0, 180, 0));
        //Quaternion d = Quaternion.Euler(new Vector3(-current_x, 0, 0));
        
        gameobject_transform.rotation = a*quaternion;
        
        //Vector3 tmp = gameobject_transform.position+ Tvec;
        //tmp /= 1000;
        ////tmp.x *= -1;
        //tmp.y *= -1;

        //gameobject_transform.position = tmp;//+ new Vector3(0.125f,0.1195f,0.1062f);

        //Debug.Log("quaternion :" + quaternion);

    }
    // Start is called before the first frame update
    void Start()
    {
     
        
        Rotation_first_row = new Vector3(1,0,0);
        Rotation_second_row = new Vector3(0, 1, 0);
        Rotation_third_row = new Vector3(0, 0, 1);
        tvec = new Vector3(0, 0, 0);


    }
    public void Transformations(Matrix4x4 mat)
    {
        
        Rotation_first_row = mat.GetRow(0);

        Rotation_second_row = mat.GetRow(1);
        Rotation_third_row = mat.GetRow(2);
        tvec = mat.GetColumn(3);
        Debug.Log("Vector1: " + Rotation_first_row.ToString());
        Debug.Log("Vector2: " + Rotation_second_row.ToString());
        Debug.Log("Vector3: " + Rotation_third_row.ToString());
        Debug.Log("tvec: " + tvec.ToString());

        for (int i = 0; i < transform.childCount; i++)
        {
            //Debug.Log(i);

            childTransform = transform.GetChild(i);
            if (childTransform.name != "reduced_skull_lp")
            {
                for (int k = 0; k < childTransform.childCount; k++)
                {
                    
                    childchildTransform = childTransform.GetChild(k);
                    Debug.Log(childchildTransform.name);
                    ApplyTransformation(childchildTransform, Rotation_first_row, Rotation_second_row, Rotation_third_row, tvec);
                }
            }

            else
            {
                meshTransform = childTransform.GetChild(0);
                //Debug.Log(meshTransform.name);
                Vector3 tmp = new Vector3();
                meshTransform.position = new Vector3(tvec.x / 1000, -tvec.y / 1000, tvec.z / 1000);

                //Matrix4x4 Tmat_inverve = Tmat.inverse;

                mf = meshTransform.GetComponent<MeshFilter>();

                origVerts = mf.mesh.vertices;
                newVerts = new Vector3[origVerts.Length];

                int j = 0;

                while (j < origVerts.Length)
                {
                    //if (i == 0)
                    //{ Debug.Log("xyz : "+origVerts[i]); }
                    tmp = Skull_ApplyTransformation(origVerts[j], Rotation_first_row, Rotation_second_row, Rotation_third_row, tvec);
                    newVerts[j] = tmp;
                    //if (i == 0)
                    //{ Debug.Log("xyz : " + newVerts[i]); }
                    j++;
                }

                mf.mesh.vertices = newVerts;
            }
            //else
            //{
            //    childTransform = transform.GetChild(i);
            //    Vector3 tmp = childTransform.position;
            //    //tmp.x *= -1;
            //    //childTransform.position = tmp;

            //    Rotation_Skull(childTransform, Rotation_first_row, Rotation_second_row, Rotation_third_row, tvec);
            //}
        }
    }

    void ApplyTransformation(Transform gameobject_transform ,Vector3 Rotation_1_row, Vector3 Rotation_2_row, Vector3 Rotation_3_row, Vector3 Tvec)
    {
        Vector3 gameobject_xyz = gameobject_transform.position;

        gameobject_xyz.x *= -1;
        float x = Vector3.Dot(gameobject_xyz, Rotation_1_row) + Tvec.x/1000;
        float y = Vector3.Dot(gameobject_xyz, Rotation_2_row) + Tvec.y/1000;
        float z = Vector3.Dot(gameobject_xyz, Rotation_3_row) + Tvec.z/1000;

        gameobject_transform.position = new Vector3(x,-y, z);
    }

    Vector3 Skull_ApplyTransformation(Vector3 gameobject_transform, Vector3 Rotation_1_row, Vector3 Rotation_2_row, Vector3 Rotation_3_row, Vector3 Tvec)
    {
        Rotation_1_row.x *= -1;
        Rotation_2_row.x *= -1;
        Rotation_3_row.x *= -1;

        float x = Vector3.Dot(gameobject_transform, Rotation_1_row);
        float y = Vector3.Dot(gameobject_transform, Rotation_2_row);
        float z = Vector3.Dot(gameobject_transform, Rotation_3_row);
        Vector3 newxyz = new Vector3(x, -y, z);

        return newxyz;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
