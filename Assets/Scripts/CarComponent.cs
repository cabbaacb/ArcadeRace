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

            _input.OnHandBrake += OnHandBreake;

            _rigidbody.centerOfMass = _centerOfMass;
            
        }

        private void FixedUpdate()
        {
            var torque = _input.Acceleration * _torque / 2f;
            foreach (var wheel in _wheels.GetFrontWheels)
                wheel.motorTorque = torque;
            //Rotate
            _wheels.UpdateVisual(_maxSteerAngle * _input.Rotate);
        }

        private void OnHandBreake(bool value)
        {
            if(value)
            {
                foreach(var wheel in _wheels.GetRearWheels)
                {
                    wheel.brakeTorque = _handBrake;
                    wheel.motorTorque = 0f;
                }
            }
            else
            {
                foreach (var wheel in _wheels.GetRearWheels)
                {
                    wheel.brakeTorque = 0f;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.TransformPoint(_centerOfMass), 0.2f);
        }


        private void OnDestroy()
        {
            _input.OnHandBrake -= OnHandBreake;
        }
    }
}
