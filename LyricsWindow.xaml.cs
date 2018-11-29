using Presto.SDK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Presto.SWCamp.Lyrics
{
    /// <summary>
    /// LyricsWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LyricsWindow : Window
    {
        string[] lines = File.ReadAllLines(@"C:\Users\green\Documents\cndqnreo\Musics\볼빨간사춘기 - 여행.txt");
        int arr = 0;
        int count;
        int a;
        int plus = 0;
        List<List<string>> t_lines = new List<List<string>>();
        List<int> clock = new List<int>();

        public LyricsWindow()
        {
            InitializeComponent();

            PrestoSDK.PrestoService.Player.StreamChanged += Player_StreamChanged;

            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1)
            };

            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Player_StreamChanged(object sender, Common.StreamChangedEventArgs e)
        {
            string l_lines;

            clock.Clear();
            t_lines.Clear();

            var fileName = PrestoSDK.PrestoService.Player.CurrentMusic.Path;
            lines = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName)) + ".txt");

            for (int i = 0; i < lines.Length; i++)
            {
                t_lines.Add(new List<string>());
            }

            for (int i = 3; i < 6; i++)
            {
                if ((Convert.ToInt32(lines[3].Substring(1, 2)) * 6000 + Convert.ToInt32(Convert.ToSingle(lines[3].Substring(4, 5)) * 100) == (Convert.ToInt32(lines[i].Substring(1, 2)) * 6000) + Convert.ToInt32(Convert.ToSingle(lines[i].Substring(4, 5)) * 100)))
                {
                    arr = (i - 3);
                }
                else
                    break;
            }

            count = 3;

            a = arr + 1;
            plus = 0;

            for (int i = 3; i < lines.Length / (arr + 1); i++)
            {
                for (int j = 0; j <= arr; j++)
                {
                    t_lines[i - 3].Add(lines[count].ToString());
                    count++;
                }

                if (arr == 0)
                {
                    l_lines = lines[i].ToString();
                    clock.Add(Convert.ToInt32(l_lines.Substring(1, 2)) * 6000 + Convert.ToInt32(Convert.ToSingle(l_lines.Substring(4, 5)) * 100));
                }
                else
                {
                    l_lines = lines[3 + (plus * a)].ToString();
                    clock.Add(Convert.ToInt32(l_lines.Substring(1, 2)) * 6000 + Convert.ToInt32(Convert.ToSingle(l_lines.Substring(4, 5)) * 100));
                }
                plus++;
            }

            MessageBox.Show(fileName.ToString());
            tt.Text = (Path.GetFileNameWithoutExtension(PrestoSDK.PrestoService.Player.CurrentMusic.Path));


        }




        public void Timer_Tick(object sender, EventArgs e)
        {

            for (int i = 0; i < clock.Count; i++)
            {
                if (i + 1 < clock.Count)
                {
                    if (clock[i] < ((int)PrestoSDK.PrestoService.Player.Position / 10.0f) + 100 & clock[i + 1] > ((int)PrestoSDK.PrestoService.Player.Position / 10.0f) + 100)
                    {

                        tb.Text = (t_lines[i][0].Substring(10)).ToString() + Environment.NewLine;
                        if (arr == 0)
                            continue;
                        else
                        {
                            for (int j = 1; j <= arr; j++)
                            {
                                tb.Text += t_lines[i][j].Substring(10).ToString() + Environment.NewLine;
                            }
                        }
                        break;
                    }
                }
            }

        }

    }

}