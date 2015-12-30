using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using System.Dynamic;
using System.Reflection;
using System.Collections.Generic;


using TransmittedPackets;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
namespace testBD
{

    class Program
    {
        static Server server;
        static int port = 1010;// read is config file.....
        static void Main(string[] args)
        {
            server = new Server(port);
            server.ReceiverEvent += server_ReceiverEvent;
            server.ServerStart();

            testClient ts = new testClient(port);
            ts.Connect();
            ts.Sender(null);
        }

        static void server_ReceiverEvent(byte[] bytes, System.Net.Sockets.Socket client)
        {
            object otherPacket = SerealizationManager.Deserealize(bytes);
            Type typePacketObj = otherPacket.GetType();

            if (typePacketObj == typeof(RequestHalfStep))
                CustomerResponse(otherPacket as RequestHalfStep, client);

            if (typePacketObj == typeof(SaveHystoryParty)) ;

        }

        static void CustomerResponse(RequestHalfStep requestHalfStep, System.Net.Sockets.Socket client)
        {
            DBManager bdManager = new DBManager();
            bdManager.Connect();
            ChosenStep chosenStep = bdManager.GetChosenStep(requestHalfStep);
            server.Send(client, SerealizationManager.Serealize(chosenStep));
            bdManager.Close();
        }
        void EntryIntoDatabase(SaveHystoryParty objSaveHystory)
        {
            DBManager bdManager = new DBManager();
            bdManager.Connect();
            bdManager.Added(objSaveHystory);
            bdManager.Close();
        }
    }




    class testClient
    {
        Random rnd = new Random();
        private byte[,] randomMatri()
        {
            byte[,] matrix = new byte[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    matrix[i, j] = (byte)rnd.Next(0, 12);
                }
            }
            return matrix;
        }

        PointV2 randPoint()
        {
            PointV2 p = new PointV2(rnd.Next(0, 7), rnd.Next(0, 7));
            return p;
        }



        private bool client_running;
        /// <summary>
        ///  Сокет клиента
        /// </summary>

        private Socket client;
        /// Адрес сервера
        // Расшарить, разобратся с нат.
        private IPAddress ip = IPAddress.Parse("192.168.1.3");
        // Порт, по которому будем присоединяться
        private int port = 1010;
        // Список потоков
        private List<Thread> threads = new List<Thread>();

        public testClient(int port)
        {
            this.port = port;
        }


        public void Connect()
        {
            try
            {
                client_running = true;
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(ip, port);
                Receiver();
            }
            catch { System.Console.WriteLine("Что же не так то пошло при подключении"); }
        }
        /// <summary>
        /// Метод, принимающий данные от сервера
        /// </summary>
        public void Receiver()
        {
            Thread th = new Thread(delegate()
            {
                while (client_running)
                {
                    try
                    {
                        byte[] bytes = new byte[1024];
                        // Принимает данные от сервера в формате "X|Y"
                        client.Receive(bytes);
                        if (bytes.Length != 0)
                        {
                            ChosenStep cs = (ChosenStep)TransmittedPackets.SerealizationManager.Deserealize(bytes);

                            Console.WriteLine("Отсервера получено сообщение: " + cs.direction);
                            // string[] split_data = data.Split(new Char[] { '|' });
                            // Передаем отпарсенные значения методу Draw на отрисовку

                        }
                    }
                    catch { }
                }
            });
            th.Start();
            threads.Add(th);
        }
        /// <summary>
        /// Метод для отправки пакетов серверу
        /// </summary>
        /// <param name="msg"></param>
        public void Sender(string msg)
        {
            try
            {
                TransmittedPackets.RequestHalfStep zapros = new RequestHalfStep(randomMatri(), 10);

                client.Send(SerealizationManager.Serealize(zapros));
            }
            catch
            {
                System.Console.WriteLine("Что же не так то пошло при отправке");
            }

        }

    
    }
}