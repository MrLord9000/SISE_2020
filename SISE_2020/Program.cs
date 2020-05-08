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
            PuzzleReturn puzzleReturn = new PuzzleReturn();
            switch (algorithm)
            {
                case "bfs":
                    puzzleReturn = pf.BFS(new PuzzleMatrix(inputValues), strategy);
                    break;

                case "dfs":
                    puzzleReturn = pf.DFS(new PuzzleMatrix(inputValues), strategy, 5);
                    break;

                case "astr":
                    if (strategy == "hamm")
                        puzzleReturn = PuzzleFifteen.Astar(new PuzzleMatrix(inputValues), PuzzleFifteen.Hamming);
                    if (strategy == "manh")
                        puzzleReturn = PuzzleFifteen.Astar(new PuzzleMatrix(inputValues), PuzzleFifteen.Manhattan);
                    break;
            }
            Console.WriteLine("Time: " + puzzleReturn.time.ToString("f3") + "ms");
            Console.WriteLine("Created states: " + puzzleReturn.visitedStates.ToString());
            Console.WriteLine("Parsed states: " + puzzleReturn.processedStates.ToString());
            Console.WriteLine("Depth: " + puzzleReturn.depth.ToString());
            if (puzzleReturn.resolvedMatrix != null)
                Console.WriteLine("Return command: " + puzzleReturn.resolvedMatrix.Command);
        }
    }
}
