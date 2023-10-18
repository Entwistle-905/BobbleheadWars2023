using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 50.0f;
    private CharacterController characterController;
    public Rigidbody head;
    public LayerMask layerMask;
    private Vector3 currentLookTarget = Vector3.zero;
    public Animator bodyAnimator;
    public float[] hitForce;

    public float timeBetweenHits = 2.5f;    // grace period after the hero sustains damage
    private bool isHit = false;             // a flag that indicates the hero took a hit
    private float timeSinceHit = 0;         // tracks amount of time in the grace period
    private int hitNumber = -1;             // the hero took a hit

    public Rigidbody marineBody;
    //private bool isDead = false;

    private DeathParticles deathParticles;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        deathParticles = gameObject.GetComponentInChildren<DeathParticles>();
    }

    // Update is called once per frame
/*    void Update()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0,
                        Input.GetAxis("Vertical"));
        characterController.SimpleMove(moveDirection * moveSpeed);
    }*/

    private void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0,
                        Input.GetAxis("Vertical"));
        characterController.SimpleMove(moveDirection * moveSpeed);
        if (moveDirection == Vector3.zero)
        {
            //characterController.SimpleMove(moveDirection * moveSpeed);
            bodyAnimator.SetBool("IsMoving", false);
        }
        else
        {
            head.AddForce(transform.right * 150, ForceMode.Acceleration);
            bodyAnimator.SetBool("IsMoving", true);
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);

        Debug.Log(Physics.Raycast(ray, out hit, 1000, layerMask, QueryTriggerInteraction.Ignore));

        if (Physics.Raycast(ray, out hit, 9999, layerMask, QueryTriggerInteraction.Ignore))
        {
            
            if (hit.point != currentLookTarget)
            {
                currentLookTarget = hit.point;
            }
            // 1
            Vector3 targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            // 2
            Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
            // 3
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10.0f);
        }

        if (isHit)
        {
            timeSinceHit += Time.deltaTime;
            if (timeSinceHit > timeBetweenHits)
            {
                isHit = false;
                timeSinceHit = 0;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Alien alien = other.gameObject.GetComponent<Alien>();
        if (alien != null)
        {
            if (!isHit)
            {
                hitNumber += 1;
                CameraShake cameraShake = Camera.main.GetComponent<CameraShake>();
                if (hitNumber < hitForce.Length)
                {
                    cameraShake.intensity = hitForce[hitNumber];
                    cameraShake.Shake();
                }
                else
                {
                    Die();
                }
                isHit = true;
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.hurt);
            }
            alien.Die();
        }
    }


    public void Die()
    {
        bodyAnimator.SetBool("IsMoving", false);
        marineBody.transform.parent = null;
        marineBody.isKinematic = false;
        marineBody.useGravity = true;
        marineBody.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        marineBody.gameObject.GetComponent<Gun>().enabled = false;

        Destroy(head.gameObject.GetComponent<HingeJoint>());
        head.transform.parent = null;
        head.useGravity = true;
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.marineDeath);

        deathParticles.Activate();
        Destroy(gameObject);
    }
}
