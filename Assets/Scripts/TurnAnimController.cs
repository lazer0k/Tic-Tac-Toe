using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class TurnAnimController : MonoBehaviour, IState
    {
        public Transform PointPrefab; // Префаб точки
        public GameObject XMarkPrefab; // Префаб Х
        public GameObject OMarkPrefab; // Префаб О
        public Transform[] FieldMarkTransforms; // Все родительские трансформы - клетки
        [HideInInspector]
        public int FieldMarkPlaceId; // ИД выбранной клетки
        [HideInInspector]
        public List<GameObject> MarksObjects = new List<GameObject>(); // Лист созданых префабов
        private DrawModel _currentDrawModel; // Модель для анимации крестика
        private Image _oMarkImg; // IMG для анимации нолика (можно было сделать как крестик, но так проще и оптимизирование)

        void Awake()
        {
            StateController.TurnAnim = this;
            StateController.StateDictionary.Add(StateController.EnumStateType.TurnAnim, this);
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
            _drawMark();
        }

        public void EndState()
        {

        }

        /// <summary>
        /// Запуск логики отрисовки знака
        /// </summary>
        private void _drawMark()
        {
            if (StateController.TurnChoise.MarkType == LogicController.EnumFieldMarkType.XMark)
            {
                _currentDrawModel = new DrawModel()
                {
                    Pos = new Vector3(-40f, 40f),
                    FirstItem = true
                };
                _drawXMark();
            }
            else
            {
                GameObject mark = Instantiate(OMarkPrefab, FieldMarkTransforms[FieldMarkPlaceId], false);
                _oMarkImg = mark.GetComponent<Image>();
                MarksObjects.Add(mark);
                _drawOMark();
            }

        }

        /// <summary>
        /// Рисовать Нолик
        /// </summary>
        private void _drawOMark()
        {
            _oMarkImg.fillAmount += 0.03f;
            if (_oMarkImg.fillAmount >= 1)
            {
                _endDrawMark();
            }
            else
            {
                StartCoroutine(_waitCoroutine(0.01f, _drawOMark));
            }
        }

        /// <summary>
        /// Рисовать крестик
        /// </summary>
        private void _drawXMark()
        {

            for (int i = 0; i < 2; i++)
            {
                Transform point = PoolBoss.Spawn(PointPrefab, Vector3.zero, FieldMarkTransforms[FieldMarkPlaceId].rotation, FieldMarkTransforms[FieldMarkPlaceId]);
                point.localPosition = _currentDrawModel.Pos;
                point.localScale = Vector3.one;

                _currentDrawModel.Pos -= new Vector3((_currentDrawModel.FirstItem ? -2 : 2), 2);


                if (_currentDrawModel.FirstItem)
                {
                    if (_currentDrawModel.Pos == new Vector3(42f, -42f))
                    {
                        _currentDrawModel.Pos = new Vector3(40f, 40f);
                        _currentDrawModel.FirstItem = false;
                    }
                }
                else
                {
                    if (_currentDrawModel.Pos == new Vector3(-42f, -42f))
                    {
                        // _endDrawMark();
                        StartCoroutine(_waitCoroutine(0.01f, _endDrawMark));// Для того, что бы мы увидели последнюю созданную точку, ставим задержку 0.01
                        return;
                    }
                }
            }

            StartCoroutine(_waitCoroutine(0.01f, _drawXMark));
        }

        /// <summary>
        /// Конец отрисовки знака
        /// </summary>
        private void _endDrawMark()
        {
            if (StateController.TurnChoise.MarkType == LogicController.EnumFieldMarkType.XMark)
            {
                var mark = Instantiate(XMarkPrefab, FieldMarkTransforms[FieldMarkPlaceId], false);
                mark.transform.SetAsFirstSibling();
                MarksObjects.Add(mark);
            }

            StartCoroutine(_waitCoroutine(0.01f, PoolBoss.DespawnAllPrefabs)); // для оптимизации

            StateController.ChangeState(StateController.EnumStateType.Logic);
        }


        private IEnumerator _waitCoroutine(float waitDelay, Action action)
        {
            yield return new WaitForSeconds(waitDelay);
            action();
        }
    }

    public struct DrawModel
    {
        public Vector3 Pos;
        public bool FirstItem;
    }
}
