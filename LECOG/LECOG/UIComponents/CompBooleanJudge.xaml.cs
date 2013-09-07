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
    /// CompBooleanJudge.xaml 的互動邏輯
    /// </summary>
    public partial class CompBooleanJudge : UserControl
    {
        public int msSizeW = 700;
        public int msSizeH = 500;

        public delegate void OnConfirm();
        public delegate void OnDeny();
        public delegate void OnFlip();

        public OnConfirm mfOnConfirm = null;
        public OnDeny mfOnDeny = null;
        public OnFlip mfOnFlip = null;

        public CompBooleanJudge()
        {
            InitializeComponent();
        }

        private void setHighLighted(object sender)
        {
            ((Label)sender).Background =
                new SolidColorBrush(Color.FromRgb(134, 217, 255));
        }

        private void setHighLightOff(object sender)
        {
            ((Label)sender).Background =
                new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }

        private void setDarkLight(object sender)
        {
            ((Label)sender).Background =
                new SolidColorBrush(Color.FromRgb(104, 197, 235));
        }

        private void amLabelRight_MouseEnter(object sender, MouseEventArgs e)
        {
            setHighLighted(sender);
        }

        private void amLabelRight_MouseLeave(object sender, MouseEventArgs e)
        {
            setHighLightOff(sender);
        }

        private void amLabelRight_MouseUp(object sender, MouseButtonEventArgs e)
        {
            setHighLighted(sender);
            mfOnConfirm();
            mfOnFlip();
        }

        private void amLabelWrong_MouseUp(object sender, MouseButtonEventArgs e)
        {
            setHighLighted(sender);
            mfOnDeny();
            mfOnFlip();
        }

        private void amLabelWrong_MouseEnter(object sender, MouseEventArgs e)
        {
            setHighLighted(sender);
        }

        private void amLabelWrong_MouseLeave(object sender, MouseEventArgs e)
        {
            setHighLightOff(sender);
        }

        private void amLabelRight_MouseDown(object sender, MouseButtonEventArgs e)
        {
            setDarkLight(sender);
        }

        private void amLabelWrong_MouseDown(object sender, MouseButtonEventArgs e)
        {
            setDarkLight(sender);
        }
    }
}
