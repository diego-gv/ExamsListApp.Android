using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Database;
using Android.Database.Sqlite;

namespace ExamsListAndroid
{
	[ContentProvider(new[] { "examslistandroid.examslistandroid.QuestionProvider" }, Name = "examslistandroid.examslistandroid.QuestionContentProvider")]
	public class QuestionContentProvider : ContentProvider
	{
		public static string AUTHORITY = "examslistandroid.examslistandroid.QuestionProvider";

		public static Android.Net.Uri CONTENT_URI = Android.Net.Uri.Parse("content://" + AUTHORITY + "/questions");

		private const int QUESTIONS = 1;
		private const int QUESTION_ID = 2;


		private const string QUESTION_TYPE = "vnd.android.cursor.dir/vnd.examslistandroid.questions";
		private const string QUESTION_ITEM_TYPE = "vnd.android.cursor.item/vnd.examslistandroid.questions";

		private static UriMatcher uriMatcher;
		private QuestionsDataBaseHelper dbHelper;
		private static IDictionary<string, string> mProjectionMap;

		public QuestionContentProvider()
		{
			mProjectionMap = new Dictionary<string, string>();
			mProjectionMap.Add(QuestionsDataBaseHelper._ID, QuestionsDataBaseHelper._ID);
			mProjectionMap.Add(QuestionsDataBaseHelper._IDEXAMEN, QuestionsDataBaseHelper._IDEXAMEN);
			mProjectionMap.Add(QuestionsDataBaseHelper._ENUNCIADO, QuestionsDataBaseHelper._ENUNCIADO);
			mProjectionMap.Add(QuestionsDataBaseHelper._A, QuestionsDataBaseHelper._A);
			mProjectionMap.Add(QuestionsDataBaseHelper._B, QuestionsDataBaseHelper._B);
			mProjectionMap.Add(QuestionsDataBaseHelper._VALOR_A, QuestionsDataBaseHelper._VALOR_A);
			mProjectionMap.Add(QuestionsDataBaseHelper._VALOR_B, QuestionsDataBaseHelper._VALOR_B);

			uriMatcher = new UriMatcher(UriMatcher.NoMatch);
			uriMatcher.AddURI(AUTHORITY, "questions", QUESTIONS);
			uriMatcher.AddURI(AUTHORITY, "questions/#", QUESTION_ID);
		}

		public override int Delete(Android.Net.Uri uri, string selection, string[] selectionArgs)
		{
			SQLiteDatabase db = dbHelper.WritableDatabase;
			int count;
			switch (uriMatcher.Match(uri))
			{
				case QUESTIONS:
					count = db.Delete(QuestionsDataBaseHelper.DATABASE_TABLE_NAME_QUESTIONS, selection, selectionArgs);
					break;
				case QUESTION_ID:
					string questionId = uri.PathSegments.ElementAt(1);
					string select = "";

					if (selection != null && selection.Length > 0)
						select = " AND (" + selection + ")";
					count = db.Delete(QuestionsDataBaseHelper.DATABASE_TABLE_NAME_QUESTIONS, QuestionsDataBaseHelper._ID + "=" + questionId + select, selectionArgs);
					break;
				default:
					throw new ArgumentException("Unknown URI " + uri);
			}

			Context.ContentResolver.NotifyChange(uri, null);
			return count;
		}

		public override string GetType(Android.Net.Uri uri)
		{
			switch (uriMatcher.Match(uri))
			{
				case QUESTIONS:
					return QUESTION_TYPE;
				case QUESTION_ID:
					return QUESTION_ITEM_TYPE;
				default:
					throw new ArgumentException("Unsupported URI: " + uri);
			}
		}

		public override Android.Net.Uri Insert(Android.Net.Uri uri, ContentValues values)
		{
			if (uriMatcher.Match(uri) != QUESTIONS)
				throw new ArgumentException("Unknown URI " + uri);

			ContentValues contentValues;
			if (values != null)
				contentValues = new ContentValues(values);
			else contentValues = new ContentValues();

			if (contentValues.ContainsKey(QuestionsDataBaseHelper._ENUNCIADO) == false)
				throw new SQLException("Failed to insert row into " + uri + ". Query is missing TEXT!");

			SQLiteDatabase db = dbHelper.WritableDatabase;
			long rowId = db.Insert(QuestionsDataBaseHelper.DATABASE_TABLE_NAME_QUESTIONS, QuestionsDataBaseHelper._ENUNCIADO, contentValues);
			if (rowId > 0)
			{
				Android.Net.Uri questionUri = ContentUris.WithAppendedId(CONTENT_URI, rowId);
				Context.ContentResolver.NotifyChange(questionUri, null);
				return questionUri;
			}

			throw new SQLException("Failed to insert row into " + uri);
		}

		public override bool OnCreate()
		{
			dbHelper = new QuestionsDataBaseHelper(Context);
			return true;
		}

		public override ICursor Query(Android.Net.Uri uri, string[] projection, string selection, string[] selectionArgs, string sortOrder)
		{
			SQLiteQueryBuilder qb = new SQLiteQueryBuilder();
			qb.Tables = QuestionsDataBaseHelper.DATABASE_TABLE_NAME_QUESTIONS;

			switch (uriMatcher.Match(uri))
			{
				case QUESTIONS:
					qb.SetProjectionMap(mProjectionMap);
					break;
				case QUESTION_ID:
					qb.SetProjectionMap(mProjectionMap);
					qb.AppendWhere(QuestionsDataBaseHelper._ID + "=" + uri.PathSegments.ElementAt(1));
					break;
				default:
					throw new ArgumentException("Unknown URI " + uri);
			}

			string orderBy;
			if (sortOrder == null || sortOrder.Length < 1)
			{
				orderBy = QuestionsDataBaseHelper.DEFAULT_SORT_ORDER;
			}
			else
			{
				orderBy = sortOrder;
			}

			// Get the database and run the query
			SQLiteDatabase db = dbHelper.ReadableDatabase;
			ICursor c = qb.Query(db, projection, selection, selectionArgs, null, null, orderBy);
			c.SetNotificationUri(Context.ContentResolver, uri);

			return c;
		}

		public override int Update(Android.Net.Uri uri, ContentValues values, string selection, string[] selectionArgs)
		{
			SQLiteDatabase db = dbHelper.WritableDatabase;
			int count;
			switch (uriMatcher.Match(uri))
			{
				case QUESTIONS:
					count = db.Update(QuestionsDataBaseHelper.DATABASE_TABLE_NAME_QUESTIONS, values, selection, selectionArgs);
					break;
				case QUESTION_ID:
					string questionId = uri.PathSegments.ElementAt(1);
					string select = "";
					if (selection != null && selection.Length > 0)
						select = " AND (" + selection + ")";
					count = db.Update(QuestionsDataBaseHelper.DATABASE_TABLE_NAME_QUESTIONS, values, QuestionsDataBaseHelper._ID + "=" + questionId + select, selectionArgs);
					break;
				default:
					throw new ArgumentException("Unknown URI " + uri);
			}

			Context.ContentResolver.NotifyChange(uri, null);
			return count;
		}
	}
}

