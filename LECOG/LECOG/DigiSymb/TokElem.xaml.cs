using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;

namespace LECOG.DigiSymb
{
    /// <summary>
    /// TokElem.xaml 的互動邏輯
    /// </summary>
    public partial class TokElem : UserControl
    {
        public int mTokIden = -1;

        public TokElem()
        {
            InitializeComponent();
        }

        public void SetHighLight()
        {
            this.Background = new SolidColorBrush(Color.FromRgb(134, 217, 255));
        }

        public void UnSetHighLight()
        {
            this.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }

        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);
        private IntPtr mIPtr = IntPtr.Zero;

        public void SetPicture(System.Drawing.Bitmap bmp)
        {
            if (bmp != null)
            {
                if (mIPtr != IntPtr.Zero)
                {
                    DeleteObject(mIPtr);
                    mIPtr = IntPtr.Zero;
                }

                mIPtr = bmp.GetHbitmap();

                amImage.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    mIPtr, IntPtr.Zero, System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(bmp.Width, bmp.Height));
            }
            else
            {
                amImage.Source = null;
            }
        }

        public void SetDarkHightLight()
        {
            this.Background = new SolidColorBrush(Color.FromRgb(104, 197, 235));
        }

        public void SetText(String text)
        {
            amLabel.Content = text;
        }

        ~TokElem()
        {
            if (mIPtr != IntPtr.Zero)
            {
                DeleteObject(mIPtr);
                mIPtr = IntPtr.Zero;
            }
        }
    }
}
