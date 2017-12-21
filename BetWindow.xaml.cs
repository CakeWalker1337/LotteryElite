/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 04/08/2017
 * Time: 17:15
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LotteryElite
{
	/// <summary>
	/// Interaction logic for BetWindow.xaml
	/// </summary>
	public partial class BetWindow : Window
	{
		Base dbase;
		Game game;
		int count = 0, maxnum = 0, gamecount = 0;
		List<Button> buttons = new List<Button>();
		private bool[] isClicked = new bool[49];
		public BetWindow(Base nbase, Game nGame)
		{
			dbase = nbase;
			game = nGame;
			InitializeComponent();
			InitializeButtons();
		}
		
		/// <summary>
		/// Создание кнопок для выбора чисел в билете в зависимости от типа игры
		/// </summary>
		public void InitializeButtons()
		{
			Thickness th = new Thickness(0, 10, 0, 0);
			StackPanel pan = new StackPanel();

			for(int i = 0; i<49; i++)
			{
				isClicked[i] = false;
				if(i%7==0)
				{
					StackPanel stackpan = new StackPanel();
					th.Left = 0;
					th.Top = 10;
					stackpan.Width = 283;
					stackpan.Height = 29;
					stackpan.Margin = th;
					stackpan.Orientation = Orientation.Horizontal;
					ButtStack.Children.Add(stackpan);
					pan = stackpan;
				}
				
				Button but = new Button();
				but.Height = 29;
				but.Width = 29;
				th.Left = 10;
				th.Top = 0;
				but.Margin = th;
				but.Background = new SolidColorBrush(Colors.LightSkyBlue);
				but.Content = (i+1).ToString();
				but.Click += NumButtons_Click;
				but.Visibility = Visibility.Hidden;
				pan.Children.Add(but);
				buttons.Add(but);
			}
			
			maxnum = game.GetMaxNum();
			gamecount = game.GetNumCount();
			StatusBox.Text = "Выберите " + gamecount + " чисел";
			for(int i = 0; i<maxnum; i++)
				buttons[i].Visibility = Visibility.Visible;
		
		}
		
		/// <summary>
		/// Callback-метод для кнопок выбора чисел в билете
		/// </summary>
		void NumButtons_Click(object sender, RoutedEventArgs e)
		{
			Button b = ((Button)sender);
			int id = Convert.ToInt32(b.Content);
			if(isClicked[id-1])
			{
				b.Background = new SolidColorBrush(Colors.LightSkyBlue);
				isClicked[id-1] = false;
				count--;
			}
			else
			{
				b.Background = new SolidColorBrush(Colors.LawnGreen);
				isClicked[id-1] = true;
				count++;
			}
			if(count>=gamecount)
			{
				for(int i = 0; i<maxnum; i++)
				{
					if(!isClicked[i])	buttons[i].IsEnabled = false;	
				}
			}
			else
			{
				for(int i = 0; i<maxnum; i++)
				{
					buttons[i].IsEnabled = true;	
				}
			}
			
		}

		/// <summary>
		/// Callback-метод кнопки "Купить билет". Окончательное применение выбранной комбинации.
		/// </summary>
		void BetButton_Click(object sender, RoutedEventArgs e)
		{
			if(count == gamecount)
			{
				if(dbase.Money>=game.TicketCost)
				{
					Ticket tick = new Ticket();
					tick.Combination = GetCurrentCombination();
					tick.GameId = game.GameId;
					tick.OwnerId = dbase.Id;				
					if(dbase.CreateTicket(tick))
					{
						tick.TicketId = dbase.GetMyLastInsertTicketId();
						if(tick.IsTicketValid())
						{
							dbase.Money -= game.TicketCost;
							dbase.AddTicket(tick);
							dbase.SaveMe();
							dbase.AddGame(game);
							MainWindow1 main = new MainWindow1(dbase);
							main.Show();
							Close();
						}
					}	
				}
				else
				{
					StatusBox.Foreground = new SolidColorBrush(Colors.Red);
					StatusBox.Text = "У вас недостаточно денег!";
				}
			}
			else
			{
				StatusBox.Foreground = new SolidColorBrush(Colors.Red);
				StatusBox.Text = "Выберите больше чисел!";
			}
		}
		
		/// <summary>
		/// Callback-метод кнопки выхода
		/// </summary>
		void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			MainWindow1 main = new MainWindow1(dbase);
			main.Show();
			Close();
		}

		/// <summary>
		/// Получает текущую комбинацию чисел с активных кнопок
		/// </summary>
		/// <returns>Комбинация чисел</returns>
		public String GetCurrentCombination()
		{
			String res = ""; int fl = 0;
			for(int i = 0; i<maxnum; i++)
			{
				if(isClicked[i])
				{
					if(fl == 0)
					{
						res = (string)buttons[i].Content;
						fl = 1;
					}
					else res = res + "_" + buttons[i].Content;
				}
			}
			return res;
		}	
	}
}