using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Alien : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    public float navigatioinUpdate;
    private float navigationTime = 0;

    public UnityEvent OnDestory;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

        if (target != null)
        {
            agent.destination = target.position;
            navigationTime += Time.deltaTime;
            if (navigationTime > navigatioinUpdate)
            {
                agent.destination = target.position;
                navigationTime = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Die();
        OnDestory.Invoke(); // notifies all listeners, including GameManager.
        OnDestory.RemoveAllListeners();

        SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDeath);
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
