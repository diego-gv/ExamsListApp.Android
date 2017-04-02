using Android.Content;
using Android.Database.Sqlite;

namespace ExamsListAndroid
{
	public class ExamsDataBaseHelper : SQLiteOpenHelper
	{
		public const string DATABASE_TABLE_NAME_EXAMS = "Exams";
		public const int DATABASE_VERSION = 1;
		public const string DEFAULT_SORT_ORDER = _TITULO + " DESC";

		public const string _ID = "_id";
		public const string _TITULO = "Titulo";
		public const string _NPREGUNTAS = "NPreguntas";
		public const string _RESULTADO = "Resultado";
		public const string _COMPLETADO = "Completado";
		public const string _DESCRIPCION = "Descripcion";
		public const string _SINCRONIZADO = "Sincronizado";


		public static string[] PROJECTION = { _ID, _TITULO, _NPREGUNTAS, _RESULTADO, _COMPLETADO, _DESCRIPCION, _SINCRONIZADO };

		public ExamsDataBaseHelper(Context context) : base(context, DATABASE_TABLE_NAME_EXAMS, null, DATABASE_VERSION)
		{
		}

		public override void OnCreate(SQLiteDatabase db)
		{
			db.ExecSQL(@"CREATE TABLE " + DATABASE_TABLE_NAME_EXAMS + " ("
					   + _ID + " INTEGER PRIMARY KEY NOT NULL, "
					   + _TITULO + " TEXT NOT NULL, "
					   + _NPREGUNTAS + " INTEGER NOT NULL, "
					   + _RESULTADO + " INTEGER NOT NULL, "
					   + _COMPLETADO + " NUMERIC NOT NULL, "
					   + _DESCRIPCION + " TEXT NOT NULL, "
			           + _SINCRONIZADO + " NUMERIC NOT NULL);"
					  );
		}

		public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
		{
			db.ExecSQL("DROP TABLE IF EXISTS " + DATABASE_TABLE_NAME_EXAMS);

			OnCreate(db);
		}
	}
}
