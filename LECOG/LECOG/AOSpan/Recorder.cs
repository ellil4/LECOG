using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LECOG.AOSpan
{
    public class Recorder
    {
        public String[] mMathHeaderMode = {"Eqt", "MRst", "MAnsCrns", "MRT" };
        public String[] mOrderHeaderMode = { "Order", "OrderAns", "ORt", "OCrcns" };

        public String mBasePath;
        public static String mathTok = "M";
        public static String orderTok = "O";
        public StreamWriter mSWMath;
        public StreamWriter mSWOrd;

        public MainWindow mMainWindow;

        public Recorder(String basePath, MainWindow mw)
        {
            mBasePath = basePath;
            mMainWindow = mw;

            bool bWriteHeader = false;
            String mathPath = mBasePath + mathTok + mMainWindow.mSubjectInfoString + ".txt";
            String orderPath = mBasePath + orderTok + mMainWindow.mSubjectInfoString + ".txt";

            if (!File.Exists(mathPath))
            {
                File.CreateText(mathPath).Close();

                if(File.Exists(orderPath))
                    File.Delete(orderPath);

                File.CreateText(orderPath).Close();

                bWriteHeader = true;
            }

            mSWMath = new StreamWriter(mathPath, true, Encoding.GetEncoding("gb2312"));
            mSWOrd = new StreamWriter(orderPath, true, Encoding.GetEncoding("gb2312"));

            if (bWriteHeader)
                writeHeader();
        }

        private void writeHeader()
        {
            String headerMath = "";
            String headerOrder = "";
            for (int j = 0; j < mMathHeaderMode.Length; j++)
            {
                headerMath += mMathHeaderMode[j] + "\t";
            }

            for (int i = 0; i < mOrderHeaderMode.Length; i++)
            {
                headerOrder += mOrderHeaderMode[i] + "\t";
            }

            mSWMath.WriteLine(headerMath);
            mSWOrd.WriteLine(headerOrder);
        }

        public void SaveRec(List<AOSpanItemGrp> content)
        {
            for (int i = 0; i < content.Count; i++)
            {
                
                for (int j = 0; j < content[i].Equations.Count; j++)
                {
                    String mathLine = "";
                    mathLine += content[i].Equations[j] + "\t";
                    mathLine += content[i].MathAnswers[j] + "\t";
                    mathLine += content[i].MathAnswerCorrectness[j].ToString() + "\t";
                    mathLine += content[i].MathRT[j].ToString() + "\t";
                    mSWMath.WriteLine(mathLine);
                }

                String orderLine = "";
                String CharacterStr = "";
                String CharaAnsStr = "";
                for(int k = 0; k < content[i].Characters.Count; k++)
                {
                    CharacterStr += content[i].Characters[k];
                }

                for (int l = 0; l < content[i].CharaAns.Count; l++)
                {
                    CharaAnsStr += content[i].CharaAns[l];
                }

                orderLine += CharacterStr + "\t" + CharaAnsStr + "\t" + 
                    content[i].OrderRT + "\t" + content[i].OrderCorrectness + "\t";
                mSWOrd.WriteLine(orderLine);
            }

            mSWMath.Close();
            mSWOrd.Close();
        }
    }
}
