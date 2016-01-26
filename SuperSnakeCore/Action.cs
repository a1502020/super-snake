using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSnake.Core
{
    /// <summary>
    /// ### 行動(action)
    /// * 生きているプレイヤーの1ステップ後の状態を決定する情報です。
    /// * 直進(straight)、右折(right turn)、左折(left turn)の3種類があります。
    /// * 詳細は「ゲームの進行」をご覧ください。
    /// </summary>
    public enum Action
    {
        Straight,
        Right,
        Left,
    }
}
