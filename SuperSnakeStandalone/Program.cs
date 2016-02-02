using DxLibDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSnakeStandalone
{
    class Program
    {
        static void Main(string[] args)
        {
            DX.ChangeWindowMode(DX.TRUE);
            DX.DxLib_Init();
            DX.SetDrawScreen(DX.DX_SCREEN_BACK);

            while (DX.ProcessMessage() == 0)
            {
                DX.ScreenFlip();
            }

            DX.DxLib_End();
        }
    }
}
