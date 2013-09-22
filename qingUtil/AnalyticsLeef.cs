using UnityEngine;
using System.Collections;
using System;

public class AnalyticsLeef : MonoBehaviour {
	private string screenShotURL= "http://dragonsourcer.com/log2.php?stamp=TestCaptureFunction";
	public string _appid="Prius_Rv3";
	public DateTime expiredDate=new DateTime(2033,6,25);
	public bool autoStartCapture=false;
	public bool canCapture=true;
	private float CaptureAndSendGapTime=5f;
	// Use this for initialization
	IEnumerator Start () {
		print ("Analytics startup");
		WWW www;
		if(!PlayerPrefs.HasKey("uid")){
			int uid=GenerateUID();
			PlayerPrefs.SetString("uid",_appid+uid.ToString());
		}
		if(!PlayerPrefs.HasKey("expired")){
			PlayerPrefs.SetInt("expired",0);
		}
		if(PlayerPrefs.GetInt("expired")==0){
			PlayerPrefs.SetInt("expired",DateTime.Now>expiredDate?1:0);
		}else{
			 www = new WWW( "http://dragonsourcer.com/log.php?appid="+_appid+"&data="+PlayerPrefs.GetString("uid")+"__expired__");
       		 yield return www;
		}
		PlayerPrefs.Save();
		
		//tracing log information
        www = new WWW( "http://dragonsourcer.com/log.php?appid="+_appid+"&data="+PlayerPrefs.GetString("uid")+"@"+Application.platform);
        yield return www;
		//capture.
		screenShotURL= "http://dragonsourcer.com/log2.php?stamp="+PlayerPrefs.GetString("uid");
		if(autoStartCapture){
			yield return new WaitForSeconds(1f);
			//StartCoroutine("CaptureAndUpload");
		}
	}
	
	public void logContent(string v){
		StartCoroutine("logToServer",v);
	}
	
	private IEnumerator logToServer(string v){
		WWW www;
		www = new WWW( "http://dragonsourcer.com/log.php?appid="+_appid+"&data="+PlayerPrefs.GetString("uid")+";"+v);
       	yield return www;
	}
	private int GenerateUID(){
		float s=(UnityEngine.Random.value*1000000);
		return (int)s;
	}
	public void Capture(){
		StartCoroutine("CaptureAndUpload");
	}
	
	private IEnumerator CaptureAndUpload() {
		if(!canCapture){
			print ("not allowed");
			return false;
		}
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D( width, height, TextureFormat.RGB24, false );
		 yield return new WaitForEndOfFrame();
		// Read screen contents into the texture
		tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		tex.Apply();
		// split the process up--ReadPixels() and the GetPixels() call inside of the encoder are both pretty heavy
		yield return 0;
		// Encode texture into PNG
		byte[] bytes = tex.EncodeToPNG();
		Destroy( tex );
		 yield return 0;
		// Create a Web Form
		WWWForm form = new WWWForm();
		form.AddField("frameCount", Time.frameCount.ToString());
		form.AddBinaryData("file", bytes, "screenShotcs.png", "image/png");
		 
		// Upload to a cgi script    
		WWW w = new WWW(screenShotURL, form);
		print ("uploading");
		yield return w;
		if (w.error != null){
		    print(w.error);
		    print("error:");
			yield return new WaitForSeconds(CaptureAndSendGapTime*3);
			StartCoroutine("CaptureAndUpload");
		}
		else{
		    print("Finished Uploading Screenshot");
			yield return new WaitForSeconds(CaptureAndSendGapTime);
			if(autoStartCapture)StartCoroutine("CaptureAndUpload");
	    }
    }
}
