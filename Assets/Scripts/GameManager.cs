using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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
        [SerializeField] private Text _showInfoText;

        private float _timeCount = 0f;
        private string _playerName;
        private int _counter;
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
            StringCounter();
            _counter++;
            _ = WriteInfo(_playerName, _timeCount, _counter);
            SortInfo();
            StopAllCoroutines();
        }

        

        static async Task WriteInfo(string playerName, float time, int counter)
        {
            using (StreamWriter stream = new StreamWriter(path, true))
            {
                await Task.Run(() =>
                {
                    stream.WriteLine("{0}. {1} finished at {2}", counter, playerName, time);

                });
            }
        }

        private void StringCounter()
        {
            int count = 0;

            using(StreamReader reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                    if (reader.Read() == '\n')
                        count++;
            }
            _counter = count;
        }
        
        private void SortInfo()
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string line1;
                string line2;
                string[] words1;
                string[] words2;
                float time1, time2;
                while((line1 = reader.ReadLine()) != null)
                {
                    string line = line1;
                    words1 = line1.Split(' ');
                    time1 = float.Parse(words1[4], System.Globalization.NumberStyles.Float);
                    line2 = reader.ReadLine();
                    words2 = line2.Split(' ');
                    time2 = float.Parse(words2[4], System.Globalization.NumberStyles.Float);

                    Debug.Log(time1);
                    print(time2);

                    if (time1 < time2)
                    {
                        string text = reader.ReadToEnd();
                        string str = line;
                        string str2 = line2;
                        text.Replace(line, "putline2here");
                        text.Replace(line2, "putlinehere");
                        text.Replace("putline2here", str2);
                        text.Replace("putlinehere", str);
                    }

                }
            }

            _showInfoText.gameObject.SetActive(true);

        }

        /*
        private void ReplaceStrings(string line1, string line2)
        {
            using (StreamWriter stream = new StreamWriter(path, true))
            {
                 stream.WriteLine("{0}. {1} finished at {2}", counter, playerName, time);
            }
        }
        
        private async void ReadInfo()
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                
                while ((line = await reader.ReadLineAsync()) != null)
                {


                    string[] words = line.Split(' ');
                    bool destroy = (words[4] == "Destroyed");
                    string whereToMove = words[4] == "Moved" ? words[6] : null;
                    
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
