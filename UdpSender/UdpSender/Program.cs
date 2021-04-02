using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UdpSender
{
    class Program
    {
        public static bool control;
        public static string jpg;
        public static string jpgfile;  

       

        static void Main(string[] args)
        {
           
            UdpClient server = new UdpClient(8080);
            server.Connect(new IPEndPoint(IPAddress.Parse("10.31.8.215"), 8081));
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 8080);
            

            /******************************/
            control = true;
            while (control == true) {
                Console.WriteLine(".jpg file");
                jpgfile = Console.ReadLine();
                if (File.Exists(jpgfile))
                {
                    using (StreamReader sr = new StreamReader(jpgfile))
                        jpg = sr.ReadToEnd(); //Read jpg
                    Console.WriteLine("Jpg is done");

                    control = false;

                }
                else
                {
                    control = true;
                }
            }

            byte[] packetdata = Encoding.ASCII.GetBytes(jpg);
            Console.WriteLine("" + packetdata.Length);                 
            List<byte[]> byteArrayList = new List<byte[]>();
            int x = packetdata.Length / 1024;
            int y = packetdata.Length % 1024;

            for (int b = 0; b < x; b++)
            {
                byte[] bytes = new byte[1024];
                int index = 0;
                for (int a = b*1024; a < b*1024+1024; a++)
                {
                    
                    bytes[index] = packetdata[a];
                    index++;
                }
                byteArrayList.Add(bytes);
            }
            byte[] remain = new byte[y];
            int index2 = 0;
            for (int c = x*1024; c < x*1024+y; c++)
            {
                remain[index2] = packetdata[c];
                index2++;
            }
            byteArrayList.Add(remain);
            for (int i = 0; i <x+1; i++)
            {
                server.Send(byteArrayList[i], byteArrayList[i].Length);
                Byte[] receiveBytes = server.Receive(ref RemoteIpEndPoint);
                string returndata = Encoding.ASCII.GetString(receiveBytes);
                if (receiveBytes == null)
                {
                    server.Send(byteArrayList[i], byteArrayList[i].Length);
                }
                
                Console.WriteLine(".jpg is sending." + byteArrayList[i].Length);
            }
            string final = "Done";
            byte[] finalbyte = Encoding.ASCII.GetBytes(final);
            server.Send(finalbyte , finalbyte.Length);
            Console.WriteLine(".jpg is sent succesfully.");
            Thread.Sleep(50000);
        }
    }
}

