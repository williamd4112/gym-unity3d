using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GymUnity3D
{
    public delegate void OnCollisionEvent(Collision collision);
    public delegate void OnTriggerEvent(Collider collider);

    [RequireComponent(typeof(Collider))]
    public class CollisionEventTrigger : MonoBehaviour
    {
        public event OnCollisionEvent OnCollisionEnterEvent;
        public event OnCollisionEvent OnCollisionStayEvent;
        public event OnCollisionEvent OnCollisionExitEvent;

        public event OnTriggerEvent OnTriggerEnterEvent;
        public event OnTriggerEvent OnTriggerStayEvent;
        public event OnTriggerEvent OnTriggerExitEvent;

        void OnTriggerEnter(Collider collider)
        {
            if (OnTriggerEnterEvent != null)
            {
                OnTriggerEnterEvent.Invoke(collider);
            }
        }

        void OnTriggerStay(Collider collider)
        {
            if (OnTriggerStayEvent != null)
            {
                OnTriggerStayEvent.Invoke(collider);
            }
        }

        void OnTriggerExit(Collider collider)
        {
            if (OnTriggerExitEvent != null)
            {
                OnTriggerExitEvent.Invoke(collider);
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (OnCollisionEnterEvent != null)
            {
                OnCollisionEnterEvent.Invoke(collision);
            }
        }

        void OnCollisionStay(Collision collision)
        {
            if (OnCollisionStayEvent != null)
            {
                OnCollisionStayEvent.Invoke(collision);
            }
        }

        void OnCollisionExit(Collision collision)
        {
            if (OnCollisionExitEvent != null)
            {
                OnCollisionExitEvent.Invoke(collision);
            }
        }
    }
}
