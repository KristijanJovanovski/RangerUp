using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RangerUp
{
    public class Plane:IDisposable
    {
        
        public float X { get; set; }
        public float Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Image PlaneImage { get; set; }
        public int PlaneHealth { get; set; }
        public PointF heroPoint { get; set; }
        public PointF centerPoint { get; set; }
        public bool isDead { get; set; }
        public bool outOfScreen;
        public bool isExploding { get; set; }
        private List<Image> explosionImages;
        private int indexExplosionImages = 0;
        public HiResTimer AtomBombTimer { get; set; }
        public AtomBomb atomBomb;
        public HiResTimer BulletTimer { get; set; }
        public Bullet bullet;
        public Present present;
        public int minAtomBombTimeSpawn;
        public int maxAtomBombTimeSpawn;
        public int minBulletTimeSpawn;
        public int maxBulletTimeSpawn;
        public int PlaneDropPresent;

        public Plane()
        {
            LoadAssets();
            Width = 200;
            Height = 100;
            isDead = isExploding = false;
            PlaneHealth = 100;
            AtomBombTimer = new HiResTimer();
            AtomBombTimer.Start();
            BulletTimer = new HiResTimer();
            BulletTimer.Start();
        }

        private void LoadAssets()
        {
            PlaneImage = new Random().Next()%2==0 ? Properties.Resources.PlaneRed : Properties.Resources.PlaneGray;
            explosionImages = new List<Image>();
            explosionImages.Add(Properties.Resources.inAirExplosion01);
            explosionImages.Add(Properties.Resources.inAirExplosion02);
            explosionImages.Add(Properties.Resources.inAirExplosion03);
            explosionImages.Add(Properties.Resources.inAirExplosion04);
            explosionImages.Add(Properties.Resources.inAirExplosion05);
            explosionImages.Add(Properties.Resources.inAirExplosion06);
            explosionImages.Add(Properties.Resources.inAirExplosion07);
            explosionImages.Add(Properties.Resources.inAirExplosion08);
            explosionImages.Add(Properties.Resources.inAirExplosion09);
            explosionImages.Add(Properties.Resources.inAirExplosion10);
            explosionImages.Add(Properties.Resources.inAirExplosion11);
        }

        private void DropBomb()
        {
            if ((AtomBombTimer.ElapsedMilliseconds > (new Random().Next(minAtomBombTimeSpawn, maxAtomBombTimeSpawn))) && !isExploding && !isDead)
            {
                AtomBombTimer.Stop();
                AtomBombTimer.Start();
                atomBomb = new AtomBomb();
                atomBomb.X = centerPoint.X;
                atomBomb.Y = centerPoint.Y;
            }
        }

        private void FireMachineGun()
        {
            if ((BulletTimer.ElapsedMilliseconds > (new Random().Next(minBulletTimeSpawn, maxBulletTimeSpawn))) &&
                !isExploding && !isDead)
            {
                BulletTimer.Stop();
                BulletTimer.Start();
                bullet = new Bullet((int) heroPoint.X, (int) heroPoint.Y, centerPoint.X, centerPoint.Y);
                bullet.Velocity = 10;
            }
        }

        private void DropPresent()
        {
            if (new Random().Next(0, PlaneDropPresent) == 0)
            {
                present = new Present()
                {
                    X = centerPoint.X,
                    Y = centerPoint.Y
                };
            }
        }

        public void Move(PointF heroCenterPoint)
        {
            centerPoint= new PointF(X+((float)Width/2),Y + ((float)Height/2));
            CheckHealth();
            heroPoint = heroCenterPoint;
            if (isExploding)
            {
                if(indexExplosionImages < explosionImages.Count)
                    PlaneImage = explosionImages[indexExplosionImages++];
                else
                    isDead = true;
            }
            else
            {
                X -= 15;
            }

            if (isDead)
                DropPresent();
            else
            {
                DropBomb();
                FireMachineGun();
            }
        }

        public void Draw(Graphics g)
        {
            if(isExploding)
                g.DrawImage(PlaneImage, X, Y, Width, Height+50);
            else
                g.DrawImage(PlaneImage,X,Y,Width,Height);

            g.DrawRectangle(new Pen(new SolidBrush(Color.Red), 2),
                new Rectangle((int)X + 50, (int)Y - 20, 100, 10));
            g.FillRectangle(new SolidBrush(Color.Yellow),
                new RectangleF(X + 50, Y - 20, ((float)100) * ((float)PlaneHealth / 100), 10));
        }

        public bool Collision(Ammunition ammunition)
        {
            if (ammunition.CenterPoint.X > X && ammunition.CenterPoint.X < X + Width &&
                ammunition.CenterPoint.Y < Y + Height && ammunition.CenterPoint.Y > Y)
            {
                if (ammunition.IsBullet)
                {
                    if ((ammunition as Bullet).FiredFromHero)
                    {
                        PlaneHealth = PlaneHealth - 10;
                        ammunition.didHit = true;
                        return true;
                    }
                }
                else
                {
                    PlaneHealth = PlaneHealth - 50;
                    ammunition.didHit = true;
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
                isExploding = true;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    PlaneImage.Dispose();
                    foreach (var image in explosionImages)
                    {
                        image.Dispose();
                    }
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
