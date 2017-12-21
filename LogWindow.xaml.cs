/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 26.03.2017
 * Time: 11:03
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows;
using System.Windows.Media;

namespace LotteryElite
{
	/// <summary>
	/// Interaction logic for LogWindow.xaml
	/// </summary>
	public partial class LogWindow : Window
	{
		Base dbase;
		public LogWindow()
		{
			dbase = new Base();
			InitializeComponent();
		}		
		
		/// <summary>
		/// Callback-Метод для входа игрока в систему
		/// </summary>
		void LogButton_Click(object sender, RoutedEventArgs e)
		{
			int res = dbase.LogIn(LogBox.Text, PassBox.Text);
			Color col = new Color();
			col.A = 255;
			SolidColorBrush br = new SolidColorBrush();
			switch(res)
			{
				case 0:
					StatusBlock.Text = "Неверный логин!";
					col.R = 177;
					col.G = 66;
					col.B = 66;
					br.Color = col;
					StatusBlock.Foreground = br;
					break;
					
				case 1:
					StatusBlock.Text = "Неверный пароль!";
					col.R = 177;
					col.G = 66;
					col.B = 66;
					br.Color = col;
					StatusBlock.Foreground = br;
					break;
					
				case 2:
					StatusBlock.Text = "Вход";
					col.R = 74;
					col.G = 176;
					col.B = 40;
					br.Color = col;
					StatusBlock.Foreground = br;
					
					var main = new MainWindow1(dbase);
					main.Show();
					this.Close();
					break;
			}
		}
		
		/// <summary>
		/// Callback-метод регистрации игрока в системе
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void RegButton_Click(object sender, RoutedEventArgs e)
		{
			int res = dbase.SignUp(LogBox.Text, PassBox.Text);
			Color col = new Color();
			col.A = 255;
			SolidColorBrush br = new SolidColorBrush();
			switch(res)
			{
				case 0:
					StatusBlock.Text = "Некорректный логин или пароль!";
					col.R = 177;
					col.G = 66;
					col.B = 66;
					br.Color = col;
					StatusBlock.Foreground = br;
					break;
					
				case 1:
					StatusBlock.Text = "Логин уже используется!";
					col.R = 177;
					col.G = 66;
					col.B = 66;
					br.Color = col;
					StatusBlock.Foreground = br;
					break;
					
				case 2:
					StatusBlock.Text = "Успешно!";
					col.R = 74;
					col.G = 176;
					col.B = 40;
					br.Color = col;
					StatusBlock.Foreground = br;
					var main = new MainWindow1(dbase);
					main.Show();
					Close();
					break;
				case 3:
					StatusBlock.Text = "Ошибка сервера!";
					col.R = 177;
					col.G = 66;
					col.B = 66;
					br.Color = col;
					StatusBlock.Foreground = br;
					break;
			}
		}
	
	}
}