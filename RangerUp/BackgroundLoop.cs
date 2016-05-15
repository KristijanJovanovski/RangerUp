using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RangerUp
{
    class BackgroundLoop
    {
        public Image BackgroundCoverImage { get; set; }
        public int XPosition { get; set; }
        public int FormWidth { get; set; }
        public int FormHeight { get; set; }

        public BackgroundLoop()
        {
            BackgroundCoverImage = Properties.Resources.BackgroundCover;
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
