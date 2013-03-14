using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class PureData {
	#if UNITY_ANDROID
	static AndroidJavaObject PdWrapper;
	#endif

	/* Interface to native implementation */
	[DllImport ("__Internal")]
	private static extern void _openFile(char[] filename, int filenameLength);
	
	[DllImport ("__Internal")]
	private static extern void _closeFile(char[] filename, int filenameLength);
	
	[DllImport ("__Internal")]
	private static extern void _initPd();
	
	[DllImport ("__Internal")]
	private static extern void _startAudio();
	
	[DllImport ("__Internal")]
	private static extern void _sendBangToReceiver(char[] receiver, int length);
	
	[DllImport ("__Internal")]
	private static extern void _sendFloat(float aValue, char[] receiver, int length);
	
	[DllImport ("__Internal")]
	private static extern void _sendSymbolToReceiver(char[] symbol, int symLength, char[] receiver, int recLength);
	
	[DllImport ("__Internal")]
	private static extern void _sendMessageToReceiver(char[] message, int messageLength, char[] arguments, int argumentsLength, char[] receiver, int recLength);
	
	[DllImport ("__Internal")]
	private static extern void _sendListToReceiver(char[] arguments, int argumentsLength, char[] receiver, int recLength);
	
	[DllImport ("__Internal")]
	private static extern void _subscribe(char[] symbol, int symLength, char[] gameObject, int objLength, char[] methodName, int methLength);
	
//	[DllImport ("__Internal")]
//	private static extern void _unsubscribe();
	
	/* Public interface for use inside C# / JS code */
	
	public static void openFile(string filename)
	{
		#if UNITY_IPHONE
			if (Application.platform != RuntimePlatform.OSXEditor)
				_openFile(filename.ToCharArray(), filename.Length * 2);
		#elif UNITY_ANDROID
			PdWrapper.Call( "openFile", filename );
			// int handle = PdBase.CallStatic<int>("openPatch", filename);
		#endif
	}
	
	public static void closeFile(string filename)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
			_closeFile(filename.ToCharArray(), filename.Length * 2);
	}
	
	public static void initPd()
	{
		#if UNITY_IPHONE
			if (Application.platform != RuntimePlatform.OSXEditor)
				_initPd();
		#elif UNITY_ANDROID
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"); 

			//Initialize the Wrapper object with the current activity
			PdWrapper = new AndroidJavaObject( "org.puredata.android.unity.Wrapper", jo );
		#endif
	}
	
	public static void startAudio()
	{
		#if UNITY_IPHONE
		if (Application.platform != RuntimePlatform.OSXEditor)
			_startAudio();
		#elif UNITY_ANDROID
			// PdBase.CallStatic("startAudio");
		#endif
	}
	
	public static void sendBangToReceiver(string receiver)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
			_sendBangToReceiver(receiver.ToCharArray(), receiver.Length * 2);
	}
	
	public static void sendFloat(float aValue, string receiver)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
			_sendFloat(aValue, receiver.ToCharArray(), receiver.Length * 2);
	}
	
	public static void subscribe(string symbol, string objectName, string methodName)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
			_subscribe(symbol.ToCharArray(), symbol.Length * 2, objectName.ToCharArray(), objectName.Length * 2, methodName.ToCharArray(), methodName.Length * 2);
	}
	
	public static void sendSymbolToReceiver(string symbol, string receiver)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
			_sendSymbolToReceiver(symbol.ToCharArray(), symbol.Length * 2, receiver.ToCharArray(), receiver.Length * 2);
	}
	
	public static void sendMessageToReceiver(string message, ArrayList arguments, string receiver)
	{
		if (Application.platform != RuntimePlatform.OSXEditor) {
			if (arguments != null) {
				string argumentList = "";//string.Join(":", arguments);
				foreach (object anObject in arguments) {
					if (anObject.GetType() == typeof(float) || anObject.GetType() == typeof(int)) {
						argumentList = argumentList + anObject.ToString() + "%f" + ":";	
					}
					else {
						argumentList = argumentList + anObject.ToString() + ":";	
					}
				}
				_sendMessageToReceiver(message.ToCharArray(), message.Length * 2, argumentList.ToCharArray(), argumentList.Length * 2, receiver.ToCharArray(), receiver.Length * 2);
			}
			else {
				_sendMessageToReceiver(message.ToCharArray(), message.Length * 2, null, 0, receiver.ToCharArray(), receiver.Length * 2);	
			}
		}
	}
	
	public static void sendListToReceiver(ArrayList list, string receiver)
	{
		if (Application.platform != RuntimePlatform.OSXEditor) {
			if (list != null) {
				string argumentList = "";// = string.Join(":", list);
				foreach (object anObject in list) {
					if (anObject.GetType() == typeof(float) || anObject.GetType() == typeof(int)) {
						argumentList = argumentList + anObject.ToString() + "%f" + ":";	
					}
					else {
						argumentList = argumentList + anObject.ToString() + ":";	
					}
				}
				argumentList = argumentList.Remove(argumentList.LastIndexOf(":"));
				_sendListToReceiver(argumentList.ToCharArray(), argumentList.Length * 2, receiver.ToCharArray(), receiver.Length * 2);
			}
		}
	}
	
//	public static void unsubscribe(string symbol)
//	{
//		if (Application.platform != RuntimePlatform.OSXEditor)
//			_unsubscribe(symbol.ToCharArray());
//	}
}
