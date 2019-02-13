using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Canvas gameHUD;

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown(0)) 
		{
            Debug.Log ("Mouse button clicked");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, 100);

            //Debug.Log("Hit Target");
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject.tag);
                //Destroy(hit.collider.gameObject);
				gameHUD.GetComponent<GameController>().TargetHit();
            }      
		}
	}
}
