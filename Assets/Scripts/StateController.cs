using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public static class StateController
    {
       
        public enum EnumStateType
        {
            MainMenu,
            GameStart,
            TurnChoise,
            TurnAnim,
            Logic,
            GameEnd

        }

        public static EnumStateType CurrentState = EnumStateType.MainMenu;
        public static MainMenuController MainMenu;
        public static GameStartController GameStart;
        public static TurnChoiseController TurnChoise;
        public static TurnAnimController TurnAnim;
        public static LogicController Logic;
        public static GameEndController GameEnd;
        public static Dictionary<EnumStateType, IState> StateDictionary = new Dictionary<EnumStateType, IState>();


        /// <summary>
        /// Сменить стейт
        /// </summary>
        /// <param name="newState"></param>
        public static void ChangeState(EnumStateType newState)
        {
            StateDictionary[CurrentState].EndState();

            CurrentState = newState;

            Debug.Log("Current state : " + newState);

            StateDictionary[CurrentState].StartState();


        }
    }
}


