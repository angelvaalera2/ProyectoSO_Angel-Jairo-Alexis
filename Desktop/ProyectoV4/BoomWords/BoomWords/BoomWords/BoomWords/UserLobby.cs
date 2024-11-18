using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoomWords
{
    public partial class UserLobby : Form
    {
        string username;
        Socket userSocket;
        Thread userThread;

        private Dictionary<string, Game> gameForms = new Dictionary<string, Game>();
        public UserLobby(IPEndPoint ipep, string username)
        {
            InitializeComponent();
            this.username = username;

            userSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                userSocket.Connect(ipep);  //Intentamos conectar el socket

                ThreadStart ts = delegate { UserAttendServer(); };
                userThread = new Thread(ts);
                userThread.Start();


            }
            catch
            {
                MessageBox.Show("Error en User");
                this.Close();
            }

        }

        private void UserLobby_Load(object sender, EventArgs e)
        {
            UsernameLabel.Text = this.username;
            string mensaje = "ActiveGames/";
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);

            userSocket.Send(msg);
        }

        private void UserAttendServer()
        {

            string mensaje = $"User/{username}";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            userSocket.Send(msg);
            while (true)
            {
                //Recibimos mensaje del servidor
                msg = new byte[80];
                userSocket.Receive(msg);
                string[] serverResponse = Encoding.ASCII.GetString(msg).Split('/');
                string game_name;
                switch (serverResponse[0])
                {
                    case "Create":
                        switch (Convert.ToInt32(serverResponse[1]))
                        {
                            case -1:
                                MessageBox.Show("Juego ya existente, cambia el nombre");
                                break;
                            case 0:
                                MessageBox.Show("El servidor no soporta mas juegos");
                                break;
                            case 1:
                                Game game;
                                game_name = GamenameBox.Text;
                                ThreadStart ts = delegate {
                                    game = Create_GameForm(game_name);
                                    game.DeclareHost();
                                    gameForms.Add(game_name, game);
                                    gameForms[game_name].ShowDialog();
                                    gameForms.Remove(game_name);
                                };
                                Thread T = new Thread(ts);
                                T.Start();
                                this.Invoke(new Action(() =>
                                {
                                    GamenameBox.Text = "";
                                    PasswordBox.Text = "";
                                }));
                                break;
                        }
                        this.Invoke(new Action(() =>
                        {
                            EnterGameButton.Enabled = true;
                        }));
                        break;

                    case "Join":
                        switch (Convert.ToInt32(serverResponse[1]))
                        {
                            case -1:
                                MessageBox.Show("Lobby No existente, cree uno nuevo");
                                break;
                            case 0:
                                MessageBox.Show("Wrong Password");
                                break;
                            case 1:
                                Game game;
                                game_name = GamenameBox.Text;

                                ThreadStart ts = delegate { 
                                    game = Create_GameForm(game_name);
                                    gameForms.Add(game_name, game);
                                    gameForms[game_name].ShowDialog();
                                    gameForms.Remove(game_name);
                                };
                                Thread T = new Thread(ts);
                                T.Start();
                                this.Invoke(new Action(() =>
                                {
                                    GamesGrid.ClearSelection();
                                    BackButton.Visible = false;
                                    GameEnterLabel.Text = "CREATE GAME";
                                    GamenameBox.Text = "";
                                    PasswordBox.Text = "";
                                    GamenameBox.Enabled = true;
                                    EnterGameButton.Text = "Create";
                                }));

                                break;
                        }
                        this.Invoke(new Action(() =>
                        {
                            EnterGameButton.Enabled = true;
                        }));
                        break;

                    case "ActiveGames":
                        this.Invoke(new Action(() =>
                        {
                            GamesGrid.Rows.Clear();
                            var GameBoard = serverResponse[1].Split('|')
                                .Select(t => t.Trim())
                                .Select(t =>
                                {
                                    var part = t.Split(',');

                                    return new { name = part[0], numPlayers = int.Parse(part[1]), state = int.Parse(part[2]) };

                                })
                                .Where(game => game != null)
                                .OrderByDescending(game => gameForms.ContainsKey(game.name)) // Ordenar primero los que están en "PLAYING"
                                .ThenBy(game => game.state) // Luego "JOIN" (estado 0) y finalmente "ONGOING" (estado 1)
                                .ToList();

                            string status = "";
                            Color backColor = Color.White;
                            Color foreColor = Color.Black;
                            foreach (var game in GameBoard)
                            {
                                if (gameForms.ContainsKey(game.name))
                                {
                                    status = "PLAYING";
                                    backColor = Color.SkyBlue;
                                    foreColor = Color.Black;
                                }
                                else if (game.state == 0) // Si es 0, mostrar botón con "Join" y color de fondo
                                {
                                    status = "JOIN";
                                    backColor = Color.White;
                                    foreColor = Color.Black;
                                }
                                
                                else if (game.state == 1) // Si es 0, mostrar botón con "Join" y color de fondo
                                {
                                    status = "ONGOING";
                                    backColor = Color.Maroon;
                                    foreColor = Color.White;
                                }
                                int rowIndex = GamesGrid.Rows.Add(game.name, game.numPlayers, status);
                                GamesGrid.Rows[rowIndex].DefaultCellStyle.BackColor = backColor;
                                GamesGrid.Rows[rowIndex].DefaultCellStyle.ForeColor = foreColor;
                                if (status != "JOIN")
                                {
                                    GamesGrid.Rows[rowIndex].DefaultCellStyle.SelectionBackColor = backColor;
                                    GamesGrid.Rows[rowIndex].DefaultCellStyle.SelectionForeColor = foreColor;
                                }
                                
                            }
                        }));
                        GamesGrid.ClearSelection();
                        break;


                    case "Game":
                        game_name = serverResponse[1];
                        string game_request = string.Join("/", serverResponse.Skip(2).Take(serverResponse.Length - 2));
                        this.Invoke(new Action(() =>
                        {
                            gameForms[game_name].GameResponses(game_request);
                        }));
                        break;

                }
            }
        }


        private Game Create_GameForm(string game_name)
        {
            Game game = new Game(username, game_name, userSocket);
            return game;
        }

        private void EnterGameButton_Click(object sender, EventArgs e)
        {
            if (GamenameBox.Text == "" || PasswordBox.Text == "") return;

            byte[] msg = System.Text.Encoding.ASCII.GetBytes("");
            if (EnterGameButton.Text == "Create")
            {
                string mensaje = "Create/" + GamenameBox.Text + "," + PasswordBox.Text;
                // Enviamos al servidor el nombre tecleado
                msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            }
            else if (EnterGameButton.Text == "Join")
            {
                string mensaje = "Join/" + GamenameBox.Text + "," + PasswordBox.Text;
                // Enviamos al servidor el nombre tecleado
                msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            }
            EnterGameButton.Enabled = false;

            userSocket.Send(msg);
        }

        private void LogOut()
        {
            string mensaje = "LogOut/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            userSocket.Send(msg);

            // Nos desconectamos
            userThread.Abort();
            userSocket.Shutdown(SocketShutdown.Both);
            userSocket.Close();
            this.Close();
        }
        private void UserLobby_Closed(object sender, FormClosedEventArgs e)
        {
            LogOut();
        }
        private void LogOutButton_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GamesGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex >=0 && GamesGrid.Rows[rowIndex].Cells["State"].Value.ToString() == "JOIN" )
            {
                BackButton.Visible = true;
                GameEnterLabel.Text = "JOIN GAME";
                GamenameBox.Text = GamesGrid.Rows[rowIndex].Cells["Game"].Value.ToString();
                GamenameBox.Enabled = false;
                EnterGameButton.Text = "Join";
            }
            else if (GameEnterLabel.Text == "JOIN GAME")
            {
                BackButton.Visible = false;
                GameEnterLabel.Text = "CREATE GAME";
                GamenameBox.Text = "";
                GamenameBox.Enabled = true;
                EnterGameButton.Text = "Create";
            }
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            GamesGrid.ClearSelection();
            BackButton.Visible = false;
            GameEnterLabel.Text = "CREATE GAME";
            GamenameBox.Text = "";
            GamenameBox.Enabled = true;
            EnterGameButton.Text = "Create";
        }
    }
}
