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
    /// DisplayPage.xaml 的互動邏輯
    /// </summary>
    public partial class DisplayPage : UserControl
    {
        public delegate void OnMouseUpFunc();
        public OnMouseUpFunc mfOnMouseUp;

        public DisplayPage()
        {
            InitializeComponent();
        }

        public void SetText(String text)
        {
            amTextBlock.Text = text;
        }

        private void canvas1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mfOnMouseUp();
        }
    }
}
