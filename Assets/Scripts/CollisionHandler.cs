using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
	// Params
	[SerializeField] float endDelay = 2;

	// Refs
	[SerializeField] AudioClip crashSFX;
	[SerializeField] AudioClip successSFX;
	[SerializeField] ParticleSystem crashParticle;
	[SerializeField] ParticleSystem successParticle;
	[SerializeField] ParticleSystem thrustParticle;
	[SerializeField] ParticleSystem rightThrustParticle;
	[SerializeField] ParticleSystem leftThrustParticle;

	// Cache
	AudioSource audioSource;

	// State
	bool bIsControllable = true;
	bool bCanCrash = true;
	
	
	// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {
		// Caching
        audioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update ()
    {
        RespondToDebugKeys();
    }


	void OnCollisionEnter (Collision other)
    {
		if ( !bIsControllable ) { return; }

        switch ( other.gameObject.tag ) 
        {
			case "Finish":
				StartSuccessSequence();
				break;

			case "Friendly":
                // Collision won't destroy the player.
                break;

            default:
                StartCrashSequence();
                break;
        }
    }


	void StartCrashSequence()
	{
		if ( bCanCrash )
		{
			bIsControllable = false;
			GetComponent<Rigidbody>().freezeRotation = false;
			GetComponent<Movement>().enabled = false;
			audioSource.Stop();
			audioSource.PlayOneShot(crashSFX, 0.5f);
			StopThrusterParticles();
			crashParticle.Play();
			Invoke("ReloadLevel", endDelay);
		}
	}
	

	void StartSuccessSequence()
	{
		bIsControllable = false;
		GetComponent<Movement>().enabled = false;
		audioSource.Stop();
		audioSource.PlayOneShot(successSFX, 0.7f);
		StopThrusterParticles();
		successParticle.Play();
		Invoke("LoadNextLevel", endDelay);
	}


	void StopThrusterParticles ()
	{
		thrustParticle.Stop();
		rightThrustParticle.Stop();
		leftThrustParticle.Stop();
	}


    void LoadNextLevel ()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;

        if ( nextScene >= SceneManager.sceneCountInBuildSettings )
        {
            nextScene = 0;  // Restart from first level.
        }

        SceneManager.LoadScene(nextScene);
    }


    void ReloadLevel ()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }


	void RespondToDebugKeys()
	{
		if ( Keyboard.current.lKey.wasReleasedThisFrame )
		{
			LoadNextLevel();
		}

		if ( Keyboard.current.cKey.wasReleasedThisFrame )
		{
			bCanCrash = !bCanCrash;
			Debug.Log("bCanCrash = " + bCanCrash);
		}
	}
}
