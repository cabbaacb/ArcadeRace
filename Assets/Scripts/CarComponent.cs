using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cars
{
    [RequireComponent(typeof(BaseInputController), typeof(Rigidbody), typeof(WheelsComponent))]
    public class CarComponent : MonoBehaviour
    {
        private BaseInputController _input;
        private WheelsComponent _wheels;
        private Rigidbody _rigidbody;

        [SerializeField, Min(100f)]
        private float _torque = 2500f;
        [SerializeField, Range(10f, 50f)]
        private float _maxSteerAngle = 25f;
        [SerializeField, Range(0.1f, float.MaxValue)]
        private float _handBrake = float.MaxValue;
        [SerializeField]
        private Vector3 _centerOfMass;

        private void Start()
        {
            _input = GetComponent<BaseInputController>();
            _wheels = GetComponent<WheelsComponent>();
            _rigidbody = GetComponent<Rigidbody>();
            
        }

        private void FixedUpdate()
        {
            var torque = _input.Acceleration * _torque / 2f;
            foreach (var wheel in _wheels.GetFrontWheels)
                wheel.motorTorque = torque;
        }
    }
}
