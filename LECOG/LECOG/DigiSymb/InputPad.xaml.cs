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

namespace LECOG.DigiSymb
{
    /// <summary>
    /// InputPad.xaml 的互動邏輯
    /// </summary>
    public partial class InputPad : UserControl
    {
        public List<TokElem> mElems;
        public delegate void MouseUpDele(int btnIden);
        public static int[] mArrScheme = { 1, 6, 8, 7, 5, 3, 4, 9, 2 };
        public MouseUpDele mfMouseUp = null;

        public static String GetPicFileName(int index)
        {
            return "ds" + index;
        }

        public InputPad()
        {
            InitializeComponent();

            mElems = new List<TokElem>();
            mElems.Add(tokElem1);
            mElems.Add(tokElem2);
            mElems.Add(tokElem3);
            mElems.Add(tokElem4);
            mElems.Add(tokElem5);
            mElems.Add(tokElem6);
            mElems.Add(tokElem7);
            mElems.Add(tokElem8);
            mElems.Add(tokElem9);

            for (int i = 0; i < mElems.Count; i++)
            {
                mElems[i].SetPicture(
                    (System.Drawing.Bitmap)
                    Properties.Resources.ResourceManager.GetObject(
                    GetPicFileName(mArrScheme[i])));

                mElems[i].mTokIden = mArrScheme[i];

                mElems[i].MouseEnter += new MouseEventHandler(InputPad_MouseEnter);
                mElems[i].MouseLeave += new MouseEventHandler(InputPad_MouseLeave);
                mElems[i].MouseDown += new MouseButtonEventHandler(InputPad_MouseDown);
                mElems[i].MouseUp += new MouseButtonEventHandler(InputPad_MouseUp);
            }
        }

        void InputPad_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mfMouseUp(((TokElem)sender).mTokIden);
        }

        void InputPad_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TokElem elem = (TokElem)sender;
            elem.SetDarkHightLight();
        }

        void InputPad_MouseLeave(object sender, MouseEventArgs e)
        {
            TokElem elem = (TokElem)sender;
            elem.UnSetHighLight();
        }

        void InputPad_MouseEnter(object sender, MouseEventArgs e)
        {
            TokElem elem = (TokElem)sender;
            elem.SetHighLight();
        }
    }
}
