using UnityEngine;
using System.Collections;

public class SlingshotBehaviour : MonoBehaviour
{
	public float MaxStretch = 3.0f;

	private SpringJoint2D spring;
	private Rigidbody2D rigidBody2D;

	private bool clickedOn;

	private Ray rayToMouse;
	private Vector3 anchor;
	private Vector3 prevVelocity;
	private float maxStretchSqr;

	void Awake ()
	{
		spring = GetComponent<SpringJoint2D> ();
		anchor = spring.connectedBody.transform.position;
		rigidBody2D = GetComponent<Rigidbody2D> ();
		maxStretchSqr = MaxStretch * MaxStretch;
	}

	// Use this for initialization
	void Start ()
	{
		rayToMouse = new Ray (anchor, Vector3.zero);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (clickedOn) {
			Dragging ();
		}

		if (spring != null) {
			if (!rigidBody2D.isKinematic && prevVelocity.sqrMagnitude > rigidBody2D.velocity.sqrMagnitude) {
				Destroy (spring);
				rigidBody2D.velocity = prevVelocity;
			}

			if (!clickedOn) {
				prevVelocity = rigidBody2D.velocity;
			}
		} else {

		}
	}

	void OnMouseDown ()
	{
		spring.enabled = false;
		clickedOn = true;
	}

	void OnMouseUp ()
	{
		spring.enabled = true;
		rigidBody2D.isKinematic = false;
		clickedOn = false;
	}

	void Dragging ()
	{
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 anchorToMouse = mouseWorldPoint - anchor;

		if (anchorToMouse.sqrMagnitude > maxStretchSqr) {
			rayToMouse.direction = anchorToMouse;
			mouseWorldPoint = rayToMouse.GetPoint (MaxStretch);
		}

		mouseWorldPoint.z = 0f;
		transform.position = mouseWorldPoint;
	}
}
