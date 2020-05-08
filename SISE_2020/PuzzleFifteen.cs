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
            astarReturn.visitedStates = 1;
            astarReturn.processedStates = 0;
            astarReturn.depth = 0;

            List<(PuzzleMatrix, int)> openStates = new List<(PuzzleMatrix, int)>();
            List<PuzzleMatrix> closedStates = new List<PuzzleMatrix>();

            // Initialize the current states list with initial state
            openStates.Add((beginMatrix, Fscore(beginMatrix, beginMatrix, heuristic)));
            int bestMatrix = 0;

            char[] operationOrder = { 'L', 'R', 'U', 'D' };

            Stopwatch watch = new Stopwatch();
            watch.Start();

            while (true)
            {
                for (int i = 0; i < openStates.Count; i++)
                {
                    // Set current index based on lowest fscore
                    if (openStates[i].Item2 < openStates[bestMatrix].Item2)
                    {
                        bestMatrix = i;
                    }
                }

                // Check if selected matrix is valid
                if (openStates[bestMatrix].Item1.Validate())
                {
                    watch.Stop();
                    astarReturn.time = watch.Elapsed.TotalMilliseconds;
                    astarReturn.depth = heuristic(beginMatrix, openStates[bestMatrix].Item1);
                    astarReturn.resolvedMatrix = new PuzzleMatrix(openStates[bestMatrix].Item1);
                    return astarReturn;
                }
                // If validation fails create new matrices and evaluate Fscores
                foreach (var operation in operationOrder)
                {
                    PuzzleMatrix newMat = openStates[bestMatrix].Item1.MoveFreeSpace(operation);
                    if (newMat != null && !WasThatStateBefore(newMat, closedStates))
                    {
                        openStates.Add((newMat, Fscore(beginMatrix, newMat, heuristic)));
                        astarReturn.visitedStates++;
                    }
                }

                closedStates.Add(openStates[bestMatrix].Item1);
                astarReturn.processedStates++;
                openStates.RemoveAt(bestMatrix);
                
            }

            watch.Stop();
            astarReturn.time = watch.ElapsedMilliseconds / 1000.0;

            return astarReturn;
        }

        private static int Fscore(PuzzleMatrix startState, PuzzleMatrix state, Heuristic heuristic)
        {
            return heuristic(startState, state) + heuristic(state, PuzzleMatrix.GetTargetPuzzle());
        }

        public PuzzleReturn BFS(PuzzleMatrix beginMatrix, string commandOrder)
        {
            returnVariables = new PuzzleReturn();
            returnVariables.visitedStates = 1;
            returnVariables.processedStates = 0;
            returnVariables.depth = 0;
            returnVariables.maxDepth = 0;
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
                        returnVariables.visitedStates++;
                        if (!WasThatStateBefore(state)) // if that state was not before
                        {
                            allStates.Add(state);
                            if(state.Validate()) //if is valid
                            {
                                watch.Stop();
                                returnVariables.time = watch.Elapsed.TotalMilliseconds;
                                returnVariables.resolvedMatrix = new PuzzleMatrix(state);
                                returnVariables.maxDepth = returnVariables.depth;
                                return returnVariables;
                            }
                            else
                            {
                                for(int i = 0; i < commandOrder.Length; ++i)
                                {
                                    PuzzleMatrix theMatrix = state.MoveFreeSpace(commandOrder[i]);
                                    if(theMatrix != null )
                                    {
                                        newStates.Add( theMatrix );
                                    }
                                }
                            }
                            returnVariables.processedStates++;
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
            returnVariables.visitedStates = 1;
            returnVariables.processedStates = 0;
            returnVariables.maxDepth = 0;
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
            if(currentMatrix.recursionDepth > returnVariables.maxDepth)
            {
                returnVariables.maxDepth = currentMatrix.recursionDepth;
            }
            bool returnFlag = false;
            if (currentMatrix != null && currentMatrix.recursionDepth < maxDepth)
            {
                returnVariables.processedStates++;
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
                        returnVariables.visitedStates++;
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
            returnVariables.depth = currentMatrix.recursionDepth;
            return false;
        }

        public delegate int Heuristic(PuzzleMatrix stateFrom, PuzzleMatrix stateTo);

        public static int Hamming(PuzzleMatrix stateFrom, PuzzleMatrix stateTo)
        {
            int value = 0;
            for (int i = 0; i < stateFrom.matrix.GetLength(0); i++)
            {
                for (int j = 0; j < stateFrom.matrix.GetLength(1); j++)
                {
                    if (stateFrom.matrix[i, j] != stateTo.matrix[i, j])
                    {
                        value++;
                    }
                }
            }
            return value;
        }

        public static int Manhattan(PuzzleMatrix stateFrom, PuzzleMatrix stateTo)
        {
            int value = 0;
            for (int i = 0; i < stateFrom.matrix.GetLength(0); i++)
            {
                for (int j = 0; j < stateFrom.matrix.GetLength(1); j++)
                {
                    int targetRow = 0, targetColumn = 0;
                    int fromValue = stateFrom.matrix[i, j];
                    for (int k = 0; k < stateTo.matrix.GetLength(0); k++)
                    {
                        for (int l = 0; l < stateTo.matrix.GetLength(1); l++)
                        {
                            if (fromValue == stateTo.matrix[k, l])
                            {
                                targetRow = k;
                                targetColumn = l;
                                k = int.MaxValue - 1;
                                l = int.MaxValue - 1;
                            }
                        }
                    }
                    value += Math.Abs(i - targetRow);
                    value += Math.Abs(j - targetColumn);
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
        public int visitedStates; // Aka createdStates
        public int processedStates; // Aka parsedStates
        public int depth;
        public int maxDepth;
        public PuzzleMatrix resolvedMatrix;
    };
}
