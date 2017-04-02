using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Views;
using Android.Widget;
using ExamsListAndroid.Models;

namespace ExamsListAndroid
{
	[Activity(Label = "ExamList", MainLauncher = true, Icon = "@drawable/icon")]
	public class ExamListActivity : Activity, LoaderManager.ILoaderCallbacks
	{
		private const int CODE_REFRESH = 10;
		private const int CODE_SEND = 20;

		ExamCursorAdapter adapter;
		ListView examList;
		List<Exam> listExams;
		IMenuItem countPending;
		int count;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.MainLayout);

			examList = FindViewById<ListView>(Resource.Id.examListView);
			examList.FastScrollEnabled = true;
			examList.ItemClick += OnItemClick;

			LoaderManager.InitLoader(0, null, this);
			adapter = new ExamCursorAdapter(this, null);
			examList.Adapter = adapter;
		}

		void OnItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			var exam = (Exam) adapter.getItem(e.Position);
			StartActivity(exam.ToIntent(this, typeof(ExamDetailsActivity)));
		}

		public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.menu, menu);
			countPending = menu.GetItem(0);
			count = DBHelper.QueryExams(this, ExamsDataBaseHelper._SINCRONIZADO + "=?", new string[] { "0" }).Count;
			countPending.SetTitle(count.ToString());
			return true;
		}

		public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
		{
			ConnectionService connection = new ConnectionService();
			switch (item.ItemId)
			{
				case Resource.Id.pending:
					if (count != 0)
					{
						AlertDialog.Builder alert = new AlertDialog.Builder(this);
						alert.SetTitle(GetString(Resource.String.PendingUpload));
						ICursor cursor = DBHelper.QueryExams(this, ExamsDataBaseHelper._SINCRONIZADO + "=?", new string[] { "0" });
						alert.SetCursor(cursor, delegate
						{

						}, "Titulo");
						alert.SetNeutralButton("Sincronizar", (senderAlert, args) =>
						{
							listExams = new List<Exam>();
							cursor.MoveToFirst();
							do
								listExams.Add(CursorItemToExam(cursor));
							while (cursor.MoveToNext());

							if (!connection.SendResultExam(this, listExams))
								StartActivityForResult(typeof(LoginActivity), CODE_SEND);
							foreach (var exam in listExams)
							{
								DBHelper.InsertOrUpdateExam(this, exam.ToValues(), ExamsDataBaseHelper._ID + "=?", new string[] { exam.ID.ToString() });
							}
							count = DBHelper.QueryExams(this, ExamsDataBaseHelper._SINCRONIZADO + "=?", new string[] { "0" }).Count;
							countPending.SetTitle(count.ToString());
							Toast.MakeText(this, "Sincronizado!", ToastLength.Short).Show();
						});
						Dialog dialog = alert.Create();
						dialog.Show();
					}
					return true;
				//case Resource.Id.logout:
				//	LoginActivity.SetToken(this, null);
				//	Logout(GetString(Resource.String.MessageLogOut));
				//	return true;
				case Resource.Id.refresh:
					if (!connection.GetExams(this))
						StartActivityForResult(typeof(LoginActivity), CODE_REFRESH);
					return true;
				default:
					return base.OnOptionsItemSelected(item);
			}
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			ConnectionService connection = new ConnectionService();
			if (resultCode == Result.Ok)
			{
				switch (requestCode)
				{
					case CODE_REFRESH:
						connection.GetExams(this);
						break;
					case CODE_SEND:
						connection.SendResultExam(this, listExams);
						listExams.Clear();
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			return;
		}

		public Loader OnCreateLoader(int id, Bundle args)
		{
			return new CursorLoader(this, ExamContentProvider.CONTENT_URI, ExamsDataBaseHelper.PROJECTION, null, null, null);
		}

		public void OnLoaderReset(Loader loader)
		{
			adapter.ChangeCursor(null);
		}

		public void OnLoadFinished(Loader loader, Java.Lang.Object data)
		{
			adapter.ChangeCursor((ICursor)data);
		}

		protected override void OnRestart()
		{
			base.OnRestart();
			count = DBHelper.QueryExams(this, ExamsDataBaseHelper._SINCRONIZADO + "=?", new string[] { "0" }).Count;
			countPending.SetTitle(count.ToString());
		}

		Exam CursorItemToExam(ICursor cursor)
		{
			long id = cursor.GetLong(cursor.GetColumnIndex(ExamsDataBaseHelper._ID));
			string title = cursor.GetString(cursor.GetColumnIndex(ExamsDataBaseHelper._TITULO));
			int nQuestions = cursor.GetInt(cursor.GetColumnIndex(ExamsDataBaseHelper._NPREGUNTAS));
			int result = cursor.GetInt(cursor.GetColumnIndex(ExamsDataBaseHelper._RESULTADO));
			bool complete = (cursor.GetInt(cursor.GetColumnIndex(ExamsDataBaseHelper._COMPLETADO)) == 1);
			string description = cursor.GetString(cursor.GetColumnIndex(ExamsDataBaseHelper._DESCRIPCION));

			return new Exam { ID = id, Titulo = title, NPreguntas = nQuestions, Resultado = result, Completado = complete, Descripcion = description, Sincronizado = true };
		}
	}
}

