using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cars
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Button _confirmButton;
        [SerializeField] private InputField _inputField;
        [SerializeField] private GameObject _panel;
        [SerializeField] private Text _time;

        private float _timeCount = 0f;
        private string _playerName;
        private static string path = Environment.CurrentDirectory + @"\Assets\RaceHistory.txt";

        // Start is called before the first frame update
        void Start()
        {
            Time.timeScale = 0f;
            _confirmButton.onClick.AddListener(StartTheGame);
        }

        private void StartTheGame()
        {
            _playerName = _inputField.text;
            Time.timeScale = 1f;
            //_timeCount = Time.time;
            StartCoroutine(TimeCount());
            _panel.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            PlayerInputController.OnFinish += EndTheGame;
        }

        private void OnDisable()
        {
            _confirmButton.onClick.RemoveAllListeners();
            PlayerInputController.OnFinish -= EndTheGame;
        }

        private IEnumerator TimeCount()
        {
            while (true)
            {
                _timeCount += Time.deltaTime;
                _time.text = _timeCount.ToString();
                yield return null;
            }
        }


        private void EndTheGame()
        {
            Time.timeScale = 0f;
            StopAllCoroutines();
        }

        /*

        static async Task WriteInfo()
        {

        }

        */



        public delegate void ObserverReadHandler(string playerColor, string componentName, bool isDestroyed, string whereToMove);
        public static event ObserverReadHandler OnObserverRead;

        private bool _stringIsready = true;
        /*
        private void OnEnable()
        {
            Player.OnObserverWrite += WriteInfo;
            Player.OnStringIsReady += StringIsReady;

            if (!_isPlaying)
                File.WriteAllText(path, string.Empty);
            else
                PlayInfo();
        }

        private void OnDisable()
        {
            Player.OnObserverWrite -= WriteInfo;
            Player.OnStringIsReady -= StringIsReady;
        }



        static async Task WriteInfo(ColorType color, string chipInfo, ChipCondition chipcondition, string cellInfo)
        {
            using (StreamWriter stream = new StreamWriter(path, true))
            {
                await Task.Run(() =>
                {
                    stream.WriteLine("{0} chip {1} was {2} at {3}", color, chipInfo, chipcondition, cellInfo);

                });
                //await stream.WriteAllLinesAsync(write);
                //stream.Write("{0} chip was {1} at {2}", color, chipcondition, cellInfo);
                //stream.WriteLine();
                //stream.WriteLine("Started");
            }
        }


        private void StringIsReady(bool stringIsReady)
        {
            _stringIsready = stringIsReady;
        }

        private void NextStringIsReady()
        {
            //bool isReady = false;


            return;
        }

        private async void PlayInfo()
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    string[] words = line.Split(' ');
                    bool destroy = (words[4] == "Destroyed");
                    string whereToMove = words[4] == "Moved" ? words[6] : null;
                    /*
                    Debug.Log(words[0]);
                    Debug.Log(words[2]);
                    Debug.Log(destroy);
                    Debug.Log(whereToMove);

                    OnObserverRead?.Invoke(words[0], words[2], destroy, whereToMove);
                    //await Task.Run(() => NextStringIsReady());
                    //await Task.Run()
                    //System.Threading.Thread.Sleep(10000);

                    while (!_stringIsready)
                    {
                        await Task.Run(() => NextStringIsReady());
                    }
                    _stringIsready = false;

                }
            }
        }


    }*/
    }
}
