using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISE_2020
{
    class PuzzleMatrix
    {
        private int[,] matrix;

        /// <summary>
        /// Command to make this state (LRUD)
        /// </summary>
        private string command;

        private int recursionDepth;


        public PuzzleMatrix(int x, int y, int[] values)
        {
            matrix = new int[y, x];
            command = "";
            recursionDepth = 0;
            if (values.Length == matrix.Length)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    matrix[i / 4, i % 4] = values[i];
                }
            }
            else
            {
                throw new ArgumentException($"Generated puzzle matrix has size {values.Length} while provided values had size {matrix.Length}. Aborting.");
            }
        }

        public PuzzleMatrix(string[] values)
        {
            command = "";
            recursionDepth = 0;
            int rows = int.Parse(values[0]);
            int columns = int.Parse(values[1]);
            matrix = new int[rows, columns];
            if (values.Length - 2 == matrix.Length)
            {
                for (int i = 0; i < values.Length - 2; i++)
                {
                    matrix[i / 4, i % 4] = int.Parse(values[i + 2]);
                }
            }
            else
            {
                throw new ArgumentException($"Generated puzzle matrix has size {values.Length} while provided values had size {matrix.Length}. Aborting.");
            }
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder($"Puzzle Matrix. Size: {matrix.GetUpperBound(0) + 1}x{matrix.GetUpperBound(1) + 1}\n");
            for (int i = 0; i < matrix.Length; i++)
            {
                output.Append(matrix[i / 4, i % 4].ToString());
                if ((i + 1) % 4 == 0 && i != 0)
                {
                    output.Append("\n");
                }
                else
                {
                    output.Append("\t");
                }
            }
            return output.ToString();
        }
    }
}
