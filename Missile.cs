using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using WMPLib;

namespace RangerUp
{
    public class Missile:Ammunition
    {
        public Missile(int _XmousePosition, int _YmousePosition, float _XfiredFrom, float _YfiredFrom,float _velocity)
        {
            LoadAssets();
            X = _XfiredFrom-40;
            Y = _YfiredFrom-40;
            Angle = CalculateAngle(_XmousePosition, _YmousePosition, _XfiredFrom, _YfiredFrom);
            Width = Height = 80;
            Velocity = _velocity;
            HorizontalGradient = (float)(Math.Cos(Angle) * Velocity);
            VerticalGradient = (float)(Math.Sin(Angle) * Velocity);
        }

        protected override void LoadAssets()
        {
            AmmoImage = Properties.Resources.missile;
        }
    }
}
