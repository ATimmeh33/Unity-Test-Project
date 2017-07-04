using System.Collections;
using UnityEngine;
using RNG = System.Random;

public class CubeManager : MonoBehaviour {

	public Transform cube;

	private readonly RNG rng = new RNG();

	// Use this for initialization
	void Start () {
		StartCoroutine(SpawnCube());
	}

	private IEnumerator SpawnCube()
	{
		while (true) {
			Debug.Log("Spawning a cube!");
			Instantiate(
				cube,
				new Vector3(rng.Next(-10, 10), rng.Next(-10, 10), rng.Next(-10, 10)),
				Quaternion.Euler(rng.Next(180), rng.Next(180), rng.Next(180)));
			yield return new WaitForSeconds(.1f);
		}
	}

	// Update is called once per frame
	void Update () {
	}
}
