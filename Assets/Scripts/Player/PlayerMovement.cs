using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    Vector3 movement;
    Animator anim;
    Animator animWeapon;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength = 100f;
    PlayerShooting playerShooting;
    Vector3 lastPosition;
    float timeSinceLastUpdate;


    private void Awake()
    {
        // Memperoleh mask dari layer "Floor"
        floorMask = LayerMask.GetMask("Floor");
        // Get komponen Animator
        anim = GameObject.Find("Alice").GetComponent<Animator>();
        // Get komponen Rigidbody
        playerRigidbody = GetComponent<Rigidbody>();
        // Get komponen PlayerShooting
        playerShooting = FindObjectOfType<PlayerShooting>();
    }

    private void Start()
    {
        lastPosition = transform.position;
        timeSinceLastUpdate = Time.time;
    }

    private void FixedUpdate()
    {
        // Get nilai input horizontal (h) (-1, 0, 1)
        float h = Input.GetAxisRaw("Horizontal");
        // Get nilai input vertical (v) (-1, 0, 1)
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
        Animating(h, v);
    }

    // Method player sehingga dapat berjalan
    void Move(float h, float v)
    {
        // Set nilai x dan z
        movement.Set(h, 0f, v);

        // Normalize vector
        movement = movement.normalized * speed * Time.deltaTime;

        // Move player ke nilai position + movement
        playerRigidbody.MovePosition(transform.position + movement);
    }

    // Method player sehingga player dapat menghadap kursor
    void Turning()
    {
        // Buat Ray dari mouse position ke layar
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Buat raycast untuk floorHit
        RaycastHit floorHit;
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
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
    void Animating(float h, float v)
    {
        if (h == 0 && v == 0)
        {
            // Set all animation parameters to false
            anim.SetBool("IsRunForward", false);
            anim.SetBool("IsRunBackward", false);
            anim.SetBool("IsStrafeRight", false);
            anim.SetBool("IsStrafeLeft", false);
            return;
        }

        // Calculate the angle between the player's heading and movement direction
        float angle = Vector3.SignedAngle(transform.forward, new Vector3(h, 0, v), Vector3.up);

        // Set animation parameters based on angle
        bool isRunForward = angle >= -45 && angle <= 45;
        bool isRunBackward = angle >= 135 || angle <= -135;
        bool isStrafeRight = angle > 45 && angle < 135;
        bool isStrafeLeft = angle < -45 && angle > -135;

        // Set animation parameters
        anim.SetBool("IsRunForward", isRunForward);
        anim.SetBool("IsRunBackward", isRunBackward);
        anim.SetBool("IsStrafeRight", isStrafeRight);
        anim.SetBool("IsStrafeLeft", isStrafeLeft);

        if (playerShooting.areOtherWeaponsActive)
        {
            // animWeapon = GameObject.FindGameObjectWithTag("Weapon").GetComponent<Animator>(); ;
            // animWeapon.SetBool("IsWalking", isRunForward);
        }
    }
}
