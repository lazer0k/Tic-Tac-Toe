using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class LogicController : MonoBehaviour, IState
    {
        /// <summary>
        /// Типы заполнения поля
        /// </summary>
        public enum EnumFieldMarkType
        {
            Empty = 0,
            XMark = 1,
            OMark = 2,
        }

        /// <summary>
        /// Типы сложности ИИ
        /// </summary>
        public enum EnumDifficultyLevels
        {
            Easy = 0,
            Normal = 1,
            Hard = 2
        }

        [HideInInspector]
        public EnumFieldMarkType[,] GameFieldMarkTypes; // Игровое поле
        [HideInInspector]
        public EnumDifficultyLevels Difficulty; // Сложность
        [HideInInspector]
        public bool PlayWithAI; // Есть ИИ

        [HideInInspector]
        public bool OMarkAI; // ИИ играет ноликом

        private int[][,] _winCoods = new int[][,] // Выиграшные линии
        {
            new int[,] { {0,0}, {0,1}, {0,2} },
            new int[,] { {1,0}, {1,1}, {1,2} },
            new int[,] { {2,0}, {2,1}, {2,2} },
            new int[,] { {0,0}, {1,0}, {2,0} },
            new int[,] { {0,1}, {1,1}, {2,1} },
            new int[,] { {0,2}, {1,2}, {2,2} },
            new int[,] { {0,0}, {1,1}, {2,2} },
            new int[,] { {0,2}, {1,1}, {2,0} },
        };

        private Dictionary<int[,], Transform[]> _objRefDictionary; // ссылка линии, на трансформы в этих клетках

        void Awake()
        {
            StateController.Logic = this;
            StateController.StateDictionary.Add(StateController.EnumStateType.Logic, this);
        }

        // Use this for initialization
        void Start()
        {
            var fieldMark = StateController.TurnAnim.FieldMarkTransforms;
            _objRefDictionary = new Dictionary<int[,], Transform[]>()
            {
                {_winCoods[0], new Transform[] { fieldMark[0], fieldMark[1], fieldMark[2] } },
                {_winCoods[1], new Transform[] { fieldMark[3], fieldMark[4], fieldMark[5] } },
                {_winCoods[2], new Transform[] { fieldMark[6], fieldMark[7], fieldMark[8] } },
                {_winCoods[3], new Transform[] { fieldMark[0], fieldMark[3], fieldMark[6] } },
                {_winCoods[4], new Transform[] { fieldMark[1], fieldMark[4], fieldMark[7] } },
                {_winCoods[5], new Transform[] { fieldMark[2], fieldMark[5], fieldMark[8] } },
                {_winCoods[6], new Transform[] { fieldMark[0], fieldMark[4], fieldMark[8] } },
                {_winCoods[7], new Transform[] { fieldMark[6], fieldMark[4], fieldMark[2] } },
            };
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartState()
        {
            if (!_checkWin())
            {
                if (_checkDraw())
                {
                    _endGame(true);
                }
                else
                {
                    StateController.TurnChoise.MarkType = StateController.TurnChoise.MarkType == EnumFieldMarkType.XMark ? EnumFieldMarkType.OMark : EnumFieldMarkType.XMark;
                    StateController.ChangeState(StateController.EnumStateType.TurnChoise);
                }
            }
            else
            {
                _endGame(false);
            }
        }

        private void _endGame(bool draw)
        {

            StateController.GameEnd.GameResult = draw ? GameEndController.EnumGameResult.Draw : (GameEndController.EnumGameResult)(StateController.TurnChoise.MarkType);

            StateController.ChangeState(StateController.EnumStateType.GameEnd);
        }

        public void EndState()
        {

        }

        /// <summary>
        /// Проверка ничьей
        /// </summary>
        /// <returns></returns>
        private bool _checkDraw()
        {
            return GameFieldMarkTypes.Cast<EnumFieldMarkType>().All(type => type != EnumFieldMarkType.Empty);
        }

        /// <summary>
        /// Проверка выигрыша
        /// </summary>
        /// <returns></returns>
        private bool _checkWin()
        {
            for (int i = 0; i < _winCoods.Length; i++)
            {
                bool win = true;
                for (int j = 1; j < _winCoods[i].Length / 2f; j++)
                {
                    if (GameFieldMarkTypes[_winCoods[i][j, 0], _winCoods[i][j, 1]] == EnumFieldMarkType.Empty ||
                        (GameFieldMarkTypes[_winCoods[i][j, 0], _winCoods[i][j, 1]] != GameFieldMarkTypes[_winCoods[i][0, 0], _winCoods[i][0, 1]]))
                    {
                        win = false;
                        break;
                    }

                }
                if (win)
                {
                    var parentTransofrm = _objRefDictionary[_winCoods[i]];
                    for (int j = 0; j < parentTransofrm.Length; j++)
                    {
                        StateController.GameEnd.WinMarks[j] = parentTransofrm[j].GetChild(0);
                    }
                    return true;
                }
            }

            return false;
        }
    }
}
