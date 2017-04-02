using Android.Content;
using Android.Database;

namespace ExamsListAndroid
{
	public static class DBHelper
	{
		/**
		 * Acceso a BBDD de Examenes.
		 */
		public static int DeleteExam(Context context, string selection, string[] selectionArgs)
		{
			return context.ContentResolver.Delete(ExamContentProvider.CONTENT_URI, selection, selectionArgs);
		}

		public static Android.Net.Uri InsertExam(Context context, ContentValues values)
		{
			return context.ContentResolver.Insert(ExamContentProvider.CONTENT_URI, values);
		}

		public static ICursor QueryExams(Context context, string selection, string[] selectionArgs)
		{
			return context.ContentResolver.Query(ExamContentProvider.CONTENT_URI, ExamsDataBaseHelper.PROJECTION, selection, selectionArgs, null);
		}

		public static int UpdateExam(Context context, ContentValues values, string selection, string[] selectionArgs)
		{
			return context.ContentResolver.Update(ExamContentProvider.CONTENT_URI, values, selection, selectionArgs);
		}

		public static void InsertOrUpdateExam(Context context, ContentValues values, string selection, string[] selectionArgs)
		{
			if (QueryExams(context, selection, selectionArgs).Count == 0)
			{
				InsertExam(context, values);
			}
			else
			{
				UpdateExam(context, values, selection, selectionArgs);
			}
		}

		/**
		 * Acceso a BBDD de Preguntas.
		 */
		public static int DeleteQuestion(Context context, string selection, string[] selectionArgs)
		{
			return context.ContentResolver.Delete(QuestionContentProvider.CONTENT_URI, selection, selectionArgs);
		}

		public static Android.Net.Uri InsertQuestion(Context context, ContentValues values)
		{
			return context.ContentResolver.Insert(QuestionContentProvider.CONTENT_URI, values);
		}

		public static ICursor QueryQuestions(Context context, string selection, string[] selectionArgs)
		{
			return context.ContentResolver.Query(QuestionContentProvider.CONTENT_URI, QuestionsDataBaseHelper.PROJECTION, selection, selectionArgs, null);
		}

		public static int UpdateQuestion(Context context, ContentValues values, string selection, string[] selectionArgs)
		{
			return context.ContentResolver.Update(QuestionContentProvider.CONTENT_URI, values, selection, selectionArgs);
		}

		public static void InsertOrUpdateQuestion(Context context, ContentValues values, string selection, string[] selectionArgs)
		{
			if (QueryQuestions(context, selection, selectionArgs).Count == 0)
			{
				InsertQuestion(context, values);
			}
			else
			{
				UpdateQuestion(context, values, selection, selectionArgs);
			}
		}
	}
}
