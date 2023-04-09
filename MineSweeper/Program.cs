using System;
using System.Collections.Generic;
using System.IO;

namespace MineSweeper
{
    class Program
    {
        protected static Tile[,] arr;

        public static List<int[]> AddNeighbour(int row, int col)
        {
            int length = arr.GetLength(0) - 1;
            var listTile = new List<int[]>();
            if (arr[row, col].GetMine() == 0)
            {
                if (row != 0)
                {
                    if (arr[row - 1, col].isPressed == false) listTile.Add(new int[] { row - 1, col });
                    if (col != length)
                    {
                        if (arr[row - 1, col + 1].isPressed == false) listTile.Add(new int[] { row - 1, col + 1 });
                    }
                }
                if (col != 0)
                {
                    if (arr[row, col - 1].isPressed == false) listTile.Add(new int[] { row, col - 1 });
                    if (row != length)
                    {
                        if (arr[row + 1, col - 1].isPressed == false) listTile.Add(new int[] { row + 1, col - 1 });
                    }
                }
                if (row != 0 && col != 0)
                {
                    if (arr[row - 1, col - 1].isPressed == false) listTile.Add(new int[] { row - 1, col - 1 });
                }
                if (row != length && col != length)
                {
                    if (arr[row + 1, col + 1].isPressed == false) listTile.Add(new int[] { row + 1, col + 1 });
                }
                if (row != length)
                {
                    if (arr[row + 1, col].isPressed == false) listTile.Add(new int[] { row + 1, col });
                }
                if (col != length)
                {
                    if (arr[row, col + 1].isPressed == false) listTile.Add(new int[] { row, col + 1 });
                }
            }
            return listTile;
        }

        public static void RevealVoid(int row, int col)
        {
            var listTile = AddNeighbour(row, col);
            if(arr[row,col].GetCountNeighbourMine() == 0)
            {
                arr[row, col].Press();
                foreach(var neigh in listTile)
                {
                    if (arr[neigh[0], neigh[1]].GetCountNeighbourMine() != 0) arr[neigh[0], neigh[1]].Press();
                    else RevealVoid(neigh[0], neigh[1]);
                }
            }
        }

        public static void FindMine(int cursorTop, int cursorLeft)
        {
            int row = (cursorTop - 1) / 2;
            int col = (cursorLeft - 1) / 2;
            if (arr[row, col].GetMine() == 1 || arr[row, col].GetCountNeighbourMine() != 0)
            {
                arr[row, col].Press();
            }
            else
            {
                RevealVoid(row, col);
            }
        }

        public static void CountNeighbours()
        {
            int length = arr.GetLength(0) - 1;
            int count = 0;
            for (int i = 0; i <= length; i++)
            {
                for (int j = 0; j <= length; j++)
                {
                    if (arr[i, j].GetMine() == 0)
                    {
                        if (i != 0)
                        {
                            if (arr[i - 1, j].GetMine() == 1) count++;
                            if (j != length)
                            {
                                if (arr[i - 1, j + 1].GetMine() == 1) count++;
                            }
                        }
                        if (j != 0)
                        {
                            if (arr[i, j - 1].GetMine() == 1) count++;
                            if (i != length)
                            {
                                if (arr[i + 1, j - 1].GetMine() == 1) count++;
                            }
                        }
                        if (i != 0 && j != 0)
                        {
                            if (arr[i - 1, j - 1].GetMine() == 1) count++;
                        }
                        if (i != length && j != length)
                        {
                            if (arr[i + 1, j + 1].GetMine() == 1) count++;
                        }
                        if (i != length)
                        {
                            if (arr[i + 1, j].GetMine() == 1) count++;
                        }
                        if (j != length)
                        {
                            if (arr[i, j + 1].GetMine() == 1) count++;
                        }
                        arr[i, j].SetCountNeighbourMine(count);
                        count = 0;
                    }
                }
            }
        }
        public static void FillField()
        {
            int n = arr.GetLength(0);
            int tileAmount = n * n;
            int mineAmount = (int)(tileAmount * 0.15);
            var rand = new Random();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    int random = rand.Next(2);
                    if (mineAmount == 0) random = 0;

                    arr[i, j] = new Tile(0, random);

                    if (random == 1) mineAmount--;
                }
            }
            CountNeighbours();
        }
        public static void Draw()
        {
            string grid = "-+";
            int cursorLeft = Console.CursorLeft;
            int cursorTop = Console.CursorTop;
            Console.Clear();
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                Console.WriteLine();
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    Console.Write("|");
                    arr[i, j].Draw();
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.WriteLine();
            }
            Console.SetCursorPosition(cursorLeft, cursorTop);
        }
        public static void MoveCursor(int top, int left)
        {
            int posLeftCursor = Console.CursorLeft;
            int posTopCursor = Console.CursorTop;
            bool leftException = posLeftCursor == 1 && left == -2;
            bool topException = posTopCursor == 1 && top == -2;
            if (!leftException && !topException)
            {
                Console.SetCursorPosition(posLeftCursor + left, posTopCursor + top);
            }
        }
        public static void CursorInput(ConsoleKeyInfo inputKey)
        {
            switch (inputKey.Key)
            {
                case ConsoleKey.LeftArrow:
                    MoveCursor(0, -2);
                    break;
                case ConsoleKey.RightArrow:
                    MoveCursor(0, 2);
                    break;
                case ConsoleKey.UpArrow:
                    MoveCursor(-2, 0);
                    break;
                case ConsoleKey.DownArrow:
                    MoveCursor(2, 0);
                    break;
                case ConsoleKey.F1:
                    FindMine(Console.CursorTop, Console.CursorLeft);
                    Draw();
                    break;
                case ConsoleKey.F2:
                    arr[(Console.CursorTop - 1) / 2, (Console.CursorLeft - 1) / 2].SetColor(ConsoleColor.Magenta);
                    Draw();
                    break;

            }
        }
        public static void Main()
        {
            Console.WriteLine("Input Size MineSweeper");
            int n = Int32.Parse(Console.ReadLine());
            arr = new Tile[n, n];
            FillField();
            Console.CursorSize = 100;

            Draw();
            Console.SetCursorPosition(1, 1);
            Console.BackgroundColor = ConsoleColor.Black;

            ConsoleKeyInfo input = new ConsoleKeyInfo('K', ConsoleKey.K, false, false, false);

            while (input.Key != ConsoleKey.Escape)
            {
                input = Console.ReadKey();
                CursorInput(input);
            }
        }
    }

    class Tile
    {
        int mine;
        int countNeighbourMine;
        public bool isPressed = false;
        ConsoleColor color = ConsoleColor.White;

        public Tile(int countNeighbourMine, int mine)
        {
            this.mine = mine;
            this.countNeighbourMine = countNeighbourMine;
        }

        public void Press()
        {
            isPressed = true;
        }

        public void SetColor(ConsoleColor color)
        {
            this.color = color;
        }

        public int GetMine()
        {
            return mine;
        }

        public int GetCountNeighbourMine()
        {
            return countNeighbourMine;
        }
        public void SetCountNeighbourMine(int count)
        {
            countNeighbourMine = count;
        }
        public void Draw()
        {
            if (mine == 1 && isPressed)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write(' ');
            }

            if (!isPressed)
            {
                Console.BackgroundColor = color;
                Console.Write(' ');
            }

            if (mine == 0 && isPressed)
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(countNeighbourMine);
                Console.ResetColor();
            }
        }
    }
}

