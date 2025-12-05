using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 5.0f;
    private Rigidbody rb;
    private Animator animator; // Tambahkan ini

    void Start() {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>(); // Ambil komponen Animator
    }

    void FixedUpdate() {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Arah gerak relatif ke kamera
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 move = (forward * moveZ + right * moveX).normalized;

        rb.MovePosition(transform.position + move * moveSpeed * Time.fixedDeltaTime);

        // ?? Update parameter animator
        float speed = move.magnitude;
        animator.SetFloat("Speed", speed); // pastikan parameternya sama di Animator

        // ?? Opsional: rotasi menghadap arah gerak
        if (move != Vector3.zero) {
            transform.forward = move;
        }
    }
}
