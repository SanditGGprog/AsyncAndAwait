

namespace AsyncAndAwait
{
    class Program
    {
       
        class Arrow
        {
            public int Row { get; set; }
            public int Column { get; set; }
            public bool IsVisible { get; set; }
        }

        static async Task Main(string[] args)
        {
            string player = "I";
            int width = 50;
            int height = 20;
            int characterRow = (height - 2) / 2; 
            int characterColumn = (width - 2) / 2; 

            // Создание массива из 5 стрелок
            Arrow[] arrows = new Arrow[5];
            for (int i = 0; i < arrows.Length; i++)
            {
                arrows[i] = new Arrow
                {
                    Row = 1,
                    Column = i * (width - 2) / (arrows.Length - 1),
                    IsVisible = true
                };
            }

            DrawField(width, height, player, characterRow, characterColumn, arrows);

            Task playerTask = UpdatePlayerPositionAsync(characterRow, characterColumn, width, height);
            Task arrowsTask = UpdateArrowsAsync(arrows, width, height);         
            await Task.WhenAll(playerTask, arrowsTask);
        }

        static async Task UpdatePlayerPositionAsync(int characterRow, int characterColumn, int width, int height)
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

                DrawField(width, height, "I", characterRow, characterColumn, new Arrow[5]);

            }
        }

        static async Task UpdateArrowsAsync(Arrow[] arrows, int width, int height)
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

                DrawField(width, height, "I", (height - 2) / 2, (width - 2) / 2, arrows);
            }
        }

        static void DrawField(int width, int height, string player, int characterRow, int characterColumn, Arrow[] arrows)
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

