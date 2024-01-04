using System;
using UnityEngine;
using UnityEngine.UI;

namespace Test
{
    public enum ViewType 
    {
        None,
        Menu,
        Game
    }
    public class UIHandler : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private GameObject _mainView;
        [SerializeField] private GameObject _gameView;

        public static event Action OnStartAction;
        public static event Action OnBackAction;

        private void OnEnable()
        {
            _startButton.onClick.AddListener(OnStart);
            _exitButton.onClick.AddListener(OnExit);
            _backButton.onClick.AddListener(OnBack);
        }

        public void ShowView(ViewType type)
        {
            switch (type) 
            {
                case ViewType.None:
                    _mainView.SetActive(false);
                    _gameView.SetActive(false);
                    break;
                case ViewType.Menu:
                    _mainView.SetActive(true);
                    _gameView.SetActive(false);
                    break;
                case ViewType.Game:
                    _mainView.SetActive(false);
                    _gameView.SetActive(true);
                    break;
            }
        }

        private void OnStart()
        {
            OnStartAction?.Invoke();
            ShowView(ViewType.None);
        }

        private void OnBack()
        {
            OnBackAction?.Invoke();
            ShowView(ViewType.Menu);
        }

        private void OnExit() => Application.Quit();

        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(OnStart);
            _exitButton.onClick.RemoveListener(OnExit);
        }
    }
}