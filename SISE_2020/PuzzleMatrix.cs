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

        private (int, int) zeroPosition;

        public PuzzleMatrix(int[,] values, string command = "")
        {
            matrix = new int[values.GetLength(0), values.GetLength(1)];
            matrix = values;
            if(command != "")
            {
                this.command = command;
            }
            recursionDepth = 0;
            for(int i = 0; i < matrix.GetLength(0); ++i)
            {
                for(int j = 0; j < matrix.GetLength(1); ++j)
                {
                    if(matrix[i,j] == 0)
                    {
                        zeroPosition = (i, j);
                        return;
                    }

                }
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
                    var tempValue = int.Parse(values[i + 2]);
                    matrix[i / matrix.GetLength(0), i % matrix.GetLength(1)] = tempValue;
                    if (tempValue == 0)
                    {
                        zeroPosition = (i / matrix.GetLength(0), i % matrix.GetLength(1));
                    }
                }
            }
            else
            {
                throw new ArgumentException($"Generated puzzle matrix has size {values.Length} while provided values had size {matrix.Length}. Aborting.");
            }
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder($"Puzzle Matrix. Size: {matrix.GetLength(0)}x{matrix.GetLength(1)}\n");
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

        public PuzzleMatrix MoveFreeSpace(char command)
        {
            int[,] newMatrix = matrix;
            int swapTemp;
            string newCommand = this.command;
            switch(command)
            {
                case 'U':
                case 'u':
                    if(zeroPosition.Item1 > 0)
                    {
                        swapTemp = newMatrix[zeroPosition.Item1, zeroPosition.Item2];
                        newMatrix[zeroPosition.Item1, zeroPosition.Item2] = newMatrix[zeroPosition.Item1 - 1, zeroPosition.Item2];
                        newMatrix[zeroPosition.Item1 - 1, zeroPosition.Item2] = swapTemp;
                        newCommand += "U";
                    }
                    else
                    {
                        return null;
                    }
                    break;
                case 'L':
                case 'l':
                    if (zeroPosition.Item2 > 0)
                    {
                        swapTemp = newMatrix[zeroPosition.Item1, zeroPosition.Item2];
                        newMatrix[zeroPosition.Item1, zeroPosition.Item2] = newMatrix[zeroPosition.Item1, zeroPosition.Item2 - 1];
                        newMatrix[zeroPosition.Item1, zeroPosition.Item2 - 1] = swapTemp;
                        newCommand += "L";
                    }
                    else
                    {
                        return null;
                    }
                    break;
                case 'R':
                case 'r':
                    if (zeroPosition.Item2 < newMatrix.GetLength(1))
                    {
                        swapTemp = newMatrix[zeroPosition.Item1, zeroPosition.Item2];
                        newMatrix[zeroPosition.Item1, zeroPosition.Item2] = newMatrix[zeroPosition.Item1, zeroPosition.Item2 + 1];
                        newMatrix[zeroPosition.Item1, zeroPosition.Item2 + 1] = swapTemp;
                        newCommand += "R";
                    }
                    else
                    {
                        return null;
                    }
                    break;
                case 'D':
                case 'd':
                    if (zeroPosition.Item1 < newMatrix.GetLength(0))
                    {
                        swapTemp = newMatrix[zeroPosition.Item1, zeroPosition.Item2];
                        newMatrix[zeroPosition.Item1, zeroPosition.Item2] = newMatrix[zeroPosition.Item1 + 1, zeroPosition.Item2];
                        newMatrix[zeroPosition.Item1 + 1, zeroPosition.Item2] = swapTemp;
                        newCommand += "D";
                    }
                    else
                    {
                        return null;
                    }
                    break;
            }
            return new PuzzleMatrix(newMatrix, newCommand);
        }

        public bool Validate()
        {
            bool validity = false;

            for (int i = 0; i < matrix.Length - 1; i++)
            {
                if (matrix[i / matrix.GetLength(0), i % matrix.GetLength(1)] == i + 1)
                {
                    validity = true;
                }
                else
                {
                    validity = false;
                    break;
                }
            }
            return validity;
        }

        public static bool operator ==(PuzzleMatrix left, PuzzleMatrix right)
        {
            int rowsL = left.matrix.GetLength(0), columnsL = left.matrix.GetLength(1);
            int rowsR = right.matrix.GetLength(0), columnsR = right.matrix.GetLength(1);
            // Check for equal dimensions
            if (rowsL != rowsR || columnsL != columnsR)
            {
                return false;
            }
            for (int i = 0; i < left.matrix.Length; i++)
            {
                if (left.matrix[i / rowsL, i % columnsL] != right.matrix[i / rowsR, i % columnsR])
                {
                    return false;
                }
            }
            return true;
        }

        public static bool operator !=(PuzzleMatrix left, PuzzleMatrix right)
        {
            int rowsL = left.matrix.GetLength(0), columnsL = left.matrix.GetLength(1);
            int rowsR = right.matrix.GetLength(0), columnsR = right.matrix.GetLength(1);
            // Check for equal dimensions
            if (rowsL != rowsR || columnsL != columnsR)
            {
                return true;
            }
            for (int i = 0; i < left.matrix.Length; i++)
            {
                if (left.matrix[i / rowsL, i % columnsL] != right.matrix[i / rowsR, i % columnsR])
                {
                    return true;
                }
            }
            return false;
        }
    }
}
