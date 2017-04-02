using System;
using Android.Content;
using Newtonsoft.Json;
using SQLite;

namespace ExamsListAndroid.Models
{
	public class Exam
    {
		[PrimaryKey, AutoIncrement, JsonProperty(PropertyName = "ID")]
		public long ID { get; set; }

		// public long idEmpresa { get; set; }
		[JsonProperty(PropertyName = "Titulo")]
        public string Titulo { get; set; }

		[JsonProperty(PropertyName = "NPreguntas")]
        public int NPreguntas { get; set; }

		[JsonProperty(PropertyName = "Resultado")]
        public int Resultado { get; set; }

		[JsonProperty(PropertyName = "Completado")]
		public bool Completado { get; set; }

		[JsonProperty(PropertyName = "Descripcion")]
        public string Descripcion { get; set; }

		[JsonProperty(PropertyName = "Sincronizado")]
		public bool Sincronizado { get; set; }


		public Exam()
		{

		}

		public Exam(ContentValues values)
		{
			ID = values.GetAsLong(ExamsDataBaseHelper._ID);
			Titulo = values.GetAsString(ExamsDataBaseHelper._TITULO);
			NPreguntas = values.GetAsInteger(ExamsDataBaseHelper._NPREGUNTAS);
			Resultado = values.GetAsInteger(ExamsDataBaseHelper._RESULTADO);
			Completado = values.GetAsBoolean(ExamsDataBaseHelper._COMPLETADO);
			Descripcion = values.GetAsString(ExamsDataBaseHelper._DESCRIPCION);
			Sincronizado = values.GetAsBoolean(ExamsDataBaseHelper._SINCRONIZADO);

		}

		public override string ToString()
		{
			return string.Format("[Exam: ID={0}, Titulo={1}, NPreguntas={2}, Resultado={3}, Completado={4}, Descripcion={5}, Sincronizado={6}]", ID, Titulo, NPreguntas, Resultado, Completado, Descripcion, Sincronizado);
		}

		public ContentValues ToValues()
		{
			ContentValues values = new ContentValues();
			values.Put(ExamsDataBaseHelper._ID, ID);
			values.Put(ExamsDataBaseHelper._TITULO, Titulo);
			values.Put(ExamsDataBaseHelper._NPREGUNTAS, NPreguntas);
			values.Put(ExamsDataBaseHelper._RESULTADO, Resultado);
			values.Put(ExamsDataBaseHelper._COMPLETADO, Completado);
			values.Put(ExamsDataBaseHelper._DESCRIPCION, "dafasfasd"); // TODO
			values.Put(ExamsDataBaseHelper._SINCRONIZADO, Sincronizado);
			return values;
		}

		public Intent ToIntent(Context context, Type type)
		{
			var intent = new Intent(context, type);
			intent.PutExtra(ExamsDataBaseHelper._ID, ID);
			intent.PutExtra(ExamsDataBaseHelper._TITULO, Titulo);
			intent.PutExtra(ExamsDataBaseHelper._NPREGUNTAS, NPreguntas);
			intent.PutExtra(ExamsDataBaseHelper._RESULTADO, Resultado);
			intent.PutExtra(ExamsDataBaseHelper._COMPLETADO, Completado);
			intent.PutExtra(ExamsDataBaseHelper._DESCRIPCION, Descripcion);
			intent.PutExtra(ExamsDataBaseHelper._SINCRONIZADO, Sincronizado);

			return intent;
		}
	}
}