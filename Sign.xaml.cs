/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 15.04.2017
 * Time: 19:48
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows;

namespace LotteryElite
{
	/// <summary>
	/// Interaction logic for Sign.xaml
	/// </summary>
	public partial class Sign : Window
	{	
		/// <summary>
		/// Конструктор класса окна уведомления
		/// </summary>
		/// <param name="message">строка уведомления</param>
		public Sign(String message)
		{
			InitializeComponent();
			Note.Text = message;
		}
		
		//Конструктор класса окна уведомления
		//
		//в зависимости от ситуации
		/// <summary>
		/// Конструктор класса окна уведомления. 
		/// Применяется, когда нужно создать один объект и менять уведомление.
		/// </summary>
		public Sign()
		{
			InitializeComponent();
		}
		
		void Exit_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
		
		/// <summary>
		/// Метод, изменяющий строку уведомления
		/// </summary>
		/// <param name="message">строка уведомления</param>
		public void SetMessage(String message)
		{
			Note.Text = message;
		}
	}
}