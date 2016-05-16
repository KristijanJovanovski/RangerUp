using System;
using RangerUp.Properties;

namespace RangerUp
{
    public class Missile:Ammunition
    {
        public Missile(int xmousePosition, int ymousePosition, float xfiredFrom, float yfiredFrom,float velocity)
        {
            LoadAssets();
            X = xfiredFrom-40;
            Y = yfiredFrom-40;
            Angle = CalculateAngle(xmousePosition, ymousePosition, xfiredFrom, yfiredFrom);
            Width = Height = 80;
            Velocity = velocity;
            HorizontalGradient = (float)(Math.Cos(Angle) * Velocity);
            VerticalGradient = (float)(Math.Sin(Angle) * Velocity);
        }

        protected sealed override void LoadAssets()
        {
            AmmoImage = Resources.missile;
        }
    }
}
