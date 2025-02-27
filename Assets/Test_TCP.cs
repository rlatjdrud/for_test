using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class Test_TCP : MonoBehaviour
{   
    
    private TcpListener listener;
    private int port = 9091;
    private bool connected = false;
    private Matrix4x4 mat = new Matrix4x4();
    private Thread serverThread;
    // Start is called before the first frame update

    void Start()
    {
        connected = true;
        serverThread = new Thread(StartServer);
        serverThread.Start();
    }

    void Transformations(Matrix4x4 mat)
    {
       for (int i =0; i< transform.childCount; i++) 
       {
            Transform child = transform.GetChild(i);
            child.position = mat.GetRow(i);
       }
    }
    void StopServer()
    {
        connected = false;
        listener.Stop();
        serverThread.Abort();
    }

    private void OnDestroy()
    {
        StopServer();
    }

    void StartServer()
    {
        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        Debug.Log("Waiting for Client....");

        while (connected)
        {
            TcpClient client = listener.AcceptTcpClient();//클라이언트와 직통으로 통신하는 소켓 생성됨
            NetworkStream stream = client.GetStream();

            byte[] buf = new byte[1024];
            int byteRead=stream.Read(buf, 0, buf.Length); // 읽어들인 데이터의 크기를 리턴한다.
            string receivedMessage = Encoding.UTF8.GetString(buf, 0, byteRead); //
            // 암호화된 string 데이터를 해독하고, string으로 데이터를 읽는다.
            Debug.Log("Received: " + receivedMessage);

            string[] values = receivedMessage.Split(','); 
            //데이터가 0.9984,0.0854,----- 와 같이 일렬로 들어오므로 ,를 기준으로 나누고 각 나눠진 요소들을 string 배열에 저장
            for (int i = 0; i < 12; i++)
            {
                mat[i/3,i%3] = float.Parse(values[i]);

            }

            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                Transformations(mat);
            });

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
