using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Test
{
    public class RequestHandler : MonoBehaviour
    {
        [SerializeField] private string _url = "https://devtest.memoryos.com/test208/";
        [SerializeField] private BallSpawner _ballSpawner;
        [SerializeField] private WebViewHandler _webViewHandler;
        [SerializeField] private UIHandler _uiHandler;

        private void OnEnable()
        {
            UIHandler.OnStartAction += MakeRequest;
        }

        public void MakeRequest() => StartCoroutine(Request());

        private IEnumerator Request()
        {
            UnityWebRequest request = UnityWebRequest.Get(_url);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string response = request.downloadHandler.text;
                HandleResponse(response);
            }
            else
            {
                Debug.Log("Error: " + request.error);
                _uiHandler?.ShowView(ViewType.Menu);
            }
        }

        private void HandleResponse(string response)
        {
            if (IsNumeric(response))
            {
                _uiHandler?.ShowView(ViewType.Game);
                int numberOfBalls = Convert.ToInt32(response);
                _ballSpawner?.CreateBalls(numberOfBalls);
            }
            else if (Uri.IsWellFormedUriString(response, UriKind.Absolute))
            {
                _uiHandler?.ShowView(ViewType.Menu);
                _webViewHandler?.OpenWebView(response);
            }
        }

        private bool IsNumeric(string str)
        {
            return int.TryParse(str, out _);
        }

        private void OnDisable()
        {
            UIHandler.OnStartAction -= MakeRequest;
        }
    }
}