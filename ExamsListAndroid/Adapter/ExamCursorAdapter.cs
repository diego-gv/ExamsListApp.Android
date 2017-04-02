using System;
using Android.App;
using Android.Content;
using Android.Database;
using Android.Views;
using Android.Widget;
using ExamsListAndroid.Models;

namespace ExamsListAndroid
{
	public class ExamCursorAdapter : CursorAdapter
	{
		public class ViewHolder : Java.Lang.Object
		{
			public TextView Title { get; set; }
			public TextView Result { get; set; }
		}

		Activity _context;

		public ExamCursorAdapter(Activity context, ICursor cursor) : base(context, cursor)
   		{
			_context = context;
		}

		public override void BindView(View view, Context context, ICursor cursor)
		{
			ViewHolder holder = (ViewHolder) view.Tag;

			string formatResult = context.GetString(Resource.String.Result);
			string result = cursor.GetString(cursor.GetColumnIndex(ExamsDataBaseHelper._RESULTADO));

			holder.Title.Text = cursor.GetString(cursor.GetColumnIndex(ExamsDataBaseHelper._TITULO));
			holder.Result.Text = string.Format(formatResult, result);
		}

		public override View NewView(Context context, ICursor cursor, ViewGroup parent)
		{
			ViewHolder holder = new ViewHolder();

			View view = _context.LayoutInflater.Inflate(Resource.Layout.ExamRowLayout, parent, false);
			holder.Title = view.FindViewById<TextView>(Resource.Id.textTitle);
			holder.Result = view.FindViewById<TextView>(Resource.Id.textResult);
			view.Tag = holder;

			return view;
		}

		public Object getItem(int position)
		{
			ICursor cursor = this.Cursor;
			cursor.MoveToPosition(position);

			long id = cursor.GetLong(cursor.GetColumnIndex(ExamsDataBaseHelper._ID));
			string title = cursor.GetString(cursor.GetColumnIndex(ExamsDataBaseHelper._TITULO));
			int nQuestions = cursor.GetInt(cursor.GetColumnIndex(ExamsDataBaseHelper._NPREGUNTAS));
			int result = cursor.GetInt(cursor.GetColumnIndex(ExamsDataBaseHelper._RESULTADO));
			bool complete = (cursor.GetInt(cursor.GetColumnIndex(ExamsDataBaseHelper._COMPLETADO)) == 1);
			string description = cursor.GetString(cursor.GetColumnIndex(ExamsDataBaseHelper._DESCRIPCION));
			bool sincronizado = (cursor.GetInt(cursor.GetColumnIndex(ExamsDataBaseHelper._SINCRONIZADO)) == 1);

			return new Exam { ID = id, Titulo = title, NPreguntas = nQuestions, Resultado = result, Completado = complete, Descripcion = description, Sincronizado = sincronizado };
		}
	}
}
