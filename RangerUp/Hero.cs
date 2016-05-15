using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RangerUp
{
    public class Hero:IDisposable
    {
        
        readonly int _boxLoc = 565;
        public readonly int G = 23;

        public bool isDead { get; set; }
        public bool right;
        public bool left;
        public bool jump;

        public int Force;

        public float HeroHealth { get; set; }
        public  float HeroArmor { get; set; }

        public float X { get; set; }
        public float Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public PointF mouseLocation;
        public PointF centerPoint;

        private Image heroImage;
        private List<Image> heroImagesRight;
        private List<Image> heroImagesLeft;
        private int indexRightImages;
        private int indexLeftImages;

        public HiResTimer bulletReloadTimer;
        public HiResTimer missileReloadTimer;

        public bool ReloadingBullets;
        public bool ReloadingMissiles;
        private readonly int clipSize = 30;

        private int missileCounter;
        private int bulletsInClip;
        private float fillAngleBullet;
        private float fillAngleMissile;

        public Bullet bullet;
        public Missile missile;

        public Hero()
        {
            left = right = jump = false;
            Width = 80;
            Height = 80;
            X = 50;
            Y = _boxLoc-Height;
            LoadAssets();
            HeroHealth = 100;
            HeroArmor = 100;
            bulletReloadTimer = new HiResTimer();
            missileReloadTimer = new HiResTimer();
            fillAngleBullet = fillAngleMissile = 360;
            bulletsInClip = clipSize;
            missileCounter = 10;
        }

        private void CheckHealth()
        {
            if (HeroHealth <= 0)
                isDead = true;
        }

        public void Move()
        {
            if (mouseLocation.X < centerPoint.X)
            {
                if (indexLeftImages == heroImagesLeft.Count) indexLeftImages = 0;
                heroImage = heroImagesLeft[indexLeftImages++];
            }
            else if (mouseLocation.X > centerPoint.X)
            {
                if (indexRightImages == heroImagesRight.Count) indexRightImages = 0;
                heroImage = heroImagesRight[indexRightImages++];
            }
            if (left){X -= 15;}
            else if (right){X += 15;}
            else
            {
                if (indexLeftImages > indexRightImages)
                    heroImage = heroImagesLeft[0];
                else if (indexLeftImages < indexRightImages)
                    heroImage = heroImagesRight[0];
                indexLeftImages = indexRightImages = 0;
            }
            if (jump)
            {
                Y -= Force;
                Force -=1;
            }
            if (Y + Height >= _boxLoc )
            {
                jump = false;
                Y = _boxLoc - Height;
            }
            else{Y += 5;}
            if (X < 0) X = 0;
            centerPoint = new PointF(X+((float)Width/2),Y+((float)Height/2));
            

            RunBulletClock();
            RunMissileClock();
            CheckHealth();
        }

        private void RunBulletClock()
        {
            if (bulletsInClip == 0 && !ReloadingBullets)
            {
                Reload();
            }

            if (bulletReloadTimer.isActive() && bulletReloadTimer.ElapsedMilliseconds >= 100)
            {
                bulletReloadTimer.Stop();
                bulletReloadTimer.Start();
                fillAngleBullet += 12;
                bulletsInClip++;
            }
            if (bulletsInClip == clipSize)
            {
                bulletReloadTimer.Stop();
                ReloadingBullets = false;
            }
        }

        private void RunMissileClock()
        {
            if (missileReloadTimer.isActive() && missileReloadTimer.ElapsedMilliseconds >= 50)
            {
                missileReloadTimer.Stop();
                missileReloadTimer.Start();
                fillAngleMissile += 10;
            }
            if (fillAngleMissile >= 360)
            {
                missileReloadTimer.Stop();
                ReloadingMissiles = false;
            }
        }

        public void Draw(Graphics g)
        {
            g.DrawRectangle(new Pen(Color.OrangeRed), new Rectangle(800, 620, 200, 20));
            g.FillRectangle(new SolidBrush(Color.Green), new RectangleF(
                800, 620, ((float)200) * ((float)HeroHealth / 100), 20));
            TextRenderer.DrawText(g, "HEALTH: " + Math.Floor(HeroHealth),
                Form1.DefaultFont, new Rectangle(800, 600, 200, 20), Color.White);

            g.DrawRectangle(new Pen(Color.DarkMagenta), new Rectangle(800, 660, 200, 20));
            g.FillRectangle(new SolidBrush(Color.RoyalBlue), new RectangleF(
                800, 660, ((float)200) * ((float)HeroArmor / 100), 20));
            TextRenderer.DrawText(g, "ARMOR: " + Math.Floor(HeroArmor),
                Form1.DefaultFont, new Rectangle(800, 640, 200, 20), Color.White);


            g.DrawImage(heroImage,X,Y);

            DrawClipReload(g);
            DrawMissileReload(g);

        }

        private void LoadAssets()
        {
            heroImagesRight = new List<Image>();
            heroImagesLeft = new List<Image>();
            indexRightImages = indexLeftImages = 0;

            heroImage = Properties.Resources._1;
            heroImagesRight.Add(Properties.Resources._1);
            heroImagesRight.Add(Properties.Resources._2);
            heroImagesRight.Add(Properties.Resources._3);
            heroImagesRight.Add(Properties.Resources._4);
            heroImagesRight.Add(Properties.Resources._5);
            heroImagesRight.Add(Properties.Resources._6);
            heroImagesRight.Add(Properties.Resources._7);

            heroImagesLeft.Add(Properties.Resources._1m);
            heroImagesLeft.Add(Properties.Resources._2m);
            heroImagesLeft.Add(Properties.Resources._3m);
            heroImagesLeft.Add(Properties.Resources._4m);
            heroImagesLeft.Add(Properties.Resources._5m);
            heroImagesLeft.Add(Properties.Resources._6m);
            heroImagesLeft.Add(Properties.Resources._7m);
        }

        public void FireBurst(Point _mousePoint)
        {
            mouseLocation = _mousePoint;
            //bullet firing
            bullet = null;
            if (!ReloadingBullets)
            {
                bullet = new Bullet(_mousePoint.X, _mousePoint.Y, centerPoint.X, centerPoint.Y - 10);
                bullet.Velocity = 15;
                bullet.FiredFromHero = true;
                bulletsInClip--;
                fillAngleBullet = (360/clipSize)*bulletsInClip;
            }
        }
        public void Reload()
        {
            if (bulletsInClip < clipSize)
            {
                bulletReloadTimer.Start();
                ReloadingBullets = true;
            }
        }

        public void FireMissile(Point _mousePoint)
        {
            mouseLocation = _mousePoint;
            missile = null;
            //missile firing
            if (!ReloadingMissiles && missileCounter > 0)
            {
                missile = new Missile(_mousePoint.X, _mousePoint.Y, centerPoint.X, centerPoint.Y - 10, 20);
                
                missileCounter--;
                missileReloadTimer.Start();
                ReloadingMissiles = true;
                fillAngleMissile = 0;
            }
        }

        public void AssessDamage(PointF centerOfExplosion)
        {
            float scaler = 50;
            if (Math.Abs(centerPoint.X - centerOfExplosion.X) <= 100)
            {
                if (Math.Abs(centerPoint.X - centerOfExplosion.X) > 0)
                {
                    scaler = scaler - Math.Abs(centerPoint.X - centerOfExplosion.X) / 2;
                }
                HeroHealth -= scaler;
            }
        }

        public bool Collision(Ammunition ammunition)
        {
            if (ammunition.CenterPoint.X > X && ammunition.CenterPoint.X < X + Width &&
                ammunition.CenterPoint.Y > Y && ammunition.CenterPoint.Y < Y + Height)
            {
                if (ammunition.IsBullet)
                {
                    if (!(ammunition as Bullet).FiredFromHero)
                    {
                        HeroHealth = HeroHealth- (float)(0.5f + Math.Pow(2, -100/(100-(HeroArmor-1))))*5;
                        HeroArmor -= new Random().Next(1,7);
                        if (HeroArmor < 0)
                            HeroArmor = 0;
                        ammunition.didHit = true;
                        return true;
                    }
                }
            }
            return false;
        }

        public void PickedUpPresent(Present present)
        {
            if (present.TypeHealth)
            {
                HeroHealth += present.PlusHealth;
                if (HeroHealth > 100)
                    HeroHealth = 100;
            }
            else if (present.TypeMissile)
            {
                missileCounter += present.PlusMissile;
            }
            else
            {
                HeroArmor += present.PlusArmor;
                if (HeroArmor > 100)
                    HeroArmor = 100;
            }
        }

        public void DrawClipReload(Graphics g)
        {
            SolidBrush b = new SolidBrush(SystemColors.MenuHighlight);
            GraphicsPath path1 = new GraphicsPath();
            GraphicsPath path2 = new GraphicsPath();
            int d = 100;
            int x = 500;
            int y = 630;

            path1.AddPie((float)(x - d / 2), (float)(y - d / 2), (float)d, (float)d, 270, fillAngleBullet);
            path2.AddEllipse((float)(x - d / 4), (float)(y - d / 4), ((float)d / 2), ((float)d / 2));
            Region region = new Region(path1);
            region.Exclude(path2);
            g.FillRegion(b, region);

            if (ReloadingBullets)
            {
                TextRenderer.DrawText(g, "RELOADING...", Form1.DefaultFont, new Point(x - 35, y + 50), Color.White, Color.Transparent);
            }
            TextRenderer.DrawText(g, bulletsInClip.ToString() + " x", Form1.DefaultFont, new Point(x - 20, y - 5), Color.White, Color.Transparent);
            g.DrawImage(Properties.Resources.bullet,x,y-10,30,30);

        }

        public void DrawMissileReload(Graphics g)
        {
            SolidBrush b = new SolidBrush(Color.FromKnownColor(KnownColor.OrangeRed));
            GraphicsPath path1 = new GraphicsPath();
            GraphicsPath path2 = new GraphicsPath();
            int d = 100;
            int x = 650;
            int y = 630;
            path1.AddPie((float)(x - d / 2), (float)(y - d / 2), (float)d, (float)d, 270, fillAngleMissile);
            path2.AddEllipse((float)(x - d / 4), (float)(y - d / 4), (float)(d / 2), (float)(d / 2));
            TextRenderer.DrawText(g, missileCounter.ToString() + " x", Form1.DefaultFont, new Point(x - 20, y - 5), Color.White, Color.Transparent);
            g.DrawImage(Properties.Resources.missile, x-5, y - 15, 30, 30);

            Region region = new Region(path1);
            region.Exclude(path2);
            g.FillRegion(b, region);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var image in heroImagesLeft)
                    {
                        image.Dispose();
                    }
                    foreach (var image in heroImagesRight)
                    {
                        image.Dispose();
                    }
                    heroImage.Dispose();
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
