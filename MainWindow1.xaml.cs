/*
 * Created by SharpDevelop.
 * User: Maxim
 * Date: 03/26/2017
 * Time: 12:37
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace LotteryElite
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow1 : Window
	{
		Base dbase;
		int flag = -1;
		Game findGame = new Game();
		Ticket findTicket = new Ticket();
		User findUser = new User();
		public MainWindow1(Base nbase)
		{
			dbase = nbase;
			InitializeComponent();
			WriteData();
			WriteAllGames();
		}
		
		/// <summary>
		/// Выводит всю информацию для клиента на экран
		/// </summary>
		public void WriteData()
		{
			if(dbase.Type < 2) TabAdminPanel.Visibility = Visibility.Hidden;
			DataLogBox.Text = dbase.Login;
			DataPassBox.Text = dbase.Pass;
			DataPermBlock.Text = dbase.GetTypeName();
			IdBlock.Text = dbase.Id.ToString();
			MoneyBlock.Text = dbase.Money.ToString();
			
			List<Game> games = dbase.GetGames();
			
			AllGamesBlock.Text = games.Count.ToString();
			int valid = 0, maxwin = 0, lastwin = 0, sum = 0;
			List<Ticket> tickets = dbase.GetTickets();
			for(int i = 0; i<games.Count; i++)
			{
				if(games[i].IsGameActive()) valid++;
				
				for(int j = 0; j<tickets.Count; j++)
				{
					if(tickets[j].GameId == games[i].GameId && tickets[j].Combination == games[i].WinCombination)
					{
						lastwin = games[i].Pool;
						sum += games[i].Pool;
						if(games[i].Pool > maxwin) maxwin = games[i].Pool;
					}
				}
			}
			
			ActiveGamesBlock.Text = valid.ToString();
			MoneyGainedBlock.Text = sum.ToString();
			LastPrizeBlock.Text = lastwin.ToString();
			MaxPrizeBlock.Text = maxwin.ToString();
			for(int i = 0; i<games.Count; i++)
			{
				Sample sam = new Sample();
				sam = games[i].ToSample();
				sam.P7 = dbase.GetMyTicket(games[i].GameId).GetStringFromCombination();
				MyGamesGrid.Items.Add(sam);
			}
		}
		
		/// <summary>
		/// Callback-метод кнопки применения изменений профиля
		/// </summary>
		void SubmitButton_Click(object sender, RoutedEventArgs e)
		{
			if(dbase.Login != DataLogBox.Text)
			{
				if(dbase.IsLoginCorrect(DataLogBox.Text) && !dbase.CheckLogin(DataLogBox.Text))
				{
					dbase.Pass = DataPassBox.Text;
					dbase.Login = DataLogBox.Text;
					dbase.SaveMe();
					var mes = new Sign("Изменения приняты!");
					mes.Show();
				}
				else
				{
					DataPassBox.Text = dbase.Pass;
					DataLogBox.Text = dbase.Login;
					var mes = new Sign("Ошибка!");
					mes.Show();
				}
			}
			else
			{
				dbase.Pass = DataPassBox.Text;
				dbase.SaveMe();
				var mes = new Sign("Изменения приняты!");
				mes.Show();
			}
		}
		
		/// <summary>
		/// Callback-метод кнопки выхода из аккаунта
		/// </summary>
		void LogOutButton_Click(object sender, RoutedEventArgs e)
		{
			dbase.LogOut();
			var logwin = new LogWindow();
			logwin.Show();
			Close();
		}
		
		/// <summary>
		/// Callback-метод кнопки обновления
		/// </summary>
		void RefreshData_Click(object sender, RoutedEventArgs e)
		{
			MyGamesGrid.Items.Clear();
			dbase.LoadMe();
			WriteData();
		}	
		
		///////////////////////////////////////////////////////////////////////ALLGAMES////////////////////////////////////////////////////////
		
		/// <summary>
		/// Выводит в сетку все игры
		/// </summary>
		public void WriteAllGames()
		{
			List<Game> games = dbase.ShowAllGames();
			for(int i = 0; i<games.Count; i++)
			{
				if(!dbase.IsMyGame(games[i]))
				{
					Sample sam = new Sample();
					sam = games[i].ToSample();
					AllGamesGrid.Items.Add(sam);
					
				}
			}
			
		}
		
		/// <summary>
		/// Callback-метод при нажатии на игру в сетке игр
		/// </summary>
		void Double_Click(object sender, RoutedEventArgs e)
		{
			Sample row = (Sample)AllGamesGrid.SelectedItem;
			Int32 id = Convert.ToInt32(row.P1);
			if(row.P6 == "Opened")
			{
				BetWindow bet = new BetWindow(dbase, row.ToGame());
				bet.Show();
				Close();
			}
			else
			{
				var mes = new Sign("Игра закрыта!");
				mes.Show();
			}
		}
		
		/// <summary>
		/// Callback-метод кнопки обновления игр
		/// </summary>
		void RefreshAllGamesButton_Click(object sender, RoutedEventArgs e)
		{
			RefreshAllGames();
		}
		
		/// <summary>
		/// Метод обновления игр (перезапись)
		/// </summary>
		public void RefreshAllGames()
		{	
			AllGamesGrid.Items.Clear();
			WriteAllGames();
		}
		
		///////////////////////////////////////////////////////////////////////////////////ADMINPANEL////////////////////////////////////////////////////////////////
		
		/// <summary>
		/// Callback-метод выбора пункта в выпадающем меню
		/// </summary>
		void ComboBox_Select(object sender, SelectionChangedEventArgs e)
		{
			
			if(flag == -1)
			{
				flag = TypeComboBox.SelectedIndex;
				return;
			}
			ClearFindBlocks();
			flag = TypeComboBox.SelectedIndex;
			FindProfileData.IsEnabled = false;
			FindMyGames.IsEnabled = false;
			findUser.Clear();
			findGame.Clear();
			findTicket.Clear();
			switch (flag) {
				case 0:
					FindTextBlock.Text = "Введите Id или Login";
					FindDataP1.Content = "Логин";
					FindDataP2.Content = "Пароль";
					FindDataP3.Content = "Доступ";
					FindDataP4.Content = "ID";
					FindDataP5.Visibility = Visibility.Visible;
					FindDataPointBox.Visibility = Visibility.Visible;
					FindDataP5.Content = "Очки";
					StartGameButton.Visibility = Visibility.Hidden;
					DeleteButton.Content = "Удалить пользователя";
					CreateButton.Content = "Создать пользователя";
					break;
				case 1:
					List<Game> games = dbase.ShowAllGames();
					if(games.Count == 0)
					{
						FindProfileData.IsEnabled = true;
						FindMyGames.IsEnabled = true;
					}
					FindTextBlock.Text = "Введите Id игры";
					FindDataP1.Content = "Выигрыш";
					FindDataP2.Content = "Тип";
					FindDataP3.Content = "Комбинация";
					FindDataP4.Content = "ID";
					FindDataP5.Visibility = Visibility.Visible;
					FindDataPointBox.Visibility = Visibility.Visible;
					FindDataP5.Content = "Цена";
					DeleteButton.Content = "Удалить игру";
					CreateButton.Content = "Создать игру";
					if(dbase.Type < 3)
					{
						StartGameButton.Visibility = Visibility.Hidden;
					}
					else StartGameButton.Visibility = Visibility.Visible;
					break;
				case 2:
					FindTextBlock.Text = "Введите Id билета";
					FindDataP1.Content = "ID игры";
					FindDataP2.Content = "ID владельца";
					FindDataP3.Content = "Комбинация";
					FindDataP4.Content = "ID";
					FindDataP5.Visibility = Visibility.Hidden;
					FindDataPointBox.Visibility = Visibility.Hidden;
					DeleteButton.Content = "Удалить билет";
					CreateButton.Content = "Создать билет";
					StartGameButton.Visibility = Visibility.Hidden;
					break;
			}
		}
		
		/// <summary>
		/// Callback-метод кнопки поиска содержимого строки поиска
		/// в зависимости от выбора в выпадающем меню
		/// </summary>
		void FindButton_Click(object sender, RoutedEventArgs e)
		{
			int a = 0;
			
			if(flag == 0)
			{
				FindMyGamesGrid.Items.Clear();
				if(int.TryParse(FindTextBox.Text, out a)) findUser = dbase.LoadClient(a);
				else findUser = dbase.LoadClient(FindTextBox.Text);
				if(findUser.IsUserValid())
				{
					FindProfileData.IsEnabled = true;
					FindMyGames.IsEnabled = true;
					
					FindDataLogBox.Text = findUser.Login;
					FindDataPassBox.Text = findUser.Pass;
					FindDataPermBox.Text = findUser.Type.ToString();
					FindDataIdBox.Text = findUser.Id.ToString();
					FindDataPointBox.Text = findUser.Money.ToString();
					if(dbase.Type<3) FindDataPermBox.IsEnabled = false;
					List<Game> games = findUser.GetGames();
					for(int i = 0; i<games.Count; i++)
					{
						Sample sam = new Sample();
						sam = games[i].ToSample();
						sam.P7 = findUser.GetTicketByGameId(games[i].GameId).GetStringFromCombination();
						FindMyGamesGrid.Items.Add(sam);
					}
				}
				else
				{
					var mes = new Sign("Пользователя не существует");
					mes.Show();
				}
			}
			else
			{
				if(Int32.TryParse(FindTextBox.Text, out a)) 
				{
					if(flag == 1)
					{
						findGame = dbase.GetGame(a);
						if(findGame.IsGameValid())
						{
							FindProfileData.IsEnabled = true;							
							FindDataLogBox.Text = findGame.Pool.ToString();
							FindDataPassBox.Text = findGame.Type.ToString();
							FindDataPermBox.Text = findGame.WinCombination;
							FindDataIdBox.Text = findGame.GameId.ToString();
							FindDataPointBox.Text = findGame.TicketCost.ToString();
							if(dbase.Type<3) FindDataPermBox.IsEnabled = false;
						}
						else
						{
							var mes = new Sign("Игры не существует");
							mes.Show();
						}
					
					}
					else if(flag == 2)
					{
						findTicket = dbase.GetTicket(a);
						if(findTicket.IsTicketValid())
						{
							FindProfileData.IsEnabled = true;
							FindDataLogBox.Text = findTicket.GameId.ToString();
							FindDataPassBox.Text = findTicket.OwnerId.ToString();
							FindDataPermBox.Text = findTicket.Combination;
							FindDataPermBox.IsEnabled = true;
							FindDataIdBox.Text = findTicket.TicketId.ToString();
						}
						else
						{
							var mes = new Sign("Билета не существует");
							mes.Show();
						}
					}
				}
		    }
		}
		
		/// <summary>
		/// Callback-метод кнопки удаления объекта
		///	в зависимости от выбора в выпадающем меню
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			var mes = new Sign();
			switch (flag) {
				case 0:
					dbase.DeleteUser(findUser.Id);
					findUser.Clear();
					ClearFindBlocks();
					mes.SetMessage("Пользователь удален!");
					break;
				case 1:
					dbase.DeleteGame(findGame.GameId);
					findGame.Clear();
					ClearFindBlocks();
					mes.SetMessage("Игра удалена!");
					break;
				case 2:
					dbase.DeleteTicket(findTicket.TicketId);
					findTicket.Clear();
					mes.SetMessage("Билет удален!");
					ClearFindBlocks();
					break;
				default:
					mes.SetMessage("Ошибка удаления!");
					break;
			}
			mes.Show();
		}
		
		/// <summary>
		/// Очищение содержимого в текстовых блоках
		/// </summary>
		public void ClearFindBlocks()
		{
			FindDataLogBox.Text = "";
			FindDataPassBox.Text = "";
			FindDataPermBox.Text = "";
			FindDataIdBox.Text = "";
			FindDataPointBox.Text = "";
		}
		
		/// <summary>
		/// Callback-метод кнопки создания
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void CreateButton_Click(object sender, RoutedEventArgs e)
		{
			var mes = new Sign();
			switch (flag) {
				case 0:
					if (CheckUserData())
					{
						dbase.CreateUser(findUser);
						mes.SetMessage("Пользователь создан!");
					}
					else mes.SetMessage("Ошибка создания!");
					break;
				case 1:
					if (CheckGameData())
					{
						dbase.CreateGame(findGame);
						mes.SetMessage("Игра создана!");
					}
					else mes.SetMessage("Ошибка создания!");
					break;
				case 2:
					if (CheckTicketData())
					{
						dbase.CreateTicket(findTicket);
						mes.SetMessage("Билет создан!");
					}
					else mes.SetMessage("Ошибка создания!");
					break;
				default:
					mes.SetMessage("Ошибка создания!");
					break;
			}
			mes.Show();
		}
		
		/// <summary>
		/// Callback-метод кнопки применения изменений
		/// </summary>
		void SubmitChangesButton_Click(object sender, RoutedEventArgs e)
		{
			var mes = new Sign();
			switch (flag) {
				case 0:
					if (CheckUserData())
					{
						dbase.SaveUser(findUser);
						mes.SetMessage("Пользователь сохранен!");
					}
					else mes.SetMessage("Ошибка сохранения!");		
					break;
				case 1:
					if (CheckGameData())
					{
						dbase.SaveGame(findGame);
						mes.SetMessage("Игра сохранена!");
					}
					else mes.SetMessage("Ошибка сохранения!");
					break;
				case 2:
					if (CheckTicketData())
					{
						dbase.SaveTicket(findTicket);
						mes.SetMessage("Билет сохранен!");
					}
					else mes.SetMessage("Ошибка сохранения!");
					break;
				default:
					mes.SetMessage("Ошибка сохранения!");
					break;
			}
			mes.Show();
		}
		
		/// <summary>
		/// Проверка введенных данных о пользователе на корректность и валидность
		/// </summary>
		/// <returns>True в случае успеха, иначе false</returns>
		public bool CheckUserData()
		{
			int res = 0;
			bool isConfirmed = true;
			if(isConfirmed && FindDataLogBox.Text != findUser.Login &&
			   (!dbase.IsLoginCorrect(FindDataLogBox.Text) || dbase.CheckLogin(FindDataLogBox.Text))) isConfirmed = false;
			if(isConfirmed && FindDataPassBox.Text != findUser.Pass && FindDataPassBox.Text == "") isConfirmed = false;
			if(isConfirmed && FindDataPermBox.Text != findUser.Type.ToString() &&
			   (!Int32.TryParse(FindDataPermBox.Text, out res) || !(res > 0 && res < 4))) isConfirmed = false;
			if(isConfirmed && FindDataPointBox.Text != findUser.Money.ToString() &&
			   (!Int32.TryParse(FindDataPointBox.Text, out res) || res < 0)) isConfirmed = false;
			if(FindDataLogBox.Text == findUser.Login && FindDataPassBox.Text == findUser.Pass && 
			   FindDataPermBox.Text == findUser.Type.ToString() && FindDataPointBox.Text == findUser.Money.ToString()) isConfirmed = false;
					
			if(isConfirmed == true)
			{
				findUser.Login = FindDataLogBox.Text;
				findUser.Pass = FindDataPassBox.Text;
				findUser.Type = Convert.ToInt32(FindDataPermBox.Text);
				findUser.Money = res;
				return true;
			}
					
			FindDataLogBox.Text = findUser.Login;
			FindDataPassBox.Text = findUser.Pass;
			FindDataPermBox.Text = findUser.Type.ToString();
			FindDataPointBox.Text = findUser.Money.ToString();
			return false;
		}
		
		/// <summary>
		/// Проверка введенных данных об игре на корректность и валидность
		/// </summary>
		/// <returns>True в случае успеха, иначе false</returns>
		public bool CheckGameData()
		{
			int res = 0;
			bool isConfirmed = true;
			if(isConfirmed && FindDataLogBox.Text != findGame.Pool.ToString() &&
			   (!Int32.TryParse(FindDataLogBox.Text, out res) || res <= 0)) isConfirmed = false;
			if(isConfirmed && FindDataPassBox.Text != findGame.Type.ToString() &&
			   (!Int32.TryParse(FindDataPassBox.Text, out res) || !(res > 0 && res < 5))) isConfirmed = false;
			if(isConfirmed && FindDataPointBox.Text != findGame.TicketCost.ToString() &&
			   (!Int32.TryParse(FindDataPointBox.Text, out res) || res < 0)) isConfirmed = false;
			if(FindDataLogBox.Text == findGame.Pool.ToString() && FindDataPassBox.Text == findGame.Type.ToString() &&
			   FindDataPermBox.Text == findGame.WinCombination && FindDataPointBox.Text == findGame.TicketCost.ToString()) isConfirmed = false;
			
			if(isConfirmed == true)
			{
				findGame.Pool = Convert.ToInt32(FindDataLogBox.Text);
				findGame.Type = Convert.ToInt32(FindDataPassBox.Text);
				findGame.WinCombination = FindDataPermBox.Text;
				findGame.TicketCost = Convert.ToInt32(FindDataPointBox.Text);
				return true;
			}
					
			FindDataLogBox.Text = findGame.Pool.ToString();
			FindDataPassBox.Text = findGame.Type.ToString();
			FindDataPermBox.Text = findGame.WinCombination;
			FindDataPointBox.Text = findGame.TicketCost.ToString();
			return false;
			
		}
		
		/// <summary>
		/// Проверка введенных данных о билете на корректность и валидность
		/// </summary>
		/// <returns>True в случае успеха, иначе false</returns>
		public bool CheckTicketData()
		{
			int res = 0;
			bool isConfirmed = true;
			if(isConfirmed && FindDataLogBox.Text != findTicket.GameId.ToString() &&
			   (!Int32.TryParse(FindDataLogBox.Text, out res) || !dbase.CheckGameId(res))) isConfirmed = false;
			if(isConfirmed && FindDataPassBox.Text != findTicket.OwnerId.ToString())
		   	{
				if(Int32.TryParse(FindDataPassBox.Text, out res))
				{
					findUser = dbase.LoadClient(res);
					if(findUser.IsUserValid())
					{
						isConfirmed = true;
						findUser.Clear();
					}
			   		else isConfirmed = false;
				}
				else isConfirmed = false;
		   	}
			if(isConfirmed && FindDataPermBox.Text != findTicket.Combination) isConfirmed = false;
			if(FindDataLogBox.Text == findTicket.GameId.ToString() && FindDataPassBox.Text == findTicket.OwnerId.ToString() &&
			   FindDataPermBox.Text == findTicket.Combination) isConfirmed = false;

			
			if(isConfirmed)
			{
				findTicket.GameId = Convert.ToInt32(FindDataLogBox.Text);
				findTicket.OwnerId = Convert.ToInt32(FindDataPassBox.Text);
				findTicket.Combination = FindDataPermBox.Text;
				return true;
			}
			FindDataLogBox.Text = findTicket.GameId.ToString();
			FindDataPassBox.Text = findTicket.OwnerId.ToString();
			FindDataPermBox.Text = findTicket.Combination;
			return false;
		}		
		
		/// <summary>
		/// Callback-метод кнопки начала игры
		/// </summary>
		void StartGameButton_Click(object sender, RoutedEventArgs e)
		{
			if(findGame.WinCombination == "")
			{
				Random rand = new Random();
				StringBuilder str = new StringBuilder();
				int[] randmas = new int[findGame.GetMaxNum()];
				for(int i = 0; i<findGame.GetMaxNum(); i++)	randmas[i] = i+1;
				int currand = 0;
				for(int i = 0; i<findGame.GetNumCount(); i++)
				{
					currand = rand.Next()%(findGame.GetMaxNum());
					if(currand != i)
					{
						randmas[i] += randmas[currand];
						randmas[currand] = randmas[i] - randmas[currand];
						randmas[i] -= randmas[currand];
					}
				}	
				Array.Sort(randmas, 0, findGame.GetNumCount());
				
				for(int i = 0; i<findGame.GetNumCount(); i++)
				{
					str.AppendFormat("_{0}", randmas[i]);
				}
				str.Remove(0,1);
				findGame.WinCombination = str.ToString();
				dbase.SaveGame(findGame);
				FindDataPermBox.Text = str.ToString();
				List<Ticket> tickets = dbase.GetTicketsByGameId(findGame.GameId);
				int[][] ticketMas = new int[tickets.Count][];
				for(int i = 0; i<tickets.Count; i++) ticketMas[i] = new int[findGame.GetNumCount()+1];
				
				for(int i = 0; i<tickets.Count; i++)
				{
					ticketMas[i] = dbase.SplitCombination(tickets[i].Combination, findGame.GetNumCount());
				}
				
				for(int i = 0; i<findGame.GetNumCount(); i++)
				{
					for(int j = 0; j<tickets.Count; j++)
					{
						for(int k = 0; k<findGame.GetNumCount(); k++)
						{
							if(randmas[i]>ticketMas[j][k]) break;
							if(randmas[i] == ticketMas[j][k])
							{
								ticketMas[j][findGame.GetNumCount()]++;
								break;
							}					
						}
					}
				}
				
				for(int i = 0; i<tickets.Count; i++)
				{
					for(int j = findGame.GetNumCount(); j>0; j--)
					{
						if(ticketMas[i][findGame.GetNumCount()] == j)
						{
							dbase.SetPrize(tickets[i].OwnerId, dbase.GetPrizeByNumberOfMatches(findGame.Pool, j, findGame.GetNumCount()));
						}
					}
				}
				var mes = new Sign("Игра успешно проведена!");
				mes.Show();
			}
		}		
	}
}