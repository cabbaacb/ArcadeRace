using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cars
{
    public class PlayerInputController : BaseInputController
    {
        private CarControls _controls;

        public static event Action OnFinish;

        private void Awake()
        {
            _controls = new CarControls();
            GameManager.OnUnblockInput += StartMoving;
            _controls.Car.HandBrake.performed += _ => CallHandBrake(true);
            _controls.Car.HandBrake.canceled += _ => CallHandBrake(false);
        }

        protected override void FixedUpdate()
        {
            var direction = _controls.Car.Rotate.ReadValue<float>();

            if(direction == 0f && Rotate != 0f)
            {
                Rotate = Rotate > 0f
                    ? Rotate - Time.fixedDeltaTime
                    : Rotate + Time.fixedDeltaTime;
            }
            else
            {
                Rotate = Mathf.Clamp(Rotate + (direction * Time.fixedDeltaTime), -1f, 1f);
            }

            Acceleration = _controls.Car.Acceleration.ReadValue<float>();

        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Finish>() != null)
            {
                OnFinish?.Invoke();
                //Time.timeScale = 0f;
            }
        }

        private void StartMoving(bool start)
        {
            if (start)
                _controls.Car.Enable();
        }


        private void OnDisable()
        {
            _controls.Car.Disable();
        }

        private void OnDestroy()
        {
            _controls.Dispose();
        }
    }
}