using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS8
{
    public interface IAnalysisView
    {
        event Action<string, string> GameStart;






        void SetLetters(List<string> LetterList);


    }
}
