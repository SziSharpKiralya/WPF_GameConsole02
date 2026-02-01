using System.IO;
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

		private void GameIntermission()
		{
			if (score > 0)
			{
                SaveScore();
            }

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
            score = 0;
            state = true;
            scoreText.Content = score.ToString();
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
					(r.Tag?.ToString() == "TopPipe" || r.Tag?.ToString() == "BottomPipe" || r.Tag?.ToString() == "ScoredPipe"))
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
				Tag = "TopPipe"
			};

			Rectangle bottomPipe = new Rectangle
			{
				Width = pipeWidth,
				Height = canvasHeight - topPipeHeight - pipeGap,
				Fill = Brushes.DarkGreen,
				Tag = "BottomPipe"
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
                    (pipe.Tag?.ToString() == "TopPipe" || pipe.Tag?.ToString() == "BottomPipe" || pipe.Tag?.ToString() == "ScoredPipe"))
                {
					double x = Canvas.GetLeft(pipe);
					Canvas.SetLeft(pipe, x - pipeSpeed);

					if (x + pipe.Width < 0)
					{
						GameCanvas.Children.RemoveAt(i);
					}

					double birdTop = birdY + 3;
					double birdBottom = birdY + Bird.Height - 6;
					double birdLeft = Canvas.GetLeft(Bird) + 5;
					double birdRight = birdLeft + Bird.Width - 8;

					double pipeLeft = Canvas.GetLeft(pipe);
					double pipeRight = pipeLeft + pipe.Width;
					double pipeTop = Canvas.GetTop(pipe);
					double pipeBottom = pipeTop + pipe.Height;

					bool xOverlap = birdRight > pipeLeft && birdLeft < pipeRight;
					bool yOverlap = birdBottom > pipeTop && birdTop < pipeBottom;

                    if (pipe.Tag.ToString() == "TopPipe")
                        {
                        if (pipeRight < birdLeft)
                        {
                            score++;
                            pipe.Tag = "ScoredPipe";
                            scoreText.Content = score.ToString();
                        }
                    }

                    if (xOverlap && yOverlap)
					{
                        GameIntermission();
						return;
					}
				}
			}
		}

        private void SaveScore()
        {
            string filePath = "scores.txt";
            string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string line = $"{timeStamp} | {mode} |  Score: {score}";

            File.AppendAllText(filePath, line + Environment.NewLine);
        }
    }
}