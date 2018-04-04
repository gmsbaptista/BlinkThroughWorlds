using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float moveSpeed;
    private int worldIndex = -1; //-1 is physical, 1 is spirit
    public float worldCenter;
    private Animator animator;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0.0f));
        animator.SetFloat("Moving", Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 ? 1.0f : 0.0f);

        if (Input.GetMouseButtonDown(1))
        {
            var offset = transform.position.x - worldIndex * worldCenter;
            worldIndex /= -1;
            transform.position = new Vector3(worldIndex * worldCenter + offset, transform.position.y, transform.position.z);
        }
    }
}
