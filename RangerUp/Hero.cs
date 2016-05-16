using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using RangerUp.Properties;

namespace RangerUp
{
    public class Hero:IDisposable
    {
        readonly int _standingLocation = 565;
        public readonly int G = 23;

        public bool IsDead { get; set; }
        public bool Right;
        public bool Left;
        public bool Jump;

        public int Force;

        public float HeroHealth { get; set; }
        public  float HeroArmor { get; set; }

        public float X { get; set; }
        public float Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public PointF MouseLocation;
        public PointF CenterPoint;

        private Image _heroImage;
        private List<Image> _heroImagesRight;
        private List<Image> _heroImagesLeft;
        private int _indexRightImages;
        private int _indexLeftImages;

        public HiResTimer BulletReloadTimer;
        public HiResTimer MissileReloadTimer;

        public bool ReloadingBullets;
        public bool ReloadingMissiles;
        private readonly int _clipSize = 30;

        private int _missileCounter;
        private int _bulletsInClip;
        private float _fillAngleBullet;
        private float _fillAngleMissile;

        public Bullet Bullet;
        public Missile Missile;

        public Hero()
        {
            Left = Right = Jump = false;
            Width = 80;
            Height = 80;
            X = 50;
            Y = _standingLocation-Height;
            LoadAssets();
            HeroHealth = 100;
            HeroArmor = 100;
            BulletReloadTimer = new HiResTimer();
            MissileReloadTimer = new HiResTimer();
            _fillAngleBullet = _fillAngleMissile = 360;
            _bulletsInClip = _clipSize;
            _missileCounter = 10;
        }

        private void CheckHealth()
        {
            if (HeroHealth < 0)
                IsDead = true;
        }

        public void Move()
        {
            if (MouseLocation.X < CenterPoint.X)
            {
                if (_indexLeftImages == _heroImagesLeft.Count) _indexLeftImages = 0;
                _heroImage = _heroImagesLeft[_indexLeftImages++];
            }
            else if (MouseLocation.X > CenterPoint.X)
            {
                if (_indexRightImages == _heroImagesRight.Count) _indexRightImages = 0;
                _heroImage = _heroImagesRight[_indexRightImages++];
            }
            if (Left){X -= 15;}
            else if (Right){X += 15;}
            else
            {
                if (_indexLeftImages > _indexRightImages)
                    _heroImage = _heroImagesLeft[0];
                else if (_indexLeftImages < _indexRightImages)
                    _heroImage = _heroImagesRight[0];
                _indexLeftImages = _indexRightImages = 0;
            }
            if (Jump)
            {
                Y -= Force;
                Force -=1;
            }
            if (Y + Height >= _standingLocation )
            {
                Jump = false;
                Y = _standingLocation - Height;
            }
            else{Y += 5;}
            if (X < 0) X = 0;
            CenterPoint = new PointF(X+((float)Width/2),Y+((float)Height/2));
            

            RunBulletClock();
            RunMissileClock();
            CheckHealth();
        }

        private void RunBulletClock()
        {
            if (_bulletsInClip == 0 && !ReloadingBullets)
            {
                Reload();
            }

            if (BulletReloadTimer.IsActive() && BulletReloadTimer.ElapsedMilliseconds >= 100)
            {
                BulletReloadTimer.Stop();
                BulletReloadTimer.Start();
                _fillAngleBullet += 12;
                _bulletsInClip++;
            }
            if (_bulletsInClip == _clipSize)
            {
                BulletReloadTimer.Stop();
                ReloadingBullets = false;
            }
        }

        private void RunMissileClock()
        {
            if (MissileReloadTimer.IsActive() && MissileReloadTimer.ElapsedMilliseconds >= 50)
            {
                MissileReloadTimer.Stop();
                MissileReloadTimer.Start();
                _fillAngleMissile += 10;
            }
            if (_fillAngleMissile >= 360)
            {
                MissileReloadTimer.Stop();
                ReloadingMissiles = false;
            }
        }

        public void Draw(Graphics g)
        {
            g.DrawRectangle(new Pen(Color.OrangeRed), new Rectangle(800, 620, 200, 20));
            g.FillRectangle(new SolidBrush(Color.Green), new RectangleF(
                800, 620, 200 * (HeroHealth / 100), 20));
            TextRenderer.DrawText(g, "HEALTH: " + Math.Floor(HeroHealth),
                new Font("Century Gothic", 9.25f), new Rectangle(800, 600, 200, 20), Color.White);

            g.DrawRectangle(new Pen(Color.DarkMagenta), new Rectangle(800, 660, 200, 20));
            g.FillRectangle(new SolidBrush(Color.RoyalBlue), new RectangleF(
                800, 660, 200 * (HeroArmor / 100), 20));
            TextRenderer.DrawText(g, "ARMOR: " + Math.Floor(HeroArmor),
               new Font("Century Gothic",9.25f), new Rectangle(800, 640, 200, 20), Color.White);


            g.DrawImage(_heroImage,X,Y);

            DrawClipReload(g);
            DrawMissileReload(g);

        }

        private void LoadAssets()
        {
            _heroImagesRight = new List<Image>();
            _heroImagesLeft = new List<Image>();
            _indexRightImages = _indexLeftImages = 0;

            _heroImage = Resources._1;
            _heroImagesRight.Add(Resources._1);
            _heroImagesRight.Add(Resources._2);
            _heroImagesRight.Add(Resources._3);
            _heroImagesRight.Add(Resources._4);
            _heroImagesRight.Add(Resources._5);
            _heroImagesRight.Add(Resources._6);
            _heroImagesRight.Add(Resources._7);

            _heroImagesLeft.Add(Resources._1m);
            _heroImagesLeft.Add(Resources._2m);
            _heroImagesLeft.Add(Resources._3m);
            _heroImagesLeft.Add(Resources._4m);
            _heroImagesLeft.Add(Resources._5m);
            _heroImagesLeft.Add(Resources._6m);
            _heroImagesLeft.Add(Resources._7m);
        }

        public void FireBurst(Point mousePoint)
        {
            MouseLocation = mousePoint;
            Bullet = null;
            if (!ReloadingBullets)
            {
                Bullet = new Bullet(mousePoint.X, mousePoint.Y, CenterPoint.X, CenterPoint.Y - 10);
                Bullet.Velocity = 15;
                Bullet.FiredFromHero = true;
                _bulletsInClip--;
                _fillAngleBullet = (360/_clipSize)*_bulletsInClip;
            }
        }

        public void Reload()
        {
            if (_bulletsInClip >= _clipSize) return;
            BulletReloadTimer.Start();
            ReloadingBullets = true;
        }

        public void FireMissile(Point mousePoint)
        {
            MouseLocation = mousePoint;
            Missile = null;
            if (ReloadingMissiles || _missileCounter <= 0) return;
            Missile = new Missile(mousePoint.X, mousePoint.Y, CenterPoint.X, CenterPoint.Y - 10, 20);
                
            _missileCounter--;
            MissileReloadTimer.Start();
            ReloadingMissiles = true;
            _fillAngleMissile = 0;
        }

        public void AssessDamage(PointF centerOfExplosion)
        {
            float scaler = 50;
            if (!(Math.Abs(CenterPoint.X - centerOfExplosion.X) <= 100)) return;
            if (Math.Abs(CenterPoint.X - centerOfExplosion.X) > 0)
            {
                scaler = scaler - Math.Abs(CenterPoint.X - centerOfExplosion.X) / 2;
            }
            HeroHealth -= scaler;
        }

        public bool Collision(Ammunition ammunition)
        {
            if (!(ammunition.CenterPoint.X > X) || !(ammunition.CenterPoint.X < X + Width) ||
                !(ammunition.CenterPoint.Y > Y) || !(ammunition.CenterPoint.Y < Y + Height)) return false;
            if (!ammunition.IsBullet) return false;
            var bullet = ammunition as Bullet;
            if (bullet == null || bullet.FiredFromHero) return false;
            HeroHealth = HeroHealth- (float)(0.5f + Math.Pow(2, -100/(100-(HeroArmor-1))))*5;
            HeroArmor -= new Random().Next(1,7);
            if (HeroArmor < 0)
                HeroArmor = 0;
            ammunition.DidHit = true;
            return true;
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
                _missileCounter += present.PlusMissile;
            }
            else
            {
                HeroArmor += present.PlusArmor;
                if (HeroArmor > 100)
                    HeroArmor = 100;
            }
        }

        private void DrawClipReload(Graphics g)
        {
            SolidBrush b = new SolidBrush(SystemColors.MenuHighlight);
            GraphicsPath path1 = new GraphicsPath();
            GraphicsPath path2 = new GraphicsPath();
            int d = 100;
            int x = 500;
            int y = 630;

            path1.AddPie(x - d / 2, y - d / 2, d, (float)d, 270, _fillAngleBullet);
            path2.AddEllipse(x - d / 4, y - d / 4, ((float)d / 2), ((float)d / 2));
            Region region = new Region(path1);
            region.Exclude(path2);
            g.FillRegion(b, region);

            if (ReloadingBullets)
            {
                TextRenderer.DrawText(g, "RELOADING...", new Font("Century Gothic", 9.25f), new Point(x - 35, y + 50), Color.White, Color.Transparent);
            }
            TextRenderer.DrawText(g, _bulletsInClip + " x", new Font("Century Gothic", 9.25f), new Point(x - 20, y - 5), Color.White, Color.Transparent);
            g.DrawImage(Resources.bullet,x,y-10,30,30);

        }

        private void DrawMissileReload(Graphics g)
        {
            var b = new SolidBrush(Color.FromKnownColor(KnownColor.OrangeRed));
            var path1 = new GraphicsPath();
            var path2 = new GraphicsPath();
            var d = 100;
            var x = 650;
            var y = 630;
            path1.AddPie(x - d / 2, y - d / 2, d, (float)d, 270, _fillAngleMissile);
            path2.AddEllipse(x - d / 4, y - d / 4, ((float)d / 2), ((float)d / 2));
            TextRenderer.DrawText(g, _missileCounter + " x", new Font("Century Gothic", 9.25f), new Point(x - 20, y - 5), Color.White, Color.Transparent);
            g.DrawImage(Resources.missile, x-5, y - 15, 30, 30);

            var region = new Region(path1);
            region.Exclude(path2);
            g.FillRegion(b, region);
        }

        #region IDisposable Support
        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    foreach (var image in _heroImagesLeft)
                    {
                        image.Dispose();
                    }
                    foreach (var image in _heroImagesRight)
                    {
                        image.Dispose();
                    }
                    _heroImage.Dispose();
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
