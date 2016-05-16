using System;
using System.Collections.Generic;
using System.Drawing;
using RangerUp.Properties;

namespace RangerUp
{
    public class Plane:IDisposable
    {
        
        public float X { get; set; }
        public float Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public PointF HeroPoint { get; set; }
        public PointF CenterPoint { get; set; }


        public Image PlaneImage { get; set; }
        public int PlaneHealth { get; set; }

        public bool IsDead { get; set; }
        public bool OutOfScreen;
        public bool IsExploding { get; set; }

        private List<Image> _explosionImages;
        private int _indexExplosionImages;

        public HiResTimer AtomBombTimer { get; set; }

        public AtomBomb AtomBomb;
        public HiResTimer BulletTimer { get; set; }
        public Bullet Bullet;
        public Present Present;

        public int MinAtomBombTimeSpawn;
        public int MaxAtomBombTimeSpawn;
        public int MinBulletTimeSpawn;
        public int MaxBulletTimeSpawn;
        public int PlaneDropPresent;

        public Plane()
        {
            LoadAssets();
            Width = 200;
            Height = 100;
            IsDead = IsExploding = false;
            PlaneHealth = 100;
            AtomBombTimer = new HiResTimer();
            AtomBombTimer.Start();
            BulletTimer = new HiResTimer();
            BulletTimer.Start();
        }

        private void LoadAssets()
        {
            PlaneImage = new Random().Next()%2==0 ? Resources.PlaneRed : Resources.PlaneGray;
            _explosionImages = new List<Image>();
            _explosionImages.Add(Resources.inAirExplosion01);
            _explosionImages.Add(Resources.inAirExplosion02);
            _explosionImages.Add(Resources.inAirExplosion03);
            _explosionImages.Add(Resources.inAirExplosion04);
            _explosionImages.Add(Resources.inAirExplosion05);
            _explosionImages.Add(Resources.inAirExplosion06);
            _explosionImages.Add(Resources.inAirExplosion07);
            _explosionImages.Add(Resources.inAirExplosion08);
            _explosionImages.Add(Resources.inAirExplosion09);
            _explosionImages.Add(Resources.inAirExplosion10);
            _explosionImages.Add(Resources.inAirExplosion11);
        }

        private void DropBomb()
        {
            if ((AtomBombTimer.ElapsedMilliseconds > (new Random().Next(MinAtomBombTimeSpawn, MaxAtomBombTimeSpawn))) && !IsExploding && !IsDead)
            {
                AtomBombTimer.Stop();
                AtomBombTimer.Start();
                AtomBomb = new AtomBomb();
                AtomBomb.X = CenterPoint.X;
                AtomBomb.Y = CenterPoint.Y;
            }
        }

        private void FireMachineGun()
        {
            if ((BulletTimer.ElapsedMilliseconds > (new Random().Next(MinBulletTimeSpawn, MaxBulletTimeSpawn))) &&
                !IsExploding && !IsDead)
            {
                BulletTimer.Stop();
                BulletTimer.Start();
                Bullet = new Bullet((int) HeroPoint.X, (int) HeroPoint.Y, CenterPoint.X, CenterPoint.Y);
                Bullet.Velocity = 10;
            }
        }

        private void DropPresent()
        {
            if (new Random().Next(0, PlaneDropPresent) == 0)
            {
                Present = new Present
                {
                    X = CenterPoint.X,
                    Y = CenterPoint.Y
                };
            }
        }

        public void Move(PointF heroCenterPoint)
        {
            CenterPoint= new PointF(X+((float)Width/2),Y + ((float)Height/2));
            CheckHealth();
            HeroPoint = heroCenterPoint;
            if (IsExploding)
            {
                if(_indexExplosionImages < _explosionImages.Count)
                    PlaneImage = _explosionImages[_indexExplosionImages++];
                else
                    IsDead = true;
            }
            else
            {
                X -= 15;
            }

            if (IsDead)
                DropPresent();
            else
            {
                DropBomb();
                FireMachineGun();
            }
        }

        public void Draw(Graphics g)
        {
            if (IsExploding)
                g.DrawImage(PlaneImage, X, Y, Width, Height + 50);
            else
            {
                g.DrawImage(PlaneImage, X, Y, Width, Height);

                g.DrawRectangle(new Pen(new SolidBrush(Color.Red), 2),
                    new Rectangle((int) X + 50, (int) Y - 20, 100, 10));
                g.FillRectangle(new SolidBrush(Color.Yellow),
                    new RectangleF(X + 50, Y - 20, 100*((float) PlaneHealth/100), 10));
            }
        }

        public bool Collision(Ammunition ammunition)
        {
            if (ammunition.CenterPoint.X > X && ammunition.CenterPoint.X < X + Width &&
                ammunition.CenterPoint.Y < Y + Height && ammunition.CenterPoint.Y > Y)
            {
                if (ammunition.IsBullet)
                {
                    var bullet = ammunition as Bullet;
                    if (bullet != null && bullet.FiredFromHero)
                    {
                        PlaneHealth = PlaneHealth - 10;
                        ammunition.DidHit = true;
                        return true;
                    }
                }
                else
                {
                    PlaneHealth = PlaneHealth - 50;
                    ammunition.DidHit = true;
                    return true;
                }
                CheckHealth();
            }
            return false;
        }

        private void CheckHealth()
        {
            if (PlaneHealth <= 0)
            {
                IsExploding = true;
            }
        }

        #region IDisposable Support
        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    PlaneImage.Dispose();
                    foreach (var image in _explosionImages)
                    {
                        image.Dispose();
                    }
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
