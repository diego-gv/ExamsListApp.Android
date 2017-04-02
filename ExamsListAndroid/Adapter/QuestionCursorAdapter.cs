using System;
using Android.App;
using Android.Content;
using Android.Database;
using Android.Views;
using Android.Widget;


namespace ExamsListAndroid
{
	public class QuestionCursorAdapter : CursorAdapter
	{

		public class ViewHolder : Java.Lang.Object
		{
			public TextView Text { get; set; }
			public Button Option1 { get; set; }
			public Button Option2 { get; set; }
		}

		Activity _context;

		public QuestionCursorAdapter(Activity context, ICursor cursor) : base(context, cursor, false)
		{
			_context = context;
		}

		public override void BindView(View view, Context context, ICursor cursor)
		{
			ViewHolder holder = (ViewHolder)view.Tag;

			holder.Text.Text = cursor.GetString(cursor.GetColumnIndex(QuestionsDataBaseHelper._ENUNCIADO));
			holder.Option1.Text = cursor.GetString(cursor.GetColumnIndex(QuestionsDataBaseHelper._A));
			holder.Option2.Text = cursor.GetString(cursor.GetColumnIndex(QuestionsDataBaseHelper._B));
		}

		public override View NewView(Context context, ICursor cursor, ViewGroup parent)
		{
			ViewHolder holder = new ViewHolder();

			View view = _context.LayoutInflater.Inflate(Resource.Layout.QuestionLayout, parent, false);
			holder.Text = view.FindViewById<TextView>(Resource.Id.textQuestion);
			holder.Option1 = view.FindViewById<Button>(Resource.Id.option1);
			holder.Option2 = view.FindViewById<Button>(Resource.Id.option2);
			view.Tag = holder;

			return view;
		}

		public Object getItem(int position)
		{
			ICursor cursor = this.Cursor; 
			cursor.MoveToPosition(position);

			long id = cursor.GetLong(cursor.GetColumnIndex(QuestionsDataBaseHelper._ID));
			long examId = cursor.GetLong(cursor.GetColumnIndex(QuestionsDataBaseHelper._IDEXAMEN));
			string text = cursor.GetString(cursor.GetColumnIndex(QuestionsDataBaseHelper._ENUNCIADO));
			string op1 = cursor.GetString(cursor.GetColumnIndex(QuestionsDataBaseHelper._A));
			string op2 = cursor.GetString(cursor.GetColumnIndex(QuestionsDataBaseHelper._B));
			int val1 = cursor.GetInt(cursor.GetColumnIndex(QuestionsDataBaseHelper._VALOR_A));
			int val2 = cursor.GetInt(cursor.GetColumnIndex(QuestionsDataBaseHelper._VALOR_B));

			return new Question { ID = id, id_examen = examId, Enunciado = text, A = op1, B = op2, ValorA = val1, ValorB = val2 };
		}
	}
}
