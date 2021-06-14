using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

	private bool explodeSelf;
	public bool useConstantForce;
	public float constantForceSpeed;
	public float explodeAfter;
	private bool hasStartedExplode;

	private bool hasCollided;
	public Transform explosionPrefab;
	public float force = 5000f;
	public float despawnTime = 30f;

	public float radius = 50.0F;
	public float power = 250.0F;
	public bool usesParticles;
	public ParticleSystem smokeParticles;
	public ParticleSystem flameParticles;
	public float destroyDelay;

	private void Start () 
	{
		if (!useConstantForce) 
		{
			//Launch the projectile forward by adding force to it at start
			GetComponent<Rigidbody> ().AddForce 
				(gameObject.transform.forward * force);
		}
		//Start the destroy timer
		StartCoroutine (DestroyTimer ());
	}
		
	private void FixedUpdate()
	{
		//Rotates the projectile according to the direction it is going
		if(GetComponent<Rigidbody>().velocity != Vector3.zero)
			GetComponent<Rigidbody>().rotation = 
				Quaternion.LookRotation(GetComponent<Rigidbody>().velocity);  

		//If using constant force
		if (useConstantForce == true && !hasStartedExplode) {
			//Launch the projectile forward with a constant force (used for rockets)
			GetComponent<Rigidbody>().AddForce 
				(gameObject.transform.forward * constantForceSpeed);

			//Start self explode
			StartCoroutine (ExplodeSelf ());

			//Stop looping
			hasStartedExplode = true;
		}
	}

	//Used for when the rocket is flying into the sky for example, 
	//it should blow up after some time
	private IEnumerator ExplodeSelf () 
	{
		//Wait set amount of time
		yield return new WaitForSeconds (explodeAfter);
		//Spawn explosion particle prefab
		if (!hasCollided) {
			Instantiate (explosionPrefab, transform.position, transform.rotation);
		}
		//Hide projectile
		gameObject.GetComponent<MeshRenderer> ().enabled = false;
		gameObject.GetComponent<Rigidbody> ().isKinematic = true;
		gameObject.GetComponent<BoxCollider>().isTrigger = true;
		//Stop particles and let them finish playing before destroying
		if (usesParticles == true) {
			flameParticles.GetComponent <ParticleSystem> ().Stop ();
			smokeParticles.GetComponent<ParticleSystem> ().Stop ();
		}
		//Wait more to let particle systems disappear
		yield return new WaitForSeconds (destroyDelay);
		//Destroy projectile
		Destroy (gameObject);
	}

	private IEnumerator DestroyTimer () 
	{
		//Destroy the projectile after set amount of time
		yield return new WaitForSeconds (despawnTime);
		//Destroy gameobject
		Destroy (gameObject);
	}

	private IEnumerator DestroyTimerAfterCollision () 
	{
		//Wait set amount of time after collision to destroy projectile
		yield return new WaitForSeconds (destroyDelay);
		//Destroy gameobject
		Destroy (gameObject);
	}

	//If the projectile collides with anything
	private void OnCollisionEnter (Collision collision) 
	{

		hasCollided = true;

		//Hide projectile
		gameObject.GetComponent<MeshRenderer> ().enabled = false;
		//Freeze object
		gameObject.GetComponent<Rigidbody> ().isKinematic = true;
		//Disable collider
		gameObject.GetComponent<BoxCollider>().isTrigger = true;

		if (usesParticles == true) {
			flameParticles.GetComponent <ParticleSystem> ().Stop ();
			smokeParticles.GetComponent<ParticleSystem> ().Stop ();
		}

		StartCoroutine (DestroyTimerAfterCollision ());

		//Instantiate explosion prefab at collision position
		Instantiate(explosionPrefab,collision.contacts[0].point,
			Quaternion.LookRotation(collision.contacts[0].normal));

		//Explosion force
		Vector3 explosionPos = transform.position;
		//Use overlapshere to check for colliders in range
		Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
		foreach (Collider hit in colliders) {
			Rigidbody rb = hit.GetComponent<Rigidbody> ();

			//Add force to nearby rigidbodies
			if (rb != null)
				rb.AddExplosionForce (power * 50, explosionPos, radius, 3.0F);
		}
	}
}