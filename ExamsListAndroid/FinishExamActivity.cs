using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace ExamsListAndroid
{
	[Activity(Label = "FinishExam")]
	public class FinishExamActivity : Activity
	{
		ContentValues examValues;
		TextView title, result, finalResult;
		Button ok;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.FinishExamLayout);

			title = FindViewById<TextView>(Resource.Id.titleExam);
			result = FindViewById<TextView>(Resource.Id.numberResultExam);
			finalResult = FindViewById<TextView>(Resource.Id.finalResult);
			ok = FindViewById<Button>(Resource.Id.buttonFinal);

			examValues = IntentToValues();

			title.Text = examValues.GetAsString(ExamsDataBaseHelper._TITULO);
			var points = examValues.GetAsInteger(ExamsDataBaseHelper._RESULTADO);
			var nQues = examValues.GetAsInteger(ExamsDataBaseHelper._NPREGUNTAS);
			var f = ((points / 4) * 10);
			result.Text = points.ToString() + "/" + nQues.ToString();
			finalResult.Text = string.Format(GetString(Resource.String.FinalResult), f.ToString());

			var _id = examValues.GetAsString(ExamsDataBaseHelper._ID);
			DBHelper.InsertOrUpdateExam(this, examValues, ExamsDataBaseHelper._ID + "=?", new string[] { _id });

			ok.Click += delegate
			{
				Finish();
			};
		}

		public ContentValues IntentToValues()
		{
			ContentValues values = new ContentValues();
			values.Put(ExamsDataBaseHelper._ID, Intent.GetLongExtra(ExamsDataBaseHelper._ID, 0));
			values.Put(ExamsDataBaseHelper._TITULO, Intent.GetStringExtra(ExamsDataBaseHelper._TITULO));
			values.Put(ExamsDataBaseHelper._NPREGUNTAS, Intent.GetIntExtra(ExamsDataBaseHelper._NPREGUNTAS, 0));
			values.Put(ExamsDataBaseHelper._RESULTADO, Intent.GetIntExtra(ExamsDataBaseHelper._RESULTADO, 0));
			values.Put(ExamsDataBaseHelper._COMPLETADO, Intent.GetBooleanExtra(ExamsDataBaseHelper._COMPLETADO, false));
			values.Put(ExamsDataBaseHelper._DESCRIPCION, Intent.GetStringExtra(ExamsDataBaseHelper._DESCRIPCION));
			values.Put(ExamsDataBaseHelper._SINCRONIZADO, Intent.GetBooleanExtra(ExamsDataBaseHelper._SINCRONIZADO, true));

			return values;
		}
	}
}
