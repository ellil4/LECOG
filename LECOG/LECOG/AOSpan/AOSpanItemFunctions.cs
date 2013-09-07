using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LECOG.AOSpan
{
    public class AOSpanItemFunctions
    {

        public static int mMaxRank = 4;
        public static int mMinRank = 0;

        public static String[] CharsSet = { "内", "互", "升", "屯", "也", "凡", "及", "卡", "立", "刁", "亏", "石" };
        public Random mRdm;
        public AOSpanItemFunctions()
        {
            mRdm = new Random();
        }

        public AOSpanItemGrp GenItemGrp(int length)
        {
            AOSpanItemGrp retval = new AOSpanItemGrp();
            retval.Characters = genRandomChars(length);

            if (length % 2 != 0)
            {
                List<int> equationOne = genEquationByRank((mMaxRank - mMinRank) / 2 + mMinRank, false);
                retval.Equations.Add(getEquationText(equationOne));
                retval.MathAnswers.Add(equationOne[3]);
            }

            for (int k = 0; k < length / 2; k++)
            {
                int rank = mRdm.Next(mMinRank, mMaxRank + 1);

                List<int> equationRaw = genEquationByRank(rank, false);
                retval.Equations.Add(getEquationText(equationRaw));
                retval.MathAnswers.Add(equationRaw[3]);

                List<int> equationRaw2 = genEquationByRank(mMaxRank - rank, false);
                retval.Equations.Add(getEquationText(equationRaw2));
                retval.MathAnswers.Add(equationRaw2[3]);
            }

            return retval;
        }

        private int[] genDigitNum(int digiWidth)
        {
            int[] retval = new int[digiWidth];

            for (int i = 0; i < digiWidth; i++)
            {
                if (i != digiWidth - 1)
                {
                    retval[i] = mRdm.Next(0, 10);
                }
                else
                {
                    retval[i] = mRdm.Next(1, 9);
                }

            }

            return retval;
        }

        private List<String> genRandomChars(int count)
        {
            List<String> retval = new List<string>();

            List<String> waitList = new List<string>();
            for (int i = 0; i < CharsSet.Length; i++)
            {
                waitList.Add(CharsSet[i]);
            }

            for(int j = 0; j < count; j++)
            {
                int idx = mRdm.Next(0, waitList.Count);
                retval.Add(waitList[idx]);
                waitList.Remove(waitList[idx]);
            }

            return retval;
        }

        private String getEquationText(List<int> equationRaw)
        {
            String mark = "";
            if (equationRaw[1] == 1)
            {
                mark = " + ";
            }
            else
            {
                mark = " - ";
            }

            return equationRaw[0] + mark + equationRaw[2] + " = ?";
        }

        private List<int> genEquationByRank(int rank, bool allowNegative)
        {
            List<int> retval = null;

            switch (rank)
            {
                case 0:
                    retval = genMathEquation(genDigitNum(1), 0, allowNegative);
                    break;
                case 1:
                    retval = genMathEquation(genDigitNum(2), 0, allowNegative);
                    break;
                case 2:
                    retval = genMathEquation(genDigitNum(1), 1, allowNegative);
                    break;
                case 3:
                    retval = genMathEquation(genDigitNum(3), 0, allowNegative);
                    break;
                case 4:
                    retval = genMathEquation(genDigitNum(2), 1, allowNegative);
                    break;
                case 5:
                    retval = genMathEquation(genDigitNum(3), 1, allowNegative);
                    break;
                case 6:
                    retval = genMathEquation(genDigitNum(2), 2, allowNegative);
                    break;
                case 7:
                    retval = genMathEquation(genDigitNum(3), 2, allowNegative);
                    break;
                case 8:
                    retval = genMathEquation(genDigitNum(3), 3, allowNegative);
                    break;
            }

            return retval;
        }

        //returns firstNum, method(+1 = + or -1 = -), secondNum, result
        private List<int> genMathEquation(int[] digi1, int carryoverCt, bool allowNegative)
        {
            if(carryoverCt > digi1.Length)
            {
                return null;
            }

            List<int> retval = new List<int>();
            int digiWidth = digi1.Length;
            int[] digi2 = new int[digiWidth];

            int method = 0;

            if (mRdm.Next(0, 2) == 0)
            {
                method = -1;
            }
            else
            {
                method = 1;
            }

            if (carryoverCt == digi1.Length)//special situation no minus
            {
                method = 1;
            }

            //decide which digit pos have carryover
            bool[] carryMark = new bool[digiWidth];
            for (int j = 0; j < digiWidth; j++)
            {
                carryMark[j] = false;
            }

            List<int> poses = new List<int>();
            for (int k = 0; k < digiWidth; k++)
            {
                poses.Add(k);
            }

            for (int l = 0; l < carryoverCt; l++)
            {
                int chosenAt;
                if (allowNegative)
                {
                    chosenAt = mRdm.Next(0, poses.Count);
                }
                else
                {
                    chosenAt = mRdm.Next(0, poses.Count - 1);
                }
                carryMark[chosenAt] = true;
                poses.Remove(poses[chosenAt]);
            }

            //generate numbers
            for (int i = 0; i < digiWidth; i++)
            {
                int first = 0;
                int second = 0;
                if (carryMark[i] == true)//carry combinition
                {
                    if (method == 1)//add
                    {
                        first = digi1[i];
                        second = mRdm.Next(10 - first, 10);
                    }
                    else if (method == -1)//minus
                    {
                        first = digi1[i];
                        second = mRdm.Next(first + 1, 10);
                    }
                }
                else//non-carry conbinition
                {
                    if (method == 1)//add
                    {
                        first = digi1[i];
                        second = mRdm.Next(0, 10 - first - 1);
                        /*if (i != digiWidth - 1)
                        {

                        }
                        else//cannot be zero
                        {
                            first = digi1[i];
                            second = rdm.Next(1, 10 - first);
                        }*/
                    }
                    else if (method == -1)
                    {
                        first = digi1[i];
                        second = mRdm.Next(0, first);
                        /*if (i != digiWidth - 1)
                        {

                        }
                        else//cannot be zero
                        {
                            first = digi1[i];
                            second = rdm.Next(0, first);
                        }*/
                    }
                }

                digi2[i] = second;
            }

            int firstFinal = 0;
            int secondFinal = 0;

            for (int k = 0; k < digiWidth; k++)
            {
                firstFinal += digi1[k] * (int)Math.Pow(10, k);
                secondFinal += digi2[k] * (int)Math.Pow(10, k);
            }

            retval.Add(firstFinal);
            retval.Add(method);
            retval.Add(secondFinal);
            int result = 0;
            if (method == 1)
            {
                result = firstFinal + secondFinal;
            }
            else if (method == -1)
            {
                result = firstFinal - secondFinal;
            }

            retval.Add(result);

            return retval;
        }
    }
}
