using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoomWords
{
    public partial class Game : Form
    {
        bool host = false;
        string username;
        string gamename;
        Socket userSocket;
        public Game(string user_name, string game_name, Socket userSocket)
        {
            InitializeComponent();
            this.username = user_name;
            this.gamename = game_name;
            this.userSocket = userSocket;
        }

        private void Game_Load(object sender, EventArgs e)
        {
            this.UsernameLabel.Text = this.username;
            this.GamenameLabel.Text = this.gamename;
            if (host)
                StartButton.Visible = true;
            string mensaje = $"Game/{gamename}/Join";
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);

            userSocket.Send(msg);
        }


        public void DeclareHost()
        { 
            this.host = true;   
        }


        public void GameResponses(string gameResponse)
        {
            string[] message = gameResponse.Split('/');
            switch (message[0])
            {

                case "Turn":
                    string user_turn = message[1];
                    string syllable = message[2];

                    this.Invoke(new Action(() =>
                    {
                        WordBox.BackColor = Color.White;
                        WordBox.ForeColor = Color.Black;
                        WordBox.Text = "";
                        this.TurnLabel.Text = user_turn;
                        this.SyllableLabel.Text = syllable;
                        if (user_turn == this.username)
                        {
                            WordBox.Enabled = true;
                            WordBox.Focus();
                        }
                        else
                        {
                            WordBox.Enabled = false;
                        }
                    }));
                    break;
                case "Word":
                    this.Invoke(new Action(() =>
                    {
                        WordBox.Text = message[1];
                        if (TurnLabel.Text == this.username)
                        {
                            if (message[2] == "1")
                            {
                                WordBox.BackColor = Color.Green;
                            }
                            else
                            {
                                WordBox.BackColor = Color.Red;
                            }

                        }
                        else
                        {
                            if (message[2] == "1")
                            {
                                WordBox.ForeColor = Color.Green;
                            }
                            else
                            {
                                WordBox.ForeColor = Color.Red;
                            }
                          
                        }
                        

                    }));
                    break;

                case "Boom":
                    this.Invoke(new Action(() =>
                    {
                        foreach (DataGridViewRow row in PlayersGrid.Rows)
                        {
                            
                            if (row.Cells["user"].Value.ToString() == message[1].Split(' ')[0])
                            {
                                row.Cells["lives"].Value = Convert.ToInt32(message[1].Split(' ')[1]);
                                break;
                            }
                        }
                        if (host)
                        {
                            RunDelay.Start();
                        }
                    }));
                        
                    break;

                case "Win":
                    BoomTimer.Stop();
                    MessageBox.Show($"Winner: {message[1]}");
                    break;

                case "Load":
                    this.Invoke(new Action(() =>
                    {
                        PlayersGrid.Rows.Clear();
                        var Players = message[1].Split(',')
                            .Select(t => t.Trim())
                            .Select(t =>
                            {
                                var part = t.Split(' ');
                                return new { name = part[0], lives = int.Parse(part[1]) };

                            })
                            .ToList();

                        foreach (var player in Players)
                        {
                            PlayersGrid.Rows.Add(player.name, player.lives);
                        }
                        if (host)
                        {
                            if (message[1].Split(',').Length >= 2)
                                StartButton.Enabled = true;
                            else
                                StartButton.Enabled = false;
                        }
                    }));
                    break;
                case "Refresh":
                    foreach (DataGridViewRow row in PlayersGrid.Rows)
                    {
                        if (row.Cells["User"].Value.ToString() == message[1].Split(' ')[0])
                        {
                            row.Cells["Lives"].Value = Convert.ToInt32(message[1].Split(' ')[1]);
                            break;
                        }
                    }
                    break;


                case "End":
                    this.Close();
                    break;

            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            string mensaje = $"Game/{gamename}/Start";
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            userSocket.Send(msg);
            RunDelay.Start();
        }

        private void RunDelay_Tick(object sender, EventArgs e)
        {
            RunDelay.Stop();
            StartButton.Enabled = false;
            BoomTimer.Start();
            string mensaje = $"Game/{gamename}/Run";
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            userSocket.Send(msg);
        }

        private void BoomTimer_Tick(object sender, EventArgs e)
        {
            BoomTimer.Stop();
            string mensaje = $"Game/{gamename}/Boom";
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);

            userSocket.Send(msg);
        }
        private void CheckWord(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                string mensaje = $"Game/{gamename}/Word/{WordBox.Text}";
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                userSocket.Send(msg);
            }
        }

        private void LeaveButton_Click(object sender, EventArgs e)
        {
            string mensaje = $"Game/{gamename}/Leave";
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);

            userSocket.Send(msg);
            this.Close();
        }

        

        
    }
}
