package md55b5d186084d19538c73e2dc75efd9909;


public class QuestionActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer,
		android.app.LoaderManager.LoaderCallbacks
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onCreateLoader:(ILandroid/os/Bundle;)Landroid/content/Loader;:GetOnCreateLoader_ILandroid_os_Bundle_Handler:Android.App.LoaderManager/ILoaderCallbacksInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_onLoaderReset:(Landroid/content/Loader;)V:GetOnLoaderReset_Landroid_content_Loader_Handler:Android.App.LoaderManager/ILoaderCallbacksInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_onLoadFinished:(Landroid/content/Loader;Ljava/lang/Object;)V:GetOnLoadFinished_Landroid_content_Loader_Ljava_lang_Object_Handler:Android.App.LoaderManager/ILoaderCallbacksInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("ExamsListAndroid.QuestionActivity, ExamsListAndroid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", QuestionActivity.class, __md_methods);
	}


	public QuestionActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == QuestionActivity.class)
			mono.android.TypeManager.Activate ("ExamsListAndroid.QuestionActivity, ExamsListAndroid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public android.content.Loader onCreateLoader (int p0, android.os.Bundle p1)
	{
		return n_onCreateLoader (p0, p1);
	}

	private native android.content.Loader n_onCreateLoader (int p0, android.os.Bundle p1);


	public void onLoaderReset (android.content.Loader p0)
	{
		n_onLoaderReset (p0);
	}

	private native void n_onLoaderReset (android.content.Loader p0);


	public void onLoadFinished (android.content.Loader p0, java.lang.Object p1)
	{
		n_onLoadFinished (p0, p1);
	}

	private native void n_onLoadFinished (android.content.Loader p0, java.lang.Object p1);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
