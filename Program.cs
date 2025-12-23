using System;
using System.Threading;

namespace MiniGame;

class Program
{
    private const int FreezeDurationMs = 1000;
    private static readonly string[] States = { "('-')", "(^-^)", "(X_X)" };
    private static readonly string[] Foods = { "@@@@@", "$$$$$", "#####" };

    private static Random random = new Random();
    private static string player = States[0];
    private static int foodIndex = 0;
    private static bool shouldExit = false;

    private static int playerX = 0;
    private static int playerY = 0;
    private static int foodX = 0;
    private static int foodY = 0;
    private static int width;
    private static int height;

    static void Main(string[] args)
    {
        InitializeConsole();
        InitializeGame();

        while (!shouldExit)
        {
            if (TerminalResized())
            {
                ExitGame();
                break;
            }

            CheckCollisions();
            ProcessPlayerState();
            Move();
        }
    }

    private static void InitializeConsole()
    {
        Console.CursorVisible = false;
        height = Console.WindowHeight - 1;
        width = Console.WindowWidth - 5;
    }

    private static void InitializeGame()
    {
        Console.Clear();
        ShowFood();
        Console.SetCursorPosition(0, 0);
        Console.Write(player);
    }

    private static bool TerminalResized()
    {
        return height != Console.WindowHeight - 1 || width != Console.WindowWidth - 5;
    }

    private static void ExitGame()
    {
        shouldExit = true;
        Console.Clear();
        Console.WriteLine("Console was resized. Program exiting.");
    }

    private static void CheckCollisions()
    {
        if (foodX == playerX && foodY == playerY)
        {
            ChangePlayer();
            ShowFood();
        }
    }

    private static void ProcessPlayerState()
    {
        if (player.Equals(States[2]))
        {
            FreezePlayer();
        }
    }

    private static void ShowFood()
    {
        foodIndex = random.Next(0, Foods.Length);
        foodX = random.Next(0, width - player.Length);
        foodY = random.Next(0, height - 1);

        Console.SetCursorPosition(foodX, foodY);
        Console.Write(Foods[foodIndex]);
    }

    private static void ChangePlayer()
    {
        player = States[foodIndex];
        Console.SetCursorPosition(playerX, playerY);
        Console.Write(player);
    }

    private static void FreezePlayer()
    {
        Thread.Sleep(FreezeDurationMs);
        player = States[0];
    }

    private static void Move()
    {
        int lastX = playerX;
        int lastY = playerY;
        int speed = player.Equals(States[1]) ? 3 : 1;

        switch (Console.ReadKey(true).Key)
        {
            case ConsoleKey.UpArrow:
                playerY -= speed;
                break;
            case ConsoleKey.DownArrow:
                playerY += speed;
                break;
            case ConsoleKey.LeftArrow:
                playerX -= speed;
                break;
            case ConsoleKey.RightArrow:
                playerX += speed;
                break;
            default:
                shouldExit = true;
                return;
        }

        Console.SetCursorPosition(lastX, lastY);
        for (int i = 0; i < player.Length; i++)
        {
            Console.Write(" ");
        }

        playerX = Math.Clamp(playerX, 0, width);
        playerY = Math.Clamp(playerY, 0, height);

        Console.SetCursorPosition(playerX, playerY);
        Console.Write(player);
    }
}
