using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
        [SerializeField] private Text _countDownText;

        private float _timeCount = 0f;
        private string _playerName;
        private int _counter;
        private static string path = Environment.CurrentDirectory + @"\Assets\RaceHistory.txt";

        public static event Action<bool> OnUnblockInput;

        // Start is called before the first frame update
        void Start()
        {
            Time.timeScale = 0f;
            _confirmButton.onClick.AddListener(StartTheGame);
            _showInfoText.text = "";
        }

        private void StartTheGame()
        {

            _playerName = _inputField.text;
            Time.timeScale = 1f;
            //_timeCount = Time.time;
            //StartCoroutine(TimeCount());

            _panel.gameObject.SetActive(false);
            StartCoroutine(CountDown());
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
            //Time.timeScale = 1f;
            _showInfoText.text += "Your time is " + _time.text + "\n";
            StringCounter();
            _counter++;
            _ = WriteInfo(_playerName, _timeCount, _counter);
            //SortInfo();

            StopAllCoroutines();
            ShowInfo();
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
        
        private void ShowInfo()
        {
            
            Dictionary<string, float> info = new Dictionary<string, float>();

            using (FileStream file = File.Open(path, FileMode.OpenOrCreate))
            {
                StreamReader reader = new StreamReader(path);
                while (!reader.EndOfStream)
                {
                    string[] words = reader.ReadLine().Split(' ');
                    info.Add(words[1], float.Parse(words[4], System.Globalization.NumberStyles.Float));
                }

                file.Close();
            }

            //_showInfoText.gameObject.SetActive(true);
            foreach(var inf in info)
            {
                //string str = "{inf.Key} finished at {inf.Value}\n";

                _showInfoText.text += inf.Key + " finished at " + inf.Value + "\n";
                //print(str);
            }
        }


        private IEnumerator CountDown()
        {
            _countDownText.text = "3";
            yield return new WaitForSeconds(1f);
            _countDownText.text = "2";
            yield return new WaitForSeconds(1f);
            _countDownText.text = "1";
            yield return new WaitForSeconds(1f);
            _countDownText.text = "GO!";
            yield return new WaitForSeconds(1f);
            _countDownText.text = string.Empty;
            OnUnblockInput?.Invoke(true);
            StartCoroutine(TimeCount());
        }
    }
}
