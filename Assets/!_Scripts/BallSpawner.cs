using UnityEngine;
using UnityEngine.Pool;

namespace Test
{
    public class BallSpawner : MonoBehaviour
    {
        [SerializeField] private Ball _ballPrefab;
        [SerializeField] private float _minSpeed;
        [SerializeField] private float _maxSpeed;

        private ObjectPool<Ball> _pool;
        private Camera _mainCamera;

        private System.Collections.Generic.List<Ball> _balls;

        private void Start()
        {
            _mainCamera = Camera.main;
            _pool = new ObjectPool<Ball>(OnCreate, OnGet, OnRelease, OnDestroyAction, false);
        }

        private void OnEnable()
        {
            UIHandler.OnBackAction += ReleaseAllBalls; 
        }

        public void CreateBalls(int amount)
        {
            _balls = new();
            for (int i = 0; i < amount; i++)
            {
                Ball ball = _pool.Get();
                _balls.Add (ball);
                float speed = Random.Range(_minSpeed, _maxSpeed);
                ball.Initialize(_mainCamera, speed);
            }
        }

        private void ReleaseAllBalls()
        {
            foreach (Ball ball in _balls)
            {
                _pool.Release(ball);
            }
            _balls.Clear();
        }

        private void OnDisable()
        {
            UIHandler.OnBackAction -= ReleaseAllBalls;
        }

        #region ObjectPoolActions
        private Ball OnCreate() => Instantiate(_ballPrefab);
        private void OnGet(Ball ball) => ball.gameObject.SetActive(true);
        private void OnRelease(Ball ball) => ball.gameObject.SetActive(false);
        private void OnDestroyAction(Ball ball) => Destroy(ball.gameObject);
        #endregion
    }
}