using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameStartController : MonoBehaviour, IState
    {
        public GameObject GameFieldObj; // Обьект родитель игрового поля
        public GameObject[] GameFieldGrid; // Игровое поле
        void Awake()
        {
            StateController.GameStart = this;
            StateController.StateDictionary.Add(StateController.EnumStateType.GameStart, this);
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Начало анимации - появление сетки
        /// </summary>
        private void _startAnim()
        {
            Tweener tweenScale = null;

            for (int i = 0; i < GameFieldGrid.Length; i++)
            {
                tweenScale = GameFieldGrid[i].transform.DOScaleX(1f, 1f);
            }

            tweenScale.OnComplete(_endAnim);
        }

        /// <summary>
        /// Окончание анимации - появление сетки
        /// </summary>
        private void _endAnim()
        {
            StateController.ChangeState(StateController.EnumStateType.TurnChoise);
        }

        /// <summary>
        /// Обновить матрицу в логике
        /// </summary>
        private void _updateDefultParam()
        {
            LogicController logic = StateController.Logic;
            logic.GameFieldMarkTypes = new LogicController.EnumFieldMarkType[3, 3];
        }

        public void StartState()
        {
            GameFieldObj.SetActive(true);
            _updateDefultParam();
            _startAnim();
        }

        public void EndState()
        {

        }

    }
}
