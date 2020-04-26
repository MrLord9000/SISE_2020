﻿using System;
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
        

        public bool Astar(PuzzleMatrix beginMatrix, string heuristic)
        {


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
                    int targetRow = state.matrix[i, j] / state.matrix.GetLength(0);
                    int targetColumn = state.matrix[i, j] % state.matrix.GetLength(1);
                    value += Math.Abs(targetRow - i);
                    value += Math.Abs(targetColumn - j);
                }
            }
            return value;
        }

        public BFSReturn BFS(PuzzleMatrix beginMatrix, string commandOrder)
        {
            BFSReturn returnVariables = new BFSReturn();
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
                        returnVariables.createdStates++;
                        if(!findInAllStates(state)) // if that state was not before
                        {
                            allStates.Add(state);
                            if(state.Validate()) //if is valid
                            {
                                watch.Stop();
                                returnVariables.time = watch.ElapsedMilliseconds / 1000.0;
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
                    returnVariables.time = watch.ElapsedMilliseconds / 1000.0;
                    returnVariables.resolvedMatrix = null;
                    return returnVariables;
                }
                currentStates = new List<PuzzleMatrix>(newStates);
                newStates.Clear();
                returnVariables.depth++;
            }
        }
        public bool findInAllStates(PuzzleMatrix current)
        {
            foreach(var state in allStates)
            {
                if(state == current)
                {
                    return true;
                }
            }
            return false;
        }
    }


    struct BFSReturn
    {
        public double time;
        public int createdStates;
        public int parsedStates;
        public int depth;
        public PuzzleMatrix resolvedMatrix;
    };
}
