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
using LECOG.UIComponents;
using LECOG.AOSpan;
using LECOG.DigiSymb;
using LECOG.PaperFold;

namespace LECOG
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public int msSizeW = 800;
        public int msSizeH = 600;

        public String mSubjectInfoString = "not_defined";

        public MainWindow()
        {
            InitializeComponent();
            for (int i = 5; i < 101; i++)
                amComboBoxAge.Items.Add((object)i.ToString());

            amComboBoxAge.SelectedIndex = 0;
            amComboBoxGender.SelectedIndex = 0;
        }

        public void EnableDemogInfoComps()
        {
            amComboBoxAge.Visibility = System.Windows.Visibility.Visible;
            amComboBoxGender.Visibility = System.Windows.Visibility.Visible;
            amNameTextBox.Visibility = System.Windows.Visibility.Visible;
            label1.Visibility = System.Windows.Visibility.Visible;
            label2.Visibility = System.Windows.Visibility.Visible;
            label3.Visibility = System.Windows.Visibility.Visible;
        }

        public void DisableDemogInfoComps()
        {
            amComboBoxAge.Visibility = System.Windows.Visibility.Hidden;
            amComboBoxGender.Visibility = System.Windows.Visibility.Hidden;
            amNameTextBox.Visibility = System.Windows.Visibility.Hidden;
            label1.Visibility = System.Windows.Visibility.Hidden;
            label2.Visibility = System.Windows.Visibility.Hidden;
            label3.Visibility = System.Windows.Visibility.Hidden;
        }

        private String getDocNameHead()
        {
            String retval = "";
            DateTime dt = DateTime.Now;
            retval += dt.Year.ToString("D4") + dt.Month.ToString("D2") + 
                dt.Day.ToString("D2") + dt.Hour.ToString("D2") + 
                dt.Minute.ToString("D2") + dt.Second.ToString("D2");

            retval += "_" + amNameTextBox.Text.Replace(" ", "");
            retval += "_" + amComboBoxGender.Text;
            retval += "_" + amComboBoxAge.Text;
            return retval;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Set2Menu();
            //GoDigiSymb();
        }

        public void Set2Menu()
        {
            amCanvas.Children.Clear();

            EnableDemogInfoComps();

            CompColorBtn AOSpan = new CompColorBtn();
            AOSpan.Init(this, 110);
            AOSpan.SetText("AOSpan");
            AOSpan.SetMouseUpFunc(GoAO, (object)null);
            amCanvas.Children.Add(AOSpan);
            Canvas.SetTop(AOSpan, 100);
            Canvas.SetLeft(AOSpan, 100);

            CompColorBtn DigiSymb = new CompColorBtn();
            DigiSymb.Init(this, 200);
            DigiSymb.SetText("Digit Symbol");
            DigiSymb.SetMouseUpFunc(GoDigiSymb, (object)null);
            amCanvas.Children.Add(DigiSymb);
            Canvas.SetTop(DigiSymb, 100);
            Canvas.SetLeft(DigiSymb, 240);

            CompColorBtn PaperFolding = new CompColorBtn();
            PaperFolding.Init(this, 200);
            PaperFolding.SetText("Paper Folding");
            PaperFolding.SetMouseUpFunc(GoPF, (object)null);
            amCanvas.Children.Add(PaperFolding);
            Canvas.SetTop(PaperFolding, 300);
            Canvas.SetLeft(PaperFolding, 100);
        }

        public void GoAO(object objPara=null)
        {
            DisableDemogInfoComps();
            mSubjectInfoString = getDocNameHead();
            AOSpanRunner AORunner = new AOSpanRunner(this);
            AORunner.mSteps = AOSpanRunner.AOSpanSteps.instruction;
            AORunner.Next(null);
        }

        public void GoDigiSymb(object objPara = null)
        {
            DisableDemogInfoComps();
            mSubjectInfoString = getDocNameHead();
            DigiSymbRunner DSRunner = new DigiSymbRunner(this, mSubjectInfoString);
            //DSRunner.ShowTest();
        }

        public void GoPF(object objPara = null)
        {
            DisableDemogInfoComps();
            mSubjectInfoString = getDocNameHead();
            PagePFTest ppft = new PagePFTest(this);
            amCanvas.Children.Clear();
            amCanvas.Children.Add(ppft);
            Canvas.SetLeft(ppft, 0);
            Canvas.SetTop(ppft, 0);
        }

        public void Set2CompPage(object uco)
        {
            UserControl uc = (UserControl)uco;
            amCanvas.Children.Clear();
            amCanvas.Children.Add(uc);
            Canvas.SetLeft(uc, (this.Width - uc.Width) / 2);
            Canvas.SetTop(uc, (this.Height - uc.Height) / 2);
        }


    }
}
