using UnityEngine;

namespace Test
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Ball : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        private float _speed;
        private float _duration = 0.2f;
        private Vector2 _initialScale;
        public void Initialize(Camera mainCamera, float speed)
        {
            if (_rigidbody2D == null)
            {
                _rigidbody2D = GetComponent<Rigidbody2D>();
            }
            _initialScale = transform.localScale;
            _speed = speed;

            SetRandomPosition(mainCamera);
            StartCoroutine(ScaleOverTime());
            SetRandomDirection();
        }

        private void SetRandomPosition(Camera mainCamera)
        {
            float cameraHeight = mainCamera.orthographicSize * 2;
            float cameraWidth = cameraHeight * mainCamera.aspect;

            float randomX = Random.Range(-cameraWidth / 2f, cameraWidth / 2f);
            float randomY = Random.Range(-cameraHeight / 2f, cameraHeight / 2f);

            Vector2 position = new Vector2(randomX, randomY);

            transform.position = position;
        }

        private void SetRandomDirection()
        {
            Vector2 direction = new Vector2(GetRandomCoordinate(), GetRandomCoordinate());
            direction.Normalize();
            _rigidbody2D.velocity = direction * _speed;
        }

        private float GetRandomCoordinate()
        {
           float randomValue = Random.Range(-1f, 1f);
            randomValue = randomValue == 0f ? Random.Range(0.1f, 1f) : randomValue;
            randomValue *= Mathf.Sign(randomValue);
            return randomValue;
        }

        private System.Collections.IEnumerator ScaleOverTime()
        {
            float elapsedTime = 0f;

            yield return new WaitUntil(() =>
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / _duration;

                transform.localScale = Vector2.Lerp(Vector2.zero, _initialScale, t);

                return elapsedTime >= _duration;
            });

            transform.localScale = new Vector2(1f, 1f);
        }
    }
}