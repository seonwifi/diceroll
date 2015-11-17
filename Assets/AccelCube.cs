using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AccelCube : MonoBehaviour 
{
	[System.Serializable]
	public class  ThrowInfo
	{
		[HideInInspector]
		public Dice 	m_Dicerig;
		public GameObject m_target;
		public GameObject m_startPos;
		public float      m_speed;

		public Quaternion m_defaultAngle = Quaternion.Euler(0,0,0);
		  
		public void Init(GameObject dicePrefab)
		{
			if(dicePrefab == null)
			{
				Debug.LogError("if(dicePrefab == null))");
				return;
			}

			if(m_startPos == null)
			{
				Debug.LogError("if(m_startPos == null)");
			}

			m_defaultAngle = dicePrefab.transform.rotation;

			if (m_Dicerig == null) 
			{


				GameObject cObj = GameObject.Instantiate(dicePrefab, m_startPos.transform.position, m_defaultAngle) as GameObject; 
				m_Dicerig = cObj.GetComponent<Dice>();
			}

			if(m_Dicerig == null)
			{
				Debug.LogError("if(m_Dicerig == null)");
				return;
			}

			m_Dicerig.Pause (); 
		}

		public void Shot(System.Action<Dice> callStopedDice)
		{
			if(m_Dicerig == null)
			{
				Debug.LogError("if(m_Dicerig == null)");
				return;
			}

			Vector3 angleValo = new Vector3 (Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));

			m_Dicerig.Throw (m_target.transform.position, angleValo, m_speed, callStopedDice);
		}

		public void SetRandomAngle()
		{
			m_Dicerig.transform.rotation = GetRandomAngle() * m_defaultAngle;
		}

		
		public Quaternion GetRandomAngle()
		{
			RandomIndex<float> xAngle = new RandomIndex<float> ();
			
			xAngle.AddNewValue(1, 0);
			xAngle.AddNewValue(1, 90);
			xAngle.AddNewValue(1, 180);
			xAngle.AddNewValue(1, 270);
			
			RandomIndex<float> yAngle = new RandomIndex<float> ();
			
			yAngle.AddNewValue(1, 0);
			yAngle.AddNewValue(1, 90);
			yAngle.AddNewValue(1, -90);
			
			float xSelect = xAngle.Select ();
			float ySelect = yAngle.Select ();
			
			return Quaternion.Euler( xSelect, ySelect, 0);
		}

		public void Reset()
		{
			if(m_Dicerig == null)
			{
				Debug.LogError("if(m_Dicerig == null)");
				return;
			}

			m_Dicerig.transform.position = m_startPos.transform.position;
			m_Dicerig.transform.rotation = m_defaultAngle; 
			m_Dicerig.Pause (); 
		}
	} 

	public GameObject	m_DicePrefab;
	public ThrowInfo 	m_1DiceThrow = new ThrowInfo();
	public ThrowInfo 	[]m_2DiceThrow = new ThrowInfo[2]; 

	public bool			m_isOneDice = false;

	// Use this for Rigidbodyinitialization
	void Awake ()
	{
		foreach(var x in m_2DiceThrow)
		{
			x.Init(m_DicePrefab);  
		}
		m_buttonShot.gameObject.SetActive (true);
		m_number.text = "";
	}
  
	ThrowInfo []m_throwedDice = null;

	public void ThrowDice(int countOfDice)
	{
		if(countOfDice == 1)
		{
			Shot(new ThrowInfo[]{m_1DiceThrow});
		}
		else if(countOfDice == 2)
		{
			Shot(m_2DiceThrow);
		}
		else
		{
			Debug.LogError("");
		}
	}

	public void Shot(ThrowInfo [] throwInfos)
	{
		m_throwedDice = throwInfos;
		if(m_throwedDice == null)
		{
			Debug.LogError("if(m_throwedDice == null)");
			return;
		}

		m_buttonShot.gameObject.SetActive (false);
		m_number.text = "";

		foreach(var x in m_throwedDice)
		{
			x.Reset();
			x.Shot(StopedDice);  
		} 
	}

	void StopedDice(Dice dice)
	{
		bool isAllStop = true;

		foreach(var x in m_throwedDice)
		{
			if(x == null)
			{
				Debug.LogError("if(x == null)");
				continue;
			}

			if(x.m_Dicerig == null)
			{
				Debug.LogError("if(x.m_Dicerig == null)");
				continue;
			}

			if(x.m_Dicerig.IsStop())
			{
				isAllStop = false;
				break;
			}
		}

		if(isAllStop)
		{
			AllStopedDice();
		}
	}

	public Camera m_mainCam; 
	public UnityEngine.UI.Text 		m_number;
	public UnityEngine.UI.Button 	m_buttonShot;
	public AudioSource m_audioSource;
	void AllStopedDice()
	{ 
		 
		//UnityEngine.ADBannerView
		if(m_throwedDice == null)
		{
			return;
		}

		if(m_audioSource)
		{
			m_audioSource.PlayOneShot(m_audioSource.clip, 0.7f);
		}
		
		m_buttonShot.gameObject.SetActive (true);

		List<ThrowInfo> throwInfoSorted = new List<ThrowInfo> ();

		throwInfoSorted.AddRange (m_throwedDice);

		throwInfoSorted.Sort ((x , y) =>
		{
			Vector3 xScrPos = m_mainCam.WorldToScreenPoint(x.m_Dicerig.transform.position);
			Vector3 yScrPos = m_mainCam.WorldToScreenPoint(y.m_Dicerig.transform.position);
			return xScrPos.x.CompareTo(yScrPos.y);
		});

		m_number.text = "";

		for(int i = 0; i < throwInfoSorted.Count; ++i) 
		{
			var x = throwInfoSorted[i];
			m_number.text += x.m_Dicerig.GetCurrentNumber(new Vector3(0,1,0)).ToString();
			if(i != throwInfoSorted.Count-1)
			{
				m_number.text += "\t\t";
			} 
		}
	}

	public void OnClick_Shot()
	{
		if(m_isOneDice)
		{
			ThrowDice(1);
		}
		else
		{
			ThrowDice(2);
		}
	}
}
