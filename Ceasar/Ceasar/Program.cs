using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceasar
{
  using System.Security.Cryptography.X509Certificates;

  class Program
  {
    static void Main(string[] args)
    {
      string input1 = "ABCabcXYZxyz";
      Console.WriteLine($"{input1} - {Ceasar(input1, 1)}");
    }

    public static string Ceasar(string text, int key)
    {
      return text.ToArray().Select(c => Encoding.Default.getBy(c)[0]).Join();
    }
  }
}
