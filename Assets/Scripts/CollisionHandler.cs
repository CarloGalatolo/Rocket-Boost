using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CollisionHandler : MonoBehaviour
{
	[SerializeField] float endDelay = 2;
	[SerializeField] AudioClip crashSound;
	[SerializeField] AudioClip successSound;

	AudioSource audioSource;

	bool isControllable = true;
	
	
	// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {
        audioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update ()
    {
        
    }


    void OnCollisionEnter (Collision other)
    {
		if ( !isControllable ) { return; }

        switch ( other.gameObject.tag ) 
        {
			case "Finish":
				StartSuccessSequence();
				break;

			case "Friendly":
                Debug.Log("Nice and smooth.");
                break;

            case "Pickable":
                Debug.Log("Yummy!");
                break;

            default:
                StartCrashSequence();
                break;
        }
    }


	void StartCrashSequence()
    {
		isControllable = false;
		GetComponent<Rigidbody>().freezeRotation = false;
		GetComponent<Movement>().enabled = false;
		audioSource.Stop();
		audioSource.PlayOneShot(crashSound, 0.5f);
		Invoke("ReloadLevel", endDelay);
    }


	void StartSuccessSequence()
	{
		isControllable = false;
		GetComponent<Movement>().enabled = false;
		audioSource.Stop();
		audioSource.PlayOneShot(successSound, 0.7f);
		Invoke("LoadNextLevel", endDelay);
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
}
