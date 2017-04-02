using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Database;
using Android.Database.Sqlite;

namespace ExamsListAndroid
{
	[ContentProvider(new[] { "examslistandroid.examslistandroid.ExamProvider" }, Name = "examslistandroid.examslistandroid.ExamContentProvider")]
	public class ExamContentProvider : ContentProvider
	{
		public static string AUTHORITY = "examslistandroid.examslistandroid.ExamProvider";

		public static Android.Net.Uri CONTENT_URI = Android.Net.Uri.Parse("content://" + AUTHORITY + "/exams");

		private const int EXAMS = 1;
		private const int EXAM_ID = 2;


		private const string EXAM_TYPE = "vnd.android.cursor.dir/vnd.examslistandroid.exams";
		private const string EXAM_ITEM_TYPE = "vnd.android.cursor.item/vnd.examslistandroid.exams";

		private static UriMatcher uriMatcher;
		private ExamsDataBaseHelper dbHelper;
		private static IDictionary<string, string> mProjectionMap;

		public ExamContentProvider()
		{
			mProjectionMap = new Dictionary<string, string>();
			mProjectionMap.Add(ExamsDataBaseHelper._ID, ExamsDataBaseHelper._ID);
			mProjectionMap.Add(ExamsDataBaseHelper._TITULO, ExamsDataBaseHelper._TITULO);
			mProjectionMap.Add(ExamsDataBaseHelper._NPREGUNTAS, ExamsDataBaseHelper._NPREGUNTAS);
			mProjectionMap.Add(ExamsDataBaseHelper._RESULTADO, ExamsDataBaseHelper._RESULTADO);
			mProjectionMap.Add(ExamsDataBaseHelper._COMPLETADO, ExamsDataBaseHelper._COMPLETADO);
			mProjectionMap.Add(ExamsDataBaseHelper._DESCRIPCION, ExamsDataBaseHelper._DESCRIPCION);
			mProjectionMap.Add(ExamsDataBaseHelper._SINCRONIZADO, ExamsDataBaseHelper._SINCRONIZADO);

			uriMatcher = new UriMatcher(UriMatcher.NoMatch);
			uriMatcher.AddURI(AUTHORITY, "exams", EXAMS);
			uriMatcher.AddURI(AUTHORITY, "exams/#", EXAM_ID);
		}

		public override int Delete(Android.Net.Uri uri, string selection, string[] selectionArgs)
		{
			SQLiteDatabase db = dbHelper.WritableDatabase;
			int count;
			switch (uriMatcher.Match(uri))
			{
				case EXAMS:
					count = db.Delete(ExamsDataBaseHelper.DATABASE_TABLE_NAME_EXAMS, selection, selectionArgs);
					break;
				case EXAM_ID:
					string examId = uri.PathSegments.ElementAt(1);
					string select = "";

					if (selection != null && selection.Length > 0)
						select = " AND (" + selection + ")";
					count = db.Delete(ExamsDataBaseHelper.DATABASE_TABLE_NAME_EXAMS, ExamsDataBaseHelper._ID + "=" + examId + select, selectionArgs);
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
				case EXAMS:
					return EXAM_TYPE;
				case EXAM_ID:
					return EXAM_ITEM_TYPE;
				default:
					throw new ArgumentException("Unsupported URI: " + uri);
			}		
		}

		public override Android.Net.Uri Insert(Android.Net.Uri uri, ContentValues values)
		{
			if (uriMatcher.Match(uri) != EXAMS) 
				throw new ArgumentException("Unknown URI " + uri);

			ContentValues contentValues;
			if (values != null)
				contentValues = new ContentValues(values);
			else contentValues = new ContentValues();

			if (contentValues.ContainsKey(ExamsDataBaseHelper._TITULO) == false)
				throw new SQLException("Failed to insert row into " + uri + ". Query is missing TITLE!");

			SQLiteDatabase db = dbHelper.WritableDatabase;
			long rowId = db.Insert(ExamsDataBaseHelper.DATABASE_TABLE_NAME_EXAMS, ExamsDataBaseHelper._TITULO, contentValues);
			if (rowId > 0)
			{
				Android.Net.Uri examUri = ContentUris.WithAppendedId(CONTENT_URI, rowId);
				Context.ContentResolver.NotifyChange(examUri, null);
				return examUri;
			}

			throw new SQLException("Failed to insert row into " + uri);
		}

		public override bool OnCreate()
		{
			dbHelper = new ExamsDataBaseHelper(Context);
			return true;
		}

		public override ICursor Query(Android.Net.Uri uri, string[] projection, string selection, string[] selectionArgs, string sortOrder)
		{
			SQLiteQueryBuilder qb = new SQLiteQueryBuilder();
			qb.Tables = ExamsDataBaseHelper.DATABASE_TABLE_NAME_EXAMS;

			switch (uriMatcher.Match(uri))
			{
				case EXAMS:
					qb.SetProjectionMap(mProjectionMap);
					break;
				case EXAM_ID:
					qb.SetProjectionMap(mProjectionMap);
					qb.AppendWhere(ExamsDataBaseHelper._ID + "=" + uri.PathSegments.ElementAt(1));
					break;
				default:
					throw new ArgumentException("Unknown URI " + uri);
			}

			string orderBy;
			if (sortOrder == null || sortOrder.Length < 1)
			{
				orderBy = ExamsDataBaseHelper.DEFAULT_SORT_ORDER;
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
				case EXAMS:
					count = db.Update(ExamsDataBaseHelper.DATABASE_TABLE_NAME_EXAMS, values, selection, selectionArgs);
					break;
				case EXAM_ID:
					string examId = uri.PathSegments.ElementAt(1);
					string select = "";
					if (selection != null && selection.Length > 0)
						select = " AND (" + selection + ")";
					count = db.Update(ExamsDataBaseHelper.DATABASE_TABLE_NAME_EXAMS, values, ExamsDataBaseHelper._ID + "=" + examId + select, selectionArgs);
					break;
				default:
					throw new ArgumentException("Unknown URI " + uri);
			}

			Context.ContentResolver.NotifyChange(uri, null);
			return count;
		}
	}
}
