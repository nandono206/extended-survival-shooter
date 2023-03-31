using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength = 100f;

    private void Awake() {
        // Memperoleh mask dari layer "Floor"
        floorMask = LayerMask.GetMask("Floor");
        // Get komponen Animator
        anim = GetComponent<Animator>();
        // Get komponen Rigidbody
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        // Get nilai input horizontal (h) (-1, 0, 1)
        float h = Input.GetAxisRaw("Horizontal");
        // Get nilai input vertical (v) (-1, 0, 1)
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
        Animating(h, v);
    }

    // Method player sehingga dapat berjalan
    void Move(float h, float v) {
        // Set nilai x dan z
        movement.Set(h, 0f, v);

        // Normalize vector
        movement = movement.normalized * speed * Time.deltaTime;

        // Move player ke nilai position + movement
        playerRigidbody.MovePosition(transform.position + movement);
    }

    // Method player sehingga player dapat menghadap kursor
    void Turning() {
        // Buat Ray dari mouse position ke layar
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Buat raycast untuk floorHit
        RaycastHit floorHit;
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask)) {
            // Get vector dari player dan floorHit position
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            // Get look rotation baru ke hit position
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            // Rotasi player
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    // Method untuk scripting animasi player
    void Animating(float h, float v) {
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }
}
