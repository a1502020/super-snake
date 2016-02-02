using SuperSnake.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSnakeStandalone
{
    public class InitialGameStateReader
    {
        public InitialGameState Read(string fileName)
        {
            using (var reader = new StreamReader(fileName))
            {
                return this.Read(reader);
            }
        }

        public InitialGameState Read(TextReader reader)
        {
            try
            {
                // フィールドの名前と大きさ
                var name = reader.ReadLine();
                var sp = reader.ReadLine().Split(' ');
                var w = int.Parse(sp[0]);
                var h = int.Parse(sp[1]);

                // フィールドのセル
                var cells = new List<IList<CellState>>();
                for (var x = 0; x < w; ++x)
                {
                    var row = new List<CellState>();
                    for (var y = 0; y < h; ++y)
                    {
                        row.Add(new CellState(new ColorState(0, 0, 0), true));
                    }
                    cells.Add(row);
                }
                for (var y = 0; y < h; ++y)
                {
                    sp = reader.ReadLine().Split(' ');
                    for (var x = 0; x < w; ++x)
                    {
                        cells[x][y] = new CellState(
                            new ColorState(
                                int.Parse(sp[1 + x].Substring(1, 2), System.Globalization.NumberStyles.HexNumber),
                                int.Parse(sp[1 + x].Substring(3, 2), System.Globalization.NumberStyles.HexNumber),
                                int.Parse(sp[1 + x].Substring(5, 2), System.Globalization.NumberStyles.HexNumber)
                            ),
                            sp[0][x] == 'o'
                        );
                    }
                }

                // プレイヤー数
                var pc = int.Parse(reader.ReadLine());

                // プレイヤー
                var players = new List<InitialPlayerState>();
                for (var i = 0; i < pc; ++i)
                {
                    sp = reader.ReadLine().Split(' ');
                    var px = int.Parse(sp[0]);
                    var py = int.Parse(sp[1]);
                    var dir = Direction.Right;
                    sp[2] = sp[2].ToLower();
                    if (sp[2] == "right") dir = Direction.Right;
                    else if (sp[2] == "rightup") dir = Direction.RightUp;
                    else if (sp[2] == "up") dir = Direction.Up;
                    else if (sp[2] == "leftup") dir = Direction.LeftUp;
                    else if (sp[2] == "left") dir = Direction.Left;
                    else if (sp[2] == "leftdown") dir = Direction.LeftDown;
                    else if (sp[2] == "down") dir = Direction.Down;
                    else if (sp[2] == "rightdown") dir = Direction.RightDown;
                    else throw new FormatException();

                    players.Add(new InitialPlayerState(new PositionState(px, py), new DirectionState(dir)));
                }

                return new InitialGameState(new FieldState(name, cells), players);
            }
            catch (Exception e)
            {
                throw new FormatException("initial game state format error.", e);
            }
        }

        public GameState Combine(InitialGameState gameState, IList<PlayerInfo> playerInfos)
        {
            if (playerInfos.Count > gameState.PlayersCount)
            {
                throw new ArgumentException();
            }

            var players = new List<PlayerState>();
            for (var i = 0; i < playerInfos.Count; ++i)
            {
                var init = gameState.Players[i];
                var info = playerInfos[i];
                players.Add(new PlayerState(i, info.Name, info.Color, init.Position, init.Direction, true));
            }

            return new GameState(gameState.Field, players);
        }
    }
}
