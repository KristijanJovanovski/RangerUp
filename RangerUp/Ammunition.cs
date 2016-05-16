using System;
using System.Drawing;

namespace RangerUp
{
    public abstract class Ammunition: IDisposable
    {
        public readonly int StandingLocation = 565;

        public float X { get; set; }
        public float Y { get; set; }

        public int Width;
        public int Height;
        public double Angle;
        public float Velocity;
        public PointF CenterPoint { get; set; }
        public float VerticalGradient { get; set; }
        public float HorizontalGradient { get; set; }

        public Image AmmoImage { get; set; }

        public bool OutOfScreen;
        public bool DidHit;
        public bool IsBullet;
        protected bool Right;
        protected bool Top;

        public void Move()
        {
            if (Right) X += HorizontalGradient;
            else X -= HorizontalGradient;

            if (Top) Y += -VerticalGradient;
            else Y += VerticalGradient;

            CenterPoint = new PointF(X + ((float)Width / 2), Y + ((float)Height / 2));
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(RotateImage(AmmoImage), X, Y);
        }

        public bool Collision(int width,int height)
        {
            if (CenterPoint.X > width || CenterPoint.X < 0 || CenterPoint.Y > StandingLocation || CenterPoint.Y < 0)
            {
                OutOfScreen = true;
                return true;
            }
            return false;
        }

        private Bitmap RotateImage(Image b)
        {
            Bitmap returnBitmap = new Bitmap(Width, Height);
            using (Graphics g = Graphics.FromImage(returnBitmap))
            {
                g.TranslateTransform((float)Width / 2, (float)Height / 2);
                if (Top && Right)
                    g.RotateTransform((float)(90 - Angle * (180 / Math.PI)));
                else if (!Top && Right)
                    g.RotateTransform((float)(90 + Angle * (180 / Math.PI)));
                else if (Top && !Right)
                    g.RotateTransform((float)(270 + Angle * (180 / Math.PI)));
                else
                    g.RotateTransform((float)(270 - Angle * (180 / Math.PI)));
                g.TranslateTransform(-(float)Width / 2, -(float)Height / 2);
                g.DrawImage(b, new Point(0, 0));
                g.Dispose();
            }
            return returnBitmap;
        }

        protected double CalculateAngle(int xmousePosition, int ymousePosition, float xfiredFrom, float yfiredFrom)
        {
            float h = 0, v = 0;

            if (xfiredFrom < xmousePosition)
            {
                h = xmousePosition - xfiredFrom;
                Right = true;
            }
            else if (xfiredFrom > xmousePosition)
            {
                h = xfiredFrom - xmousePosition;
                Right = false;
            }
            if (yfiredFrom < ymousePosition)
            {
                v = ymousePosition - yfiredFrom;
                Top = false;
            }
            else if (yfiredFrom > ymousePosition)
            {
                v = yfiredFrom - ymousePosition;
                Top = true;
            }
            return Math.Atan(v / h);
        }

        protected virtual void LoadAssets(){ }
        #region IDisposable Support
        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    AmmoImage.Dispose();
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
