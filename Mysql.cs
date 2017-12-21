/*
 * Created by SharpDevelop.
 * User: Andr
 * Date: 23.01.2017
 * Time: 18:52
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.IO;


namespace LotteryElite
{
	/// <summary>
	/// Description of Mysql.
	/// </summary>
	public class Mysql
	{		
		private string Connect;
		private MySqlConnection myConnection;
		private MySqlCommand myCommand;
		
		public Mysql()
		{
			Write();
			GetConnectionPaths();
			myConnection = new MySqlConnection(Connect);
			myConnection.Open();
		}
		
		/// <summary>
		/// Метод считывания пути из бинарного файла
		/// </summary>
		void GetConnectionPaths()
		{
			BinaryReader read = new BinaryReader(File.Open("paths.dat", FileMode.Open));
			Connect = read.ReadString();
			read.Close();
		}
		
		/// <summary>
		/// Метод записи пути в бинарный файл
		/// </summary>
		void Write()
		{
			BinaryWriter bw = new BinaryWriter(File.Open("paths.dat", FileMode.Open));
			bw.Write("Database=testbase;Data Source=localhost;User Id=root;Password=1111");
			bw.Close();
		}
		
		/*///////////////////////////////////////////////////////////////////////////////USERS/////////////////////////////////////////////////////////////////////////////////////////////*/
		
		/// <summary>
		/// Метод загрузки пользователя из базы по логину
		/// </summary>
		/// <param name="login">Логин пользователя</param>
		/// <returns>Заполненный объект класса User, если юзер найден, иначе пустой объект класса User</returns>
		public User LoadClient(String login)
		{
			User user = new User();
			StringBuilder str = new StringBuilder();
			str.AppendFormat("SELECT `id`, `pass`, `type`, `money` from `users` where `login` = '{0}';", login);
			
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				MySqlDataReader datareader = myCommand.ExecuteReader();
				if(datareader.Read())
				{
					user.Id = datareader.GetInt32(0);
					user.Pass = datareader.GetString(1);
					user.Type = datareader.GetInt32(2);
					user.Money = datareader.GetInt32(3);
					user.Login = login;
				}		
								
				datareader.Close();
			}
			catch(Exception ex)
			{
				Console.Write("\nMysql Error: {0}!\n", ex);
				return user;
			}			
			
			return GetUserData(user);
		}
		
		/// <summary>
		/// Метод загрузки пользователя из базы по айди
		/// </summary>
		/// <param name="id">ID пользователя в БД</param>
		/// <returns>Заполненный объект класса User, если юзер найден, иначе пустой объект класса User</returns>
		public User LoadClient(Int32 id)
		{
			User user = new User();
			StringBuilder str = new StringBuilder();
			str.AppendFormat("SELECT `login`, `pass`, `type`, `money` from `users` where `id` = '{0}';", id);
			
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				MySqlDataReader datareader = myCommand.ExecuteReader();
				if(datareader.Read())
				{
					user.Login = datareader.GetString(0);
					user.Pass = datareader.GetString(1);
					user.Type = datareader.GetInt32(2);
					user.Money = datareader.GetInt32(3);
					user.Id = id;
				}		
				datareader.Close();				
			}
			catch(Exception ex)
			{
				Console.Write("\nMysql Error: {0}!\n", ex);
				return user;
			}			
			return GetUserData(user);
		}
		
		/// <summary>
		/// Получение из базы данных билеты и игры пользователя
		/// </summary>
		/// <param name="user">Объект класса User, в который заносятся данные</param>
		/// <returns>Объект класса User с заполненными данными</returns>
		public User GetUserData(User user)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("SELECT `id`, `gameid`, `comb` from `tickets` where `ownerid` = '{0}';", user.Id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				MySqlDataReader datareader = myCommand.ExecuteReader();
				while(datareader.Read())
				{
					Ticket ticket = new Ticket();
					ticket.TicketId = datareader.GetInt32(0);
					ticket.GameId = datareader.GetInt32(1);
					ticket.Combination = datareader.GetString(2);
					ticket.OwnerId = user.Id;
					user.AddTicket(ticket);
				}		
				datareader.Close();				
			}
			catch(Exception ex)
			{
				Console.Write("\nMysql Error: {0}!\n", ex);
				return user;
			}	
			
			Game game = new Game();
			for(int i = 0; i<user.GetTickets().Count; i++)
			{
				user.AddGame(GetGame(user.GetTickets()[i].GameId));
			}
			return user;
		}
		
		/// <summary>
		/// Метод создания пользователя в БД
		/// </summary>
		/// <param name="user">Объект с данными для занесения в БД</param>
		/// <returns>True, если создание прошло успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool CreateUser(User user)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("INSERT INTO `users` (`login`, `pass`, `type`, `money`) VALUES ('{0}', '{1}', '{2}', '{3}');", user.Login, user.Pass, user.Type, user.Money);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				Console.Write("\nMysql Error: {0}!\n", ex);
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Метод сохранения пользователя в БД
		/// </summary>
		/// <param name="user">Объект с данными для сохранения в БД</param>
		/// <returns>True, если сохранение прошло успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool SaveUser(User user)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("UPDATE `users` SET `login` = '{0}', `pass` = '{1}', `type` = '{2}', `money` = '{3}' where `id` = '{4}';", user.Login, user.Pass, user.Type, user.Money, user.Id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				Console.Write("\nMysql Error: {0}!\n", ex);
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Проверяет, есть ли логин в БД
		/// </summary>
		/// <param name="sLogin">Логин для проверки</param>
		/// <returns>True, если есть, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool CheckLogin(String sLogin)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("SELECT `id` from `users` where `login` = '{0}';", sLogin);
			
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				MySqlDataReader datareader = myCommand.ExecuteReader();
				bool res = datareader.Read();
				datareader.Close();
				return res;
			}
			catch(Exception ex)
			{
				Console.Write("\nMysql Error: {0}!\n", ex);
				return false;
			}			
		}
		
		/// <summary>
		/// Проверяет, есть ли пароль в БД
		/// </summary>
		/// <param name="sLogin">Логин для проверки</param>
		/// <param name="sPass">Пароль для проверки в связке с логином</param>
		/// <returns>True, если есть, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool CheckPass(String sLogin, String sPass)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("SELECT `pass` from `users` where `login` = '{0}';", sLogin);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				MySqlDataReader datareader = myCommand.ExecuteReader();
				if(datareader.Read() && sPass == datareader.GetString(0))
				{
					datareader.Close();
					return true;
				}
				datareader.Close();
				
			}
			catch(Exception ex)
			{
				Console.Write("\nMysql Error: {0}!\n", ex);
			}		
			return false;			
		}
		
		/// <summary>
		/// Удаляет пользователя из БД по айди
		/// </summary>
		/// <param name="id">Айди для удаления</param>
		/// <returns>True, если успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool DeleteUser(Int32 id)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("DELETE from `users` where `id` = '{0}';", id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				Console.Write("\nMysql Error: {0}!\n", ex);
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Удаляет пользователя из БД по логину
		/// </summary>
		/// <param name="login">Логин для удаления</param>
		/// <returns>True, если успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool DeleteUser(String login)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("DELETE from `users` where `login` = '{0}';", login);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				Console.Write("\nMysql Error: {0}!\n", ex);
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Выдает выигрыш пользователю
		/// </summary>
		/// <param name="id">Айди пользователя</param>
		/// <param name="prize">Сумма выигрыша</param>
		public void SetWinnersCash(Int32 id, Int32 prize)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("UPDATE `users` SET `money` = `money`+'{0}' where `id` = '{1}';", prize, id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				Console.Write("\nMysql Error: {0}!\n", ex);
				return;
			}
		}
		
		/*///////////////////////////////////////////////////////////////////////////////GAMES/////////////////////////////////////////////////////////////////////////////////////////////*/
		
		/// <summary>
		/// Получение данных об игре из БД
		/// </summary>
		/// <param name="id">Айди игры в БД</param>
		/// <returns>Заполненный объект класса Game или пустой объект Game в случае неудачи</returns>
		public Game GetGame(Int32 id)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("SELECT `type`, `pool`, `ticketcost`, `wincomb` from `games` where `id` = '{0}';", id);
			Game game = new Game();
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				MySqlDataReader datareader = myCommand.ExecuteReader();
				
				if(datareader.Read())
				{
					
					game.Type = datareader.GetInt32(0);
					game.Pool = datareader.GetInt32(1);
					game.TicketCost = datareader.GetInt32(2);
					game.WinCombination = datareader.GetString(3);
					if(game.WinCombination != "") game.Status = "Closed";
					else game.Status = "Opened";
					game.GameId = id;
				}		
				datareader.Close();				
			}
			catch(Exception ex)
			{
				Console.Write("\nMysql Error: {0}!\n", ex);
			}	
			return game;
		}
		
		/// <summary>
		/// Проверяет, есть ли игра в базе
		/// </summary>
		/// <param name="id">Айди игры для проверки</param>
		/// <returns>True, если есть, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool CheckGameId(Int32 id)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("SELECT `ticketcost` from `games` where `id` = '{0}';", id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				MySqlDataReader datareader = myCommand.ExecuteReader();
				if(datareader.Read())
				{
					datareader.Close();
					return true;
				}
				datareader.Close();
				
			}
			catch(Exception ex)
			{
				Console.Write("\nMysql Error: {0}!\n", ex);
			}	
			return false;
		}
		
		/// <summary>
		/// Метод сохранения игры в БД
		/// </summary>
		/// <param name="game">Объект с данными для сохранения в БД</param>
		/// <returns>True, если сохранение прошло успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool SaveGame(Game game)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("UPDATE `games` SET `type` = '{0}', `pool` = '{1}', `ticketcost` = '{2}', `wincomb` = '{3}' where `id` = '{4}';", game.Type, game.Pool, game.TicketCost, game.WinCombination, game.GameId);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				Console.Write("\nMysql Error: {0}!\n", ex);
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Метод создания игры в БД
		/// </summary>
		/// <param name="game">Объект с данными для создания в БД</param>
		/// <returns>True, если создание прошло успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool CreateGame(Game game)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("INSERT INTO `games` (`type`, `pool`, `ticketcost`, `wincomb`) VALUES ('{0}', '{1}', '{2}', '{3}');", game.Type, game.Pool, game.TicketCost, game.WinCombination);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				Console.Write("\nMysql Error: {0}!\n", ex);
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Метод удаления игры из БД
		/// </summary>
		/// <param name="id">Айди для удаления из БД</param>
		/// <returns>True, если удаление прошло успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool DeleteGame(Int32 id)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("DELETE from `games` where `id` = '{0}';", id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				Console.Write("\nMysql Error: {0}!\n", ex);
				return false;
			}
			
			
			str.AppendFormat("DELETE from `tickets` where `gameid` = '{0}';", id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				Console.Write("\nMysql Error: {0}!\n", ex);
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Получает список всех игр в базе
		/// </summary>
		/// <returns>Список всех игр или пустой список, если игр нет или ошибка MySQL</returns>
		public List<Game> GetAllGames()
		{
			List<Game> games = new List<Game>();
			
			StringBuilder str = new StringBuilder();
			str.AppendFormat("SELECT * from `games` ORDER BY `id` DESC;");
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				MySqlDataReader datareader = myCommand.ExecuteReader();
				
				while(datareader.Read())
				{
					Game game = new Game();
					game.GameId = datareader.GetInt32(0);
					game.Type = datareader.GetInt32(1);
					game.Pool = datareader.GetInt32(2);
					game.TicketCost = datareader.GetInt32(3);
					game.WinCombination = datareader.GetString(4);
					if(game.WinCombination == "") game.Status = "Opened";
					else game.Status = "Closed";
					games.Add(game);					
				}		
				datareader.Close();				
			}
			catch(Exception ex)
			{
				Console.Write("\nMysql Error: {0}!\n", ex);
			}	
			return games;
		}
		
/*///////////////////////////////////////////////////////////////////////////////TICKETS/////////////////////////////////////////////////////////////////////////////////////////////*/
		
		/// <summary>
		/// Метод создания билета в БД
		/// </summary>
		/// <param name="ticket">Объект с данными для создания в БД</param>
		/// <returns>True, если создание прошло успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool CreateTicket(Ticket ticket)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("INSERT INTO `tickets` (`gameid`, `ownerid`, `comb`) VALUES ('{0}', '{1}', '{2}');", ticket.GameId, ticket.OwnerId, ticket.Combination);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				Console.Write("\nMysql Error: {0}!\n", ex);
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Метод сохранения билета в БД
		/// </summary>
		/// <param name="ticket">Объект с данными для сохранения в БД</param>
		/// <returns>True, если сохранение прошло успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool SaveTicket(Ticket ticket)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("UPDATE `tickets` SET `gameid` = '{0}', `ownerid` = '{1}', `comb` = '{2}' where `id` = '{3}';", ticket.GameId, ticket.OwnerId, ticket.Combination, ticket.TicketId);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				Console.Write("\nMysql Error: {0}!\n", ex);
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Проверяет, есть ли билет в БД
		/// </summary>
		/// <param name="id">Айди билета для проверки</param>
		/// <returns>True, если есть, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool CheckTicket(Int32 id)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("SELECT `gameid` from `tickets` where `id` = '{0}';", id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				MySqlDataReader datareader = myCommand.ExecuteReader();
				if(datareader.Read())
				{
					datareader.Close();
					return true;
				}
				datareader.Close();
			}
			catch(Exception ex)
			{
				Console.Write("\nMysql Error: {0}!\n", ex);
			}	
			return false;
		}
		
		/// <summary>
		/// Получает билет из БД по айди
		/// </summary>
		/// <param name="id">Айди билета</param>
		/// <returns>Заполненный объект класса Ticket или пустой
		/// объект, если билет не найден или ошибка в MySQL</returns>
		public Ticket GetTicket(Int32 id)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("SELECT `gameid`, `ownerid`, `comb` from `tickets` where `id` = '{0}';", id);
			Ticket ticket = new Ticket();
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				MySqlDataReader datareader = myCommand.ExecuteReader();
				if(datareader.Read())
				{
					ticket.TicketId = id;
					ticket.GameId = datareader.GetInt32(0);
					ticket.OwnerId = datareader.GetInt32(1);
					ticket.Combination = datareader.GetString(2);
				}	
				datareader.Close();				
			}
			catch(Exception ex)
			{
				Console.Write("\nMysql Error: {0}!\n", ex);
			}	
			return ticket;
		}
		
		/// <summary>
		/// Удаляет билет из БД
		/// </summary>
		/// <param name="id">Айди билета для удаления</param>
		/// <returns>True, если сохранение прошло успешно, иначе false (также false может означать ошибку в MySQL)</returns>
		public bool DeleteTicket(Int32 id)
		{
			StringBuilder str = new StringBuilder();	
			str.AppendFormat("DELETE from `tickets` where `id` = '{0}';", id);
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				myCommand.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				Console.Write("\nMysql Error: {0}!\n", ex);
				return false;
			}
			return true;
		
		}		
		
		/// <summary>
		/// Получение последнего айди билета у данного юзера
		/// </summary>
		/// <param name="user">Объект классa User</param>
		/// <returns>Последний айди билета юзера</returns>
		public Int32 GetLastInsertTicketId(User user)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("SELECT `id` from `tickets` where `ownerid` = '{0}' order by `id` DESC LIMIT 1;", user.Id);
			Ticket ticket = new Ticket();
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				MySqlDataReader datareader = myCommand.ExecuteReader();
				if(datareader.Read())
				{
					Int32 res = datareader.GetInt32(0);
					datareader.Close();
					return res;
				}
				datareader.Close();
			}
			catch(Exception ex)
			{
				Console.Write("\nMysql Error: {0}!\n", ex);
			}	
			return 0;
		}
		
		/// <summary>
		/// Получает список билетов по айди игры в БД
		/// </summary>
		/// <param name="id">Айди игры в БД</param>
		/// <returns>Список билетов игры</returns>
		public List<Ticket> GetTicketsByGameId(Int32 id)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("SELECT `id`, `ownerid`, `comb` from `tickets` where `gameid` = '{0}';", id);
			List<Ticket> winners = new List<Ticket>();
			try
			{
				myCommand = new MySqlCommand(str.ToString(), myConnection);
				MySqlDataReader datareader = myCommand.ExecuteReader();
				while(datareader.Read())
				{
					Ticket ticket = new Ticket();
					ticket.TicketId = datareader.GetInt32(0);
					ticket.OwnerId = datareader.GetInt32(1);
					ticket.GameId = id;
					ticket.Combination = datareader.GetString(2);
					
					winners.Add(ticket);
				}
				datareader.Close();
			}
			catch(Exception ex)
			{
				Console.Write("\nMysql Error: {0}!\n", ex);
			}	
			return winners;
		}
		
		
/*////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

		~Mysql()
		{
			myConnection.Close();
		}
	}
}
