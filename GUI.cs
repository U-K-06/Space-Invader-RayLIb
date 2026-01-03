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
            bool GameOver = false;
            List<Bullet> bullets = new List<Bullet>();
            List<Enemy> enemies = new List<Enemy>();
            string BGM = "RevenantFight.mp3";
            Raylib.InitWindow(100, 100, "Space Invader");
            int length = Raylib.GetMonitorHeight(0);
            int width = Raylib.GetMonitorWidth(0);
            int score = 0;
            Raylib.CloseWindow();
            Raylib.InitWindow(width, length, "Space Invader");
            Raylib.SetWindowState(ConfigFlags.BorderlessWindowMode | ConfigFlags.UndecoratedWindow);
            Raylib.SetTargetFPS(90);
            Raylib.InitAudioDevice();
            Raylib.SetMasterVolume(1f);
            Music BackgroundMusic = Raylib.LoadMusicStream(BGM);
            //Raylib.PlayMusicStream(BackgroundMusic);
            Player player = new Player();
            Console.WriteLine($"LENGTH IS {length} AND WIDTH IS {width}");
            player.InitPlayer(84, 40, 10, "space-invaders.png", (int)(width / 2), length - 100);
            int EnemeyStartX = 0;
            int EnemeyStartY = (int)(length*1/4);
            int TotalEnemies = ((int)Math.Truncate(2880.0 / 108.0))*3;
            int EnemeySize = 45;
            int spacing = 12;
            int IncrementRow = 0;
            int col = 0;
            int row = 0;

            for (int i = 0; i < TotalEnemies; i++)
            {
                int x = EnemeyStartX + col * (EnemeySize + spacing);
                int y = EnemeyStartY + row * (EnemeySize + spacing);

                Enemy enemy = new Enemy();
                enemy.InitPlayer(45, 40, 1, "game.png", x, y);
                enemies.Add(enemy);

                col += 2;
                if (x + EnemeySize + spacing > width)
                {
                    col = 0;
                    row += 2;
                }
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
                    Raylib.DrawText($"Press enter to start the game!", (int)(width / 2 - 350), (int)(length / 2), 80, Color.White);
                }
                else
                {
                    if(enemies.Count() <= 0)
                    {
                        GameOver = true;
                    }
                    if (GameOver)
                    {
                        Raylib.DrawText($"Game over! Press enter to continue", (int)(width / 2 - 270), (int)(length / 2), 50, Color.White);
                        Raylib.DrawText($"You scored: {score}!", (int)(width / 2 - 270), (int)(length / 2 + 60), 80, Color.White);
                        player.UpdatePosition((int)(width / 2), length - 100);
                        int i = 0;
                        foreach(Enemy enemy in enemies)
                        {
                            enemy.UpdatePosition(EnemeyStartX+(36*i), 0+36*i);
                            i++;
                        }
                        if (Raylib.IsKeyPressed(KeyboardKey.Enter))
                        {
                            GameOver = false;
                            onMenu = true;
                            score = 0;
                        }
                    }
                    else
                    {
                        if (Raylib.IsKeyPressed(KeyboardKey.Space))
                        {
                            Bullet bullet = new Bullet(player);
                            bullet.shoot();
                            bullets.Add(bullet);
                        }
                        foreach (Bullet bullet in bullets)
                        {
                            if(bullet.GetPosY() < 0 )
                            {
                                bullet.isDone = true;
                            }
                            bullet.UpdatePosition();
                            bullet.DrawBullet();
                        }
                        foreach (Enemy enemy in enemies)
                        {
                            enemy.MoveEnemey();
                            enemy.DrawCharacter();
 
                            GameOver = enemy.KillPlayer(player);
                        }
                        foreach (Enemy enemy in enemies)
                        {
                            foreach (Bullet bullet in bullets)
                            {
                                int bulletX = bullet.GetPosX();
                                int bulletY = bullet.GetPosY();
                                int enemyX = enemy.GetPosX();
                                int enemyY = enemy.GetPosY();
                                bool hitX = bulletX >= enemyX && bulletX <= enemyX + EnemeySize;
                                bool hitY = bulletY <= enemyY && bulletY <= enemyY + EnemeySize;
                                if (hitX && hitY)
                                {
                                    bullet.isDone = true;
                                    enemy.isKilled = true;
                                    score++;
                                }
                            }
                        }
                        enemies.RemoveAll(e => e.isKilled);
                        bullets.RemoveAll(e => e.isDone);
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
                }
                Raylib.EndDrawing();

            }
            Raylib.CloseWindow();
        }
    }
}