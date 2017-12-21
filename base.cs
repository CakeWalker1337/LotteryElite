/*
 * Created by SharpDevelop.
 * User: Andr
 * Date: 15.01.2017
 * Time: 20:47
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace LotteryElite
{
	/// <summary>
	/// Description of base.
	/// </summary>
	/// 
	public class Base
	{
		private Mysql mysql = new Mysql();
		private Authorise auth = new Authorise();
		private bool isLogged = false;
		private User client = new User();
		
		// Методы-обертки для методов из других классов
		public String Login { get{return client.Login;} set{client.Login = value;}}
		public String Pass {get{return client.Pass;} set{client.Pass = value;}}
		public Int32 Type {get{return client.Type;} set{client.Type = value;}}
		public Int32 Id {get{return client.Id;} set{client.Id = value;}}
		public Int32 Money {get{return client.Money;} set{client.Money = value;}}
		public String GetTypeName(){return client.GetTypeName();}
		
		public List<Ticket> GetTickets() {return client.GetTickets();}
		public void SetTickets(List<Ticket> newTickets){client.SetTickets(newTickets);}
		public Ticket GetMyTicket(Int32 gameId){return client.GetTicketByGameId(gameId);}
		
		public void AddTicket(Ticket ticketId){ client.AddTicket(ticketId); }
		public bool RemoveTicket(Ticket ticketId){ return client.RemoveTicket(ticketId); }
		
		
		public List<Game> GetGames() {return client.GetGames();}
		public void SetGames(List<Game> game){client.SetGames(game);}
		public bool IsMyGame(Game game){return client.IsMyGame(game);}
		public void AddGame(Game game){ client.AddGame(game); }
		public bool RemoveGame(Game game){ return client.RemoveGame(game); }
		
		public bool DeleteUser(Int32 id){return mysql.DeleteUser(id);}
		public bool SaveUser(User user){return mysql.SaveUser(user);}
		public bool CheckLogin(String login){return mysql.CheckLogin(login);}
		public User LoadClient(Int32 id){return mysql.LoadClient(id);}
		public User LoadClient(String id){return mysql.LoadClient(id);}
		public bool CreateUser(User user){return mysql.CreateUser(user);}
		public void SetPrize(Int32 Id, Int32 prize){mysql.SetWinnersCash(Id, prize);}
		
		/// <summary>
		/// Метод загрузки данных клиента из БД
		/// </summary>
		public void LoadMe()
		{
			int id = client.Id;
			client.Clear();
			client = mysql.LoadClient(id);
		}
		
		public bool DeleteGame(Int32 id){return mysql.DeleteGame(id);}
		public bool SaveGame(Game game){return mysql.SaveGame(game);}
		public Game GetGame(Int32 id){return mysql.GetGame(id);}
		public bool CheckGameId(Int32 id){return mysql.CheckGameId(id);}
		public bool CreateGame(Game game){return mysql.CreateGame(game);}
		
		public Ticket GetTicket(Int32 id){return mysql.GetTicket(id);}
		public bool CreateTicket(Ticket ticket){return mysql.CreateTicket(ticket);}
		public bool DeleteTicket(Int32 id){return mysql.DeleteTicket(id);}
		public bool SaveTicket(Ticket ticket){return mysql.SaveTicket(ticket);}
		public List<Ticket> GetTicketsByGameId(Int32 id){return mysql.GetTicketsByGameId(id);}
		public Int32 GetMyLastInsertTicketId(){ return mysql.GetLastInsertTicketId(client);}
		
		/// <summary>
		/// Проверяет, подходит ли логин под формат (A-Z, a-z)
		/// </summary>
		/// <param name="login">Логин для проверки</param>
		/// <returns>True, если корректен, иначе false</returns>
		public bool IsLoginCorrect(String login)
		{
			if(login.Length == 0) return false;
			for(int i = 0; i<login.Length; i++)
			{
				if((login[i]>='A' && login[i]<='Z') || (login[i]>='a' && login[i]<='z')) return true;			
			}
			return false;
		}
		
		/// <summary>
		/// Метод, получающий все игры из БД
		/// </summary>
		/// <returns>Список всех игр</returns>
		public List<Game> ShowAllGames()
		{
			return mysql.GetAllGames();	
		}
		
		/// <summary>
		/// Метод регистрации в системе
		/// </summary>
		/// <param name="login">Логин пользователя</param>
		/// <param name="pass">Пароль пользователя</param>
		/// <returns>Результат проверок на корректность и на валидность</returns>
		public int SignUp(String login, String pass)
		{
			int res = auth.SignUp(mysql, login, pass);
			if(res == 2)
			{
				if(mysql.CreateUser(auth.Client))
				{
					client = mysql.LoadClient(auth.Client.Login);
				}
				else res = 3;
			}
			return res;
		}
		
		/// <summary>
		/// Метод авторизации в системе
		/// </summary>
		/// <param name="login">Логин пользователя</param>
		/// <param name="pass">Пароль пользователя</param>
		/// <returns>Результат проверок на корректность и на валидность</returns>
		public int LogIn(String login, String pass)
		{
			int res = auth.LogIn(mysql, login, pass);
			if(res == 2) client = mysql.LoadClient(auth.Client.Login);
			
			return res;	
		}
		
		/// <summary>
		/// Метод выхода из системы
		/// </summary>
		public void LogOut()
		{
			auth.LogOut();
			mysql.SaveUser(client);
			client.Clear();
		}
		
		/// <summary>
		/// Метод обновления данных пользователя
		/// </summary>
		public void Refresh()
		{
			if(isLogged)
			{
				client = mysql.LoadClient(client.Login);	
			}
		}
		
		//
		//AdminPanel
		//
		
		/// <summary>
		/// Метод сохранения данных пользователя в БД
		/// </summary>
		public void SaveMe()
		{
			mysql.SaveUser(client);
		}
		
		/// <summary>
		/// Метод для разбиения комбинации на числа для обработки
		/// </summary>
		/// <param name="comb">Строка с комбинацией</param>
		/// <param name="count">Количество чисел</param>
		/// <returns>Массив с числами из комбинации</returns>
		public int[] SplitCombination(String comb, Int32 count)
		{
			var res = new int[count+1];
			int l = 0;
			char[] arr = comb.ToCharArray();
			for(int i = 0; i<comb.Length; i++)
			{
				if(arr[i] == '_')	l++;
				else res[l] = res[l]*10 + (((int)comb[i])-48);
			}
			return res;
		}
		
		/// <summary>
		/// Метод, реализующий алгоритм выдачи приза по количеству совпадений в билете
		/// </summary>
		/// <param name="pool">Призовой фонд</param>
		/// <param name="count">Количесвто совпадений</param>
		/// <param name="maxnumcount">Максимальное количество совпадений</param>
		/// <returns></returns>
		public int GetPrizeByNumberOfMatches(int pool, int count, int maxnumcount)
		{
			if(count == maxnumcount) return pool;
			double res = pool/10000.0;
			for(int i = 1; i<count; i++)
			{
				if(i%2==0) res *= 5;
				else res *= 2;
			}
			return (int)res;
		}
	}
}
 