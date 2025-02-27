using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if !UNITY_EDITOR
    using Windows.Networking;
    using Windows.Networking.Sockets;
    using Windows.Storage.Streams;
#endif

//Able to act as a reciever 
public class HololensServer : MonoBehaviour

{
    public String _input = "Waiting";
    //private Transformation trans;

    public Vector3 Rotation_first_row = new Vector3(1f, 0, 0);
    public Vector3 Rotation_second_row = new Vector3(0, 1f, 0);
    public Vector3 Rotation_third_row = new Vector3(0, 0, 1f);
    private MeshFilter mf;
    private Vector3[] origVerts;
    private Vector3[] newVerts;

    public Vector3 tvec = new Vector3(0f, 0f, 0f);

    Transform childTransform;
    Transform meshTransform;
    Transform childchildTransform;

    public Matrix4x4 matrix;
    

#if !UNITY_EDITOR
        StreamSocket socket;
        StreamSocketListener listener;
        String port;
        String message;
#endif

    // Use this for initialization
    void Start()
    {
        //trans = FindObjectOfType<Transformation>();
#if !UNITY_EDITOR
        listener = new StreamSocketListener();
        port = "9090";
        listener.ConnectionReceived += Listener_ConnectionReceived;
        listener.Control.KeepAlive = false;

        Listener_Start();
        
        
#endif
    }

#if !UNITY_EDITOR

    public void ApplyTransformation(Transform gameobject_transform ,Vector3 Rotation_1_row, Vector3 Rotation_2_row, Vector3 Rotation_3_row, Vector3 Tvec)
    {
        Vector3 gameobject_xyz = gameobject_transform.position;

        gameobject_xyz.x *= -1;
        float x = Vector3.Dot(gameobject_xyz, Rotation_1_row) + Tvec.x/1000;
        float y = Vector3.Dot(gameobject_xyz, Rotation_2_row) + Tvec.y/1000;
        float z = Vector3.Dot(gameobject_xyz, Rotation_3_row) + Tvec.z/1000;

        gameobject_transform.position = new Vector3(x,-y, z);
    }

    public Vector3 Skull_ApplyTransformation(Vector3 gameobject_transform, Vector3 Rotation_1_row, Vector3 Rotation_2_row, Vector3 Rotation_3_row, Vector3 Tvec)
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

    private async void Listener_Start()
    {
        Debug.Log("Listener started");
        try
        {
            await listener.BindServiceNameAsync(port);
        }
        catch (Exception e)
        {
            Debug.Log("Error: " + e.Message);
        }

        Debug.Log("Listening");
    }

    private async void Listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
    {
        Debug.Log("Connection received");

        try
        {

            using (var dr = new DataReader(args.Socket.InputStream))
            {
                dr.InputStreamOptions = InputStreamOptions.Partial;
                await dr.LoadAsync(256);  // 충분히 큰 버퍼
                string input = dr.ReadString(dr.UnconsumedBufferLength);
                Debug.Log("received: " + input);
                _input = input;

                // 수신한 데이터를 행렬로 변환
                string[] values = input.Split(',');
                for (int i = 0; i < 16; i++)
                {
                    matrix[i / 4, i % 4] = float.Parse(values[i]);
                }

                Debug.Log("Matrix:");
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        Debug.Log(matrix[i, j] + " ");
                    }
                }
 
            }
            Transformations(matrix);


            using (var dw = new DataWriter(args.Socket.OutputStream))
            {
                dw.WriteString("Completed Receive!");
                await dw.StoreAsync();
                dw.DetachStream();
            } 
          
        }
        catch (Exception e)
        {
            Debug.Log("disconnected!!!!!!!! " + e);
        }

    }

#endif

    void Update()
    {
        //tMP_Text = GetComponent<TMP_Text>();
        //tMP_Text.text=_input;
    }
}
