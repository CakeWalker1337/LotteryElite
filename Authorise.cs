/*
 * Created by SharpDevelop.
 * User: Andr
 * Date: 19.01.2017
 * Time: 19:06
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace LotteryElite
{
	/// <summary>
	/// Description of Authorise.
	/// </summary>
	public class Authorise
	{
		private User user = new User();
		public User Client {get{return user;} set{user = value;}}
		
		/// <summary>
		/// Метод авторизации в системе
		/// </summary>
		/// <param name="mysql">Объект класса БД</param>
		/// <param name="login">Логин пользователя</param>
		/// <param name="pass">Пароль пользователя</param>
		/// <returns>0 - если некорректный логин, 1 - если корректный логин
		///	но некорректный пароль, 2 - если всё корректно</returns>
		public int LogIn(Mysql mysql, String login, String pass)
		{	
			if(!mysql.CheckLogin(login)) 		return 0;
			if(!mysql.CheckPass(login, pass)) 	return 1;
				
			user.Login = login;
			return 2;
		}
		
		/// <summary>
		/// Метод регистрации в системе
		/// </summary>
		/// <param name="mysql">Объект класса БД</param>
		/// <param name="login">Логин пользователя</param>
		/// <param name="pass">Пароль пользователя</param>
		/// <returns>0 - если одно из данных пустое, 1 - если логин занят,
		/// 2 - если всё корректно</returns>
		public int SignUp(Mysql mysql, String login, String pass)
		{
			if(login == "" || pass == "") 	return 0;
			if(mysql.CheckLogin(login))	 	return 1;
			
			user.Login = login;
			user.Pass = pass;
			return 2;
		}
		
		/// <summary>
		/// Проверка данных на валидность (без пробелов и длина от 5 до 15 символов)
		/// </summary>
		/// <param name="str">Строка для проверки</param>
		/// <returns>True если корректно, иначе false</returns>
		public static bool IsDataValid(String str)
		{
			if(str.Length>15 || str.Length < 5) return false;
			for(int i = 0; i<str.Length; i++)
			{
				if(str[i] == ' ') return false;
			}
			return true;
		}
		
		/// <summary>
		/// Выход из системы (очищение данных объекта)
		/// </summary>
		public void LogOut()
		{
			user.Login = "";
			user.Pass = "";
		}
	}
}
