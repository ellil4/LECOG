using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;
namespace LECOG
{
    public class LECOGCommon
    {
        public int CanvasSizeX = 779;
        public int CanvasSizeY = 600;

        public static String GetExeLoc()
        {
            return AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        }
    }
}
