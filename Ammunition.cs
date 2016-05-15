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
    public abstract class Ammunition: IDisposable
    {

        public readonly int _boxLoc = 565;

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

        public bool outOfScreen;
        public bool didHit;
        public bool IsBullet;
        protected bool right;
        protected bool top;

        public void Move()
        {
            if (right) X += HorizontalGradient;
            else X -= HorizontalGradient;

            if (top) Y += -VerticalGradient;
            else Y += VerticalGradient;

            CenterPoint = new PointF(X + ((float)Width / 2), Y + ((float)Height / 2));
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(RotateImage(AmmoImage), X, Y);
        }

        public bool Collision(int _width,int _height)
        {
            if (CenterPoint.X > _width || CenterPoint.X < 0 || CenterPoint.Y > _boxLoc || CenterPoint.Y < 0)
            {
                outOfScreen = true;
                return true;
            }
            return false;
        }

        protected Bitmap RotateImage(Image b)
        {
            Bitmap returnBitmap = new Bitmap(Width, Height);
            using (Graphics g = Graphics.FromImage(returnBitmap))
            {
                g.TranslateTransform((float)Width / 2, (float)Height / 2);
                if (top && right)
                    g.RotateTransform((float)(90 - Angle * (180 / Math.PI)));
                else if (!top && right)
                    g.RotateTransform((float)(90 + Angle * (180 / Math.PI)));
                else if (top && !right)
                    g.RotateTransform((float)(270 + Angle * (180 / Math.PI)));
                else
                    g.RotateTransform((float)(270 - Angle * (180 / Math.PI)));
                g.TranslateTransform(-(float)Width / 2, -(float)Height / 2);
                g.DrawImage(b, new Point(0, 0));
                g.Dispose();
            }
            return returnBitmap;
        }

        protected double CalculateAngle(int _XmousePosition, int _YmousePosition, float _XfiredFrom, float _YfiredFrom)
        {
            float h = 0, v = 0;

            if (_XfiredFrom < _XmousePosition)
            {
                h = _XmousePosition - _XfiredFrom;
                right = true;
            }
            else if (_XfiredFrom > _XmousePosition)
            {
                h = _XfiredFrom - _XmousePosition;
                right = false;
            }
            if (_YfiredFrom < _YmousePosition)
            {
                v = _YmousePosition - _YfiredFrom;
                top = false;
            }
            else if (_YfiredFrom > _YmousePosition)
            {
                v = _YfiredFrom - _YmousePosition;
                top = true;
            }
            return Math.Atan(v / h);
        }

        protected virtual void LoadAssets(){ }
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    AmmoImage.Dispose();
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
