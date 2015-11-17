using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour {

	[System.Serializable]
	public class DiceInfo
	{
		public Transform m_zdir;
		public int		 m_num = 1;

		public float GetDot(Vector3 up)
		{
			return Vector3.Dot (m_zdir.forward, up); 
		}
	}

	public Rigidbody m_rigidbody;

	public DiceInfo []m_diceInfos = new DiceInfo[6];
 
	bool  		m_checkSleepTime = false;
	bool 		m_throwed = false;
	float 		m_speedThrow; 
	float 		m_SleepTimeMax = 1.0f;
	float 		m_SleepTime = 0;
	Vector3  	m_valo;
	Vector3  	m_angle;
	Vector3  	m_targetVector;

	public void Pause()
	{
		if (m_rigidbody == null)
		{
			Debug.LogError("if (m_rigidbody == null)");
			return;
		}

		m_rigidbody.useGravity = false;
		m_rigidbody.isKinematic = true;
	}

	// Use this for initialization
	public void Throw (Vector3 targetPos, Vector3 angleValo, float speedThrow, System.Action<Dice> endCall) 
	{
		m_SleepTime 	= 0;
		m_throwed 		= true;
		m_endCall 		= endCall;
		m_targetVector 	= targetPos;
		m_speedThrow	= speedThrow;
		ThrowDice (angleValo);

		CancelInvoke ("CheckStop");
		InvokeRepeating ("CheckStop", 0.5f, 0.5f);
	}

	void ThrowDice(Vector3 angleValo)
	{
		if (m_rigidbody == null)
		{
			Debug.LogError("if (m_rigidbody == null)");
			return;
		}
			

		m_rigidbody.useGravity = true;
		m_rigidbody.isKinematic = false;
		Vector3 l_dir = m_targetVector - transform.position;
		l_dir.Normalize ();
		l_dir *= m_speedThrow;
		m_rigidbody.velocity = l_dir;
		m_rigidbody.angularVelocity = angleValo;
	}



	public int GetCurrentNumber(Vector3 normalUp)
	{
		DiceInfo closestNum = null;
		float closestDotValue = 0;

		int countDiceInfos = m_diceInfos.Length;
		for(int i = 0; i < countDiceInfos; ++i)
		{
			float dotValue = m_diceInfos[i].GetDot(normalUp);
			if(closestNum == null || closestDotValue < dotValue)
			{
				closestNum = m_diceInfos[i];
				closestDotValue = dotValue;
			}
		}

		if(closestNum != null)
		{
			return closestNum.m_num; 
		}

		return 0;
	}

	// Update is called once per frame
	void CheckStop ()
	{
		if (m_throwed)
		{
			m_valo = m_rigidbody.velocity;
			m_angle = m_rigidbody.angularVelocity;

			if (m_valo.sqrMagnitude < 0.1f * 0.1f && m_angle.sqrMagnitude < 0.1f * 0.1f)
			{
				if(m_checkSleepTime == false)
				{
					CheckSleepTime (); 
				} 
			} 
			else
			{
				if(m_checkSleepTime == true)
				{
					CancleCheckSleepTime();
				} 
			}

			if(m_checkSleepTime)
			{
				float checkedSleepTime = Time.realtimeSinceStartup - m_SleepTime;
				if(checkedSleepTime >= m_SleepTimeMax)
				{
					SetStop();
				}
			}
		} 
 
	}

	void CheckSleepTime()
	{ 
		m_checkSleepTime = true;
		m_SleepTime = Time.realtimeSinceStartup;
	}

	void CancleCheckSleepTime()
	{
		m_checkSleepTime = false;
	}

	void SetStop()
	{
		if(m_throwed)
		{
			CancleCheckSleepTime ();
			CancelInvoke ("CheckStop");
			
			if (m_endCall != null) 
			{
				m_endCall(this);
			}
		} 
		m_throwed = false;
	}

	public bool IsStop()
	{
		return !m_throwed;
	}

	System.Action<Dice> m_endCall = null;

	public AudioSource m_audioSource;
	void OnCollisionEnter(Collision collision) 
	{
//		foreach (ContactPoint contact in collision.contacts)
//		{
//			Debug.DrawRay(contact.point, contact.normal, Color.white);
//		}

#if UNITY_EDITOR
		Debug.Log("collision.relativeVelocity.magnitude => " + collision.relativeVelocity.magnitude.ToString());

#endif
		float length = collision.relativeVelocity.magnitude;
		  
		if ( length > 1)
		{ 
			float volumeScale 	= length/22.0f;
			volumeScale 		= Mathf.Clamp01(volumeScale);
			m_audioSource.PlayOneShot(m_audioSource.clip, volumeScale); 
		} 
	}
}
