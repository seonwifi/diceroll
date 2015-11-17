using UnityEngine;
using System.Collections;
 

public class AdView : MonoBehaviour {

#if UNITY_IPHONE
	private UnityEngine.iOS.ADBannerView banner = null;

	// Use this for initialization
	void Start () 
	{

		banner = new UnityEngine.iOS.ADBannerView(UnityEngine.iOS.ADBannerView.Type.Banner, UnityEngine.iOS.ADBannerView.Layout.Top);
		UnityEngine.iOS.ADBannerView.onBannerWasClicked += OnBannerClicked;
		UnityEngine.iOS.ADBannerView.onBannerWasLoaded  += OnBannerLoaded;
	}


	void OnBannerClicked()
	{
		Debug.Log("Clicked!\n");
	}
	
	void OnBannerLoaded()
	{
		Debug.Log("Loaded!\n");
		banner.visible = true;
	}

	// Update is called once per frame
	void Update () {
	
	}
#endif

}
