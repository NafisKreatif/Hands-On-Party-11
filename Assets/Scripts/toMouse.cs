using UnityEngine;

public class toMouse : MonoBehaviour
{
    public float speed;
    Vector3 mousePos;

	void Start ()
    {
        
	}
	
	void Update ()
    {
        if(Input.GetKey(KeyCode.Mouse0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        transform.position = new Vector3 (mousePos.x, mousePos.y, 0);
    }
}
