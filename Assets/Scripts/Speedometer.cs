using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cars
{
    public class Speedometer : MonoBehaviour
    {
        private const float _convertFromMInSecToKmInH = 3.6f;
        private Transform _player;

        [SerializeField]
        private Color _minColor = Color.green;
        [SerializeField]
        private Color _maxColor = Color.red;
        [SerializeField]
        private float _maxSpeed = 300f;

        [Space, SerializeField, Range(0.1f, 1f)]
        private float _delay = 0.3f;
        [SerializeField]
        private Text _text;
        [SerializeField]
        private PlayerInputController _playerTarget;
        private void Start()
        {
            _player = _playerTarget.transform;
            StartCoroutine(Speed());
        }

        private IEnumerator Speed()
        {
            var prevPos = _player.position;
            while(true)
            {
                var distance = Vector3.Distance(_player.position, prevPos);
                var speed = Mathf.RoundToInt(distance / _delay * _convertFromMInSecToKmInH);

                _text.color = Color.Lerp(_minColor, _maxColor, speed / _maxSpeed);
                _text.text = speed.ToString();
                prevPos = _player.position;
                yield return new WaitForSeconds(_delay);

            }
        }

        

    }
}
