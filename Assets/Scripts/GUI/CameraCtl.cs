using UnityEngine;
using System.Collections;


/*
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 *		URL:		clomax.me.uk
 *		email:		crl9@aber.ac.uk
 *
 */


public class CameraCtl : MonoBehaviour {

    private float sensitivityX = 2.5F;
    private float sensitivityY = 2.5F;
	
	private Transform _t;

    float hdg = 0F;
    float pitch = 0F;

	PauseMenu pause;
	
	void Awake () {
		_t = transform;
	}
	
    void Start() {
		pause = GetComponent<PauseMenu>();
		pitch = _t.localEulerAngles.x;
    }

    void Update() {
		if (!(pause.IsGamePaused())) {
			if (!(Input.GetMouseButton(0) || Input.GetMouseButton(1)))
				return;

			float deltaX = Input.GetAxis("Mouse X") * sensitivityX;
			float deltaY = Input.GetAxis("Mouse Y") * sensitivityY;

			if (Input.GetMouseButton(1) && !Input.GetMouseButton(0)) {
				setHeight(deltaY);
			} else {
				if (Input.GetMouseButton(0) && !(Input.GetButton("Fire2"))) {
					moveForwards(deltaY);
					setHeading(deltaX);
				} else if (Input.GetMouseButton(1)) {
					setPitch(-deltaY);
				}
			}
			
			if(Input.GetButton("Fire2") && Input.GetMouseButton(0)) {
				setHeading(deltaX);
				setPitch(-deltaY);
			}
		}


    }

    void moveForwards(float n) {
        Vector3 fwd = transform.forward;
        fwd.y = 0;
        fwd.Normalize();
        transform.position += n * fwd;
    }

    void setHeight(float n) {
        transform.position += n * Vector3.up;
    }

    void setHeading(float n) {
        hdg += n;
        wrapAngle(ref hdg);
        transform.localEulerAngles = new Vector3(pitch, hdg, 0);
    }

    void setPitch(float n) {
        pitch += n;
        wrapAngle(ref pitch);
        transform.localEulerAngles = new Vector3(pitch, hdg, 0);
    }

    private static void wrapAngle(ref float angle) {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
    }
}
