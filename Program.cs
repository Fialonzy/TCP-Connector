using System;
using System.Net.Sockets;
using System.Text;

namespace TSP_Connector
{
    class Program
    {
        private static string Host = "192.168.1.148";
        private static int Port = 180;
        static void Main(string[] args)
        {
            System.Console.WriteLine($"Пытаемся подключиться к серверу: {Host}:{Port}");
            try
            {
                using(var tcpClient = new TcpClient())
                {
                    System.Console.WriteLine("Подключение...");
                    tcpClient.Connect(Host, Port);
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
            catch (System.Exception e)
            {
                System.Console.WriteLine($"Возникла проблема в ходе работы: {e.Message}");
            }
            System.Console.WriteLine("Завершение работы. Нажмите любую клавишу для завершения...");
            Console.Read();
        }
    }
}
