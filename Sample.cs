/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 03/28/2017
 * Time: 18:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace LotteryElite
{
	/// <summary>
	/// Description of Sample.
	/// </summary>
	public class Sample
	{
		private String p1, p2, p3, p4, p5, p6, p7;
		public Sample()
		{
			p1 = "";
			p2 = "";
			p3 = "";
			p4 = "";
			p5 = "";
			p6 = "";
			p7 = "";
		}
		
		/// <summary>
		/// Параметр 1
		/// </summary>
		public String P1 { get{return p1;} set{p1 = value;}}
		
		/// <summary>
		/// Параметр 2
		/// </summary>
		public String P2 { get{return p2;} set{p2 = value;}}
		
		/// <summary>
		/// Параметр 3
		/// </summary>
		public String P3 { get{return p3;} set{p3 = value;}}
		
		/// <summary>
		/// Параметр 4
		/// </summary>
		public String P4 { get{return p4;} set{p4 = value;}}
		
		/// <summary>
		/// Параметр 5
		/// </summary>
		public String P5 { get{return p5;} set{p5 = value;}}
		
		/// <summary>
		/// Параметр 6
		/// </summary>
		public String P6 { get{return p6;} set{p6 = value;}}
		
		/// <summary>
		/// Параметр 7
		/// </summary>
		public String P7 { get{return p7;} set{p7 = value;}}
		
		/// <summary>
		/// Преобразует объект класса Sample в объект класса User
		/// </summary>
		/// <returns>Возвращает объект класса User</returns>
		public User ToUser()
		{
			User user = new User();
			user.Id = Convert.ToInt32(p1);
			user.Login = p2;
			user.Pass = p3;
			user.Type = GetUserTypeNumber(p4);
			user.Money = Convert.ToInt32(p5);
			return user;
		}
		
		/// <summary>
		/// Преобразует объект класса Sample в объект класса Ticket
		/// </summary>
		/// <returns>Возвращает объект класса Ticket</returns>
		public Ticket ToTicket()
		{
			Ticket ticket = new Ticket();
			ticket.TicketId = Convert.ToInt32(p1);
			ticket.GameId = Convert.ToInt32(p2);
			ticket.OwnerId = Convert.ToInt32(p3);
			ticket.Combination = GetCombinationFromString(p4);
			return ticket;
		}
		
		/// <summary>
		/// Преобразует объект класса Sample в объект класса Game
		/// </summary>
		/// <returns>Возвращает объект класса Game</returns>
		public Game ToGame()
		{
			Game game = new Game();
			game.GameId = Convert.ToInt32(p1);
			game.Type = GetGameTypeNumber(p2);
			game.Pool = Convert.ToInt32(p3);
			game.TicketCost = Convert.ToInt32(p4);
			game.WinCombination = GetCombinationFromString(p5);
			game.Status = p6;
			return game;
		}
		
		/// <summary>
		/// Преобразует название типа игры в номер типа игры
		/// </summary>
		/// <param name="type">Название типа игры</param>
		/// <returns>Номер типа игры или 0, если название типа некорректно</returns>
		public Int32 GetGameTypeNumber(String type)
		{
			if(type == "5x36") return 1;
			if(type == "6x42") return 2;
			if(type == "7x49") return 3;
			if(type == "1x2") return 4;
			return 0;
		}
		
		/// <summary>
		/// Преобразует название привилегии юзера в тип привилегии
		/// </summary>
		/// <param name="type">Название привилегии</param>
		/// <returns>Тип привилегии или 0, если название привилегии некорректно</returns>
		public Int32 GetUserTypeNumber(String type)
		{
			if(type == "User") return 1;
			if(type == "Moderator") return 2;
			if(type == "Admin") return 3;
			return 0;
		}
		
		/// <summary>
		/// Преобразует комбинацию из формата "num num" в формат "num_num"
		/// </summary>
		/// <param name="str">Комбинация в формате "num num"</param>
		/// <returns>Возвращает комбинацию в формате "num_num"</returns>
		public String GetCombinationFromString(String str)
		{
			char[] arr = str.ToCharArray();
			for(int i = 0; i<arr.Length; i++)
			{
				if(arr[i] == ' ') arr[i] = '_';
			}
			String s = new string(arr);
			return s;
		}
	}
}
