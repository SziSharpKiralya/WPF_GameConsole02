using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WPF_GameConsole
{
	public partial class MainWindow
	{
		DispatcherTimer gameTimer;

		private double birdY = 100;
		private double velocity = 0;

		private const double gravity = 0.5;
		private const double jumpForce = -8;

		private void GameIntermission()
		{
			if (gameTimer != null)
			{
				gameTimer.Stop();
				gameTimer.Tick -= GameLoop;
			}

			birdY = 100;
			velocity = 0;
			state = false;
			Bird.Visibility = Visibility.Visible;
			spaceToStartInfo.Visibility = Visibility.Visible;
		}


		private void GameStart()
		{
			state = true;
			Canvas.SetTop(Bird, birdY);
			spaceToStartInfo.Visibility = Visibility.Hidden;
			gameTimer = new DispatcherTimer();
			gameTimer.Interval = TimeSpan.FromMilliseconds(16);
			gameTimer.Tick += GameLoop;
			gameTimer.Start();
		}

		private void GameJump()
		{
			velocity = jumpForce;
		}

		private void GameLoop(object? sender, EventArgs e)
		{
			velocity += gravity;
			birdY += velocity;
			Canvas.SetTop(Bird, birdY);

			if (birdY + Bird.Height >= GameCanvas.ActualHeight + 10) { GameIntermission();}
			if (birdY <= -10) { GameIntermission();}
		}
	}
}