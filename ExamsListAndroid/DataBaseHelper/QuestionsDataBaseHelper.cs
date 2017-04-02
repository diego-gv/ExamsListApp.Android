using Android.Content;
using Android.Database.Sqlite;

namespace ExamsListAndroid
{
	public class QuestionsDataBaseHelper : SQLiteOpenHelper
	{
		public const string DATABASE_TABLE_NAME_QUESTIONS = "Questions";
		public const int DATABASE_VERSION = 1;
		public const string DEFAULT_SORT_ORDER = _ID + " DESC";

		public const string _ID = "_id";
		public const string _IDEXAMEN = "idExamen";
		public const string _ENUNCIADO = "Titulo";
		public const string _A = "A";
		public const string _B = "B";
		public const string _VALOR_A = "ValorA";
		public const string _VALOR_B = "ValorB";

		public static string[] PROJECTION = { _ID, _IDEXAMEN, _ENUNCIADO, _A, _B, _VALOR_A, _VALOR_B };

		public QuestionsDataBaseHelper(Context context) : base(context, DATABASE_TABLE_NAME_QUESTIONS, null, DATABASE_VERSION)
		{
		}

		public override void OnCreate(SQLiteDatabase db)
		{
			db.ExecSQL(@"CREATE TABLE " + DATABASE_TABLE_NAME_QUESTIONS + " ("
					   + _ID + " INTEGER PRIMARY KEY NOT NULL, "
			           + _IDEXAMEN + " INTEGER NOT NULL, "
			           + _ENUNCIADO + " TEXT NOT NULL, "
			           + _A + " TEXT NOT NULL, "
			           + _B + " TEXT NOT NULL, "
			           + _VALOR_A + " INTEGER NOT NULL, "
			           + _VALOR_B + " INTEGER NOT NULL);"
					  );
		}

		public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
		{
			db.ExecSQL("DROP TABLE IF EXISTS " + DATABASE_TABLE_NAME_QUESTIONS);

			OnCreate(db);
		}
	}
}
