using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS8
{
    public interface IAnalysisView
    {
        event Action<string, int> GameStart;
        event Action<string, string> Register;
        event Action<string> WordEntered;
        event Action CancelGame;

        void SetLetters(string letters);

        void Update(string[] parameters);

        void FinalWords(Dictionary<string, List<string>> Words, string player1name, string player2name);

        void GameOver();
    }
}
