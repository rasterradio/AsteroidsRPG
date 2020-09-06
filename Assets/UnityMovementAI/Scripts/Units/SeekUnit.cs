﻿using UnityEngine;

namespace UnityMovementAI
{
    public class SeekUnit : MonoBehaviour
    {
        public Transform target;
        SteeringBasics steeringBasics;

        void Start()
        {
            steeringBasics = GetComponent<SteeringBasics>();
        }

        void OnEnable()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        void FixedUpdate()
        {
            if (target != null)
            {
                Vector3 accel = steeringBasics.Seek(target.position);

                steeringBasics.Steer(accel);
                steeringBasics.LookWhereYoureGoing();
            }
            else
            {
                Debug.Log("can't find target!");
            }
        }
    }
}