/*
 * Created by SharpDevelop.
 * User: Andr
 * Date: 15.01.2017
 * Time: 20:48
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

//CREATE TABLE IF NOT EXISTS `tickets` (`id` INTEGER PRIMARY KEY AUTO_INCREMENT, `ownerid` INTEGER DEFAULT '0', `gameid` INTEGER DEFAULT '0', `comb` TEXT);

namespace LotteryElite
{
	/// <summary>
	/// Description of Ticket.
	/// </summary>
	public class Ticket
	{
		private Int32 ticketId, ownerId, gameId;
		private String comb;
		public Ticket()
		{
			Clear();
		}
		
		/// <summary>
		/// Очищает данные объекта
		/// </summary>
		public void Clear()
		{
			comb = "";
			ticketId = 0;
			ownerId= 0;
			gameId = 0;
		}
		
		/// <summary>
		/// Переменная, хранящая комбинацию билета в формате "num_num"
		/// </summary>
		public String Combination { get{return comb;} set{comb = value;}}
		
		/// <summary>
		/// Переменная, хранящая айди билета в базе
		/// </summary>
		public Int32 TicketId {get{return ticketId;} set{ticketId = value;}}
		
		/// <summary>
		/// Переменная, хранящая айди хозяина билета в базе
		/// </summary>
		public Int32 OwnerId {get{return ownerId;} set{ownerId = value;}}
		
		/// <summary>
		/// Переменная, хранящая комбинацию в формате "num_num"
		/// </summary>
		public Int32 GameId {get{return gameId;} set{gameId = value;}}
		
		/// <summary>
		/// Метод, переводящий комбинацию из формата "num_num" в формат "num num"
		/// </summary>
		/// <returns>Возвращает комбинацию в формате "num num"</returns>
		public String GetStringFromCombination()
		{
			char[] arr = comb.ToCharArray();
			for(int i = 0; i<arr.Length; i++)
			{
				if(arr[i] == '_') arr[i] = ' ';
			}
			String s = new string(arr);
			return s;
		}
		
		/// <summary>
		/// Преобразует объект класса Ticket в объект шаблонного класса Sample
		/// </summary>
		/// <returns>Возвращает объект класса Sample, содержащий данные объекта класса Ticket</returns>
		public Sample ToSample()
		{
			Sample s = new Sample();
			s.P1 = ticketId.ToString();
			s.P2 = gameId.ToString();
			s.P3 = ownerId.ToString();
			s.P4 = GetStringFromCombination();
			return s;
		}
		
		/// <summary>
		/// Проверяет билет на валидность (id>0?)
		/// </summary>
		/// <returns>true если валидный, иначе false</returns>
		public bool IsTicketValid(){ return ticketId>0;}
	}
}
