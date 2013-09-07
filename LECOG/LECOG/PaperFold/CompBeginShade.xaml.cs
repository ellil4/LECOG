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

namespace LECOG.PaperFold
{
    /// <summary>
    /// CompBeginShade.xaml 的互動邏輯
    /// </summary>
    public partial class CompBeginShade : UserControl
    {
        PagePFTest mPage;

        public CompBeginShade(PagePFTest page)
        {
            InitializeComponent();
            mPage = page;
        }

        private void amLabelBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            amLabelBtn.Background = new SolidColorBrush(Color.FromRgb(119, 216, 255));
        }

        private void amLabelBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            amLabelBtn.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }

        private void amLabelBtn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mPage.Start();
        }
    }
}
