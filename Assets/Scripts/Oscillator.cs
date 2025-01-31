using UnityEngine;

public class Oscillator : MonoBehaviour
{
	// Params
	[SerializeField] float speed;

	// State
	float movementFactor;
	Vector3 startPosition;
	Vector3 endPosition;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
		endPosition = gameObject.transform.Find("End Position").transform.position;
    }


    // Update is called once per frame
    void Update()
    {
		movementFactor = Mathf.PingPong(Time.time * speed, 1);
        transform.position = Vector3.Lerp(startPosition, endPosition, movementFactor);
    }
}
