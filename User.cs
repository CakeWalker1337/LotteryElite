/*
 * Created by SharpDevelop.
 * User: Andr
 * Date: 15.01.2017
 * Time: 19:13
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

//CREATE TABLE IF NOT EXISTS `users`(`id` INTEGER PRIMARY KEY AUTO_INCREMENT, `login` TEXT, `pass` TEXT , `type` INTEGER DEFAULT '0', `money` INTEGER DEFAULT '0');

namespace LotteryElite
{
	
	enum UserType
	{
		Client = 1,
		Moder = 2,
		Admin = 3
	};
	
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class User
	{
		private Int32 userId, type, money;
		private List<Ticket> tickets = new List<Ticket>();
		private List<Game> games = new List<Game>();
		private String login, pass;
		public User()
		{
			Clear();
		}
		
		/// <summary>
		///	Метод для получения списка билетов для данного юзера
		/// </summary>
		/// <returns>Возвращает список билетов</returns>
		public List<Ticket> GetTickets() {return tickets;}
		
		/// <summary>
		///	Метод для замещения текущего списка билетов новым для данного юзера
		/// </summary>
		/// <param name="newTickets">Новый лист билетов</param>
		public void SetTickets(List<Ticket> newTickets){tickets = newTickets;}
		
		/// <summary>
		/// Добавляет билет в лист билетов
		/// </summary>
		/// <param name="ticketId">айди нового билета в базе</param>
		public void AddTicket(Ticket ticketId){ tickets.Add(ticketId); }
		
		/// <summary>
		/// Удаляет билет из листа билетов
		/// </summary>
		/// <param name="ticketId">айди билета, который нужно удалить</param>
		/// <returns>Возвращает true, если удаление успешно, иначе false</returns>
		public bool RemoveTicket(Ticket ticketId){ return tickets.Remove(ticketId); }
	
		/// <summary>
		///	Метод для получения списка игр для данного юзера
		/// </summary>
		/// <returns>Возвращает список игр</returns>
		public List<Game> GetGames() {return games;}
		
		/// <summary>
		///	Метод для замещения текущего списка игр новым для данного юзера
		/// </summary>
		/// <param name="game">Новый лист игр</param>
		public void SetGames(List<Game> game){games = game;}
		
		/// <summary>
		/// Добавляет игру в лист игр
		/// </summary>
		/// <param name="game">айди новой игры в базе</param>
		public void AddGame(Game game){ games.Add(game); }
		
		/// <summary>
		/// Удаляет игру из листа игр
		/// </summary>
		/// <param name="game">айди игры, которую нужно удалить</param>
		/// <returns>Возвращает true, если удаление успешно, иначе false</returns>
		public bool RemoveGame(Game game){ return games.Remove(game); }
		
		/// <summary>
		/// Метод, преобразующий тип привилегий юзера в название привилегии
		/// </summary>
		/// <returns>Название привилегии или ErrorType, если тип некорректен</returns>
		public String GetTypeName()
		{
			if(type == 1) return "User";
			if(type == 2) return "Moderator";
			if(type == 3) return "Admin";
			return "ErrorType";
		}
		
		/// <summary>
		/// Логин пользователя
		/// </summary>
		public String Login { get{return login;} set{login = value;}}
		
		/// <summary>
		/// Пароль пользователя
		/// </summary>
		public String Pass {get{return pass;} set{pass = value;}}
		
		/// <summary>
		/// Тип привилегий пользователя
		/// </summary>
		public Int32 Type {get{return type;} set{type = value;}}
		
		/// <summary>
		/// Айди пользователя в БД
		/// </summary>
		public Int32 Id {get{return userId;} set{userId = value;}}
		
		/// <summary>
		/// Количество денег пользователя
		/// </summary>
		public Int32 Money {get{return money;} set{money = value;}}
		
		/// <summary>
		/// Метод, очищающий данные объекта
		/// </summary>
		public void Clear()
		{
			userId = 0;
			type = 1;
			login = "";
			pass = "";
			money = 0;
			tickets.Clear();
		}
		
		/// <summary>
		/// Проверяет, валидный ли юзер
		/// </summary>
		/// <returns>true, если валидный, иначе false</returns>
		public bool IsUserValid(){ return userId>0; }
		
		/// <summary>
		/// Проверяет, есть ли уже даненая игра в списке игр пользователя
		/// </summary>
		/// <param name="game">Игра для проверки</param>
		/// <returns>true, если уже есть, иначе false</returns>
		public bool IsGameAlreadyExists(Game game)
		{
			for(int i = 0; i<games.Count; i++)
			{
				if(game.GameId == games[i].GameId) return true;
			}
			return false;
		}
		
		/// <summary>
		/// Получение билета пользователя по айди игры, в которой он участвовал
		/// </summary>
		/// <param name="gameId">Айди игры</param>
		/// <returns>Возвращает билет для игры gameId, если он есть, иначе пустой билет</returns>
		public Ticket GetTicketByGameId(Int32 gameId)
		{
			for(int i = 0; i<tickets.Count; i++)
			{
				if(tickets[i].GameId == gameId) return tickets[i];
			}
			Ticket tick = new Ticket();
			return tick;
		}
		
		/// <summary>
		/// Проверяет, есть ли игра в списке игр пользователя
		/// </summary>
		/// <param name="game">Игра для проверки</param>
		/// <returns>True, если игра найдена, иначе false</returns>
		public bool IsMyGame(Game game)
		{
			for(int i = 0; i<games.Count; i++)
			{
				if(games[i].GameId == game.GameId) return true;
			}
			return false;
		}
		
		/// <summary>
		/// Преобразует объект класса User в объект класса Sample
		/// </summary>
		/// <returns>Объект класса Sample с заполненными данными</returns>
		public Sample ToSample()
		{
			Sample s = new Sample();
			s.P1 = Id.ToString();
			s.P2 = login;
			s.P3 = pass;
			s.P4 = GetTypeName();
			s.P5 = money.ToString();
			return s;
		}
	}
}
