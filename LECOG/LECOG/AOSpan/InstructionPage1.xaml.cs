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
using System.Drawing;
using System.Runtime.InteropServices;


namespace LECOG.AOSpan
{
    /// <summary>
    /// Instruction1.xaml 的互動邏輯
    /// </summary>
    public partial class InstructionPage1 : UserControl
    {
        public IntPtr[] mIPtrs;

        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);

        public InstructionPage1()
        {
            InitializeComponent();

            mIPtrs = new IntPtr[4];
            mIPtrs[0] = Properties.Resources.ao1.GetHbitmap();
            mIPtrs[1] = Properties.Resources.ao2.GetHbitmap();
            mIPtrs[2] = Properties.Resources.ao3.GetHbitmap();
            mIPtrs[3] = Properties.Resources.ao4.GetHbitmap();
            
            image1.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    mIPtrs[0], IntPtr.Zero, System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(
                    Properties.Resources.ao1.Width, Properties.Resources.ao1.Height));

            image2.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    mIPtrs[1], IntPtr.Zero, System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(
                    Properties.Resources.ao2.Width, Properties.Resources.ao2.Height));

            image3.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    mIPtrs[2], IntPtr.Zero, System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(
                    Properties.Resources.ao3.Width, Properties.Resources.ao3.Height));

            image4.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    mIPtrs[3], IntPtr.Zero, System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(
                    Properties.Resources.ao4.Width, Properties.Resources.ao4.Height));
        }

        ~InstructionPage1()
        {
            for (int i = 0; i < mIPtrs.Length; i++)
            {
                DeleteObject(mIPtrs[i]);
                mIPtrs[i] = IntPtr.Zero;
            }
        }
    }
}
