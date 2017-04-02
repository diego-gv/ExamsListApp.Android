using Android.App;
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Widget;
using ExamsListAndroid.Models;

namespace ExamsListAndroid
{
	[Activity(Label = "Questions")]
	public class QuestionActivity : Activity, LoaderManager.ILoaderCallbacks
	{
		ContentValues examValues;
		int points = 0, numberQ = 0;
		Question question;
		QuestionCursorAdapter cursor;
		TextView textNumber, textQuestion;
		Button option1, option2;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.QuestionLayout);

			textNumber = FindViewById<TextView>(Resource.Id.textNumber);
			textQuestion = FindViewById<TextView>(Resource.Id.textQuestion);
			option1 = FindViewById<Button>(Resource.Id.option1);
			option2 = FindViewById<Button>(Resource.Id.option2);

			examValues = IntentToValues();
			LoaderManager.InitLoader(0, null, this);
			var _id = examValues.GetAsString(ExamsDataBaseHelper._ID);
			ICursor c = DBHelper.QueryQuestions(this, QuestionsDataBaseHelper._IDEXAMEN + "=?", new string[] { _id });
			cursor = new QuestionCursorAdapter(this, c);

			SetInfoInView();

			// Subscribe buttons.
			option1.Click += delegate
			{
				onClick(question.ValorA);
			};

			option2.Click += delegate
			{
				onClick(question.ValorB);
			};
		}

		void onClick(int val)
		{
			points += val;
			numberQ++;
			if (numberQ >= cursor.Count)
			{
				examValues.Put(ExamsDataBaseHelper._RESULTADO, points);
				var exam = new Exam(examValues);
				SetResult(Result.Ok, exam.ToIntent(this, typeof(ExamDetailsActivity)));

				StartActivity(exam.ToIntent(this, typeof(FinishExamActivity)));
				Finish();
			}
			else
			{
				SetInfoInView();
			}
		}

		private void SetInfoInView()
		{
			string formatText = GetString(Resource.String.Number);

			question = (Question)cursor.getItem(numberQ);
			textNumber.Text = string.Format(formatText, numberQ + 1);
			textQuestion.Text = question.Enunciado;
			option1.Text = question.A;
			option2.Text = question.B;
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

		public Loader OnCreateLoader(int id, Bundle args)
		{
			 return new CursorLoader(this, QuestionContentProvider.CONTENT_URI, QuestionsDataBaseHelper.PROJECTION, QuestionsDataBaseHelper._IDEXAMEN + "=?", new string[] { examValues.GetAsString(ExamsDataBaseHelper._ID) }, null);
		}

		public void OnLoaderReset(Loader loader)
		{
			cursor.ChangeCursor(null);
		}

		public void OnLoadFinished(Loader loader, Java.Lang.Object data)
		{
			cursor.ChangeCursor((ICursor)data);
		}
	}
}
