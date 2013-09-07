using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibTabCharter;

namespace LECOG.DigiSymb
{
    public class Recorder
    {
        public DigiSymbRunner mRunner;
        TabCharter mCharter;

        public Recorder(DigiSymbRunner runner, String location)
        {
            mRunner = runner;
            mCharter = new TabCharter(location);
            mCharter.Create(genHeader());
        }

        private List<String> genHeader()
        {
            List<String> retval = new List<string>();
            retval.Add("Shown");
            retval.Add("Answer");
            retval.Add("ResponseSpot");
            retval.Add("Correctness");
            return retval;
        }

        public void Save()
        {
            List<String> content = new List<string>();
            for (int i = 0; i < mRunner.mUserAnswer.Count; i++)
            {
                content.Clear();

                content.Add(DigiSymbRunner.mNumScheme[i].ToString());
                content.Add(mRunner.mUserAnswer[i].ToString());
                content.Add(mRunner.mRTPoints[i].ToString());
                if(mRunner.mUserAnswer[i] == DigiSymbRunner.mNumScheme[i])
                {
                    content.Add("true");
                }
                else
                {
                    content.Add("false");
                }

                mCharter.Append(content);
            }
        }
    }
}
