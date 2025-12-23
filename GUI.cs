using Raylib_cs;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Game
{
    class GUI
    {
        public static void Main(string[] args)
        {
            int TotalEnemies = 200;
            List<Bullet> bullets = new List<Bullet>();
            List<Enemy> enemies = new List<Enemy>();
            string BGM = "RevenantFight.mp3";
            Raylib.InitWindow(100, 100, "Space Invader");
            int length = Raylib.GetMonitorHeight(0);
            int width = Raylib.GetMonitorWidth(0);
            int score = 0;
            Raylib.CloseWindow();
            Raylib.InitWindow(width, length, "Space Invader");
            Raylib.SetWindowState(ConfigFlags.FullscreenMode);
            Raylib.SetTargetFPS(90);
            Raylib.InitAudioDevice();
            Raylib.SetMasterVolume(1f);
            Music BackgroundMusic = Raylib.LoadMusicStream(BGM);
            //Raylib.PlayMusicStream(BackgroundMusic);
            Player player = new Player();
            Console.WriteLine($"LENGTH IS {length} AND WIDTH IS {width}");
            player.InitPlayer(84, 40, 10, "space-invaders.png", (int)(width / 2), length - 100);
            int EnemyStartX = 50;
            int EnemeyStartY = (int)(length*1/4);
            for (int i = 0;i<TotalEnemies;i++)
            {
                Enemy enemy = new Enemy();
                enemy.InitPlayer(36, 40, 8, "game.png",((EnemyStartX+((i*36)/TotalEnemies))),EnemeyStartY);
                enemy.MoveEnemey();
                enemies.Add(enemy);
            }
            bool onMenu = true;
            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);
                if (Raylib.IsKeyPressed(KeyboardKey.Enter))
                {
                    onMenu = !onMenu;
                }
                if (onMenu)
                {
                    Raylib.DrawText($"Press enter to start the game!", (int)(width / 2 - 270), (int)(length / 2), 34, Color.White);
                }
                else
                {
                    if(Raylib.IsKeyPressed(KeyboardKey.Space))
                    {
                        Bullet bullet = new Bullet(player);
                        bullet.shoot();
                        bullets.Add(bullet);
                    }
                    foreach(Bullet bullet in bullets)
                    {
                        bullet.UpdatePosition();
                        bullet.DrawBullet();
                    }
                    foreach (Enemy enemy in enemies){
                        enemy.DrawCharacter();
                        enemy.MoveEnemey();
                        onMenu = (enemy.KillPlayer(player)) ? true : onMenu;
                    }
                    if (!Raylib.IsMusicStreamPlaying(BackgroundMusic))
                    {
                        Raylib.PlayMusicStream(BackgroundMusic);
                    }
                    player.DrawCharacter();
                    player.HandleInput();
                    Raylib.DrawText($"Score: {score}", 10, 40, 34, Color.White);
                    Raylib.DrawText($"FPS: {(int)Raylib.GetFPS()}", width - 160, 40, 34, Color.White);
                    Raylib.UpdateMusicStream(BackgroundMusic);
                }
                Raylib.EndDrawing();

            }
            Raylib.CloseWindow();
        }
    }
}