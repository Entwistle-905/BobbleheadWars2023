using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Alien : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    public float navigatioinUpdate;
    private float navigationTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = target.position;

        if (target != null)
        {
            navigationTime += Time.deltaTime;
            if (navigationTime > navigatioinUpdate)
            {
                agent.destination = target.position;
                navigationTime = 0;
            }
        }
    }
}
