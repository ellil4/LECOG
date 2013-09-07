using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using LECOG.UIComponents;

namespace LECOG
{
    public class UIFunctions
    {
        public MainWindow mMainWindow;
        public UIFunctions(MainWindow mw)
        {
            mMainWindow = mw;
        }

        public void ShowPage(UserControl page)
        {
            mMainWindow.amCanvas.Children.Clear();
            mMainWindow.amCanvas.Children.Add(page);
            Canvas.SetTop(page, 0);
            Canvas.SetLeft(page, 0);
            page.Focus();
        }

        public CompBooleanJudge BoolJudgeFactory(int answer)
        {
            CompBooleanJudge page = new CompBooleanJudge();
            page.amTextBlock.Text = answer.ToString();
            return page;
        }

        public DisplayPage DisplayPageFactory(String text)
        {
            DisplayPage page = new DisplayPage();
            page.SetText(text);
            return page;
        }

        public DisplayPage EquationPageFactoty(String Equation)
        {
            return DisplayPageFactory(Equation);
        }

        public Comp12Cells CellsPageFactory()
        {
            Comp12Cells page = new Comp12Cells(mMainWindow);
            return page;
        }

        public int GenRandomAnswer(int answer)
        {
            int retval = 0;
            Random rdm = new Random();
            if (rdm.Next(0, 2) == 0)//fabricate
            {
                if (rdm.Next(0, 2) == 0)
                {
                    retval = answer + rdm.Next(0, 10);
                }
                else
                {
                    retval = answer + rdm.Next(0, 10) * 10;
                }
            }
            else//origin
            {
                retval = answer;
            }
            return retval;
        }

        public AOSpan.ReportPage ReportPageFactory(String text1, String text2)
        {
            AOSpan.ReportPage page = new AOSpan.ReportPage();
            page.SetText(text1, text2);
            return page;
        }
    }
}
