using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISE_2020
{
    /// <summary>
    /// Program handling fifteen puzzle with three differrent algorithms
    /// </summary>
    // TODO: Proper input arguments
    class Program
    {
        static void Main(string[] args)
        {
            string algorithm = args[0];
            string strategy = args[1];
            string input = System.IO.File.ReadAllText(args[2]);
            char[] separators = { ' ', '\n', '\r' };
            string[] inputValues = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            string outputSolutionPath = args[3];
            string outputStatsPath = args[4];

            PuzzleFifteen pf = new PuzzleFifteen();
            PuzzleReturn bfsReturn = new PuzzleReturn();
            switch (algorithm)
            {
                case "bfs":
                    bfsReturn = pf.BFS(new PuzzleMatrix(inputValues), strategy);
                    break;

                case "dfs":

                    break;

                case "astr":

                    break;
            }
            Console.WriteLine("Time: " + bfsReturn.time.ToString());
            Console.WriteLine("Created states: " + bfsReturn.createdStates.ToString());
            Console.WriteLine("Parsed states: " + bfsReturn.parsedStates.ToString());
            Console.WriteLine("Depth: " + bfsReturn.depth.ToString());
            if (bfsReturn.resolvedMatrix != null)
                Console.WriteLine("Return command: " + bfsReturn.resolvedMatrix.command);
        }
    }
}
