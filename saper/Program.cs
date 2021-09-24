using System;

namespace saper
{
    class Program
    {
        static void Main(string[] args)
        {
            bool gameEnded = false;
            int correctFlags = 0;
            Random rW = new Random(), rh = new Random();
            bool AutoPointBombState = false;
            int settingCounter = 0;
            Console.WriteLine("Hello Miner game!");
            Console.WriteLine("Please, enter size of field, and bomb amount in three following lines");
            Console.Write("width:");
            string w = Console.ReadLine();
            Console.Write("height:");
            string h = Console.ReadLine();
            Console.Write("amount:");
            string a = Console.ReadLine();
            int width = ToInt(w), height = ToInt(h), amount = ToInt(a);
            Point[,] field = new Point[height, width];
            if (amount > (width * height) / 2)
            {
                AutoPointBombState = !AutoPointBombState;
                amount = width * height - amount;
                Console.WriteLine("!");
            }
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    field[i, j] = new Point(AutoPointBombState, false, false);
                }
            }
            while (settingCounter < amount)
            {
                int xCord = rW.Next(width);
                int yCord = rh.Next(height);
                if (field[xCord, yCord].mine != !AutoPointBombState)
                {
                    settingCounter++;
                }
                field[xCord, yCord].mine = !AutoPointBombState;

            }
            SetNearBombAmount(field);
            // PrintBombs(field);
            
            while (gameEnded == false)
            {
                Console.Write("Enter next command:");
                string s = Console.ReadLine();
                string[] splited = s.Split();
                int x = ToInt(splited[0]), y = ToInt(splited[1]);
                string command = splited[2];
                System.GC.Collect();
                if ((command == "Flag" && field[x, y].flag) || (command == "Open" && field[x, y].open))
                {
                    Console.WriteLine("Re-enter correect command, please.");
                }
                else
                { 
                    if (command == "Flag")
                    {
                        if (field[x, y].flag)
                        {
                            field[x, y].flag = false;
                            if (field[x, y].mine)
                            {
                                correctFlags--;
                            }
                        }
                        else
                        {
                            field[x, y].flag = true;
                            if (field[x, y].mine)
                            {
                                correctFlags++;
                                if (correctFlags == amount)
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("You won. Gratz!");
                                    gameEnded = true;
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (command == "Open")
                        {

                            field[x, y].open = true;
                            if (field[x, y].mine)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("You lost. Feels bad.");
                                gameEnded = true;
                                Console.ForegroundColor = ConsoleColor.Gray;
                            }
                        }
                    }
                }
                if (!gameEnded)PrintMatrix(field);
            }
            PrintBombs(field);

        }

        public static int ToInt (string s)
        {
            int a = 0;
            int size = s.Length;
            while (s!= "")
            {
                a *= 10;
                a += (int)s[0] - '0';
                s = s.Substring(1, size - 1);
                size--;
                
            }
            return a;
        }
        public struct Point
        {
            public bool mine{get; set;}
            public bool flag { get; set; }
            public bool open { get; set; }

            public int bombsNear { get; set; }
            public Point(bool mine, bool flag, bool open)
            {
                this.mine = mine;
                this.flag = flag;
                this.open = open;
                bombsNear = 0;
            }
            public override string ToString()
            {
                if (open == false && !flag)
                {
                    return "C";
                }
                else
                {
                    if (mine == true && !flag) 
                    {
                        return "B";

                    }
                    else
                    {
                        if (flag)
                        {
                            return "F";
                        }
                        else
                        {
                            return bombsNear.ToString(); 
                        }
                    }
                }
            }
        }

        static void PrintMatrix (Point[,] p)
        {
            for (int i = 0; i < p.GetLength(0); i++)
            {
                string s = "";
                for (int j = 0; j < p.GetLength(1); j ++)
                {
                    s += p[i, j] + " ";
                    
                }
                Console.WriteLine(s);
            }
        }
        static bool ifInRange(int x, int y, int xMax, int yMax)
        {
            if (x >= 0 && x < xMax && y >= 0 && y < yMax)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static void SetNearBombAmount (Point[,] p)
        {
            int xCord = p.GetLength(0);
            int yCord = p.GetLength(1);
            int[,] matrix = new int[xCord, yCord];
            for (int i = 0; i < xCord; i ++)
            {
                for (int j = 0; j < yCord; j ++)
                {
                    if (p[i, j].mine == true) {

                        for (int i1 = i - 1; i1 <= i + 1; i1 ++)
                        {
                            for (int j1 = j - 1; j1 <= j + 1; j1 ++)
                            {
                                if (ifInRange(i1, j1, xCord, yCord))
                                {
                                    p[i1, j1].bombsNear++;
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void PrintBombs( Point[,] field)
        {
            int width = field.GetLength(0), height = field.GetLength(1);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (field[i, j].mine)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    Console.Write($"{field[i, j].bombsNear} ");
                    if (field[i, j].mine)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
