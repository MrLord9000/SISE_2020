using System;
using System.IO;

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
            string input = File.ReadAllText(args[2]);
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
                    puzzleReturn = pf.DFS(new PuzzleMatrix(inputValues), strategy, 25);
                    break;

                case "astr":
                    if (strategy == "hamm")
                        puzzleReturn = PuzzleFifteen.Astar(new PuzzleMatrix(inputValues), PuzzleFifteen.Hamming);
                    if (strategy == "manh")
                        puzzleReturn = PuzzleFifteen.Astar(new PuzzleMatrix(inputValues), PuzzleFifteen.Manhattan);
                    break;
            }

            using(StreamWriter writer = File.CreateText(outputSolutionPath))
            {
                if(puzzleReturn.resolvedMatrix == null)
                {
                    writer.WriteLine( (-1).ToString() );
                }
                else
                {
                    writer.WriteLine(puzzleReturn.resolvedMatrix.Command.Length);
                    writer.WriteLine(puzzleReturn.resolvedMatrix.Command);
                }
            }

            using (StreamWriter writer = File.CreateText(outputStatsPath))
            {
                if (puzzleReturn.resolvedMatrix == null)
                {
                    writer.WriteLine((-1).ToString());
                }
                else
                {
                    writer.WriteLine(puzzleReturn.resolvedMatrix.Command.Length);
                    writer.WriteLine(puzzleReturn.visitedStates.ToString());
                    writer.WriteLine(puzzleReturn.processedStates.ToString());
                    writer.WriteLine(puzzleReturn.maxDepth.ToString());
                    writer.WriteLine(puzzleReturn.time.ToString("f3"));
                }
            }
        }
    }
}
