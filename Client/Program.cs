using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Client");
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //127.0.0.1 은 나 자신임(무조건) 외워야됨 == IPAddress.Loopback
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4000);

            serverSocket.Connect(serverEndPoint);

            byte[] buffer = new byte[1024];

            string message = "500 * 250";
            // string을 byte 배열로 변환해줌
            buffer = Encoding.UTF8.GetBytes(message);

            // 데이터 전송
            // OS buffer에 모아놓았다가 보낼만 할 때 보냄(send buffer)
            int SendLength = serverSocket.Send(buffer);
            // 만약 buffer를 한 번에 못 보낼 수 있음 > 아래 코드로 예외 상황에서 처리할 수 있음
            //int SendLength = serverSocket.Send(buffer, 0, buffer.Length, SocketFlags.None); // 6번째부터 5byte만 보냄 , SocketFlags는 None만 쓰면 됨 다른 건 쓸 일 없음

            //while(SendLength < buffer.Length)
            //{
            //    // buffer 다 보낼때까지 반복
            //    // ...
            //}

            // 블로킹 
            byte[] buffer2 = new byte[1024];
            int ReceiveLength = serverSocket.Receive(buffer2);

            Console.WriteLine(Encoding.UTF8.GetString(buffer2));

            serverSocket.Close();
        }
    }
}
