using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Shapes;

namespace WPF_GameConsole
{
	public partial class MainWindow
	{
		private DispatcherTimer gameTimer;
		private DispatcherTimer pipeTimer;
		private const double pipeSpeed = 3;
		private const double pipeGap = 150;
		private const double pipeWidth = 60;

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

			if (pipeTimer != null)
			{
				pipeTimer.Stop();
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

			pipeTimer = new DispatcherTimer();
			pipeTimer.Interval = TimeSpan.FromSeconds(2);
			pipeTimer.Tick += (s, e) => SpawnPipes();
			pipeTimer.Start();

			for (int i = GameCanvas.Children.Count - 1; i >= 0; i--)
			{
				if (GameCanvas.Children[i] is Rectangle r &&
					r.Tag?.ToString() == "Pipe")
				{
					GameCanvas.Children.RemoveAt(i);
				}
			}
		}

		private void GameJump()
		{
			velocity = jumpForce;
		}

		private void SpawnPipes()
		{
			double canvasHeight = GameCanvas.ActualHeight;
			double canvasWidth = GameCanvas.ActualWidth;

			// Canvas not ready → don't spawn
			if (canvasHeight <= 0 || canvasWidth <= 0)
				return;

			double minTopHeight = 50;
			double maxTopHeight = canvasHeight - pipeGap - 50;

			if (maxTopHeight <= minTopHeight)
				return;

			double topPipeHeight = random.Next(
				(int)minTopHeight,
				(int)maxTopHeight
			);

			Rectangle topPipe = new Rectangle
			{
				Width = pipeWidth,
				Height = topPipeHeight,
				Fill = Brushes.DarkGreen,
				Tag = "Pipe"
			};

			Rectangle bottomPipe = new Rectangle
			{
				Width = pipeWidth,
				Height = canvasHeight - topPipeHeight - pipeGap,
				Fill = Brushes.DarkGreen,
				Tag = "Pipe"
			};

			Canvas.SetLeft(topPipe, canvasWidth);
			Canvas.SetTop(topPipe, 0);

			Canvas.SetLeft(bottomPipe, canvasWidth);
			Canvas.SetTop(bottomPipe, topPipeHeight + pipeGap);

			GameCanvas.Children.Add(topPipe);
			GameCanvas.Children.Add(bottomPipe);
		}

		private void GameLoop(object? sender, EventArgs e)
		{
			velocity += gravity;
			birdY += velocity;
			Canvas.SetTop(Bird, birdY);

			if (birdY + Bird.Height >= GameCanvas.ActualHeight + 10) { GameIntermission(); }
			if (birdY <= -10) { GameIntermission(); }

			for (int i = GameCanvas.Children.Count - 1; i >= 0; i--)
			{
				if (GameCanvas.Children[i] is Rectangle pipe &&
					pipe.Tag?.ToString() == "Pipe")
				{
					double x = Canvas.GetLeft(pipe);
					Canvas.SetLeft(pipe, x - pipeSpeed);

					if (x + pipe.Width < 0)
					{
						GameCanvas.Children.RemoveAt(i);
					}

					double birdTop = birdY + 6;
					double birdBottom = birdY + Bird.Height - 8;
					double birdLeft = Canvas.GetLeft(Bird) + 6;
					double birdRight = birdLeft + Bird.Width - 12;

					double pipeLeft = Canvas.GetLeft(pipe);
					double pipeRight = pipeLeft + pipe.Width;
					double pipeTop = Canvas.GetTop(pipe);
					double pipeBottom = pipeTop + pipe.Height;

					bool xOverlap = birdRight > pipeLeft && birdLeft < pipeRight;
					bool yOverlap = birdBottom > pipeTop && birdTop < pipeBottom;

					if (xOverlap && yOverlap)
					{
						GameIntermission();
						return;
					}
				}
			}
		}
	}
}