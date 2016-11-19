using UnityEngine;
using System.Collections;

public class LookAtTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter( Collider col )
	{
		print( "Collider enter" );
		transform.LookAt( col.transform );
	}
}
