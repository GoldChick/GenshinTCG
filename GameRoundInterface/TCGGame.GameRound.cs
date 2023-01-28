using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCGGame
{
    public interface IGameStage : IGameBase
    {
        RoundStage GetRoundStage();

    }
}