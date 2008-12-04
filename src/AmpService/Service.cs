using MySql.Data.MySqlClient;

namespace AmpService
{
	public class Service
	{
		public static void UpdateLastAccessTime(string username)
		{
			using (var connection = new MySqlConnection(Literals.ConnectionString))
			{
				connection.Open();
				var command = new MySqlCommand(@"
update `logs`.AuthorizationDates ad
	join usersettings.osuseraccessright ouar on ad.ClientCode = ouar.ClientCode 
set ad.IOLTime = now()
where ouar.OsUsername = ?UserName", connection);
				command.Parameters.AddWithValue("UserName", username);
				command.ExecuteNonQuery();
			}
		}
	}
}
