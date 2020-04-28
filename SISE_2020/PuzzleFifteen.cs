using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SISE_2020
{
    class PuzzleFifteen
    {
        private List<PuzzleMatrix> allStates = new List<PuzzleMatrix>();
        private List<PuzzleMatrix> currentStates = new List<PuzzleMatrix>();
        private List<PuzzleMatrix> newStates = new List<PuzzleMatrix>();

        PuzzleReturn returnVariables;

        public static PuzzleReturn Astar(PuzzleMatrix beginMatrix, Heuristic heuristic)
        {
            PuzzleReturn astarReturn = new PuzzleReturn();
            astarReturn.createdStates = 1;
            astarReturn.depth = 0;
            astarReturn.parsedStates = 0;

            List<(PuzzleMatrix, int)> currentStates = new List<(PuzzleMatrix, int)>();
            List<PuzzleMatrix> visitedStates = new List<PuzzleMatrix>();
            int currentDepth = 0;

            // Initialize the current states
            currentStates.Add((beginMatrix, Fscore(currentDepth, beginMatrix, heuristic)));
            int bestMatrix = 0;

            char[] operationOrder = { 'L', 'R', 'U', 'D' };

            Stopwatch watch = new Stopwatch();
            watch.Start();

            while (true)
            {
                for (int i = 0; i < currentStates.Count; i++)
                {
                    // Set current index based on lowest fscore
                    if (currentStates[i].Item2 < currentStates[bestMatrix].Item2)
                    {
                        bestMatrix = i;
                    }
                    // Check if selected matrix is valid
                    if (currentStates[bestMatrix].Item1.Validate())
                    {
                        watch.Stop();
                        astarReturn.time = watch.Elapsed.TotalMilliseconds;
                        astarReturn.depth = currentDepth;
                        astarReturn.resolvedMatrix = new PuzzleMatrix(currentStates[bestMatrix].Item1);
                        return astarReturn;
                    }
                    // If validation fails create new matrices and evaluate Fscores
                    currentDepth++;
                    foreach (var operation in operationOrder)
                    {
                        PuzzleMatrix newMat = currentStates[bestMatrix].Item1.MoveFreeSpace(operation);
                        if (newMat != null && !WasThatStateBefore(newMat, visitedStates))
                        {
                            currentStates.Add((newMat, Fscore(newMat.Command.Length, newMat, heuristic)));
                        }
                    }

                    visitedStates.Add(currentStates[bestMatrix].Item1);
                    currentStates.RemoveAt(bestMatrix);
                }
            }

            watch.Stop();
            astarReturn.time = watch.ElapsedMilliseconds / 1000.0;

            return astarReturn;
        }

        private static int Fscore(int depth, PuzzleMatrix state, Heuristic heuristic)
        {
            return depth + heuristic(state);
        }

        public PuzzleReturn BFS(PuzzleMatrix beginMatrix, string commandOrder)
        {
            returnVariables = new PuzzleReturn();
            returnVariables.createdStates = 1;
            returnVariables.parsedStates = 0;
            returnVariables.depth = 0;
            allStates.Clear();
            currentStates.Add(beginMatrix);
            newStates.Clear();

            Stopwatch watch = new Stopwatch();
            watch.Start();

            while(true)
            {
                if(currentStates.Count > 0)
                {
                    foreach(var state in currentStates)
                    {
                        returnVariables.parsedStates++;
                        if(!WasThatStateBefore(state)) // if that state was not before
                        {
                            allStates.Add(state);
                            if(state.Validate()) //if is valid
                            {
                                watch.Stop();
                                returnVariables.time = watch.Elapsed.TotalMilliseconds;
                                returnVariables.resolvedMatrix = new PuzzleMatrix(state);
                                return returnVariables;
                            }
                            else
                            {
                                for(int i = 0; i < commandOrder.Length; ++i)
                                {
                                    returnVariables.createdStates++;
                                    PuzzleMatrix theMatrix = state.MoveFreeSpace(commandOrder[i]);
                                    if(theMatrix != null )
                                    {
                                        newStates.Add( theMatrix );
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Unresolved");
                    watch.Stop();
                    returnVariables.time = watch.Elapsed.TotalMilliseconds;
                    returnVariables.resolvedMatrix = null;
                    return returnVariables;
                }
                currentStates = new List<PuzzleMatrix>(newStates);
                newStates.Clear();
                returnVariables.depth++;
            }
        }

        public PuzzleReturn DFS(PuzzleMatrix beginMatrix, string commandOrder, int maxDepth)
        {
            returnVariables = new PuzzleReturn();
            returnVariables.createdStates = 1;
            returnVariables.parsedStates = 0;
            allStates.Clear();
            beginMatrix.recursionDepth = 0;

            Stopwatch watch = new Stopwatch();
            watch.Start();

            if(!DFSAlgorithm(beginMatrix, commandOrder, maxDepth))
            {
                Console.WriteLine("Unresolved");
            }
            watch.Stop();
            returnVariables.time = watch.Elapsed.TotalMilliseconds;
            return returnVariables;
        }

        private bool DFSAlgorithm(PuzzleMatrix currentMatrix, string commandOrder, int maxDepth)
        {
            bool returnFlag = false;
            if (currentMatrix != null && currentMatrix.recursionDepth < maxDepth)
            {
                returnVariables.parsedStates++;
                if (!WasThatStateBefore(currentMatrix)) // if that state was not before
                {
                    allStates.Add(currentMatrix);
                    if (currentMatrix.Validate()) //if is valid
                    {
                        returnVariables.resolvedMatrix = new PuzzleMatrix(currentMatrix);
                        returnVariables.depth = currentMatrix.recursionDepth;
                        return true;
                    }
                    else
                    {
                        for (int i = 0; i < commandOrder.Length; ++i)
                        {
                            returnVariables.createdStates++;
                            var theMatrix = currentMatrix.MoveFreeSpace(commandOrder[i]);
                            if (theMatrix != null)
                            {
                                theMatrix.recursionDepth = currentMatrix.recursionDepth + 1;
                                returnFlag |= DFSAlgorithm(theMatrix, commandOrder, maxDepth);
                                if (returnFlag)
                                {
                                    return true;
                                }
                            }
                        }
                            
                    }
                }
            }
            returnVariables.depth = currentMatrix.recursionDepth;
            return false;
        }

        public delegate int Heuristic(PuzzleMatrix state);

        public static int Hamming(PuzzleMatrix state)
        {
            int value = 0;
            for (int i = 0; i < state.matrix.Length; i++)
            {
                int currentNumber = state.matrix[i / state.matrix.GetLength(0), i % state.matrix.GetLength(1)];
                if (currentNumber != i + 1 && currentNumber != 0)
                {
                    value++;
                }
            }
            return value;
        }

        public static int Manhattan(PuzzleMatrix state)
        {
            int value = 0;
            for (int i = 0; i < state.matrix.GetLength(0); i++)
            {
                for (int j = 0; j < state.matrix.GetLength(1); j++)
                {
                    if (state.matrix[i, j] != 0)
                    {
                        int targetRow = state.matrix[i, j] / state.matrix.GetLength(0);
                        int targetColumn = state.matrix[i, j] % state.matrix.GetLength(1);
                        value += Math.Abs(targetRow - i);
                        value += Math.Abs(targetColumn - j);
                    }
                }
            }
            return value;
        }


        private bool WasThatStateBefore(PuzzleMatrix state)
        {
            foreach(var s in allStates)
            {
                if(s == state)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool WasThatStateBefore(PuzzleMatrix state, List<PuzzleMatrix> all)
        {
            foreach (var s in all)
            {
                if (s == state)
                {
                    return true;
                }
            }
            return false;
        }
    }


    struct PuzzleReturn
    {
        public double time;
        public int createdStates;
        public int parsedStates;
        public int depth;
        public PuzzleMatrix resolvedMatrix;
    };
}
