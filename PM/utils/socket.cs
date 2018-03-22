using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

namespace PM.utils
{
    public class SocketConnect
    {
        string host;
        int port;
        Socket listener;
        public SocketConnect(string Host,int Port)
        {
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
             host = Host;
             port = Port;
            // 构建Socket实例、设置端口号和监听队列大小
        }
        public void Listen()
        {
            listener.Bind(new IPEndPoint(IPAddress.Parse(host), port));
            listener.Listen(5);
            Console.WriteLine("Waiting for connect...");
            // 进入死循环，等待新的客户端连入。一旦有客户端连入，就分配一个Task去做专门处理。然后自己继续等待。
            while (true)
            {
                var clientExecutor = listener.Accept();
                if (clientExecutor!=null)
                Task.Factory.StartNew(() => {
                    // 获取客户端信息，C#对(ip+端口号)进行了封装。
                    var remote = clientExecutor.RemoteEndPoint;
                    Console.WriteLine("Accept new connection from {0}", remote);
                    // 进入死循环，读取客户端发送的信息
                    var bytes = new byte[1024];
                    while (true)
                    {
                        var count = clientExecutor.Receive(bytes);
                        var msg = Encoding.UTF8.GetString(bytes, 0, count);
                        Console.WriteLine("{0}: {1}", remote, msg);
                        if(msg.Length>3)
                        {
                            clientExecutor.Send(Encoding.UTF8.GetBytes("GetPost"));
                            break;
                        }
                        Array.Clear(bytes, 0, count);
                    }
                    clientExecutor.Close();
                    Console.WriteLine("{0} closed", remote);
                });
            }
        }
    }
}
