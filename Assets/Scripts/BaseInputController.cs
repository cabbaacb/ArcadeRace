﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Cars
{
    public abstract class BaseInputController : MonoBehaviour
    {
        public float Acceleration { get; protected set; }
        public float Rotate { get; protected set; }

        public event Action<bool> OnHandBrake;

        protected abstract void FixedUpdate();


        protected void CallHandBrake(bool value)
            => OnHandBrake?.Invoke(value);

    }
}
