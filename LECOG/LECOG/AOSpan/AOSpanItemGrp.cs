using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LECOG.AOSpan
{
    public class AOSpanItemGrp
    {
        public List<int> MathAnswers;
        public List<String> Equations;
        public List<bool> MathAnswerCorrectness;
        public List<int> MathRT;

        public List<String> Characters;
        public List<String> CharaAns;
        public int OrderRT;
        public bool OrderCorrectness;

        public AOSpanItemGrp()
        {
            Characters = new List<string>();
            MathAnswers = new List<int>();
            Equations = new List<string>();
            MathAnswerCorrectness = new List<bool>();
            OrderCorrectness = false;
            MathRT = new List<int>();
        }

        public int GetLength()
        {
            return Characters.Count;
        }
    }
}
