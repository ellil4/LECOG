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
    /// Instruction.xaml 的互動邏輯
    /// </summary>
    public partial class Instruction : UserControl
    {
        public IntPtr mPtr;

        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);

        public DigiSymbRunner mRunner;

        public Instruction(DigiSymbRunner runner)
        {
            InitializeComponent();
            mPtr = Properties.Resources.DSInstruction.GetHbitmap();
            image1.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    mPtr, IntPtr.Zero, System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(
                    Properties.Resources.DSInstruction.Width, 
                    Properties.Resources.DSInstruction.Height));
            mRunner = runner;
        }

        ~Instruction()
        {
            DeleteObject(mPtr);
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mRunner.PracStart();
        }
    }
}
