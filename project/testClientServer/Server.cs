using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace testBD
{
    class Server
    {
        /// <summary>
        /// Статус сервера.
        /// </summary>
        private bool isServerRunning;
        /// <summary>
        /// Список подключенных клиентов
        /// </summary>
        private Hashtable clients;
        /// <summary>
        /// Сокет сервера.
        /// </summary>
        private Socket listener;
        /// <summary>
        ///  Порт который будет слушать сокет
        /// </summary>
        private int port;
        /// <summary>
        /// Точка для прослушки входящих соединений (состоит из адреса и порта)
        /// </summary>
        private IPEndPoint Point;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listengPort">Порт который будет прослушиватся сокетом.</param>
        public Server(int listengPort)
        {
            this.port = listengPort;
        }
        /// <summary>
        /// Запускает сервер.
        /// </summary>
        public void ServerStart()
        {
            clients = new Hashtable(30); // magik number <==============
            isServerRunning = true;
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // Определяем конечную точку, IPAddress.Any означает что наш сервер будет принимать входящие соединения с любых адресов
            Point = new IPEndPoint(IPAddress.Any, port);
            // Связываем сокет с конечной точкой
            listener.Bind(Point);
            // Начинаем слушать входящие соединения
            listener.Listen(10);
            Console.WriteLine("Сервер запущен...");
            SocketAccepter();
        }
        /// <summary>
        /// Запускает цикл прослушивание порта.
        /// </summary>
        private void SocketAccepter()
        {
            // Запускаем цикл в отдельном потоке, чтобы приложение не зависло
            Thread SocketAccepter_thread = new Thread(delegate()
            {
                Console.WriteLine("Порт " + port + " прослушивается в новом потоке...");
                while (isServerRunning)
                {
                    // Создаем новый сокет, по которому мы сможем обращаться клиенту
                    // Цикл не остановится, пока какой-нибудь клиент не попытается присоединиться к серверу
                    Socket client = listener.Accept();
                    // Теперь, обратившись к объекту client, мы сможем отсылать и принимать пакеты
                    // от последнего подключившегося пользователя.
                    // Добавляем подключенного клиента в список всех клиентов, для дальнейшей массовой рассылки пакетов
                    clients.Add(client, "");
                    // Начинаем принимать входящие пакеты
                    Thread MessageReceiver_thread = new Thread(delegate()
                    {
                        Console.WriteLine("Получено сообщение от клиента...");
                        MessageReceiver(client);
                    });
                    MessageReceiver_thread.Start();
                }
            });
            // Приведенный выше цикл пока что не работает, запускаем поток. Теперь цикл работает.
            SocketAccepter_thread.Start();
            // threads.Add(th);
        }
        /// <summary>
        /// Данные полученые от клиента
        /// </summary>
        private byte[] DataBuffer { get; set; }

        /// <summary>
        /// Извличение полученых от клиента данных. 
        /// </summary>
        /// <param name="r_client"></param>
        private void MessageReceiver(Socket r_client)
        {
            // Для каждого нового подключения, будет создан свой поток для приема пакетов
            Thread thread = new Thread(delegate()
            {
                Console.WriteLine("Новый поток для извлечения данных...");

                while (isServerRunning)
                {
                    try
                    {
                        // Сюда будем записывать принятые байты
                        byte[] bytes = new byte[1024];
                        Console.WriteLine("Извлечения данных...");
                        // Принимаем 
                        r_client.Receive(bytes);
                        if (ReceiverEvent != null)
                            ReceiverEvent(bytes, r_client);
                    }
                    catch { }
                }
            });
            thread.Start();
        }

        public delegate void ReceiveEventDelegate(byte[] bytes, Socket client);

        /// <summary>
        /// При извличении полученых от клиента данных. 
        /// </summary>
        /// <param name="bytes">Предоставляет данные в виде массива байтов</param>
        /// <param name="client">Предоставляет клиента отправившего пакет</param>
        public event ReceiveEventDelegate ReceiverEvent;

        /// <summary>
        /// Отправка данных клиенту.
        /// </summary>
        /// <param name="client">Получатель, которому отправляется пакет</param>
        /// <param name="bytes">Данные которые отпправляются получателю</param>   

        public void Send(Socket Recipient, byte[] bytes)
        {
            int countter = 0, count = 10; // Magic number  <=============
            while (true)
            {
                try
                {
                    Recipient.Send(bytes);
                    break;
                }
                catch (SocketException ex)
                {
                    System.Console.WriteLine("данные не могут быть высланы обратно клиенту..." + ex.ToString());
                    if (countter >= count)
                        break;
                    countter++;
                }
                Thread.Sleep(500);
            }
        }
    }
}
