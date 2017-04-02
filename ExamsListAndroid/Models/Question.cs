using Android.Content;
using Newtonsoft.Json;
using SQLite;

namespace ExamsListAndroid
{
	public class Question
	{
		[PrimaryKey, AutoIncrement, JsonProperty(PropertyName = "ID")]
		public long ID { get; set; }

		[JsonProperty(PropertyName = "id_Examen")]
		public long id_examen { get; set; }

		[JsonProperty(PropertyName = "Enunciado")]
		public string Enunciado { get; set; }

		[JsonProperty(PropertyName = "A")]
		public string A { get; set; }

		[JsonProperty(PropertyName = "B")]
		public string B { get; set; }

		[JsonProperty(PropertyName = "ValorA")]
		public int ValorA { get; set; }

		[JsonProperty(PropertyName = "ValorB")]
		public int ValorB { get; set; }

		public override string ToString()
		{
			return string.Format("[Question: ID={0}, idExamen={1}, Enunciado={2}, A={3}, B={4}, ValorA={5}, ValorB={6}]", ID, id_examen, Enunciado, A, B, ValorA, ValorB);
		}

		public ContentValues ToValues()
		{
			ContentValues values = new ContentValues();
			values.Put(QuestionsDataBaseHelper._ID, ID);
			values.Put(QuestionsDataBaseHelper._IDEXAMEN, id_examen);
			values.Put(QuestionsDataBaseHelper._ENUNCIADO, Enunciado);
			// TODO
			//values.Put(QuestionsDataBaseHelper._A, question.A);
			//values.Put(QuestionsDataBaseHelper._B, question.B);
			values.Put(QuestionsDataBaseHelper._A, "A");
			values.Put(QuestionsDataBaseHelper._B, "B");
			values.Put(QuestionsDataBaseHelper._VALOR_A, ValorA);
			values.Put(QuestionsDataBaseHelper._VALOR_B, ValorB);
			return values;
		}
	}
}
