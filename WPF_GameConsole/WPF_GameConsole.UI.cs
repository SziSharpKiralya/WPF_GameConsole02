using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

        private void btnModes_Click(object sender, RoutedEventArgs e)
        {
            LoadScreen_Modes();
        }

        private void btnScores_Click(object sender, RoutedEventArgs e)
        {
            LoadScreen_Scores();
        }

        private void btndiffNormal_Click(object sender, RoutedEventArgs e)
        {
            mode = "Normal";
            gravity = 0.5;
            jumpForce = -8;
            Fog.Visibility = Visibility.Hidden;
            Rain.Visibility = Visibility.Hidden;
        }

        private void btndiffRain_Click(object sender, RoutedEventArgs e)
        {
            mode = "Rain";
            gravity = 1.2;
            jumpForce = -6;
            Fog.Visibility = Visibility.Hidden;
            Rain.Visibility = Visibility.Visible;
        }

        private void btndiffFog_Click(object sender, RoutedEventArgs e)
        {
            mode = "Fog";
            gravity = 0.5;
            jumpForce = -8;
            Fog.Visibility = Visibility.Visible;
            Rain.Visibility = Visibility.Hidden;
        }

        private void LoadScreen_Menu()
		{
			current = "Menu";
			windowMenuTitle.Content = current;
			screen_Menu.Visibility = Visibility.Visible;
			screen_Game.Visibility = Visibility.Hidden;
            screen_Scores.Visibility = Visibility.Hidden;
            screen_Modes.Visibility = Visibility.Hidden;
        }

        private void LoadScreen_Modes()
        {
            current = "Modes";
            windowMenuTitle.Content = current;
            screen_Menu.Visibility = Visibility.Hidden;
            screen_Game.Visibility = Visibility.Hidden;
            screen_Scores.Visibility = Visibility.Hidden;
            screen_Modes.Visibility = Visibility.Visible;
        }

        private void LoadScreen_Scores()
        {
            current = "Scores";
            LoadScores();
            windowMenuTitle.Content = current;
            screen_Menu.Visibility = Visibility.Hidden;
            screen_Game.Visibility = Visibility.Hidden;
            screen_Scores.Visibility = Visibility.Visible;
            screen_Modes.Visibility = Visibility.Hidden;
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
            screen_Scores.Visibility = Visibility.Hidden;
            screen_Modes.Visibility = Visibility.Hidden;
        }

        private void LoadScores()
        {
            scoresPanel.Children.Clear();

            string filePath = "scores.txt";

            if (!File.Exists(filePath))
                return;

            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                Label lbl = new Label
                {
                    Content = line,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.Black,
                    Background = Brushes.Transparent,
                    FontFamily = new FontFamily("Broadway"),
                    Height = 60,
                    FontSize = 24,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(5)
                };

                scoresPanel.Children.Add(lbl);
            }
        }
    }
}