using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Threading;
using System.Timers;

namespace LECOG.DigiSymb
{
    public class DigiSymbRunner
    {
        public MainWindow mMainWindow;
        private int mCurAt = 0;
        private int mCurAtDataPage = 0;
        private List<TokElem> mItemElems;
        private List<TokElem> mInputElems;

        public List<int> mUserAnswer;//answer
        public List<long> mRTPoints;//RT

        private Stopwatch mWatch;

        private int CAPACITY_PER_PAGE = 10;

        private Recorder mRec;

        private Timer mTimer;

        //200 random integers
        public static int[] mNumScheme = { 4, 2, 5, 3, 5, 7, 6, 9, 1, 2, 4, 7, 8, 5, 7,
                                              4, 2, 7, 5, 3, 5, 9, 7, 5, 1, 2, 5, 8, 2, 3, 
                                              6, 2, 8, 7, 2, 8, 7, 2, 6, 7, 3, 8, 4, 2, 5,
                                              4, 8, 7, 3, 1, 4, 6, 9, 1, 8, 7, 6, 8, 6, 2, 
                                              8, 5, 6, 5, 6, 8, 3, 6, 8, 6, 1, 4, 5, 8, 1, 
                                              4, 5, 3, 6, 4, 2, 5, 2, 7, 8, 3, 9, 1, 4, 2, 
                                              3, 1, 7, 9, 8, 1, 4, 7, 3, 7, 5, 6, 7, 3, 1, 
                                              6, 3, 4, 3, 5, 4, 2, 3, 5, 8, 9, 1, 7, 3, 8, 
                                              1, 2, 5, 1, 2, 6, 7, 3, 7, 8, 7, 5, 1, 5, 3, 
                                              8, 1, 8, 7, 6, 7, 4, 9, 2, 4, 9, 5, 4, 6, 5, 
                                              8, 1, 5, 7, 1, 3, 8, 5, 3, 6, 9, 7, 4, 7, 4, 
                                              9, 2, 9, 3, 4, 8, 4, 3, 7, 2, 8, 7, 4, 1, 9, 
                                              5, 7, 3, 8, 1, 4, 8, 5, 8, 2, 3, 1, 4, 2, 8, 
                                              6, 5, 3, 6, 3 };

        public static int[] mPracScheme = { 7, 4, 8, 9, 7, 1, 2, 3, 5, 4};

        public int[] mCurScheme;

        public enum Status { Practise, FormalTest };

        public Status mStatus;

        public DigiSymbRunner(MainWindow mw, String infoString)
        {
            mMainWindow = mw;
            //new
            mItemElems = new List<TokElem>();
            mInputElems = new List<TokElem>();

            mUserAnswer = new List<int>();
            mRTPoints = new List<long>();
            mWatch = new Stopwatch();

            for (int i = 0; i < CAPACITY_PER_PAGE; i++)
            {
                mItemElems.Add(new TokElem());
                mInputElems.Add(new TokElem());
            }

            mRec = new Recorder(this, LECOGCommon.GetExeLoc() + "Record\\DS\\" + infoString + ".txt");

            ShowInstruction();
        }

        public void ShowInstruction()
        {
            mMainWindow.amCanvas.Children.Clear();
            mMainWindow.amCanvas.Children.Add(new Instruction(this));
        }

        private delegate void dataFunction();

        public void ShowTest()
        {
            //layout
            mMainWindow.amCanvas.Children.Clear();
            for (int j = 0; j < mItemElems.Count; j++)
            {
                mMainWindow.amCanvas.Children.Add(mItemElems[j]);
                Canvas.SetTop(mItemElems[j], 5);
                Canvas.SetLeft(mItemElems[j], 10 + j * 76);
            }

            for (int i = 0; i < mInputElems.Count; i++)
            {
                mMainWindow.amCanvas.Children.Add(mInputElems[i]);
                Canvas.SetTop(mInputElems[i], 5 + 76);
                Canvas.SetLeft(mInputElems[i], 10 + i * 76);
            }

            InputPad ip = new InputPad();
            ip.mfMouseUp = OnInput;
            mMainWindow.amCanvas.Children.Add(ip);
            Canvas.SetTop(ip, 5 + 76 * 2 + 5);
            Canvas.SetLeft(ip, (779 - 240) / 2);

            setUIData();
            addLegend();

            mTimer = new Timer();
            mTimer.Interval = 60 * 1000 * 2;
            //systest disabled
            //mTimer.Interval = 1000 * 7;
            mTimer.AutoReset = false;
            mTimer.Elapsed += new ElapsedEventHandler(mTimer_Elapsed);
            mTimer.Enabled = true;

            if (mStatus == Status.Practise)
            {
                LECOG.UIComponents.CompColorBtn btnJumpover = new UIComponents.CompColorBtn();
                btnJumpover.amLabel.Content = "此为练习，点此跳过";
                btnJumpover.mfMouseUpFunc = TestStart;
                btnJumpover.Width = 220;
                mMainWindow.amCanvas.Children.Add(btnJumpover);
                Canvas.SetTop(btnJumpover, 250);
                Canvas.SetLeft(btnJumpover, 0);
            }
        }

        private delegate void timedele();

        void mTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            mMainWindow.Dispatcher.Invoke(
                DispatcherPriority.Normal, new timedele(PhaseEnd));
        }

        private void addLegend()
        {
            for (int i = 0; i < 9; i++)
            {
                TokElem te = new TokElem();
                mMainWindow.amCanvas.Children.Add(te);
                te.SetText((i + 1).ToString());
                Canvas.SetTop(te, (5 + 76 * 2 + 5) + 240 + 5);
                Canvas.SetLeft(te, (779 - 76 * 9) / 2 + 76 * i);
                te.SetHighLight();
            }

            for (int j = 0; j < 9; j++)
            {
                TokElem te = new TokElem();
                mMainWindow.amCanvas.Children.Add(te);
                te.SetPicture((System.Drawing.Bitmap)
                    Properties.Resources.ResourceManager.GetObject(
                    "ds" + (j + 1).ToString()));
                Canvas.SetLeft(te, (779 - 76 * 9) / 2 + 76 * j);
                Canvas.SetTop(te, (5 + 76 * 2 + 5) + 240 + 5 + 76);
                te.SetHighLight();
            }
        }

        private void setCursor()
        {
            for (int i = 0; i < CAPACITY_PER_PAGE; i++)
            {
                if (i == mCurAt)
                {
                    mItemElems[i].SetHighLight();
                    mInputElems[i].SetHighLight();
                }
                else
                {
                    mItemElems[i].UnSetHighLight();
                    mInputElems[i].UnSetHighLight();
                }
            }
        }

        public void OnInput(int btnIden)
        {
            if (mCurAtDataPage < mCurScheme.Length / CAPACITY_PER_PAGE && mCurAt < CAPACITY_PER_PAGE)
            {
                if (mStatus == Status.FormalTest)
                {
                    mRTPoints.Add(mWatch.ElapsedMilliseconds);
                    mUserAnswer.Add(btnIden);
                }

                mInputElems[mCurAt].SetPicture(
                    (System.Drawing.Bitmap)Properties.Resources.ResourceManager.GetObject(
                    InputPad.GetPicFileName(btnIden)));
                mCurAt++;

                if (mCurAt < CAPACITY_PER_PAGE)
                {
                    setCursor();
                }
                else//flip page
                {
                    Timer flipTm = new Timer();
                    flipTm.Interval = 500;
                    flipTm.AutoReset = false;
                    flipTm.Elapsed += new ElapsedEventHandler(flipTm_Elapsed);
                    flipTm.Enabled = true;
                }
            }
        }

        void flipTm_Elapsed(object sender, ElapsedEventArgs e)
        {
            mMainWindow.Dispatcher.Invoke(
                DispatcherPriority.Normal, new timedele(flipPage));
        }

        private void flipPage()
        {
            mCurAt = 0;
            mCurAtDataPage++;

            if (mCurAtDataPage < mCurScheme.Length / CAPACITY_PER_PAGE)
            {
                setUIData();
            }
            else if (mCurAtDataPage == mCurScheme.Length / CAPACITY_PER_PAGE)
            {
                mTimer.Enabled = false;
                PhaseEnd();
            }
        }

        private void clearInput()
        {
            for (int i = 0; i < CAPACITY_PER_PAGE; i++)
            {
                mInputElems[i].SetPicture(null);
            }
        }

        private void setUIData()
        {
            clearInput();

            for (int i = 0; i < mItemElems.Count; i++)
            {
                mItemElems[i].SetText(
                    mCurScheme[mCurAtDataPage * CAPACITY_PER_PAGE + i].ToString());
            }

            setCursor();
        }

        private void commonStart()
        {
            if (mTimer != null && mTimer.Enabled)
            {
                mTimer.Enabled = false;
            }

            if (mStatus == Status.Practise)
            {
                mCurScheme = mPracScheme;
            }
            else if (mStatus == Status.FormalTest)
            {
                mCurScheme = mNumScheme;
                mWatch.Start();
            }

            mCurAt = 0;
            mCurAtDataPage = 0;
            ShowTest();
        }

        public void PracStart()
        {
            mStatus = Status.Practise;
            commonStart();
        }

        public void TestStart(object obj)
        {
            TestStart();
        }

        public void TestStart()
        {
            mStatus = Status.FormalTest;
            commonStart();
        }

        void doNothing()
        { }

        public void PhaseEnd()
        {
            if (mStatus == Status.FormalTest)
            {
                UIFunctions uifunc = new UIFunctions(mMainWindow);
                UIComponents.DisplayPage dp = uifunc.DisplayPageFactory("\r\n\r\n本测验结束");
                dp.mfOnMouseUp = doNothing;
                uifunc.ShowPage(dp);

                Timer endElapser = new Timer();
                endElapser.Interval = 3000;
                endElapser.AutoReset = false;
                endElapser.Elapsed += new ElapsedEventHandler(endElapser_Elapsed);
                endElapser.Enabled = true;
            }
            else if(mStatus == Status.Practise)
            {
                UIFunctions uifunc = new UIFunctions(mMainWindow);
                UIComponents.DisplayPage dp = uifunc.DisplayPageFactory("\r\n\r\n以下是正式测验，点鼠标开始");
                dp.mfOnMouseUp = TestStart;
                uifunc.ShowPage(dp);
            }
        }

        private void doQuitUIJob()
        {
            mWatch.Stop();
            mRec.Save();
            mMainWindow.Set2Menu();
        }

        void endElapser_Elapsed(object sender, ElapsedEventArgs e)
        {
            mMainWindow.Dispatcher.Invoke(
                DispatcherPriority.Normal, new timedele(doQuitUIJob));
        }

        public void Next()
        { }
    }
}
