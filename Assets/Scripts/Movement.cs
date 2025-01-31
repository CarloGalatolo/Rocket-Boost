using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
	// Parameters
    [SerializeField] float thrustStrength = 1000;
    [SerializeField] float rotationStrength = 200;

	// References
	[SerializeField] AudioClip mainEngine;
    [SerializeField] InputAction thrustInput;
    [SerializeField] InputAction rotationInput; // 1D Axis, Right is negative.
	[SerializeField] ParticleSystem thrustParticle;
	[SerializeField] ParticleSystem rightThrustParticle;
	[SerializeField] ParticleSystem leftThrustParticle;

	// Cache
    Rigidbody rigidBody;
    AudioSource audioSource;


    // Called between Awake() and Start().
    void OnEnable ()
    {
        // Inputs enabling.
        thrustInput.Enable();
        rotationInput.Enable();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {
		// Caching
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }


    // Physics update. Executes vefore Update().
    void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }


    // Update is called once per frame
    void Update ()
    {
    }


    private void ProcessThrust ()
    {
        if ( thrustInput.IsPressed() )
        {
			// Start thrusting
            if ( !audioSource.isPlaying )
            {
                audioSource.PlayOneShot(mainEngine);
            }
			if ( !thrustParticle.isPlaying )
			{
				thrustParticle.Play();
			}
            rigidBody.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);
        }
        else
        {
            audioSource.Stop();
			thrustParticle.Stop();
        }
    }


    private void ProcessRotation ()
    {
        float rotation = rotationInput.ReadValue<float>();

        if ( rotation != 0 )
        {
            // Freeze roll without releasing pitch and yaw. freezeRotation = false unfreezes all constraints at once.
            rigidBody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            transform.Rotate(Vector3.forward * rotation * rotationStrength * Time.fixedDeltaTime);
            rigidBody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

			if ( rotation < 0 )
			{
				if ( !leftThrustParticle.isPlaying )
				{
					leftThrustParticle.Play();
				}
			}
			else
			{
				if ( !rightThrustParticle.isPlaying )
				{
					rightThrustParticle.Play();
				}
			}
        }
		else
		{
			leftThrustParticle.Stop();
			rightThrustParticle.Stop();
		}
    }
}
