using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TSP_Connector
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Enter 1 to create server, else write 2 to connect to server");
            var userChoise = Console.ReadLine();
            try
            {
                if(!int.TryParse(userChoise, out int variant)) 
                    variant = 1;
                switch (variant)
                {
                    case 1: CreateTcpServer(); break;
                    case 2: ConnectToTcpServer(); break;
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine($"Возникла проблема в ходе работы: {e.Message}");
            }
            
            System.Console.WriteLine("Завершение работы. Нажмите любую клавишу для завершения...");
            Console.Read();
        }

        static void CreateTcpServer(){
            IPAddress localAddr = IPAddress.Parse(LocalServerOptions.Host);
            TcpListener server = new TcpListener(localAddr, LocalServerOptions.Port);
            server.Start();

            while(true){
                Console.WriteLine("Ожидание подключений... ");

                // получаем входящее подключение
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Подключен клиент. Выполнение запроса...");

                // получаем сетевой поток для чтения и записи
                NetworkStream stream = client.GetStream();

                // сообщение для отправки клиенту
                string response = "Привет мир";
                // преобразуем сообщение в массив байтов
                byte[] data = Encoding.UTF8.GetBytes(response);

                // отправка сообщения
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Отправлено сообщение: {0}", response);
                // закрываем поток
                stream.Close();
                // закрываем подключение
                client.Close();
            }
        }

        static void ConnectToTcpServer(){
            System.Console.WriteLine($"Пытаемся подключиться к серверу: {ExternalServerOptions.Host}:{ExternalServerOptions.Port}");
            
            using(var tcpClient = new TcpClient())
            {
                System.Console.WriteLine("Подключение...");
                tcpClient.Connect(ExternalServerOptions.Host, ExternalServerOptions.Port);
                System.Console.WriteLine("Получение потока...");
                var stream = tcpClient.GetStream();

                System.Console.WriteLine("Формирование данных для комманды...");
                var command = "ON";
                var requestData = Encoding.UTF8.GetBytes(command);

                System.Console.WriteLine("Отправка команды на сервер...");
                stream.Write(requestData, 0, requestData.Length);

                System.Console.WriteLine("Завершение работы с хостом...");
                tcpClient.Close();
            }
            
        }
    }
    public class ExternalServerOptions{
        public static string Host = "192.168.1.148";
        public static int Port = 180;
    }

    public class LocalServerOptions{
        public static string Host = "127.0.0.1";
        public static int Port = 6001;
    }
}
