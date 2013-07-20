using UnityEngine;
using System.Collections;

public class Limb : MonoBehaviour {
	
	Transform _t;
	
	ConfigurableJoint joint;
	SoftJointLimit zAngularLimit;
	JointDrive zAngDrive;
	
	float x = 0.0F;
	public static float frequency = 5F;
	public static float amplitude = 3F;

	
	void Start () {
		GameObject.CreatePrimitive(PrimitiveType.Cube);
		_t = transform;
		name = "limb";
		
		_t.localPosition = _t.position;
		_t.localScale = new Vector3(5,1,1);
		_t.rotation = new Quaternion(0f,.2f,0f,1f);
		_t.transform.parent = transform;
		_t.renderer.material.color = Color.green;
		gameObject.AddComponent("Rigidbody");
		
		joint = (ConfigurableJoint)gameObject.AddComponent("ConfigurableJoint");
		joint.connectedBody = rigidbody;
		
		// anchor
		joint.anchor = new Vector3(.5F,0,0);
		
		//lock axes
		joint.xMotion = ConfigurableJointMotion.Locked;
		joint.yMotion = ConfigurableJointMotion.Locked;
		joint.zMotion = ConfigurableJointMotion.Locked;
		
		//lock angular rotation for x and y, limit to 30 for z
		joint.angularXMotion = ConfigurableJointMotion.Locked;
		joint.angularYMotion = ConfigurableJointMotion.Locked;
		
		joint.angularZMotion = ConfigurableJointMotion.Limited;
		zAngularLimit = new SoftJointLimit();
		zAngularLimit.limit = 150F;
		joint.angularZLimit = zAngularLimit;
		
		// enable angluar z drive
		zAngDrive = new JointDrive();
		zAngDrive.mode = JointDriveMode.Velocity;
		zAngDrive.maximumForce = 3F;
		joint.angularYZDrive = zAngDrive;		
	}
	
	void FixedUpdate () {
		x = Sine(x);
		Vector3 z = new Vector3(0.0F,0.0F, x * amplitude);
		joint.targetAngularVelocity = z;
	}
	
	private static float Sine (float x){
		return Mathf.Sin(Time.timeSinceLevelLoad * frequency);
	}
	
	/**
	 * Given a variable of constant delta (age of a creature, typically),
	 * the frequency, and amplitude, return the sine
	 */
	private static float Sine (float x, float frequency, float amplitude){
		return amplitude * Mathf.Sin(2 * Mathf.PI
		                             * (frequency * x)
		                             + Time.timeSinceLevelLoad
		                            );
	}
}
