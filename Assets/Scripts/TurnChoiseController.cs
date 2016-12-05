using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class TurnChoiseController : MonoBehaviour, IState
    {
        public Text TurnDescriptionText; // Описания, чей ход
        [HideInInspector]
        public LogicController.EnumFieldMarkType MarkType = LogicController.EnumFieldMarkType.XMark; // Текущ. знак

        void Awake()
        {
            StateController.TurnChoise = this;
            StateController.StateDictionary.Add(StateController.EnumStateType.TurnChoise, this);
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartState()
        {
            TurnDescriptionText.text = "TURN : " + (MarkType == LogicController.EnumFieldMarkType.XMark ? "X" : "O");
            if (StateController.Logic.PlayWithAI) // Для ИИ
            {
                if (StateController.Logic.OMarkAI && MarkType == LogicController.EnumFieldMarkType.OMark || !StateController.Logic.OMarkAI && MarkType == LogicController.EnumFieldMarkType.XMark)
                {
                    //_AiClick();
                    StartCoroutine(_waitCoroutine(0.5f, _aIClick));
                }
            }
        }

        public void EndState()
        {

        }


        /// <summary>
        /// ИИ "кликает"
        /// </summary>
        private void _aIClick()
        {
            _onClickGameField(Random.Range(0, 9), true);
        }

        /// <summary>
        /// Игрок кликает
        /// </summary>
        /// <param name="btnId"></param>
        public void ClickGameField(int btnId)
        {
            if (StateController.Logic.PlayWithAI && (StateController.Logic.OMarkAI && MarkType == LogicController.EnumFieldMarkType.OMark ||
                !StateController.Logic.OMarkAI && MarkType == LogicController.EnumFieldMarkType.XMark))
            {
                return;
            }
            _onClickGameField(btnId);
        }

        /// <summary>
        /// Выбор, где поставить знак
        /// </summary>
        /// <param name="btnId"></param>
        /// <param name="aIChoose"></param>
        private void _onClickGameField(int btnId, bool aIChoose = false)
        {
            if (StateController.CurrentState == StateController.EnumStateType.TurnChoise) // Проверка на стейт
            {
                int x = btnId / 3; // Превращаем индекс (0-8) в индекс двухмерного массива [,] 
                int y = btnId - (x * 3);
                if (StateController.Logic.GameFieldMarkTypes[x, y] == LogicController.EnumFieldMarkType.Empty) // Проверка доступности клетки
                {
                    StateController.Logic.GameFieldMarkTypes[x, y] = MarkType;
                    StateController.TurnAnim.FieldMarkPlaceId = btnId;
                    StateController.ChangeState(StateController.EnumStateType.TurnAnim);
                }
                else if (aIChoose) // запускаем рекурсию, пока АИ, не попадёт на свободную клетку
                {
                    _onClickGameField(Random.Range(0, 9), true);
                }

            }
        }
        private IEnumerator _waitCoroutine(float waitDelay, Action action)
        {
            yield return new WaitForSeconds(waitDelay);
            action();
        }
    }
}
