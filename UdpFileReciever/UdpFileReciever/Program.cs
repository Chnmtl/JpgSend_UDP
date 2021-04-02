using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UdpFileReciever
{
    class Program
    {
        static void Main(string[] args)
        {

            
            UdpClient client = new UdpClient(8081);
            client.Connect(new IPEndPoint(IPAddress.Parse("10.31.8.215"), 8080));
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 8080);
            Console.WriteLine("Connection established");
            bool x = true;
            string ack= "ACK";
            string a ="" ;

            byte[] abyte = Encoding.ASCII.GetBytes(a);
            byte[] ackbytes = Encoding.ASCII.GetBytes(ack);
            while (x == true)
            {
                Byte[] receiveBytes = client.Receive(ref RemoteIpEndPoint); // Recieve
                client.Send(ackbytes , ackbytes.Length);
                Console.WriteLine("jpg is receiving" + receiveBytes.Length);
                if (receiveBytes == null)
                {
                    client.Send(abyte,abyte.Length); // Send error
                }
                string returndata = Encoding.ASCII.GetString(receiveBytes);// byte to string
                if (returndata == "Done")
                {
                    x = false;
                }
            }
            Console.WriteLine(".jpg is succesfully received.");
            Thread.Sleep(50000);
        }
    }
}
