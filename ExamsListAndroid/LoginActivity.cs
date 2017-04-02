using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace ExamsListAndroid
{
	[Activity(Label = "Login")]
	public class LoginActivity : Activity
	{
		/*
		 * Constante con el nombre de nuestra colección
		 * de preferencias. 
		 */
		public const string APP_PREFERENCES = "appPreferences";

		/*
		 * Constante para acceder a la variable del token de
		 * login en Preferences y SharedPreferences.
		 */
		public const string LOGIN_TOKEN = "loginToken";
		ConnectionService connection;
		Button enter;
		EditText user, pass;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.LoginLayout);

			connection = new ConnectionService();

			user = FindViewById<EditText>(Resource.Id.textUser);
			pass = FindViewById<EditText>(Resource.Id.textPassword);
			enter = FindViewById<Button>(Resource.Id.buttonEnter);
			enter.Click += delegate
			{
				onClick();
			};

		}

		public void onClick()
		{
			if (connection.LoginService(this, user.Text, pass.Text))
			{
				SetResult(Result.Ok);
				Finish();
			}
			else
			{
				TextView msgError = FindViewById<TextView>(Resource.Id.labelErrorLogin);
				msgError.Visibility = ViewStates.Visible;
			}
		}
	}
}
