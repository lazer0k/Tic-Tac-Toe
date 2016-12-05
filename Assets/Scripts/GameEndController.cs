using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameEndController : MonoBehaviour, IState
    {
        /// <summary>
        /// Типы итогов игры
        /// </summary>
        public enum EnumGameResult
        {
            WinO = 2,
            WinX = 1,
            Draw = 0
        }

        public GameObject DrawObj; // Обьект родитель для анимации ничьей
        public GameObject ResultObj; // Обьект родитель для показа результатов (всех)
        public Text ResultText; // Текст - результат игры
        public GameObject MenuBtnObject; // Кнопка перехода в меню

        [HideInInspector]
        public EnumGameResult GameResult; // Результат игры

        [HideInInspector]
        public Transform[] WinMarks = new Transform[3]; // Выигрышная линия

        void Awake()
        {
            StateController.GameEnd = this;
            StateController.StateDictionary.Add(StateController.EnumStateType.GameEnd, this);
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
            ResultObj.SetActive(true);
            if (GameResult == EnumGameResult.Draw)
            {
                _showDrawAnim();
            }
            else
            {
                _showWinAnim();
            }
            StateController.TurnChoise.TurnDescriptionText.text = "";
        }

        /// <summary>
        /// Начало показа анимации выиграша
        /// </summary>
        private void _showWinAnim()
        {
            Tweener tween = null;
            for (int i = 0; i < WinMarks.Length; i++)
            {
                tween = WinMarks[i].DOMove(new Vector3(Screen.width / 2f, Screen.height / 2f), 1f);
            }
            tween.OnComplete(_showWinMark);


        }

        /// <summary>
        /// Анимация выиграша
        /// </summary>
        private void _showWinMark()
        {
            WinMarks[0].gameObject.SetActive(false);
            WinMarks[1].gameObject.SetActive(false);

            Tweener tween = WinMarks[2].DOScale(new Vector3(2, 2, 2), 2);
            tween.OnComplete(_showMenuBtn);

            var grid = StateController.GameStart.GameFieldGrid;
            for (int i = 0; i < grid.Length; i++)
            {
                grid[i].transform.DOScaleX(0, 1f);
            }
            var marks = StateController.TurnAnim.MarksObjects;
            for (int i = 0; i < marks.Count; i++)
            {
                if (marks[i].transform != WinMarks[2])
                {
                    marks[i].transform.DORotate(new Vector3(0, 0, 180), 2);
                    marks[i].transform.DOScale(Vector3.zero, 2);
                }
            }
            ResultText.text = GameResult == EnumGameResult.WinO ? "O WIN!" : "X WIN!";
            ResultText.transform.localScale = Vector3.zero;
            ResultText.transform.DOScale(Vector3.one, 1.5f);
        }

        /// <summary>
        /// Показ кнопки перехода в меню
        /// </summary>
        private void _showMenuBtn()
        {
            MenuBtnObject.SetActive(true);
        }

        /// <summary>
        /// Показ аанимации ничьей
        /// </summary>
        private void _showDrawAnim()
        {

            var grid = StateController.GameStart.GameFieldGrid;
            for (int i = 0; i < grid.Length; i++)
            {
                grid[i].transform.DOScaleX(0, 1f);
            }
            var marks = StateController.TurnAnim.MarksObjects;
            for (int i = 0; i < marks.Count; i++)
            {
                marks[i].transform.DORotate(new Vector3(0, 0, 180), 2);
                marks[i].transform.DOScale(Vector3.zero, 2);
            }
            DrawObj.transform.localScale = Vector3.zero;
            DrawObj.SetActive(true);
            DrawObj.transform.DOScale(Vector3.one, 1.5f);
            ResultText.text = "DRAW!";
            ResultText.transform.localScale = Vector3.zero;
            var tween = ResultText.transform.DOScale(Vector3.one, 2f);
            tween.OnComplete(_showMenuBtn);
        }

        /// <summary>
        /// Переход в меню
        /// </summary>
        public void GoToMenu()
        {
            StateController.ChangeState(StateController.EnumStateType.MainMenu);
        }

        public void EndState()
        {
            ResultText.text = "";
            ResultObj.SetActive(false);
            var marks = StateController.TurnAnim.MarksObjects;
            for (int i = marks.Count - 1; i >= 0; i--)
            {
                Destroy(marks[i]);
            }
            StateController.TurnAnim.MarksObjects = new List<GameObject>();
            MenuBtnObject.SetActive(false);
            DrawObj.SetActive(false);
            ResultObj.SetActive(false);
        }
    }
}
