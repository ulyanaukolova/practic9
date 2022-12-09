using System;
using System.Diagnostics;
using static System.Console;
namespace practic9
{
    internal class Program
    {
        private const int MapWidth = 30;
        private const int MapHeight = 20;
        private const int FrameMilliseconds = 200;
        private const ConsoleColor BorderColor = ConsoleColor.Gray;
        private const ConsoleColor FoodColor = ConsoleColor.Red;
        private const ConsoleColor BodyColor = ConsoleColor.Green;
        private const ConsoleColor HeadColor = ConsoleColor.DarkGreen;
        private static readonly Random Random = new Random();

        static void Main()
        {
            SetWindowSize(MapWidth * 3, MapHeight * 2); 
            SetBufferSize(MapWidth * 3, MapHeight * 2);
            CursorVisible = false;
            WriteLine("Нажмите Esc чтобы выйти или Enter чтобы начать");
            while (true)
            {
                ConsoleKey key = ReadKey().Key;
                if (key == ConsoleKey.Escape)
                {
                    Environment.Exit(0);
                }
                else if (key == ConsoleKey.Enter)
                {
                StartGame();
                Thread.Sleep(2000);
                ReadKey();
                }
            }
        }

        static void StartGame()
        {
            int score = 0;
            Clear();
            DrawBoard();
            ReadKey();
            Snake snake = new Snake(15, 10, HeadColor, BodyColor);

            Pixel food = GenFood(snake);
            food.Draw();

            Direction currentMovement = Direction.Right;

            var sw = new Stopwatch();

            while (true)
            {
                sw.Restart();
                Direction oldMovement = currentMovement;

                while (sw.ElapsedMilliseconds <= FrameMilliseconds)
                {
                    if (currentMovement == oldMovement)
                        currentMovement = ReadMovement(currentMovement);
                }

                sw.Restart();

                if (snake.Head.X == food.X && snake.Head.Y == food.Y)
                {
                    snake.Move(currentMovement, true);
                    food = GenFood(snake);
                    food.Draw();

                    score++;
                }
                else
                {
                    snake.Move(currentMovement);
                }
                /*я нашла в интернете как сделать боже, а то у меня змея проходила через себя*/
                if (snake.Head.X == MapWidth - 1 || snake.Head.X == 0 || snake.Head.Y == MapHeight - 1 || snake.Head.Y == 0 || snake.Body.Any(b => b.X == snake.Head.X && b.Y == snake.Head.Y))
                    break;
            }

            snake.Clear();
            food.Clear();

            SetCursorPosition(MapWidth, MapHeight + 10);
            WriteLine($"GAME OVER, PLEASE OFF YOUR PC, Score: {score}");
        }

        static Direction ReadMovement(Direction currentDirection)
        {
            if (!KeyAvailable)
                return currentDirection;

            ConsoleKey key = ReadKey(true).Key;

            currentDirection = key switch 
            {
                ConsoleKey.W when currentDirection != Direction.Down => Direction.Up,
                ConsoleKey.S when currentDirection != Direction.Up => Direction.Down,
                ConsoleKey.A when currentDirection != Direction.Right => Direction.Left,
                ConsoleKey.D when currentDirection != Direction.Left => Direction.Right,
                _ => currentDirection
            };

            return currentDirection;
        }
        static void DrawBoard()
        {
            for (int i = 0; i < MapWidth; i++)
            {
                new Pixel(i, 0, BorderColor).Draw();
                new Pixel(i, MapHeight - 1, BorderColor).Draw();
            }

            for (int i = 0; i < MapHeight; i++)
            {
                new Pixel(0, i, BorderColor).Draw();
                new Pixel(MapWidth - 1, i, BorderColor).Draw();
            }
        }

        static Pixel GenFood(Snake snake) //еда
        {
            Pixel food;

            do
            {
                food = new Pixel(Random.Next(1, MapWidth - 2), Random.Next(1, MapHeight - 2), FoodColor);
            } while (snake.Head.X == food.X && snake.Head.Y == food.Y ||
                     snake.Body.Any(b => b.X == food.X && b.Y == food.Y));

            return food;
        }
    }   

}