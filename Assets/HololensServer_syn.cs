using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class HololensServer_syn : MonoBehaviour
{
    //public String _input = "Waiting";
    public Vector3 Rotation_first_row = new Vector3(1f, 0, 0);
    public Vector3 Rotation_second_row = new Vector3(0, 1f, 0);
    public Vector3 Rotation_third_row = new Vector3(0, 0, 1f);
    private MeshFilter Bonemf;
    private MeshFilter Brainmf;
    private MeshFilter Tumormf;
   
    private Vector3[] BoneVerts;
    private Vector3[] BonenewVerts;

    private Vector3[] BrainVerts;
    private Vector3[] BrainnewVerts;

    private Vector3[] TumorVerts;
    private Vector3[] TumornewVerts;
    private Vector3 tmpxyz = new Vector3();
    public Vector3 tvec = new Vector3(0f, 0f, 0f);
    public GameObject showaruco;
    public GameObject Skull;
    public GameObject Tumor;
    public GameObject Brain;
    public GameObject Pts;
    public GameObject TipPts;
    public GameObject buttons;
    public GameObject Menu;
    public GameObject OnOffs_button;
    public GameObject Adjust_button;
    public GameObject Unit_button;
    public GameObject inform_button;
    public Transform BoneTransform;
    public Transform BrainTransform;
    public Transform TumorTransform;
    public GameObject Aruco_Markers;
   


    private Vector3[] Points =
    {
        new Vector3(0.094333f,0.198672f,0.128256f),
        new Vector3(0.109691f,0.193699f,0.097772f),
        new Vector3(0.093143f,0.16514f,0.029938f),
        new Vector3(0.10421f,0.176205f,0.020763f),
        new Vector3(0.145407f,0.176253f,0.021684f),
        new Vector3(0.139026f,0.192756f,0.098541f),

      };


    Transform childTransform;
    Transform childchildTransform;
    private Matrix4x4 virtual_matrix;
    private Matrix4x4 marker_matrix;
    private Matrix4x4 prev_matrix=Matrix4x4.identity;

    private Thread serverThread;
    private TcpListener listener;
    private bool isRunning = false;
    private bool initial = true;

    // Use this for initialization
    void Start()
    {
        Bonemf = BoneTransform.GetComponent<MeshFilter>();
        BoneVerts = Bonemf.mesh.vertices;
        BonenewVerts = new Vector3[BoneVerts.Length];

        Brainmf = BrainTransform.GetComponent<MeshFilter>();
        BrainVerts = Brainmf.mesh.vertices;
        BrainnewVerts = new Vector3[BrainVerts.Length];

        Tumormf = TumorTransform.GetComponent<MeshFilter>();
        TumorVerts = Tumormf.mesh.vertices;
        TumornewVerts = new Vector3[TumorVerts.Length];


        StartServer();
    }

    void OnDestroy()
    {
        StopServer();
    }

    void StartServer()
    {
        isRunning = true;
        serverThread = new Thread(ServerLoop);
        serverThread.Start();
    }

    void StopServer()
    {
        isRunning = false;
        listener.Stop();
        serverThread.Abort();
    }

    //void ServerLoop()
    //{
    //    try
    //    {
    //        listener = new TcpListener(IPAddress.Any, 9090);
    //        listener.Start();
    //        Debug.Log("Server started on port 9090");

    //        while (isRunning)
    //        {
    //            if (listener.Pending())
    //            {
    //                TcpClient client = listener.AcceptTcpClient();
    //                Thread clientThread = new Thread(() => HandleClient(client));
    //                clientThread.Start();
    //            }
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.Log("Server exception: " + e.Message);
    //    }
    //}

    //void HandleClient(TcpClient client)
    //{
    //    try
    //    {
    //        NetworkStream stream = client.GetStream();

    //        byte[] sizebuffer = new byte[4];
    //        while (true)
    //        { 
    //        int sizeByteRead=stream.Read(sizebuffer, 0, sizebuffer.Length);
    //            Debug.Log("sizeData4 : " + sizeByteRead);
    //        int bytesRead;

    //        if (sizeByteRead != 4)
    //        {
    //            throw new Exception("Failed to read the data size");
    //        }

    //        int dataSize = BitConverter.ToInt32(sizebuffer, 0);
    //        Debug.Log("sizeData : " + dataSize);
    //        byte[] buffer = new byte[dataSize];
    //        int dataBytesRead = 0;


    //        bytesRead = stream.Read(buffer, dataBytesRead, dataSize - dataBytesRead);



    //            string receivedMessage = Encoding.UTF8.GetString(buffer);
    //            Debug.Log("Received: " + receivedMessage);
    //            _input = receivedMessage;

    //            // Convert received data to matrix
    //            string[] values = receivedMessage.Split(',');
    //            for (int i = 0; i < 16; i++)
    //            {
    //                matrix[i / 4, i % 4] = float.Parse(values[i]);
    //            }

    //            newMatrix = matrix * (prev_matrix.inverse);
    //            UnityMainThreadDispatcher.Instance().Enqueue(() =>
    //            {
    //                Transformations(newMatrix);
    //                showaruco.SetActive(false);
    //                Virtual.SetActive(true);
    //                buttons.SetActive(true);
    //            });
    //            prev_matrix = matrix;

    //            // Send acknowledgment
    //            //string responseMessage = "ACK";
    //            //byte[] responseBytes = Encoding.UTF8.GetBytes(responseMessage);
    //            //stream.Write(responseBytes, 0, responseBytes.Length);
    //        }


    //    }
    //    catch (Exception e)
    //    {
    //        Debug.Log("Client handling exception: " + e.Message);
    //    }
    //}

    void ServerLoop()
    {
        try
        {
            listener = new TcpListener(IPAddress.Any, 9090); //어떠한 IP의 9090포트와 연결하는 소켓을 제작한다.
            listener.Start();//소켓을 인터넷망에 바인딩한다.
            Debug.Log("Server started on port 9090");
            TcpClient client = listener.AcceptTcpClient();//클라이언트 소켓과 인터넷망을 통해 연결된다.
            NetworkStream stream = client.GetStream();//정보를 stream변수를 통해 주고받는다.
            
            while (isRunning)
            {

                //byte[] sizebuffer = new byte[4];
                
                //int sizeByteRead = stream.Read(sizebuffer, 0, sizebuffer.Length);
                //Debug.Log("sizeData4 : " + sizeByteRead);
                //int bytesRead;

                //if (sizeByteRead != 4)
                //{
                //    throw new Exception("Failed to read the data size");
                //}

                //int dataSize = BitConverter.ToInt32(sizebuffer, 0);
                //Debug.Log("sizeData : " + dataSize);
                byte[] buffer = new byte[1024];
                 
                int bytesRead = stream.Read(buffer, 0, buffer.Length);//버퍼에 클라이언트로 부터 받은 정보를 저장한다.
                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                //Debug.Log("Received: " + receivedMessage);
                

                // 수신한 데이터를 행렬로 변환
                string[] values = receivedMessage.Split(',');
                int length = values.Length;
                //Debug.Log("length Data : " + length);
                //Debug.Log("receved Data : " + values);

                if (length > 12) 
                {
                    for (int i = 0; i < 16; i++)
                    {
                        virtual_matrix[i / 4, i % 4] = float.Parse(values[i]);
                    }

                }

                else
                {
                    //for (int i = 0; i < 12; i++)
                    //{
                     
                    //    marker_matrix[i / 3, i % 3] = float.Parse(values[i]);
                    //}
                    for (int i =0; i < 3; i++)
                    {
                        tmpxyz[i] = float.Parse(values[i]);

                    }
               
                }
                

               

                //Debug.Log("Matrix:");
                //for (int i = 0; i < 4; i++)
                //{
                //    for (int j = 0; j < 4; j++)
                //    {
                //        Debug.Log(matrix[i, j] + " ");
                //    }
                //}

                //newMatrix = matrix * (prev_matrix.inverse);
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    if (initial)
                    {
                        Transformations(virtual_matrix);
                        showaruco.SetActive(false);
                        Menu.SetActive(true);
                        Skull.SetActive(true);
                        //Tumor.SetActive(true);
                        //Brain.SetActive(true);
                        Pts.SetActive(true);
                        buttons.SetActive(true);
                        //OnOffs_button.SetActive(true);
                        Adjust_button.SetActive(true);
                        //Unit_button.SetActive(true);
                        inform_button.SetActive(true);
                        initial =false;
                    }
                        TipPts.transform.position = tmpxyz;
                        //ArucoMarkerTransform(Aruco_Markers, marker_matrix);



                });
                
                //prev_matrix = matrix;


                string responseMessage = "ACK";
                byte[] responseBytes = Encoding.UTF8.GetBytes(responseMessage);
                stream.Write(responseBytes, 0, responseBytes.Length);


            }
            client.Close();
        }
        catch (Exception e)
        {
            Debug.Log("Server exception: " + e.Message);
        }
        
    }

    public void ApplyTransformation(Transform gameobject_transform,Vector3 Point,Vector3 Rotation_1_row, Vector3 Rotation_2_row, Vector3 Rotation_3_row, Vector3 Tvec)
    {
        Vector3 gameobject_xyz = Point;

        gameobject_xyz.x *= -1;
        float x = Vector3.Dot(gameobject_xyz, Rotation_1_row) + Tvec.x / 1000;
        float y = Vector3.Dot(gameobject_xyz, Rotation_2_row) + Tvec.y / 1000;
        float z = Vector3.Dot(gameobject_xyz, Rotation_3_row) + Tvec.z / 1000;
        
        Vector3 newPositions = new Vector3(x, -y, z);
        gameobject_transform.transform.position = newPositions;


    }

    

    public void ArucoMarkerTransform(GameObject Aruco_Markers, Matrix4x4 marker_matrix)
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject markers = Instantiate(Aruco_Markers, marker_matrix.GetRow(i), Aruco_Markers.transform.rotation);
            //childTransform.transform.position = marker_matrix.GetRow(i);
            Debug.Log("Position : " + marker_matrix.GetRow(i));
        }
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
        //Debug.Log("Vector1: " + Rotation_first_row.ToString());
        //Debug.Log("Vector2: " + Rotation_second_row.ToString());
        //Debug.Log("Vector3: " + Rotation_third_row.ToString());
        //Debug.Log("tvec: " + tvec.ToString());

        for (int i = 0; i < transform.childCount; i++)
        {
            childTransform = transform.GetChild(i);
            if (childTransform.name != "reduced_skull_lp" && childTransform.name != "reduce_brain_lps" && childTransform.name != "tumor_final_lps")
            {
                for (int k = 0; k < childTransform.childCount; k++)
                {
                    childchildTransform = childTransform.GetChild(k);
                    Debug.Log(childchildTransform.name);
                    ApplyTransformation(childchildTransform, Points[k] ,Rotation_first_row, Rotation_second_row, Rotation_third_row, tvec);
                }
            }
            else
            {
                //meshTransform = childTransform.GetChild(0);
                Vector3 tmp = new Vector3();
                Vector3 offset = new Vector3();
                Vector3 newoffset = new Vector3();

                if (childTransform.name == "reduced_skull_lp")
                {
                    BoneTransform.position = new Vector3((tvec.x / 1000), (-tvec.y / 1000), (tvec.z / 1000));
                   
                    int j = 0;
                    while (j < BoneVerts.Length)
                    {
                        tmp = Skull_ApplyTransformation(BoneVerts[j], Rotation_first_row, Rotation_second_row, Rotation_third_row, tvec);
                        BonenewVerts[j] = tmp;
                        j++;
                    }
                    Bonemf.mesh.vertices = BonenewVerts;
                }

                else if (childTransform.name == "reduce_brain_lps")
                {
                    
                    BrainTransform.position = new Vector3((tvec.x / 1000) , (-tvec.y / 1000), (tvec.z / 1000) );
                    offset.x = -(0.1225f-0.0025f);
                    offset.y = (0.1475f - 0.0146f);
                    offset.z = (0.1406f + 0.006f);

                    int j = 0;

                    while (j < BrainVerts.Length)
                    {

                        tmp = Skull_ApplyTransformation(BrainVerts[j], Rotation_first_row, Rotation_second_row, Rotation_third_row, tvec);
                        BrainnewVerts[j] = tmp;
                        j++;
                    }
                    Brainmf.mesh.vertices = BrainnewVerts;
                    newoffset.x = Vector3.Dot(offset, Rotation_first_row);
                    newoffset.y = Vector3.Dot(offset, Rotation_second_row);
                    newoffset.z = Vector3.Dot(offset, Rotation_third_row);

                    newoffset.y *= -1;
                    
                    BrainTransform.position += newoffset;
                    
                }

                else
                {
                    TumorTransform.position = new Vector3((tvec.x / 1000), (-tvec.y / 1000) , (tvec.z / 1000) );
                    offset.x = -0.1019f;
                    offset.y = 0.1304f;
                    offset.z = (0.1567f - 0.0194f);
                    int j = 0;
                    while (j < TumorVerts.Length)
                    {
                        tmp = Skull_ApplyTransformation(TumorVerts[j], Rotation_first_row, Rotation_second_row, Rotation_third_row, tvec);
                        TumornewVerts[j] = tmp;
                        j++;
                    }
                    Tumormf.mesh.vertices = TumornewVerts;
                    newoffset.x = Vector3.Dot(offset, Rotation_first_row);
                    newoffset.y = Vector3.Dot(offset, Rotation_second_row);
                    newoffset.z = Vector3.Dot(offset, Rotation_third_row);

                    newoffset.y *= -1;
                    
                    TumorTransform.position += newoffset;
                    


                }

                //mf = meshTransform.GetComponent<MeshFilter>();

                //origVerts = mf.mesh.vertices;
                //newVerts = new Vector3[origVerts.Length];




            }
        }
    }

    void Update()
    {
        
    }
}