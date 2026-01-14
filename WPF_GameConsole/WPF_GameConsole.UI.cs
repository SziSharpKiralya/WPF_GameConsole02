using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF_GameConsole
{
	public partial class MainWindow
	{
		private void console_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			DragMove();
		}

		private void btnClose_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
			LoadScreen_Menu();
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Space)
			{
				if (current == "Game")
				{
					if (state) {GameJump();}
					else {GameStart();}
				}
			}
		}

		private void btnPlay_Click(object sender, RoutedEventArgs e)
		{
			LoadScreen_Game();
		}

		private void LoadScreen_Menu()
		{
			current = "Menu";
			windowMenuTitle.Content = current;
			screen_Menu.Visibility = Visibility.Visible;
			screen_Game.Visibility = Visibility.Hidden;
		}

		private void LoadScreen_Game()
		{
			current = "Game";
			GameIntermission();
			Canvas.SetLeft(Bird, 20);
			Canvas.SetTop(Bird, birdY);
			windowMenuTitle.Content = current;
			screen_Menu.Visibility = Visibility.Hidden;
			screen_Game.Visibility = Visibility.Visible;
		}
	}
}