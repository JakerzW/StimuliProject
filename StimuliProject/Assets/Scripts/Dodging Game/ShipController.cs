using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    Vector3 movementVelocity = Vector3.zero;
    public float movementTime;
    Vector3 nextShipPosition;
    bool shipIsMoving;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {           
            nextShipPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 50)); 
            shipIsMoving = true;
        }

        if (shipIsMoving)
        {
            MoveShip(nextShipPosition);
        }
    }

    void MoveShip(Vector3 mousePosOnClick)
    {

        //gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, mousePosOnClick, ref movementVelocity, movementTime);
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, mousePosOnClick, movementTime * Time.deltaTime);
    }
}
