using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using ExamsListAndroid.Models;

namespace ExamsListAndroid
{
    [Activity(Label = "ExamDetails")]
    public class ExamDetailsActivity : Activity
    {
		private const int CODE_RESULT_EXAM = 10;
		ContentValues examValues;
		TextView titleExam, numberQuestions, result, description;
		Button start;

        protected override void OnCreate(Bundle savedInstanceState)
        {
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.ExamDetailsLayout);

			titleExam = FindViewById<TextView>(Resource.Id.textTitle);
			numberQuestions = FindViewById<TextView>(Resource.Id.textNQuestions);
			result = FindViewById<TextView>(Resource.Id.textResult);
			description = FindViewById<TextView>(Resource.Id.textDescription);
			start = FindViewById<Button>(Resource.Id.startButton);

			examValues = IntentToValues();
			SetInfoInView();

			start.Click += delegate
            {
                onClick();
            };
        }

		void onClick()
        {
			var exam = new Exam(examValues);
			StartActivityForResult(exam.ToIntent(this, typeof(QuestionActivity)), CODE_RESULT_EXAM);
        }

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			if (resultCode == Result.Ok)
			{
				switch (requestCode)
				{
					case CODE_RESULT_EXAM:
						examValues.Put(ExamsDataBaseHelper._RESULTADO, data.GetIntExtra(ExamsDataBaseHelper._RESULTADO, 0));
						examValues.Put(ExamsDataBaseHelper._COMPLETADO, true);
						examValues.Put(ExamsDataBaseHelper._SINCRONIZADO, false);
						var id = examValues.GetAsInteger(ExamsDataBaseHelper._ID);
						DBHelper.InsertOrUpdateExam(this, examValues, ExamsDataBaseHelper._ID + "=?", new String[] { id.ToString() });
						SetInfoInView();
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			return;
		}

		private void SetInfoInView()
		{
			string formatCount = GetString(Resource.String.NQuestions);
			string formatRes = GetString(Resource.String.NQuestions);
			string formatDescrip = GetString(Resource.String.Description);

			long count = examValues.GetAsInteger(ExamsDataBaseHelper._NPREGUNTAS);
			int res = examValues.GetAsInteger(ExamsDataBaseHelper._RESULTADO);
			string descrip = examValues.GetAsString(ExamsDataBaseHelper._DESCRIPCION);

			titleExam.Text = examValues.GetAsString(ExamsDataBaseHelper._TITULO);
			numberQuestions.Text = string.Format(formatCount, count.ToString());
			result.Text = string.Format(formatRes, res.ToString());
			description.Text = string.Format(formatDescrip, descrip);
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