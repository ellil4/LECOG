using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibTabCharter;

namespace LECOG.PaperFold
{
    public class Recorder
    {
        String mLocation;
        TabCharter mCharter;
        PagePFTest mPage;

        public Recorder(String location, PagePFTest page)
        {
            mLocation = location;
            mCharter = new TabCharter(location);
            mPage = page;
        }

        public void Save()
        {
            List<String> header = new List<string>();
            header.Add("UserChoice");
            header.Add("Correctness");
            mCharter.Create(header);

            List<String> content = new List<string>();
            for (int i = 0; i < PagePFTest.mStandardAns.Length; i++)
            {
                content.Clear();
                content.Add(mPage.mUserChoice[i].ToString());
                if (mPage.mUserChoice[i] == PagePFTest.mStandardAns[i])
                {
                    content.Add("True");
                }
                else
                {
                    content.Add("False");
                }

                mCharter.Append(content);
            }
        }

    }
}
