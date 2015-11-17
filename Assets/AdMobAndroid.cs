using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class AdMobAndroid : MonoBehaviour
{
#if UNITY_ANDROID
	private void RequestBanner()
	{
		#if UNITY_ANDROID
		string adUnitId = "pub-3534390662369307";
		#elif UNITY_IPHONE
		string adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";
		#else
		string adUnitId = "unexpected_platform";
		#endif
		 
		// Create a 320x50 banner at the top of the screen.
		BannerView bannerView = new BannerView(adUnitId, AdSize.MediumRectangle, AdPosition.Top);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the banner with the request.
		bannerView.LoadAd(request); 
	}


	// Use this for initialization
	void Start () 
	{
		RequestBanner ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
#endif
}

 