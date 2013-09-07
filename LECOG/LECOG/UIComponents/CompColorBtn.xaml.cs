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

namespace LECOG.UIComponents
{
    /// <summary>
    /// CompTextCasual.xaml 的互動邏輯
    /// </summary>
    public partial class CompColorBtn : UserControl
    {
        public static int HEIGHT = 53;
        public int mWidth = 0;
        public MainWindow mMW;
        public delegate void OnMouseUpFuncDele(object obj);
        public OnMouseUpFuncDele mfMouseUpFunc;
        object mObjPara;

        public CompColorBtn()
        {
            InitializeComponent();
        }

        public void Init(MainWindow mw, int width)
        {
            BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            mMW = mw;
            mWidth = width;
        }

        public void SetText(String text)
        {
            amLabel.Content = text;
            amLabel.Width = mWidth;
            this.Width = mWidth;
        }

        public String GetText()
        {
            return (String)amLabel.Content;
        }

        public void SetMouseUpFunc(OnMouseUpFuncDele func, object objPara)
        {
            mfMouseUpFunc = func;
            mObjPara = objPara;
        }

        public void SetHighLighted()
        {
            this.amLabel.Background =
                new SolidColorBrush(Color.FromRgb(134, 217, 255));
        }

        public void HighLightOff()
        {
            this.amLabel.Background =
                new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }

        public void SetDarkLight()
        {
            this.amLabel.Background =
                new SolidColorBrush(Color.FromRgb(104, 197, 235)); 
        }

        private void amLabel_MouseEnter(object sender, MouseEventArgs e)
        {
            SetHighLighted();
        }

        private void amLabel_MouseLeave(object sender, MouseEventArgs e)
        {
            HighLightOff();
        }

        private void amLabel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mfMouseUpFunc(mObjPara);
            SetHighLighted();
        }

        private void amLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetDarkLight();
        }
    }
}
