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
    public class Bullet:Ammunition, IDisposable
    {
        public bool FiredFromHero;

        public Bullet(int _XmousePosition,int _YmousePosition,float _XfiredFrom,float _YfiredFrom)
        {
            LoadAssets();
            X = _XfiredFrom;
            Y = _YfiredFrom;
            Angle = CalculateAngle(_XmousePosition, _YmousePosition, _XfiredFrom, _YfiredFrom);
            Width = Height = 20;
            Velocity = 50;
            HorizontalGradient = (float)(Math.Cos(Angle) * Velocity);
            VerticalGradient = (float)(Math.Sin(Angle) * Velocity);
            IsBullet = true;
        }

        protected override void LoadAssets()
        {
            AmmoImage = Properties.Resources.bullet;
        }

        
    }
}
