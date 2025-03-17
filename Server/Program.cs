using System.Net.Sockets;
using System.Net;
using System;
using System.Text;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server");
            // 상태 설정
            #region
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // IPAddress.Any > 4000번 포트로 들어오는 통신은 무엇이든 다 받을 거임!
            // 실제로는 절대 이렇게 코딩 안함
            IPEndPoint listenEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4000); // 4000포트는 듣기용이라 다른 작업은 소켓을 추가로 만들어서 사용해야함
            listenSocket.Bind(listenEndPoint);

            listenSocket.Listen(10); // 몇 개 들을 건지 정하는 것(정해진 게 없는데 너무 많이 하면 안됨)
            #endregion

            // 통신 주고 받기
            #region
            bool isRunning = true;

            while (isRunning)
            {
                // 동기(synchronous), 블록킹
                // 클라이언트
                Socket clientSocket = listenSocket.Accept();

                // 주문 받기
                byte[] buffer = new byte[1024];
                int RecvLength = clientSocket.Receive(buffer); // 몇 바이트 받았는지 길이 리턴
                if (RecvLength <= 0)
                {
                    // close
                    // error
                    // 연결 끊긴 거
                    isRunning = false;
                }

                string receiveMessage = Encoding.UTF8.GetString(buffer);
                char splitChar = '+';
                if (receiveMessage.Contains("+"))
                {
                    splitChar = '+';
                }
                else if (receiveMessage.Contains("-"))
                {
                    splitChar = '-';
                }
                else if (receiveMessage.Contains("*"))
                {
                    splitChar = '*';
                }
                else if (receiveMessage.Contains("/"))
                {
                    splitChar = '/';
                }
                else if (receiveMessage.Contains("%"))
                {
                    splitChar = '%';
                }

                string[] numbers = new string[2];
                numbers = receiveMessage.Split(splitChar);
                int result = 0;
                switch (splitChar)
                {
                    case '+':
                        
                        result = int.Parse(numbers[0]) + int.Parse(numbers[1]);
                        break;
                    case '-':
                        result = int.Parse(numbers[0]) - int.Parse(numbers[1]);
                        break;
                    case '/':
                        result = int.Parse(numbers[0]) / int.Parse(numbers[1]);
                        break;
                    case '*':
                        result = int.Parse(numbers[0]) * int.Parse(numbers[1]);
                        break;
                    default:
                        break;
                }
                
                

                buffer = Encoding.UTF8.GetBytes($"result: {result}");
                int SendLength = clientSocket.Send(buffer); // 몇 바이트 보냈는지 리턴
                if (SendLength <= 0)
                {
                    // 보냈는데 끊긴 거임
                    // error
                    isRunning = false;
                }
                Console.WriteLine(receiveMessage);
                Console.WriteLine(Encoding.UTF8.GetString(buffer));
                // keep alive time
                clientSocket.Close();
            }
            #endregion

            listenSocket.Close();
        }
    }
}
