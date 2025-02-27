using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SceneManager : MonoBehaviour
{
    public String _input = "Waiting";
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

    private Thread serverThread;
    private TcpListener listener;
    private bool isRunning = false;

    // Use this for initialization
    void Start()
    {
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

    void ServerLoop()
    {
        try
        {
            listener = new TcpListener(IPAddress.Any, 9090); //어떠한 IP의 9090포트와 연결하는 소켓을 제작한다.
            listener.Start();//소켓을 인터넷망에 바인딩한다.
            Debug.Log("Server started on port 9090");

            while (isRunning)
            {
                TcpClient client = listener.AcceptTcpClient();//클라이언트 소켓과 인터넷망을 통해 연결된다.
                NetworkStream stream = client.GetStream();//정보를 stream변수를 통해 주고받는다.

                byte[] buffer = new byte[256];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);//버퍼에 클라이언트로 부터 받은 정보를 저장한다.
                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Debug.Log("Received: " + receivedMessage);
                _input = receivedMessage;

                // 수신한 데이터를 행렬로 변환
                string[] values = receivedMessage.Split(',');
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

                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    Transformations(matrix);
                });

                string responseMessage = "Completed Receive!";
                byte[] responseBytes = Encoding.UTF8.GetBytes(responseMessage);
                stream.Write(responseBytes, 0, responseBytes.Length);

                client.Close();
            }
        }
        catch (Exception e)
        {
            Debug.Log("Server exception: " + e.Message);
        }
    }

    public void ApplyTransformation(Transform gameobject_transform, Vector3 Rotation_1_row, Vector3 Rotation_2_row, Vector3 Rotation_3_row, Vector3 Tvec)
    {
        Vector3 gameobject_xyz = gameobject_transform.position;

        gameobject_xyz.x *= -1;
        float x = Vector3.Dot(gameobject_xyz, Rotation_1_row) + Tvec.x / 1000;
        float y = Vector3.Dot(gameobject_xyz, Rotation_2_row) + Tvec.y / 1000;
        float z = Vector3.Dot(gameobject_xyz, Rotation_3_row) + Tvec.z / 1000;

        gameobject_transform.position = new Vector3(x, -y, z);
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
                Vector3 tmp = new Vector3();
                meshTransform.position = new Vector3(tvec.x / 1000, -tvec.y / 1000, tvec.z / 1000);

                mf = meshTransform.GetComponent<MeshFilter>();

                origVerts = mf.mesh.vertices;
                newVerts = new Vector3[origVerts.Length];

                int j = 0;

                while (j < origVerts.Length)
                {
                    tmp = Skull_ApplyTransformation(origVerts[j], Rotation_first_row, Rotation_second_row, Rotation_third_row, tvec);
                    newVerts[j] = tmp;
                    j++;
                }

                mf.mesh.vertices = newVerts;
            }
        }
    }

    void Update()
    {
        
    }
}