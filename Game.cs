/*
 * Created by SharpDevelop.
 * User: Andr
 * Date: 18.01.2017
 * Time: 20:06
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

//CREATE TABLE IF NOT EXISTS `games` (`id` INTEGER PRIMARY KEY AUTO_INCREMENT, `type` INTEGER DEFAULT '0', `pool` INTEGER DEFAULT '0', `ticketcost` INTEGER DEFAULT '0', `wincomb` TEXT);

namespace LotteryElite
{
	
	/// <summary>
	/// Description of Game.
	/// </summary>
	public class Game
	{
		private Int32 gameId, type, pool, ticketCost;
		private String winComb, status;
		
		public Game()
		{
			Clear();
		}
		
		/// <summary>
		/// Метод очистки объекта
		/// </summary>
		public void Clear()
		{
			status = "";
			winComb = "";
			gameId = 0;
			type= 0;	
			pool = 0;
			ticketCost = 0;
		}
		
		/// <summary>
		/// Преобразует номер типа игры в название типа
		/// </summary>
		/// <returns>Название типа или None в случае некорректности номера типа</returns>
		public String GetTypeName()
		{
			if(type == 1) return "5x36";
			if(type == 2) return "6x42";
			if(type == 3) return "7x49";
			if(type == 4) return "1x2";
			return "None";
		}
		
		/// <summary>
		/// Айди игры в БД
		/// </summary>
		public Int32 GameId {get{return gameId;} set{gameId = value;}}
		
		/// <summary>
		/// Тип игры
		/// </summary>
		public Int32 Type {get{return type;} set{type = value;}}
		
		/// <summary>
		/// Призовой фонд
		/// </summary>
		public Int32 Pool {get{return pool;} set{pool = value;}}
		
		/// <summary>
		/// Стоимость билета
		/// </summary>
		public Int32 TicketCost {get{return ticketCost;} set{ticketCost = value;}}
		
		/// <summary>
		/// Выигрышная комбинация (определяется после розыгрыша)
		/// </summary>
		public String WinCombination { get{return winComb;} set{winComb = value;}}
		
		/// <summary>
		/// Статус игры (Open или Close)
		/// </summary>
		public String Status { get{return status;} set{status = value;}}
		
		/// <summary>
		/// Преобразует объект класса Game в объект класса Sample
		/// </summary>
		/// <returns>Заполненный объект класса Sample</returns>
		public Sample ToSample()
		{
			Sample s = new Sample();
			s.P1 = gameId.ToString();
			s.P2 = GetTypeName();
			s.P3 = pool.ToString();
			s.P4 = ticketCost.ToString();
			s.P5 = GetStringFromCombination();
			s.P6 = status;
			return s;
		}
		
		/// <summary>
		/// Преобразует комбинацию и формата "num_num" в формат "num num"
		/// </summary>
		/// <returns>Комбинация в формате "num num"</returns>
		public String GetStringFromCombination()
		{
			char[] arr = winComb.ToCharArray();
			for(int i = 0; i<arr.Length; i++)
			{
				if(arr[i] == '_') arr[i] = ' ';
			}
			String s = new string(arr);
			return s;
		}
		
		/// <summary>
		/// Проверяет, валидная ли игра (айди>0)
		/// </summary>
		/// <returns>True, если валидная, иначе false</returns>
		public bool IsGameValid(){ return gameId>0; }
		
		/// <summary>
		/// Проверяет, активная ли игра
		/// </summary>
		/// <returns>True, если активная, иначе false</returns>
		public bool IsGameActive(){ return (winComb == "");}
		
		/// <summary>
		/// Определяет максимальное число в билете в зависимости от типа игры
		/// </summary>
		/// <returns>Максимальное число в билете или 0, если тип некорректен</returns>
		public Int32 GetMaxNum()
		{
			if(type == 1) return 36;
			if(type == 2) return 42;
			if(type == 3) return 49;
			if(type == 4) return 2;
			return 0;
		}
		
		/// <summary>
		/// Определяет количество чисел, которые нужно выбрать в билете
		/// </summary>
		/// <returns>Количество чисел для выбора или 0, если тип некорректен</returns>
		public Int32 GetNumCount()
		{
			if(type == 1) return 5;
			if(type == 2) return 6;
			if(type == 3) return 7;
			if(type == 4) return 1;
			return 0;
		}
	}
}
