using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float drag;
    [SerializeField] private float animationDuration;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask deathLayer;
    [Header("Objects")]
    [SerializeField] private GameObject attachments;
    [SerializeField] private Transform playerCam;
    [SerializeField] private Material sphereMaterial;
    [Header("Sounds")]
    [SerializeField] private AudioClip fallDownSound;
    [SerializeField] private AudioClip fallingInAirSound;
    [SerializeField] private AudioClip teleportSound;

    private ParticleSystem groundParticles;
    private Rigidbody sphereRigidBody;
    private LayerMask iceLayer;
    private AudioSource audioSourceMain;
    private AudioSource audioSourceFalling;
    public bool isOnJumpBoost = false;
    public bool isOnSpeedBoost = false;
    private bool isGrounded;
    private bool IsGrounded
    {
        get { return isGrounded; }
        set
        {
            if (value != isGrounded) 
            {
                isGrounded = value;

                if (isGrounded)
                {
                    Destroy(audioSourceFalling);

                    audioSourceMain.clip = fallDownSound;
                    float rand = Random.Range(0.5f, 0.8f);
                    audioSourceMain.volume = rand;
                    audioSourceMain.pitch = rand + 0.4f;
                    audioSourceMain.spatialBlend = 1f;
                    audioSourceMain.Play();
                }
                else
                {
                    audioSourceFalling = gameObject.AddComponent<AudioSource>();
                    audioSourceFalling.clip = fallingInAirSound;
                    audioSourceFalling.loop = true;
                    audioSourceFalling.volume = 0f;
                    audioSourceFalling.Play();
                }
            }
        }
    }
    private bool isOnIce;
    private const float speedinterval = 100f;

    private void Start()
    {
        sphereRigidBody = GetComponent<Rigidbody>();
        iceLayer = LayerMask.GetMask("Ice");
        groundParticles = attachments.transform.Find("GroundParticles").GetComponent<ParticleSystem>();
        audioSourceMain = GetComponent<AudioSource>();
    }

    [SerializeField][Range(0.1f, 2f)] float test;  
    void Update()
    {
        IsPlayerOnLayer();
        PlayerAttachmentsMovement();
        ChangePlayerMaterial();
        if (!isGrounded) PlayFallingSound();
        if (Input.GetKey(KeyCode.K))
        {
            audioSourceMain.pitch = test;
            audioSourceMain.Play();
        }
    }

    private void FixedUpdate()
    {
        IsGrounded = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), 0.2f, groundLayer);
        PlayerSphereMovement();
    }

    private void PlayerSphereMovement()
    {
        sphereRigidBody.drag = isGrounded ? (isOnIce ? 0.2f : drag) : 0.1f;
        if (!isGrounded) return;

        float speedForce = speedinterval * Time.deltaTime;
        Vector3 direction = (playerCam.right * Input.GetAxis("Horizontal")) + (playerCam.forward * Input.GetAxis("Vertical"));
        direction.y = 0f;
        sphereRigidBody.AddForce(direction * speed * speedForce * (isOnSpeedBoost ? 4f : 1f));
        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 jump = new Vector3(0f, jumpHeight, 0f) * speedForce * (isOnJumpBoost ? 12f : 4f);
            Debug.Log(jump);
            sphereRigidBody.AddForce(jump);
        }
    }

    private void PlayerAttachmentsMovement() => attachments.transform.position = transform.position;

    private void ChangePlayerMaterial()
    {
        float th = sphereMaterial.GetFloat("_TreshHold");
        sphereMaterial.SetColor("_EmissionColor", isGrounded ? new Color(13f, 0f, 0f, 1f) : new Color(1f, 0f, 0f, 1f));
        if (isGrounded)
        {
            sphereMaterial.SetFloat("_TreshHold", th < 0.34f ? th + (animationDuration * Time.deltaTime) : 0.34f);
            var main = groundParticles.main;
            main.maxParticles = 5;
        }
        else
        {
            sphereMaterial.SetFloat("_TreshHold", th > 0f ? th - (animationDuration * Time.deltaTime) : 0f);
            var main = groundParticles.main;
            main.maxParticles = 0;
        }
    }

    private void IsPlayerOnLayer()
    {
        if (Physics.CheckSphere(transform.position, 0.6f, deathLayer))
        {
            sphereRigidBody.velocity = Vector3.zero;
            sphereRigidBody.angularVelocity = Vector3.zero;
            transform.position = GameManager.Instance.GetLastCheckPointPosition();
            audioSourceMain.clip = teleportSound;
            audioSourceMain.volume = 0.8f;
            audioSourceMain.pitch = Random.Range(0.9f, 1.2f);
            audioSourceMain.spatialBlend = 0.7f;
            audioSourceMain.Play();
        }

        isOnIce = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), 0.1f, iceLayer);
    }

    private Vector3 lastPlayerPos;
    private void PlayFallingSound()
    {
        if (audioSourceFalling == null) return;
        Vector3 p1 = transform.position;
        Vector3 p2 = lastPlayerPos;
        p1.x = p1.z = p2.x = p2.z = 0f;
        float distance = Vector3.Distance(p1, p2) * 2;
        audioSourceFalling.volume = (distance > 1f ? 1f : distance);
        lastPlayerPos = transform.position;
    }
}
