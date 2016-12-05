namespace Assets.Scripts
{
    public interface IState
    {

        /// <summary>
        /// Старт класса стейт машины
        /// </summary>
        void StartState();

        /// <summary>
        /// Окончание класса стейт машины
        /// </summary>
        void EndState();
    }
}

