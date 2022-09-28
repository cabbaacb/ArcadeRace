using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cars
{
    public class BotInputController : MonoBehaviour
    {
        private int _index = 0;

        [SerializeField]
        private BotTargetPoint[] _targetPoints;

        private float GetAngle()
        {
            var target = _targetPoints[_index].transform.position;
            target.y = transform.position.y;

            var direction = target - transform.position;
            return Vector3.SignedAngle(direction, transform.forward, transform.up);
        }

        private void FixedUpdate()
        {
            var targetPoint = _targetPoints[_index].transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<BotTargetPoint>() != null)
                _index++;
        }
    }
}
