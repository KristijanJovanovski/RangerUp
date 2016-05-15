using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using WMPLib;

namespace RangerUp
{
    public class AtomBomb:IDisposable
    {
        readonly int _boxLoc = 565;
        public float X { get; set; }
        public float Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Image AtomBombImage { get; set; }
        public bool isDead { get; set; }
        public bool isExploding { get; set; }
        private List<Image> explosionImages;
        private int indexExplosionImages = 0;
        public PointF centerPoint;

        public AtomBomb()
        {
            LoadAssets();
            Width = 80;
            Height = 80;
            isDead = isExploding = false;
        }

        private void LoadAssets()
        {
            AtomBombImage = Properties.Resources.atomicBomb;
            explosionImages = new List<Image>();
            explosionImages.Add(Properties.Resources._1e);
            explosionImages.Add(Properties.Resources._1e);
            explosionImages.Add(Properties.Resources._1e);
            explosionImages.Add(Properties.Resources._2e);
            explosionImages.Add(Properties.Resources._2e);
            explosionImages.Add(Properties.Resources._2e);
            explosionImages.Add(Properties.Resources._3e);
            explosionImages.Add(Properties.Resources._3e);
            explosionImages.Add(Properties.Resources._3e);
            explosionImages.Add(Properties.Resources._4e);
            explosionImages.Add(Properties.Resources._4e);
            explosionImages.Add(Properties.Resources._4e);
        }

        public void Move()
        {
            if(Y + Height < _boxLoc && !isExploding)
                Y += 15;
            else
            {
                Y = _boxLoc - Height;
                isExploding = true;
            }
            if (isExploding)
            {
                if (indexExplosionImages < explosionImages.Count)
                    AtomBombImage = explosionImages[indexExplosionImages++];
                else
                    isDead = true;
            }
            //  Y + Hight because it is needed as center of explosion, hence the bottom point is logical.
            centerPoint = new PointF(X+((float)Width/2),Y+(Height));
        }

        public void Draw(Graphics g)
        {
            if (isExploding)
                g.DrawImage(AtomBombImage, X - 55, Y - 120, 200, 200);
            else
                g.DrawImage(AtomBombImage, X, Y, Width, Height);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    AtomBombImage.Dispose();
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
