using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAndAwait
{
    class Program
    {
        private static string player = "I";
        private static int width = 50;
        private static int height = 20;
        private static int characterRow = (height - 2) / 2; 
        private static int characterColumn = (width - 2) / 2;
        private static Arrow[] arrows = new Arrow[5];      

        class Arrow
        {
            public int Row { get; set; }
            public int Column { get; set; }
            public bool IsVisible { get; set; }
        }

        static async Task Main(string[] args)
        {                
            for (int i = 0; i < arrows.Length; i++)
            {
                arrows[i] = new Arrow
                {
                    Row = 1,
                    Column = i * (width - 2) / (arrows.Length - 1),
                    IsVisible = true
                };
            }

            DrawField();

            Task playerTask = UpdatePlayerPositionAsync();
            Task arrowsTask = UpdateArrowsAsync();

            await Task.WhenAll(playerTask, arrowsTask);
        }

        static async Task UpdatePlayerPositionAsync()
        {
            while (true)
            {
                ConsoleKeyInfo keyInfo = await Task.Run(() => Console.ReadKey(true));

                switch (keyInfo.Key)
                {
                    case ConsoleKey.LeftArrow:
                        if (characterColumn > 1)
                            characterColumn--;
                        break;
                    case ConsoleKey.RightArrow:
                        if (characterColumn < width - 2)
                            characterColumn++;
                        break;
                    case ConsoleKey.UpArrow:
                        if (characterRow > 1)
                            characterRow--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (characterRow < height - 2)
                            characterRow++;
                        break;
                    default:
                        break;
                }

                DrawField();
            }
        }

        static async Task UpdateArrowsAsync()
        {
            while (true)
            {
                await Task.Delay(500);

                foreach (Arrow arrow in arrows)
                {
                    if (arrow.IsVisible)
                    {
                        if (arrow.Row < height - 2)
                            arrow.Row++;
                        else
                        {
                            Random random = new Random();
                            arrow.Row = random.Next(1, height - 2);
                            arrow.Column = random.Next(1, width - 2);
                        }
                    }
                }

                DrawField();
            }
        }

        static void DrawField()
        {
            Console.Clear();

            string topBorder = '+' + new string('-', width - 2) + '+';
            string sideBorder = '|' + new string('#', width - 2) + '|';

            Console.WriteLine(topBorder);

            for (int i = 0; i < height - 2; i++)
            {
                Console.WriteLine(sideBorder);
            }

            Console.WriteLine(topBorder);
            
            Console.SetCursorPosition(characterColumn, characterRow);
            Console.Write(player);

            
            foreach (Arrow arrow in arrows)
            {
                if (arrow.IsVisible)
                {
                    Console.SetCursorPosition(arrow.Column, arrow.Row);
                    Console.Write("^");
                }
            }
        }
    }
}
