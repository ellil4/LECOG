using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LECOG.UIComponents;
using System.Timers;
using System.Windows.Threading;
using System.Diagnostics;

namespace LECOG.AOSpan
{
    public class AOSpanRunner
    {
        public enum AOSpanSteps
        {
            instruction, practise, test, end
        }

        public MainWindow mMainWindow;

        public AOSpanSteps mSteps = AOSpanSteps.instruction;

        public int mInstructionStep = 0;
        public int mGroupStep = 0;
        public int mInGroupStep = 0;
        public int mPhaseStep = 0;

        public List<AOSpanItemGrp> mItems;
        public UIFunctions mUIFunc;
        public AOSpanItemFunctions mItemFunc;

        public AOSpanItemGrp mCurItem;
        public CompBooleanJudge mCurJudgePage;
        public DisplayPage mCurCharaPage;
        public Comp12Cells mCurCellPage;
        public DisplayPage mCurEquationPage;
        public ReportPage mCurReportPage;
        public DisplayPage mCurInGroupTitlePage;
        public DisplayPage mWarningPage;
        public DisplayPage mEndPage;

        public delegate void CurrentProcessFunc();
        public CurrentProcessFunc mCurProcess;

        public int mGroupWrongMath = 0;
        public int mWrongCharaInPhase = 0;

        public Stopwatch mWatch;

        public Recorder mRec;

        public AOSpanRunner(MainWindow mw)
        {
            mMainWindow = mw;
            mItems = new List<AOSpanItemGrp>();
            mUIFunc = new UIFunctions(mw);
            mItemFunc = new AOSpanItemFunctions();
            mWarningPage = mUIFunc.DisplayPageFactory("请注意计算题目的正确性");
            mEndPage = mUIFunc.DisplayPageFactory("本项测验结束");
            mWatch = new Stopwatch();
            mRec = new Recorder(LECOGCommon.GetExeLoc() + "\\Record\\AO\\", mw);
        }

        public void instructionGo()
        {
            InstructionPage1 page = new InstructionPage1();
            page.MouseUp += new System.Windows.Input.MouseButtonEventHandler(instructionPage_MouseUp);
            mUIFunc.ShowPage(page);
        }

        void instructionPage_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            mSteps = AOSpanSteps.practise;
            Next(null);
        }

        private void fillMathAnswerTrue()
        {
            int elapsedMilli = (int)mWatch.ElapsedMilliseconds;
            mWatch.Stop();
            mCurItem.MathRT.Add(elapsedMilli);

            mCurItem.MathAnswerCorrectness.Add(true);
        }

        private void fillMathAnswerFalse()
        {
            int elapsedMilli = (int)mWatch.ElapsedMilliseconds;
            mWatch.Stop();
            mCurItem.MathRT.Add(elapsedMilli);

            mCurItem.MathAnswerCorrectness.Add(false);

            mGroupWrongMath++;
        }

        private void checkSaveResult()
        {
            mCurItem.OrderRT = (int)mWatch.ElapsedMilliseconds;
            mWatch.Stop();
            mWatch.Reset();
            //check and save order
            bool flag = true;

            mCurItem.CharaAns = mCurCellPage.mCharOrder;

            if (mCurCellPage.mCharOrder.Count == mCurItem.Characters.Count)
            {
                for (int i = 0; i < mCurItem.Characters.Count; i++)
                {
                    if (mCurCellPage.mCharOrder[i] != mCurItem.Characters[i])
                    {
                        flag = false;
                        break;
                    }
                }
            }
            else
            {
                flag = false;
            }

            if (flag == false)
            {
                mWrongCharaInPhase++;
            }

            mCurItem.OrderCorrectness = flag;

            //save item 2 the sys.
            mItems.Add(mCurItem);
        }

        private void showJudgePage()
        {
            mUIFunc.ShowPage(mCurJudgePage);
            mWatch.Reset();
            mWatch.Start();
        }

        private delegate void timeDele();

        //character page
        private int mCharaPage_GroupLen = 0;
        private void showCharaPage()
        {
            mCurCharaPage.mfOnMouseUp = doNothing;
            mUIFunc.ShowPage(mCurCharaPage);
            Timer timer = new Timer();
            timer.AutoReset = false;
            timer.Interval = 2000;
            timer.Elapsed += new ElapsedEventHandler(showChara_Elapsed);
            timer.Enabled = true;
        }

        void showChara_Elapsed(object sender, ElapsedEventArgs e)
        {
            //iteration
            if (mInGroupStep != mCharaPage_GroupLen - 1)//common
            {
                mInGroupStep++;
                mMainWindow.Dispatcher.Invoke(DispatcherPriority.Normal, new timeDele(mCurProcess));//end of one link
            }
            else//show cell page
            {
                mInGroupStep = 0;
                mGroupStep++;
                mMainWindow.Dispatcher.Invoke(DispatcherPriority.Normal, new timeDele(showCellPage));
            }
        }

        private void showCellPage()
        {
            mWatch.Stop();
            mWatch.Reset();
            mWatch.Start();
            mUIFunc.ShowPage(mCurCellPage);
        }

        private void showEquationPage()
        {
            mCurEquationPage.mfOnMouseUp = doNothing;
            mUIFunc.ShowPage(mCurEquationPage);
            Timer timer = new Timer();
            timer.AutoReset = false;
            timer.Interval = 2000;
            timer.Elapsed += new ElapsedEventHandler(showEquation_Elapsed);
            timer.Enabled = true;
        }

        void showEquation_Elapsed(object sender, ElapsedEventArgs e)
        {
            mMainWindow.Dispatcher.Invoke(DispatcherPriority.Normal, new timeDele(showJudgePage));
        }

        private void showInGroupTitle()
        {
            mUIFunc.ShowPage(mCurInGroupTitlePage);
        }

        private void showWarningPage()
        {
            mWarningPage.mfOnMouseUp = doNothing;
            mUIFunc.ShowPage(mWarningPage);
            Timer timer = new Timer();
            timer.AutoReset = false;
            timer.Interval = 2000;
            timer.Elapsed += new ElapsedEventHandler(warning_Elapsed);
            timer.Enabled = true;
            
        }

        void warning_Elapsed(object sender, ElapsedEventArgs e)
        {
            mMainWindow.Dispatcher.Invoke(DispatcherPriority.Normal, new timeDele(mCurProcess));
        }

        private void doNothing()
        { }

        private void showReportPage()
        {
            //set content
            int correctCount = 0;
            for(int i = 0; i < mCurItem.MathAnswerCorrectness.Count; i++)
            {
                if(mCurItem.MathAnswerCorrectness[i] == true)
                    correctCount++;
            }
            String txt1 = "一共" + mCurItem.Characters.Count + "道计算题，您做对了" + correctCount + "道";
            String txt_add;
            if(mCurItem.OrderCorrectness)
            {
                txt_add = "正确";
            }
            else
            {
                txt_add = "错误";
            }
            String txt2 = "顺序判断" + txt_add;
            mCurReportPage.SetText(txt1, txt2);
            
            //show page
            mUIFunc.ShowPage(mCurReportPage);
        }

        public void GoInChain(int groupCount, int groupLen, 
            CurrentProcessFunc currentProcess, bool doReport, int phaseStep)
        {
            if (mGroupStep < groupCount)
            {
                if (mInGroupStep == 0)
                    mCurItem = mItemFunc.GenItemGrp(groupLen);

                mCurEquationPage = mUIFunc.EquationPageFactoty("\r\n\r\n" + mCurItem.Equations[mInGroupStep]);

                int tempAnswer = mUIFunc.GenRandomAnswer(mCurItem.MathAnswers[mInGroupStep]);
                mCurJudgePage = mUIFunc.BoolJudgeFactory(tempAnswer);
                mCurJudgePage.mfOnFlip = new CompBooleanJudge.OnFlip(showCharaPage);
                mCharaPage_GroupLen = groupLen;

                mCurCellPage = mUIFunc.CellsPageFactory();

                mCurReportPage = new ReportPage();

                mCurReportPage.mfOnMouseUp = new ReportPage.OnMouseUpFunc(currentProcess);

                mCurCellPage.mfOnSaveResult = new Comp12Cells.OnSaveResult(checkSaveResult);

                //end of group
                if (doReport)
                {
                    mCurCellPage.mfOnConfirm = new Comp12Cells.OnConfirm(showReportPage);
                }
                else
                {
                    if (((float)mGroupWrongMath / (float)groupLen) < 0.5)
                    {
                        mCurCellPage.mfOnConfirm = new Comp12Cells.OnConfirm(currentProcess);
                    }
                    else//warning
                    {
                        mWarningPage.mfOnMouseUp = new DisplayPage.OnMouseUpFunc(doNothing);
                        mCurCellPage.mfOnConfirm = new Comp12Cells.OnConfirm(showWarningPage);
                    }
                }

                mCurCharaPage = mUIFunc.DisplayPageFactory("\r\n\r\n" + mCurItem.Characters[mInGroupStep]);

                if (tempAnswer == mCurItem.MathAnswers[mInGroupStep])
                {
                    mCurJudgePage.mfOnConfirm = new CompBooleanJudge.OnConfirm(fillMathAnswerTrue);
                    mCurJudgePage.mfOnDeny = new CompBooleanJudge.OnDeny(fillMathAnswerFalse);
                }
                else
                {
                    mCurJudgePage.mfOnConfirm = new CompBooleanJudge.OnConfirm(fillMathAnswerFalse);
                    mCurJudgePage.mfOnDeny = new CompBooleanJudge.OnDeny(fillMathAnswerTrue);
                }

                //entry point
                if (mInGroupStep == 0)
                {
                    mGroupWrongMath = 0;

                    String status = "";
                    if (mSteps == AOSpanSteps.practise)
                    {
                        status += "练习";
                    }
                    else if (mSteps == AOSpanSteps.test)
                    {
                        status += "正式测验";
                    }

                    mCurInGroupTitlePage = mUIFunc.DisplayPageFactory(
                        status + "\r\n" +
                        "下一题  序列长度: " + groupLen + 
                        "\r\n第" + (mGroupStep + 1) + "组\r\n\r\n点击鼠标开始");

                    mCurInGroupTitlePage.mfOnMouseUp = new DisplayPage.OnMouseUpFunc(showEquationPage);
                    showInGroupTitle();
                }
                else
                {
                    showEquationPage();
                }
            }
            else//end of phase
            {
                mInGroupStep = 0;
                mGroupStep = 0;
                mPhaseStep++;

                if (mWrongCharaInPhase < 2 || mSteps == AOSpanSteps.practise)//prictise doesn`t quit
                {
                    currentProcess();
                }
                else
                {
                    mSteps = AOSpanSteps.end;
                    Next(null);
                }

                mWrongCharaInPhase = 0;
            }
        }

        public void practiseGo()
        {
            if (mPhaseStep == 0)
            {
                GoInChain(1, 3, practiseGo, true, mPhaseStep);
            }
            else if(mPhaseStep == 1)
            {
                mPhaseStep = 0;
                mSteps = AOSpanSteps.test;
                Next(null);
            }
        }

        public void testGo()
        {
            if (mPhaseStep == 0)
            {
                GoInChain(3, 2, testGo, false, mPhaseStep);
            }
            else if (mPhaseStep == 1)
            {
                GoInChain(3, 3, testGo, false, mPhaseStep);
            }
            else if (mPhaseStep == 2)
            {
                GoInChain(3, 4, testGo, false, mPhaseStep);
            }
            else if (mPhaseStep == 3)
            {
                GoInChain(3, 5, testGo, false, mPhaseStep);
            }
            else if (mPhaseStep == 4)
            {
                GoInChain(3, 6, testGo, false, mPhaseStep);
            }
            else if (mPhaseStep == 5)
            {
                GoInChain(3, 7, testGo, false, mPhaseStep);
            }
            else if (mPhaseStep == 6)
            {
                mPhaseStep = 0;
                mSteps = AOSpanSteps.end;
                Next(null);
            }
        }

        public void Next(object obj)
        {
            switch (mSteps)
            {
                case AOSpanSteps.instruction:
                    mCurProcess = instructionGo;
                    instructionGo();
                    break;
                case AOSpanSteps.practise:
                    mCurProcess = practiseGo;
                    practiseGo();
                    break;
                case AOSpanSteps.test:
                    mCurProcess = testGo;
                    testGo();
                    break;
                case AOSpanSteps.end:
                    mCurProcess = null;
                    mEndPage.mfOnMouseUp = doNothing;
                    mUIFunc.ShowPage(mEndPage);
                    //save
                    mRec.SaveRec(mItems);

                    Timer endT = new Timer();
                    endT.Interval = 3000;
                    endT.AutoReset = false;
                    endT.Elapsed += new ElapsedEventHandler(endT_Elapsed);
                    endT.Enabled = true;

                    break;
            }
        }

        void endT_Elapsed(object sender, ElapsedEventArgs e)
        {
            mMainWindow.Dispatcher.Invoke(
                DispatcherPriority.Normal, new timeDele(mMainWindow.Set2Menu));
        }
    }
}
