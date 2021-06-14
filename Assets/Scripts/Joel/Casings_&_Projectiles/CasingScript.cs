using UnityEngine;
using System.Collections;

public class CasingScript : MonoBehaviour {

	public float minimumXForce;
	public float maximumXForce;
	public float minimumYForce;
	public float maximumYForce;
	public float minimumZForce;
	public float maximumZForce;
	public float minimumRotation;
	public float maximumRotation;
	public float despawnTime;

	public AudioClip[] casingSounds;
	public AudioSource audioSource;

	public float speed = 2500.0f;

	//Launch the casing at start
	private void Awake () 
	{
		//Random rotation of the casing
		GetComponent<Rigidbody>().AddRelativeTorque (
			Random.Range(minimumRotation, maximumRotation),
			Random.Range(minimumRotation, maximumRotation),
			Random.Range(minimumRotation, maximumRotation)
			* Time.deltaTime);

		//Random direction the casing will be ejected in
		GetComponent<Rigidbody>().AddRelativeForce (
			Random.Range (minimumXForce, maximumXForce), 
			Random.Range (minimumYForce, maximumYForce), 
			Random.Range (minimumZForce, maximumZForce));		     
	}

	private void Start () 
	{
		//Start the remove/destroy coroutine
		StartCoroutine (RemoveCasing ());
		transform.rotation = Random.rotation;
		//Start play sound coroutine
		StartCoroutine (PlaySound ());
	}

	private void FixedUpdate () 
	{
		//Spin the casing based on speed value
		transform.Rotate (Vector3.right, speed * Time.deltaTime);
		transform.Rotate (Vector3.down, speed * Time.deltaTime);
	}

	private IEnumerator PlaySound () 
	{
		//Wait for random time before playing sound clip
		yield return new WaitForSeconds (Random.Range(0.25f, 0.85f));
		//Get a random casing sound from the array 
		audioSource.clip = casingSounds
			[Random.Range(0, casingSounds.Length)];
		//Play the random casing sound
		audioSource.Play();
	}

	private IEnumerator RemoveCasing () 
	{
		//Destroy the casing after set amount of seconds
		yield return new WaitForSeconds (despawnTime);
		//Destroy casing object
		Destroy (gameObject);
	}
}