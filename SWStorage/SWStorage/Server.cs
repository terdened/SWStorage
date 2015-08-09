using SWStorage.RequestHandlers;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SWStorage
{
    class Server
    {
        TcpListener HttpListener; // Объект, принимающий TCP-клиентов

        // Запуск сервера
        public Server()
        {
            StartListener("http");

            // В бесконечном цикле
            while (true)
            {
                // Принимаем новых клиентов. После того, как клиент был принят, он передается в новый поток (ClientThread)
                // с использованием пула потоков.
                ThreadPool.QueueUserWorkItem(new WaitCallback(ClientThread), HttpListener.AcceptTcpClient());
            }
        }

        // Остановка сервера
        ~Server()
        {
            // Если "слушатель" был создан
            if (HttpListener != null)
            {
                // Остановим его
                HttpListener.Stop();
            }
        }

        private static void ClientThread(Object StateInfo)
        {
            new HttpHandler((TcpClient)StateInfo);
        }

        private static void InitThreadPool()
        {
            // Определим нужное максимальное количество потоков
            // Пусть будет по 4 на каждый процессор
            int MaxThreadsCount = Environment.ProcessorCount * 4;
            // Установим максимальное количество рабочих потоков
            ThreadPool.SetMaxThreads(MaxThreadsCount, MaxThreadsCount);
            // Установим минимальное количество рабочих потоков
            ThreadPool.SetMinThreads(2, 2);
        }

        private void StartListener(string listenerType)
        {
            bool isSuccess = false;
            switch(listenerType)
            {
                case "http":
                    HttpListener = new TcpListener(IPAddress.Any, 8080);
                    HttpListener.Start();
                    isSuccess = true;
                    break;
                default:
                    break;
            }

            if (isSuccess)
                Console.WriteLine(String.Format("Started {0} listener", listenerType));
            else
                Console.WriteLine(String.Format("Can't start {0} listener", listenerType));
        }

        static void Main(string[] args)
        {
            InitThreadPool();
            new Server();
        }
    }
}
