using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Diagnostics;
using WordJsonToSQL;

class Program
{
	static void Main()
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		string JsonData = File.ReadAllText("D:\\dev\\C#\\DeJson\\WordJsonToSQL\\word.json");

		dynamic[] JsonObjectArray = JsonConvert.DeserializeObject<dynamic[]>(JsonData);
		List<zd> zds = new();
		foreach (dynamic HzAll in JsonObjectArray)
		{
			zds.Add(new zd
			{
				bsm = HzAll.bsm,
				hz = HzAll.hz,
				pyJs = HzAll.pyJs.ToObject<List<string>>(),
			});
		}
		int Index = zds.Count-1;
		Console.WriteLine($"Size = {Index} bsm: {zds[Index].bsm} hz:{zds[Index].hz} pyJs:{zds[Index].pyJs[0]}");

		//SQL部分

		Console.WriteLine("操作数据库中。。。");
		int id = 0;
		SqlConnection connection = new SqlConnection
		{
			ConnectionString = "server=127.0.0.1;uid=TestUser;pwd=Zengtudor2008;database=TestBase;TrustServerCertificate=true"
		};
		connection.Open();
		foreach (zd Azd in zds)
		{
			SqlCommand sqlCommand = new($$$"""
			INSERT INTO QQAZK (Id,Hz,PyJs,Bsm) VALUES (@SqlId,@SqlHz,@SqlPyJs,@SqlBsm);
			""", connection);
			string AllPyJs = "";
			for(var i=0;i<Azd.pyJs.Count-1;i++)
			{
				AllPyJs += Azd.pyJs[i] + "@@#";
			}
			AllPyJs += Azd.pyJs[Azd.pyJs.Count - 1];
			sqlCommand.Parameters.AddWithValue("@SqlId", id);
			sqlCommand.Parameters.AddWithValue("@SqlHz", Azd.hz);
			sqlCommand.Parameters.AddWithValue("@SqlPyJs", AllPyJs);
			sqlCommand.Parameters.AddWithValue("@SqlBsm", Azd.bsm);
			sqlCommand.ExecuteNonQuery();
			id++;
		}
		Console.WriteLine("SQL执行完成");
		stopwatch.Stop();
		TimeSpan timeSpan = stopwatch.Elapsed;
		Console.WriteLine($"本次运行用了{timeSpan}");
		//
		Console.ReadKey();
	}
}