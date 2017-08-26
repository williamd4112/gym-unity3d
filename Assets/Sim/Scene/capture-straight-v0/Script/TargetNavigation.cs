using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Capture_Straight
{
    public class TargetNavigation : MonoBehaviour
    {
        [SerializeField]
        private Transform m_Goal;

        // Use this for initialization
        void Start()
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = m_Goal.position;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
