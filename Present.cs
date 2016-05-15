using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RangerUp
{
    public class Present:IDisposable
    {
        public Image ParachuteBox { get; set; }
        public Image Box { get; set; }
        public Image Heart { get; set; }
        public Image Missile { get; set; }
        public Image Armor { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public Point CenterPosition { get; set; }
        public bool IsBox { get; set; }
        public bool TypeHealth { get; set; }
        public bool TypeMissile { get; set; }
        public bool TypeArmor { get; set; }
        public int PlusHealth { get; set; }
        public int PlusMissile { get; set; }
        public int PlusArmor { get; set; }
        private bool presentLanded;
        private bool boxExploded;
        public bool isDead;
        public bool outOfScreen;
        public HiResTimer PresentTimer { get; set; }

        public Present()
        {
            PresentTimer = new HiResTimer();
            LoadAssets();
            GeneratePresent();
        }

        private void GeneratePresent()
        {
            switch (new Random().Next(1,4))
            {
                case 1:
                    TypeHealth = true;
                    break;
                case 2:
                    TypeArmor = true;
                    break;
                case 3:
                    TypeMissile = true;
                    break;
                default:
                    break;
            }
            if (TypeHealth)
                PlusHealth = (new Random().Next(0, 3) % 3 + 1) * 25;
            else if(TypeMissile)
                PlusMissile = new Random().Next(1, 4);
            else
                PlusArmor = (new Random().Next(0, 3) % 3 + 2) * 25;
        }

        private void LoadAssets()
        {
            ParachuteBox = Properties.Resources.BoxWithParachute;
            Box = Properties.Resources.Box;
            Heart = Properties.Resources.heart;
            Missile = Properties.Resources.missile;
            Armor = Properties.Resources.armor;
        }

        public void Move()
        {
            if (Y + 250 < 565)
                Y += 15;
            else if (Y + 250 >= 565)
            {
                Y = 565 - 240;
                presentLanded = true;
            }
            if (X + 100 < 0)
                outOfScreen = true;
            if (PresentTimer.ElapsedMilliseconds > 20000)
            {
                PresentTimer.Stop();
                isDead = true;
            }
        }

        public void Draw(Graphics g)
        {
            if (!presentLanded)
                g.DrawImage(ParachuteBox, X, Y, 150, 230);
            else if (boxExploded)
            {
                IsBox = false;
                if (TypeHealth)
                    g.DrawImage(Heart, X + 40, Y + 170, 40, 40);
                else if (TypeMissile)
                    g.DrawImage(Missile, X + 40, Y + 170, 40, 40);
                else
                    g.DrawImage(Armor, X + 40, Y + 170, 40, 40);
                TextRenderer.DrawText(g, (20 - PresentTimer.ElapsedMilliseconds/1000).ToString(), Form1.DefaultFont, new Point((int) (X + 50), (int)Y + 150), Color.Red, Color.Transparent);
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
                    if ((ammunition as Bullet).FiredFromHero)
                    {
                        boxExploded = true;
                        PresentTimer.Start();
                        ammunition.didHit = true;
                        return true;
                    }
                }
                else
                {
                    boxExploded = true;
                    PresentTimer.Start();
                    ammunition.didHit = true;
                    return true;
                }
            }
            return false;
        }

        public bool PickUpPresent(int x, int y)
        {
            if (x > X + 30 && x < X + 140 && y < Y + 250 && y > Y + 150)
            {
                if (presentLanded && boxExploded)
                {
                    isDead = true;
                    return true;
                }
            }
            return false;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Box.Dispose();
                    Heart.Dispose();
                    Missile.Dispose();
                    ParachuteBox.Dispose();
                    Armor.Dispose();
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
