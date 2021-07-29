using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    public interface IGameStateSwitcher
    {
        public void SetNextState(GameState gameState);
    }
}