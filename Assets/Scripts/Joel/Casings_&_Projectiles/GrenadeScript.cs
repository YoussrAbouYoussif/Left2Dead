using UnityEngine;
using System.Collections;

public class GrenadeScript : MonoBehaviour {

	public float grenadeTimer = 5.0f;
	public Transform explosionPrefab;
	public float radius = 25.0F;
	public float power = 350.0F;
	public float minimumForce = 1500.0f;
	public float maximumForce = 2500.0f;
	private float throwForce;
	public AudioSource impactSound;

	private void Awake () 
	{
		//Generate throw force
		//based on min and max values
		throwForce = Random.Range
			(minimumForce, maximumForce);

		//Random rotation of the grenade
		GetComponent<Rigidbody>().AddRelativeTorque 
		   (Random.Range(500, 1500),
			Random.Range(0,0), 		 
			Random.Range(0,0)  		 
			* Time.deltaTime * 5000);
	}

	private void Start () 
	{
		//Launch the projectile forward by adding force to it at start
		GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * throwForce);

		//Start the explosion timer
		StartCoroutine (ExplosionTimer ());
	}

	private void OnCollisionEnter (Collision collision) 
	{
		//Play the impact sound on every collision
		impactSound.Play ();
	}

	private IEnumerator ExplosionTimer () 
	{
		//Wait set amount of time
		yield return new WaitForSeconds(grenadeTimer);

		//Raycast downwards to check ground
		RaycastHit checkGround;
		if (Physics.Raycast(transform.position, Vector3.down, out checkGround, 50))
		{
			//Instantiate metal explosion prefab on ground
			Instantiate (explosionPrefab, checkGround.point, 
				Quaternion.FromToRotation (Vector3.forward, checkGround.normal)); 
		}

		//Explosion force
		Vector3 explosionPos = transform.position;
		//Use overlapshere to check for nearby colliders
		Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
		foreach (Collider hit in colliders) {
			Rigidbody rb = hit.GetComponent<Rigidbody> ();

			//Add force to nearby rigidbodies
			if (rb != null)
				rb.AddExplosionForce (power * 5, explosionPos, radius, 3.0F);
		}

		//Destroy the grenade object on explosion
		Destroy (gameObject);
	}
}