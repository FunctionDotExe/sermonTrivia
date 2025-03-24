using UnityEngine;

public class FlyCamera : MonoBehaviour
{
	/*
	EXTENDED FLYCAM
		Desi Quintans (CowfaceGames.com), 17 August 2012.
		Based on FlyThrough.js by Slin (http://wiki.unity3d.com/index.php/FlyThrough), 17 May 2011.
 
	LICENSE
		Free as in speech, and free as in beer.
 
	FEATURES
		WASD/Arrows:    Movement
		          Q:    Climb
		          E:    Drop
                      Shift:    Move faster
                    Control:    Move slower
                        End:    Toggle cursor locking to screen (you can also press Ctrl+P to toggle play mode on and off).
	*/

	public float mainSpeed = 100.0f;
	public float shiftAdd = 250.0f;
	public float maxShift = 1000.0f;
	public float camSens = 0.25f;

	private Vector3 lastMouse = new Vector3(255, 255, 255);
	private float totalRun = 1.0f;

	void Start()
	{
		// Update to use new cursor API
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			// Update cursor state using new API
			Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked) ? 
							  CursorLockMode.None : CursorLockMode.Locked;
			Cursor.visible = (Cursor.lockState == CursorLockMode.None);
		}
		
		lastMouse = Input.mousePosition - lastMouse;
		lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
		lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
		transform.eulerAngles = lastMouse;
		lastMouse = Input.mousePosition;
		
		// Keyboard commands
		Vector3 p = GetBaseInput();
		if (Input.GetKey(KeyCode.LeftShift))
		{
			totalRun += Time.deltaTime;
			p = p * totalRun * shiftAdd;
			p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
			p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
			p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
		}
		else
		{
			totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
			p = p * mainSpeed;
		}
		
		p = p * Time.deltaTime;
		transform.Translate(p);
	}

	private Vector3 GetBaseInput()
	{
		Vector3 p_Velocity = new Vector3();
		if (Input.GetKey(KeyCode.W))
			p_Velocity += new Vector3(0, 0, 1);
		if (Input.GetKey(KeyCode.S))
			p_Velocity += new Vector3(0, 0, -1);
		if (Input.GetKey(KeyCode.A))
			p_Velocity += new Vector3(-1, 0, 0);
		if (Input.GetKey(KeyCode.D))
			p_Velocity += new Vector3(1, 0, 0);
		return p_Velocity;
	}
}