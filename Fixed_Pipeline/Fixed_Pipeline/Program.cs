using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixed_Pipeline
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var game = new Game())
            {
                game.Run(30.0);
            }
        }
    }
}
