﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SaveTheHumans {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
   
    public partial class MainWindow : Window {
        Random rnd = new Random();
        DispatcherTimer enemyTimer = new DispatcherTimer();
        DispatcherTimer targetTimer = new DispatcherTimer();
        bool humanCaptured = false;

        public MainWindow() {
            InitializeComponent();

            enemyTimer.Tick += enemyTimer_Tick;
            enemyTimer.Interval = TimeSpan.FromSeconds(2);

            targetTimer.Tick += targetTimer_Tick;
            targetTimer.Interval = TimeSpan.FromSeconds(.1);
        }

        private void targetTimer_Tick(object sender, EventArgs e) {
            progressBar.Value += 1;
            if (progressBar.Value >= progressBar.Maximum) {
                EndTheGame();
            }
        }

        private void EndTheGame() {
            if (!playArea.Children.Contains(gameOverText)) {
                enemyTimer.Stop();
                targetTimer.Stop();
                humanCaptured = false;
                startButton.Visibility = Visibility.Visible;
                playArea.Children.Add(gameOverText);
            }
        }

        private void enemyTimer_Tick(object sender, EventArgs e) {
            AddEnemy();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e) {
            StartGame();
            //MessageBox.Show("PENIS");
        }

        private void StartGame() {
            Human.IsHitTestVisible = true;
            humanCaptured = false;
            progressBar.Value = 0;
            startButton.Visibility = Visibility.Collapsed;
            playArea.Children.Clear();
            playArea.Children.Add(target);
            playArea.Children.Add(Human);
            targetTimer.Start();
            enemyTimer.Start();
        }

        private void AddEnemy() {
            ContentControl enemy = new ContentControl();
            enemy.Template = Resources["EmptyTemplate"] as ControlTemplate;
            PropertyPath left = new PropertyPath("(Canvas.Left)");
            PropertyPath top = new PropertyPath("(Canvas.Top)");
            AnimateEnemy(enemy, 0, playArea.ActualWidth - 100, left);
            AnimateEnemy(enemy, rnd.Next((int)playArea.ActualHeight - 100), (rnd.Next((int)playArea.ActualHeight - 100)), top);
            playArea.Children.Add(enemy);
        }

        private void AnimateEnemy(ContentControl enemy, double from, double to, PropertyPath propertyToAnimate) {
            Storyboard storyboard = new Storyboard() {
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };
            DoubleAnimation animation = new DoubleAnimation() {
                From = from,
                To = to,
                Duration = new Duration(TimeSpan.FromSeconds(rnd.Next(4, 6)))
            };
            Storyboard.SetTarget(animation, enemy);
            Storyboard.SetTargetProperty(animation, propertyToAnimate);
            storyboard.Children.Add(animation);
            storyboard.Begin();
        }
    }
}
