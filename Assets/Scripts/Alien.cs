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

    public Rigidbody head;
    public bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAlive)
        {
            Die();
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDeath);
        }
    }

    public void Die()
    {
        isAlive = false;
        head.GetComponent<Animator>().enabled = false;
        head.isKinematic = false;
        head.useGravity = true;
        head.GetComponent<SphereCollider>().enabled = true;
        head.gameObject.transform.parent = null;
        head.velocity = new Vector3(0, 26.0f, 3.0f);

        OnDestory.Invoke(); // notifies all listeners, including GameManager.
        OnDestory.RemoveAllListeners();
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDeath);

        head.GetComponent<SelfDestruct>().Initiate();
        Destroy(gameObject);
    }
}
