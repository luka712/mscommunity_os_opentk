using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example_3_ElementDrawing_Uniforms
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var game = new Game())
            {
                game.Run(60.0);
            }
        }
    }
}
