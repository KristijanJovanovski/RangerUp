using System.Drawing;
using RangerUp.Properties;

namespace RangerUp
{
    class BackgroundLoop
    {
        private Image BackgroundCoverImage { get; set; }
        public int XPosition { get; set; }
        public int FormWidth { get; set; }
        private int FormHeight { get; set; }

        public BackgroundLoop()
        {
            BackgroundCoverImage = Resources.BackgroundCover;
            XPosition = 0;
        }
        public void Draw(Graphics g)
        {
            g.DrawImage(BackgroundCoverImage, XPosition, 0);
            g.DrawImage(BackgroundCoverImage, XPosition - BackgroundCoverImage.Width, 0);

            if ((BackgroundCoverImage.Width + XPosition) <= FormWidth)
                XPosition = FormWidth;
            }
    }
}
