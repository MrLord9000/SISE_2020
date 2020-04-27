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

        public PuzzleReturn Astar(PuzzleMatrix beginMatrix, string heuristic)
        {
            allStates.Clear();
            currentStates.Clear();
            newStates.Clear();
            PuzzleReturn astarReturn = new PuzzleReturn();
            astarReturn.createdStates = 1;
            astarReturn.depth = 0;
            astarReturn.parsedStates = 0;

            currentStates.Add(beginMatrix);

            Stopwatch watch = new Stopwatch();
            watch.Start();

            while (true)
            {
                if (currentStates.Count > 0)
                {
                    foreach(var state in currentStates)
                    {
                        astarReturn.createdStates++;
                        if (!allStates.Contains(state))
                        {
                            allStates.Add(state);
                            if (state.Validate())
                            {
                                watch.Stop();
                                astarReturn.time = watch.ElapsedMilliseconds / 1000.0;
                                astarReturn.resolvedMatrix = new PuzzleMatrix(state);
                                return astarReturn;
                            }
                            else
                            {

                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Unresolved");
                    astarReturn.resolvedMatrix = null;
                    break;
                }
            }

            watch.Stop();
            astarReturn.time = watch.ElapsedMilliseconds / 1000.0;

            return astarReturn;
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
                        if(!wasThatStateBefore(state)) // if that state was not before
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
                                    if(theMatrix != null)
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
                if (!wasThatStateBefore(currentMatrix)) // if that state was not before
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

        private int Hamming(PuzzleMatrix state)
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

        private int Manhattan(PuzzleMatrix state)
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


        private bool wasThatStateBefore(PuzzleMatrix state)
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
