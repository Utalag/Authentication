namespace Authentication.Client.Pages.Sudoku
{
    public class SudokuGenerator
    {
        private const int gridSize = 9;
        private const int subGridSize = 3;
        private int[,] board = new int[gridSize,gridSize];
        private Random random = new Random();

        public int[,] Generate()
        {
            FillDiagonalSubGrids();
            FillRemaining(0,subGridSize);
            return board;
        }

        private void FillDiagonalSubGrids()
        {
            for(int i = 0;i < gridSize;i += subGridSize)
            {
                FillSubGrid(i,i);
            }
        }

        private void FillSubGrid(int row,int col)
        {
            int[] nums = GetShuffledNumbers();
            for(int i = 0;i < subGridSize;i++)
            {
                for(int j = 0;j < subGridSize;j++)
                {
                    board[row + i,col + j] = nums[i * subGridSize + j];
                }
            }
        }

        private int[] GetShuffledNumbers()
        {
            int[] nums = new int[gridSize];
            for(int i = 0;i < gridSize;i++)
            {
                nums[i] = i + 1;
            }
            for(int i = 0;i < gridSize;i++)
            {
                int j = random.Next(i,gridSize);
                int temp = nums[i];
                nums[i] = nums[j];
                nums[j] = temp;
            }
            return nums;
        }

        private bool FillRemaining(int i,int j)
        {
            if(j >= gridSize && i < gridSize - 1)
            {
                i++;
                j = 0;
            }
            if(i >= gridSize && j >= gridSize)
            {
                return true;
            }
            if(i < subGridSize)
            {
                if(j < subGridSize)
                {
                    j = subGridSize;
                }
            }
            else if(i < gridSize - subGridSize)
            {
                if(j == (i / subGridSize) * subGridSize)
                {
                    j += subGridSize;
                }
            }
            else
            {
                if(j == gridSize - subGridSize)
                {
                    i++;
                    j = 0;
                    if(i >= gridSize)
                    {
                        return true;
                    }
                }
            }

            for(int num = 1;num <= gridSize;num++)
            {
                if(IsSafeToPlace(i,j,num))
                {
                    board[i,j] = num;
                    if(FillRemaining(i,j + 1))
                    {
                        return true;
                    }
                    board[i,j] = 0;
                }
            }
            return false;
        }

        private bool IsSafeToPlace(int i,int j,int num)
        {
            return !UsedInRow(i,num) && !UsedInColumn(j,num) && !UsedInSubGrid(i - i % subGridSize,j - j % subGridSize,num);
        }

        private bool UsedInRow(int i,int num)
        {
            for(int j = 0;j < gridSize;j++)
            {
                if(board[i,j] == num)
                {
                    return true;
                }
            }
            return false;
        }

        private bool UsedInColumn(int j,int num)
        {
            for(int i = 0;i < gridSize;i++)
            {
                if(board[i,j] == num)
                {
                    return true;
                }
            }
            return false;
        }

        private bool UsedInSubGrid(int rowStart,int colStart,int num)
        {
            for(int i = 0;i < subGridSize;i++)
            {
                for(int j = 0;j < subGridSize;j++)
                {
                    if(board[rowStart + i,colStart + j] == num)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
