using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Forrst;

namespace Demo
{
    class Program
    {
        static void Main(string[] args) {
            var forrst = new ForrstClient();

            Console.WriteLine("User #1's first post:");
            Console.WriteLine(new User(1, forrst).Posts.First().Content);

            Console.WriteLine();
            Console.WriteLine("Requests done:");
            foreach (var request in forrst.RequestLog)
                Console.WriteLine(request.ToString());

            Console.ReadLine();
        }
    }
}
