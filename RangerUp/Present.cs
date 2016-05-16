using System;
using System.Drawing;
using System.Windows.Forms;
using RangerUp.Properties;

namespace RangerUp
{
    public class Present:IDisposable
    {
        public Image ParachuteBox { get; set; }
        public Image Box { get; set; }
        public Image Heart { get; set; }
        public Image Missile { get; set; }
        public Image Armor { get; set; }


        public HiResTimer PresentTimer { get; set; }

        public float X { get; set; }
        public float Y { get; set; }
        public Point CenterPosition { get; set; }


        public bool IsBox { get; set; }
        public bool TypeHealth { get; set; }
        public bool TypeMissile { get; set; }
        public bool TypeArmor { get; set; }
        private bool _presentLanded;
        private bool _boxExploded;
        public bool IsDead;
        public bool OutOfScreen;

        public int PlusHealth { get; set; }
        public int PlusMissile { get; set; }
        public int PlusArmor { get; set; }

        public Present()
        {
            PresentTimer = new HiResTimer();
            LoadAssets();
            GeneratePresent();
        }

        private void GeneratePresent()
        {
            if (new Random().Next(1, 4) == 1)
            {
                TypeHealth = true;
            }
            else if (new Random().Next(1, 4) == 2)
            {
                TypeArmor = true;
            }
            else if (new Random().Next(1, 4) == 3)
            {
                TypeMissile = true;
            }
            if (TypeHealth)
                PlusHealth = (new Random().Next(0, 3) % 3 + 1) * 25;
            else if(TypeMissile)
                PlusMissile = new Random().Next(1, 4);
            else if(TypeArmor)
                PlusArmor = (new Random().Next(0, 3) % 3 + 2) * 25;
        }

        private void LoadAssets()
        {
            ParachuteBox = Resources.BoxWithParachute;
            Box = Resources.Box;
            Heart = Resources.heart;
            Missile = Resources.missile;
            Armor = Resources.armor;
        }

        public void Move()
        {
            if (Y + 250 < 565)
                Y += 15;
            else if (Y + 250 >= 565)
            {
                Y = 565 - 240;
                _presentLanded = true;
            }
            if (X + 100 < 0)
                OutOfScreen = true;
            if (PresentTimer.ElapsedMilliseconds > 20000)
            {
                PresentTimer.Stop();
                IsDead = true;
            }
        }

        public void Draw(Graphics g)
        {
            if (!_presentLanded)
                g.DrawImage(ParachuteBox, X, Y, 150, 230);
            else if (_boxExploded)
            {
                IsBox = false;
                if (TypeHealth)
                    g.DrawImage(Heart, X + 40, Y + 170, 40, 40);
                else if (TypeMissile)
                    g.DrawImage(Missile, X + 40, Y + 170, 40, 40);
                else
                    g.DrawImage(Armor, X + 40, Y + 170, 40, 40);
                TextRenderer.DrawText(g, (20 - PresentTimer.ElapsedMilliseconds/1000).ToString(), new Font("Century Gothic",9.25f), new Point((int) (X + 50), (int)Y + 150), Color.Red, Color.Transparent);
            }
            else
            {
                g.DrawImage(Box, X + 40, Y + 160, 80, 80);
                IsBox = true;
            }
        }

        public bool Collision(Ammunition ammunition)
        {
            if (ammunition.CenterPoint.X > X + 30 && ammunition.CenterPoint.X < X + 140 &&
                ammunition.CenterPoint.Y < Y + 250 && ammunition.CenterPoint.Y > Y + 150 && IsBox)
            {
                if (ammunition.IsBullet )
                {
                    var bullet = ammunition as Bullet;
                    if (bullet != null && bullet.FiredFromHero)
                    {
                        _boxExploded = true;
                        PresentTimer.Start();
                        ammunition.DidHit = true;
                        return true;
                    }
                }
                else
                {
                    _boxExploded = true;
                    PresentTimer.Start();
                    ammunition.DidHit = true;
                    return true;
                }
            }
            return false;
        }

        public bool PickUpPresent(int x, int y)
        {
            if (x > X + 30 && x < X + 140 && y < Y + 250 && y > Y + 150)
            {
                if (_presentLanded && _boxExploded)
                {
                    IsDead = true;
                    return true;
                }
            }
            return false;
        }

        #region IDisposable Support
        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Box.Dispose();
                    Heart.Dispose();
                    Missile.Dispose();
                    ParachuteBox.Dispose();
                    Armor.Dispose();
                }
                _disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
