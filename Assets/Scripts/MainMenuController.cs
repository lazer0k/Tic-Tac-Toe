using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class MainMenuController : MonoBehaviour, IState
    {
        public GameObject MainMenuObj; //Обьект Меню
        public Toggle PlayWithAIToggle; // Чекбокс игра с Комп.
        public Toggle AIOMarkToggle; // Чекбокс Комп. играет на Нолике
        public Dropdown DifficultyDropdown; // Нереализованый выбор уровня сложности

        void Awake()
        {
            StateController.MainMenu = this;
            StateController.StateDictionary.Add(StateController.EnumStateType.MainMenu, this);
        }

        // Use this for initialization
        void Start()
        {
            StartState();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartState()
        {
            MainMenuObj.SetActive(true);
        }

        public void EndState()
        {
            StateController.Logic.PlayWithAI = PlayWithAIToggle.isOn;
            StateController.Logic.Difficulty = (LogicController.EnumDifficultyLevels)DifficultyDropdown.value;
            StateController.Logic.OMarkAI = AIOMarkToggle.isOn;

            MainMenuObj.SetActive(false);
        }

        /// <summary>
        /// Событие на нажатие кнопки старт
        /// </summary>
        public void StartGameBtnClick()
        {
            StateController.ChangeState(StateController.EnumStateType.GameStart);
        }
    }
}
