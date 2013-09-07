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
using System.IO;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Threading;

namespace LECOG.PaperFold
{
    /// <summary>
    /// PagePFTest.xaml 的互動邏輯
    /// </summary>
    public partial class PagePFTest : UserControl
    {
        public MainWindow mMainWindow;
        
        public List<Image> mItemImages;
        public List<IntPtr> mItemPtrs;

        public List<Image> mSelectionImages;
        public List<IntPtr> mSelectionPtrs;
        public List<Rectangle> mSelMarks;

        public List<int> mUserChoice;

        private int mCurPage = 0;
        private int mCurSelected = -1;

        private int mItemsCount = 20;
        public static int[] mStandardAns = { 0, 3, 1, 3, 1, 4, 0, 2, 4, 4, 2, 1, 0, 4, 1, 0, 4, 2, 3, 2};

        public String mSrcFolder = LECOGCommon.GetExeLoc() + "pfsource\\";

        public CompNextPahseShade mMidPage;
        public CompBeginShade mBeginPage;
        private Timer mTimer;

        public PagePFTest(MainWindow mw)
        {
            InitializeComponent();
            mMainWindow = mw;
            compColorBtnPrev.Init(mw, 100);
            compColorBtnPrev.SetText("上一题");
            compColorBtnNext.Init(mw, 100);
            compColorBtnNext.SetText("下一题");

            mItemImages = new List<Image>();
            mItemImages.Add(imageI1);
            mItemImages.Add(imageI2);
            mItemImages.Add(imageI3);
            mItemImages.Add(imageI4);
            mItemPtrs = new List<IntPtr>();
            for (int i = 0; i < mItemImages.Count; i++)
                mItemPtrs.Add(IntPtr.Zero);

            mSelectionImages = new List<Image>();
            mSelectionImages.Add(imageS1);
            mSelectionImages.Add(imageS2);
            mSelectionImages.Add(imageS3);
            mSelectionImages.Add(imageS4);
            mSelectionImages.Add(imageS5);

            mSelMarks = new List<Rectangle>();
            mSelMarks.Add(rectangle1);
            mSelMarks.Add(rectangle2);
            mSelMarks.Add(rectangle3);
            mSelMarks.Add(rectangle4);
            mSelMarks.Add(rectangle5);

            for (int k = 0; k < mSelectionImages.Count; k++)
            {
                mSelectionImages[k].MouseEnter += new MouseEventHandler(image_MouseEnter);
                mSelectionImages[k].MouseLeave += new MouseEventHandler(image_MouseLeave);
                mSelectionImages[k].MouseUp += new MouseButtonEventHandler(image_MouseUp);
            }

            compColorBtnNext.SetMouseUpFunc(onPressingNext, null);
            compColorBtnPrev.SetMouseUpFunc(onPressingPrev, null);

            mSelectionPtrs = new List<IntPtr>();
            for (int j = 0; j < mSelectionImages.Count; j++)
                mSelectionPtrs.Add(IntPtr.Zero);

            SetPage(mCurPage);
            mUserChoice = new List<int>();

            mMidPage = new CompNextPahseShade(this);

            //prepare prefix
            mBeginPage = new CompBeginShade(this);
            amCanvas.Children.Add(mBeginPage);
            Canvas.SetTop(mBeginPage, 0);
            Canvas.SetLeft(mBeginPage, -20);
        }

        public void Start()
        {
            //trigger ticker
            mTimer = new Timer();
            mTimer.Interval = 1000 * 60 * 3;
            mTimer.AutoReset = false;
            mTimer.Elapsed += new ElapsedEventHandler(mTimer_Elapsed);
            mTimer.Enabled = true;

            amCanvas.Children.Remove(mBeginPage);
        }

        private delegate void timeDele();

        void mTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new timeDele(showMidPage));
        }

        private int getSelCompIndex(object obj)
        {
            int index = -1;

            for (int i = 0; i < mSelectionImages.Count; i++)
            {
                if (mSelectionImages[i] == obj)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        private void clearMarks()
        {
            for(int i = 0; i < mSelMarks.Count; i++)
            {
                mSelMarks[i].Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }

        void image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            clearMarks();
            int index = getSelCompIndex(sender);
            mSelMarks[index].Fill = new SolidColorBrush(Color.FromRgb(119, 216, 255));
            mCurSelected = index;
        }

        void image_MouseLeave(object sender, MouseEventArgs e)
        {
            int index = getSelCompIndex(sender);
            if(index != mCurSelected)
            {
                mSelMarks[index].Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }

        void image_MouseEnter(object sender, MouseEventArgs e)
        {
            int idx = getSelCompIndex(sender);
            mSelMarks[idx].Fill = new SolidColorBrush(Color.FromRgb(119, 216, 255));
        }

        private String getPicName(int itemNum, bool isStem, int index)
        {
            String retval = "";
            retval += (itemNum + 1).ToString();
            
            if (isStem)
            {
                retval += "I";
            }
            else
            {
                retval += "S";
            }

            retval += index.ToString();
            retval += ".jpg";

            return retval;
        }

        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);

        private void setPicToImage(int itemNum, bool isStem, int index)
        {
            String fullPath = mSrcFolder + getPicName(itemNum, isStem, index);
            if (File.Exists(fullPath))
            {
                List<Image> imgList;
                List<IntPtr> ptrList;
                if (isStem)
                {
                    imgList = mItemImages;
                    ptrList = mItemPtrs;
                }
                else
                {
                    imgList = mSelectionImages;
                    ptrList = mSelectionPtrs;
                }

                if (ptrList[index] != IntPtr.Zero)
                {
                    DeleteObject(ptrList[index]);
                    ptrList[index] = IntPtr.Zero;
                }

                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(fullPath);

                ptrList[index] = bmp.GetHbitmap();
                imgList[index].Source =
                    System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                        ptrList[index], IntPtr.Zero, System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(bmp.Width, bmp.Height));
            }
            else if(isStem)
            {
                if (mItemPtrs[index] != IntPtr.Zero)
                {
                    DeleteObject(mItemPtrs[index]);
                    mItemPtrs[index] = IntPtr.Zero;
                }

                mItemImages[index].Source = null;
            }
        }

        public void SetPage(int pageNum)
        {
            for (int i = 0; i < mItemImages.Count; i++)
            {
                setPicToImage(pageNum, true, i);
            }

            for (int j = 0; j < mSelectionImages.Count; j++)
            {
                setPicToImage(pageNum, false, j);
            }

            amPageMark.Text = "第" + (pageNum + 1).ToString() + "页";

            if ((pageNum + 1) == mItemsCount)
            {
                compColorBtnNext.IsHitTestVisible = true;
                compColorBtnNext.SetText("结束");
            }
            else if (pageNum == 9)
            {
                compColorBtnNext.SetText("");
                compColorBtnNext.IsHitTestVisible = false;
            }
            else
            {
                compColorBtnNext.SetText("下一题");
                compColorBtnNext.IsHitTestVisible = true;
            }

            if (pageNum == 0)
            {
                compColorBtnPrev.SetText("");
                compColorBtnPrev.IsHitTestVisible = false;
            }
            else
            {
                compColorBtnPrev.SetText("上一题");
                compColorBtnPrev.IsHitTestVisible = true;
            }
        }

        private void updateChoice()
        {
            if (mUserChoice.Count > mCurPage)
            {
                mUserChoice[mCurPage] = mCurSelected;
            }
            else
            {
                mUserChoice.Add(mCurSelected);
            }
        }

        private void loadChoice()
        {
            if (mCurPage < mUserChoice.Count)
            {
                mCurSelected = mUserChoice[mCurPage];
                if (mCurSelected != -1)
                {
                    mSelMarks[mCurSelected].Fill =
                        new SolidColorBrush(Color.FromRgb(119, 216, 255));
                }
            }
            else
            {
                clearMarks();
            }
        }

        private void onPressingNext(object obj)
        {
            if (mCurSelected > -1)
            {
                clearMarks();
                if (mCurPage < mItemsCount - 1)
                {
                    updateChoice();
                    mCurPage++;
                    mCurSelected = -1;
                    SetPage(mCurPage);
                    loadChoice();
                }
                else if (mCurPage == mItemsCount - 1)//save and finish
                {
                    Finish();
                }
            }
            else
            {
                MessageBox.Show("请做出选择");
            }
        }

        private void showMidPage()
        {
            mTimer.Enabled = false;
            amCanvas.Children.Add(mMidPage);
            Canvas.SetTop(mMidPage, 0);
            Canvas.SetLeft(mMidPage, -20);
        }

        private void onPressingPrev(object obj)
        {
            if (mCurSelected > -1)
            {
                clearMarks();
                if (mCurPage > 0)
                {
                    updateChoice();
                    mCurPage--;
                    mCurSelected = -1;
                    SetPage(mCurPage);
                    loadChoice();
                }
            }
            else
            {
                MessageBox.Show("请做出选择");
            }
        }

        public void BeginNextHalf()
        {
            //check answers` count
            if (mUserChoice.Count < 10)
            {
                int less = 10 - mUserChoice.Count;
                for (int i = 0; i < less; i++)
                {
                    mUserChoice.Add(-1);
                }
            }

            mCurPage = 10;
            SetPage(mCurPage);

            amCanvas.Children.Remove(mMidPage);
            mTimer = new Timer();
            mTimer.Interval = 1000 * 60 * 3;
            mTimer.AutoReset = false;
            mTimer.Elapsed += new ElapsedEventHandler(mTimer_Elapsed2);
            mTimer.Enabled = true;
        }

        void mTimer_Elapsed2(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new timeDele(Finish));
        }

        void doNothing()
        { }

        public void Finish()
        {
            //check answers` count
            if (mUserChoice.Count < 20)
            {
                int less = 20 - mUserChoice.Count;
                for (int i = 0; i < less; i++)
                {
                    mUserChoice.Add(-1);
                }
            }
            //Recorder here systest
            Recorder rec = new Recorder(LECOGCommon.GetExeLoc() + 
                "Record\\PF\\" + mMainWindow.mSubjectInfoString + ".txt", this);
            rec.Save();

            UIFunctions uifunc = new UIFunctions(mMainWindow);
            DisplayPage dp = uifunc.DisplayPageFactory("\r\n\r\n本测验结束");
            dp.mfOnMouseUp = doNothing;
            uifunc.ShowPage(dp);

            Timer endElapser = new Timer();
            endElapser.Interval = 3000;
            endElapser.AutoReset = false;
            endElapser.Elapsed += new ElapsedEventHandler(endElapser_Elapsed);
            endElapser.Enabled = true;
        }

        void endElapser_Elapsed(object sender, ElapsedEventArgs e)
        {
            mMainWindow.Dispatcher.Invoke(
                DispatcherPriority.Normal, new timeDele(mMainWindow.Set2Menu));
        }
    }
}
