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
using LECOG.AOSpan;

namespace LECOG.UIComponents
{
    /// <summary>
    /// Comp9Cells.xaml 的互動邏輯
    /// </summary>
    public partial class Comp12Cells : UserControl
    {
        //Elem: btn and cell
        public int mElemWidth = 88;
        public int mElemHeight = 53;

        public int mCellGrpWidht;
        public int mCellGrpHeight;

        public int mBtnGrpWidth;
        public int mBtnGrpHeight;

        public int mGap = 53;

        public delegate void OnConfirm();
        public delegate void OnSaveResult();
        public OnConfirm mfOnConfirm;
        public OnSaveResult mfOnSaveResult;

        private List<CompColorBtn> mCells;
        public List<String> mCharOrder;

        public MainWindow mMainWindow;

        private int mCurNum = 1;

        public Comp12Cells(MainWindow mw)
        {
            InitializeComponent();
            mMainWindow = mw;
            mCellGrpWidht = mElemWidth * 4 + mGap * 2;
            mCellGrpHeight = mElemHeight * 4 + mGap * 2;
            mBtnGrpWidth = mElemWidth * 2 + mGap;
            mBtnGrpHeight = mElemHeight;

            mCells = new List<CompColorBtn>();
            mCharOrder = new List<string>();
        }

        private void clearCellNum(CompColorBtn cell)
        {
            cell.amLabel.Content = getCellChar(cell);
        }

        private void setCellNum(CompColorBtn cell, int number)
        {
            String str = getCellChar(cell);

            str += "(" + number + ")";
            cell.amLabel.Content = str;
        }

        private int getCellNum(CompColorBtn cell)
        {
            int retval = -1;
            String text = cell.amLabel.Content.ToString();
            if (text.Length > 1)
            {
                String num = "";
                num += text[2];
                retval = Int32.Parse(num);
            }
            return retval;
        }

        private String getCellChar(CompColorBtn cell)
        {
            String retval = (String)cell.amLabel.Content;

            if (retval.Length > 1)
                retval = retval.Remove(1);

            return retval;
        }

        private void cellClicked(object btn)
        {
            if (mCurNum <= 9 && getCellNum((CompColorBtn)btn) == -1)
            {
                CompColorBtn ccb = (CompColorBtn)btn;
                setCellNum(ccb, mCurNum);
                mCharOrder.Add(getCellChar(ccb));
                mCurNum++;
            }
        }

        private void clearClicked(object none)
        {
            for (int i = 0; i < mCells.Count; i++)
            {
                clearCellNum(mCells[i]);
            }

            mCharOrder.Clear();
            mCurNum = 1;
        }

        private void confirmClicked(object none)
        {
            mfOnSaveResult();
            mfOnConfirm();
        }

        private void amCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            //build UI
            int cellBegPosX = (int)(this.Width - mCellGrpWidht) / 2;
            int cellBegPosY = mGap;

            int btnBegPosX = (int)(this.Width - mBtnGrpWidth) / 2;
            int btnBegPosY = cellBegPosY + mCellGrpHeight + mGap * 2;

            for (int i = 0; i < AOSpanItemFunctions.CharsSet.Length; i++)
            {
                CompColorBtn btn = new CompColorBtn();
                btn.Init(mMainWindow, mElemWidth);
                btn.SetText(AOSpanItemFunctions.CharsSet[i]);
                btn.SetMouseUpFunc(cellClicked, btn);
                mCells.Add(btn);
                amCanvas.Children.Add(btn);
                Canvas.SetTop(btn, cellBegPosY + (i / 4) * (mGap + mElemHeight));
                Canvas.SetLeft(btn, cellBegPosX + (i % 4) * (mGap + mElemWidth));
            }

            CompColorBtn btnConfirm = new CompColorBtn();
            btnConfirm.Init(mMainWindow, mElemWidth);
            btnConfirm.SetText("确定");
            btnConfirm.SetMouseUpFunc(confirmClicked, null);
            amCanvas.Children.Add(btnConfirm);
            Canvas.SetTop(btnConfirm, btnBegPosY);
            Canvas.SetLeft(btnConfirm, btnBegPosX);

            CompColorBtn btnClear = new CompColorBtn();
            btnClear.Init(mMainWindow, mElemWidth);
            btnClear.SetText("清空");
            btnClear.SetMouseUpFunc(clearClicked, null);
            amCanvas.Children.Add(btnClear);
            Canvas.SetTop(btnClear, btnBegPosY);
            Canvas.SetLeft(btnClear, btnBegPosX + mGap + mElemWidth);
        }
    }
}
