using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace OCW.Services
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(OCWService));
            host.Open();
            Console.WriteLine("Serviço iniciado. Enter para terminar");
            Console.ReadLine();
            Console.WriteLine("A desligar serviço...");
            host.Close();

        }
    }
}
