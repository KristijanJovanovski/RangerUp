using System;
using System.Collections.Generic;
using System.Drawing;
using RangerUp.Properties;

namespace RangerUp
{
    public class AtomBomb:IDisposable
    {
        readonly int _standingLocation = 565;

        public float X { get; set; }
        public float Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public PointF CenterPoint;

        public Image AtomBombImage { get; set; }

        public bool IsDead { get; set; }
        public bool IsExploding { get; set; }
        private List<Image> _explosionImages;
        private int _indexExplosionImages;
        

        public AtomBomb()
        {
            LoadAssets();
            Width = 80;
            Height = 80;
            IsDead = IsExploding = false;
        }

        private void LoadAssets()
        {
            AtomBombImage = Resources.atomicBomb;
            _explosionImages = new List<Image>();
            _explosionImages.Add(Resources._1e);
            _explosionImages.Add(Resources._1e);
            _explosionImages.Add(Resources._1e);
            _explosionImages.Add(Resources._2e);
            _explosionImages.Add(Resources._2e);
            _explosionImages.Add(Resources._2e);
            _explosionImages.Add(Resources._3e);
            _explosionImages.Add(Resources._3e);
            _explosionImages.Add(Resources._3e);
            _explosionImages.Add(Resources._4e);
            _explosionImages.Add(Resources._4e);
            _explosionImages.Add(Resources._4e);
        }

        public void Move()
        {
            if(Y + Height < _standingLocation && !IsExploding)
                Y += 15;
            else
            {
                Y = _standingLocation - Height;
                IsExploding = true;
            }
            if (IsExploding)
            {
                if (_indexExplosionImages < _explosionImages.Count)
                    AtomBombImage = _explosionImages[_indexExplosionImages++];
                else
                    IsDead = true;
            }
            //  Y + Hight because it is needed as center of explosion, hence the bottom point is logical.
            CenterPoint = new PointF(X+((float)Width/2),Y+(Height));
        }

        public void Draw(Graphics g)
        {
            if (IsExploding)
                g.DrawImage(AtomBombImage, X - 55, Y - 120, 200, 200);
            else
                g.DrawImage(AtomBombImage, X, Y, Width, Height);
        }

        #region IDisposable Support
        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    AtomBombImage.Dispose();
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
