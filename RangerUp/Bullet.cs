using System;
using RangerUp.Properties;

namespace RangerUp
{
    public class Bullet:Ammunition
    {
        public bool FiredFromHero;

        public Bullet(int xmousePosition,int ymousePosition,float xfiredFrom,float yfiredFrom)
        {
            LoadAssets();
            X = xfiredFrom;
            Y = yfiredFrom;
            Angle = CalculateAngle(xmousePosition, ymousePosition, xfiredFrom, yfiredFrom);
            Width = Height = 20;
            Velocity = 50;
            HorizontalGradient = (float)(Math.Cos(Angle) * Velocity);
            VerticalGradient = (float)(Math.Sin(Angle) * Velocity);
            IsBullet = true;
        }

        protected sealed override void LoadAssets()
        {
            AmmoImage = Resources.bullet;
        }

        
    }
}
