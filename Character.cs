using System;
using System.Data;
using Game;
using Raylib_cs;

namespace Game
{
    class Character
    {
        private int speed;
        private int damage;
        private int size;
        int posX;
        int posY;
        int health;
        private Texture2D texture;
        private Image image;
        public void InitPlayer(int size, int damage, int speed, string filepath, int x, int y)
        {
            this.size = size;
            this.damage = damage;
            this.speed = speed;
            this.posX = x;
            this.posY = y;
            image = Raylib.LoadImage(filepath);
            Raylib.ImageResize(ref image, size, size);
            texture = Raylib.LoadTextureFromImage(image);
            Raylib.UnloadImage(image);
            Console.WriteLine("Player created succesfuly");
        }
        public int GetSize()
        {
            return size;
        }
        public int GetDamage()
        {
            return damage;
        }
        public int GetSpeed()
        {
            return speed;
        }
        public int GetPosX()
        { return posX; }

        public int GetPosY() { return posY; }

        public void SetSpeed(int speed)
        {
            this.speed = speed;
        }
        public void SetDamage(int damage)
        {
            this.damage = damage;
        }
        public void UpdatePosition(int x, int y)
        {
            posX = Math.Clamp(x, 0, 2880 - GetSize());
            posY = Math.Clamp(y, 0, 1800 - GetSize());
        }
        public void DrawCharacter()
        {
            Raylib.DrawTexture(texture, posX, posY, Color.White);
        }
        ~Character()
        {
            Raylib.UnloadTexture(texture);
        }
    }
    class Player : Character
    {
        public void HandleInput()
        {
            if (Raylib.IsKeyDown(KeyboardKey.Left) || Raylib.IsKeyDown(KeyboardKey.A))
            {
                UpdatePosition(GetPosX() - GetSpeed(), GetPosY());
            }
            if (Raylib.IsKeyDown(KeyboardKey.Right) || Raylib.IsKeyDown(KeyboardKey.D))
            {
                UpdatePosition(GetPosX() + GetSpeed(), GetPosY());
            }
        }
    }
    class Bullet
    {
        private int speed = 5;
        private string sprite = "|";
        private int posX;
        private int posY;
        public bool isDone = false;
        private Character player;
        public Bullet(Character character)
        {
            player = character;
            posX = character.GetPosX();
            posY = character.GetPosY()-30;
            speed = (character is Player) ? 10 : 5;
        }
        public int GetSpeed()
        {
            return speed;
        }
        public void SetSpeed(int speed)
        {
            this.speed = speed;
        }
        public void shoot()
        {
            Raylib.DrawText(sprite, GetPosX()+45, GetPosY(), 40, Color.Green);
        }
        public void DrawBullet()
        {
            Raylib.DrawText(sprite,GetPosX()+45,GetPosY(),40,Color.Green);
        }
        public void UpdatePosition()
        {
            posX = GetPosX();
            posY = GetPosY() - speed;
        }
        public int GetPosX()
        { return posX; }

        public int GetPosY() { return posY; }

    }



    class Enemy : Character
    {
        public bool isKilled = false;
        public void MoveEnemey()
        {
            int EnemyX = GetPosX();
            int EnemyY = GetPosY();
            if((EnemyX+GetSize())>=2800)
            {
                UpdatePosition(50, EnemyY + 100);
            }
            else
            {
                UpdatePosition(EnemyX + GetSpeed(), EnemyY);
            }
        }
        public bool KillPlayer(Player player)
        {
            int PlayerX = player.GetPosX();
            int PlayerY = player.GetPosY();
            int EnemyY = GetPosY();
            int EnemyX = GetPosX();
            if(Math.Abs(EnemyX-PlayerX)<15 && Math.Abs(PlayerY - EnemyY)<70)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}