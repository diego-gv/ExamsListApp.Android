using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Android.Content;
using ExamsListAndroid.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExamsListAndroid
{
	public class ConnectionService
	{
		public const string nameService = "http://10.5.1.57:9010/api/"; //post?token="TOKEN"
		private const string contentType = "application/json";
		private const string method_GET = "GET";
		private const string method_POST = "POST";

		public string GetToken(Context context)
		{
			using (ISharedPreferences sharedPref = context.GetSharedPreferences(LoginActivity.APP_PREFERENCES, FileCreationMode.Private))
			{
				return sharedPref.GetString(LoginActivity.LOGIN_TOKEN, null);
			}
		}

		public void SetToken(Context context, string token)
		{
			using (ISharedPreferences sharedPref = context.GetSharedPreferences(LoginActivity.APP_PREFERENCES, FileCreationMode.Private))
			{
				ISharedPreferencesEditor editor = sharedPref.Edit();
				editor.PutString(LoginActivity.LOGIN_TOKEN, token);
				editor.Commit();
			}
		}

		public bool LoginService(Context context, string user, string pass)
		{
			//string format = nameService + "login?user={0}&pass={1}";
			var request = WebRequest.Create(nameService + "login" /*+ string.Format(format, user, pass)*/);
			request.ContentType = contentType;
			request.Method = method_GET;

			using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
			{
				using (StreamReader reader = new StreamReader(response.GetResponseStream()))
				{
					var stringJSON = reader.ReadToEnd();
					if (!string.IsNullOrWhiteSpace(stringJSON))
					{
						JObject JSON = JObject.Parse(stringJSON);
						string statusCode = (string)JSON["StatusCode"];
						string message = (string)JSON["Message"];
						System.Diagnostics.Debug.Print(statusCode + ": " + message);
						if (statusCode == "200")
						{
							string token = (string)JSON["Token"];
							SetToken(context, token);
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool GetExams(Context context)
		{
			string format = nameService + "getexamenes?token={0}";
			string token = GetToken(context);

			var request = WebRequest.Create(string.Format(format, token));
			request.ContentType = contentType;
			request.Method = method_GET;

			using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
			{
				using (StreamReader reader = new StreamReader(response.GetResponseStream()))
				{
					var stringJSON = reader.ReadToEnd();
					if (!string.IsNullOrWhiteSpace(stringJSON))
					{
						JObject JSON = JObject.Parse(stringJSON);
						string statusCode = (string)JSON["StatusCode"];
						switch (statusCode)
						{
							case "200":
								List<Exam> exams = JsonConvert.DeserializeObject<List<Exam>>(JSON["Examenes"].ToString());
								ContentValues values = new ContentValues();
								foreach (var e in exams)
								{
									values = e.ToValues();
									DBHelper.InsertOrUpdateExam(context, values, ExamsDataBaseHelper._ID + "=?", new string[] { e.ID.ToString() });
								}
								values.Clear();
								List<Question> questions = JsonConvert.DeserializeObject<List<Question>>(JSON["Preguntas"].ToString());
								foreach (var q in questions)
								{
									values = q.ToValues();
									DBHelper.InsertOrUpdateQuestion(context, values, QuestionsDataBaseHelper._ID + "=?", new string[] { q.ID.ToString() });
								}
								return true;
							case "401":
								string message = (string)JSON["Message"];
								System.Diagnostics.Debug.Print(statusCode + ": " + message);
								SetToken(context, null);
								return false;
							default:
								throw new ArgumentOutOfRangeException();
						}
					}
				}
			}
			return false;
		}

		public bool SendResultExam(Context context, List<Exam> values)
		{
			string format = nameService + "post?token={0}";
			string token = GetToken(context);

			var request = WebRequest.Create(string.Format(format, token));
			request.ContentType = contentType;
			request.Method = method_POST;

			byte[] data = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(values));
			System.Diagnostics.Debug.Print(JsonConvert.SerializeObject(values));
			//Exam exam = new Exam(values);
			//byte[] data = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(exam));
			Stream content = request.GetRequestStream();
			content.Write(data, 0, data.Length);
			content.Close();

			// TODO
			return true;

			using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
			{
				using (StreamReader reader = new StreamReader(response.GetResponseStream()))
				{
					var stringJSON = reader.ReadToEnd();
					if (!string.IsNullOrWhiteSpace(stringJSON))
					{
						JObject JSON = JObject.Parse(stringJSON);
						string statusCode = (string)JSON["StatusCode"];
						switch (statusCode)
						{
							case "200":
								return true;
							case "401":
								return false;
							default:
								throw new ArgumentOutOfRangeException();
						}
					}
				}
			}
			return false;
		}
	}
}
