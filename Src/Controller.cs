using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    class Controller
    {
        private readonly IInput input;

        private Block activeBlock;

        private readonly FileManager<ConfigData> fileManager;

        private ConfigManager configManager;

        private ConfigData data;

        public Controller(IInput input)
        {
            fileManager = new FileManager<ConfigData>(Constants.configPath);
            data = fileManager.LoadData();

            this.input = input;
        }

        public void Update()
        {
            data = fileManager.LoadData();
        }

        public void HandleInput(Grid grid, bool blockRefresh)
        {
            configManager = new ConfigManager(data);

            if (input.IsKeyJustPressed(configManager.GetKeyBinding("rotate cw"))) { activeBlock.ClampedRotateCounterClockwise(grid); }
            if (input.IsKeyJustPressed(configManager.GetKeyBinding("rotate ccw"))) { activeBlock.ClampedRotateClockwise(grid); }
            if (input.IsKeyJustPressed(configManager.GetKeyBinding("left"))) { activeBlock.HorizontalTranslation(-1); }
            if (input.IsKeyJustPressed(configManager.GetKeyBinding("right"))) { activeBlock.HorizontalTranslation(1); }
            if (input.IsKeyDown(configManager.GetKeyBinding("down")) && blockRefresh) { activeBlock.VerticalTranslation(1/2); }
        }

        public void SetActiveBlock(Block block)
        {
            activeBlock = block;
        }
    }
}
