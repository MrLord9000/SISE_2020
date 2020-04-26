using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISE_2020
{
    class PuzzleFifteen
    {
        private List<PuzzleMatrix> allStates;
        private PuzzleMatrix currentState;
        private PuzzleMatrix newState;
        
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
    }
}
