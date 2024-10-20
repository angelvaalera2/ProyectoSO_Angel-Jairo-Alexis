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
        string ip = "192.168.56.105";
        string port = "9050";
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
                                MessageBox.Show("Lobby ya existente, cambia el nombre");
                                break;
                            case 0:
                                MessageBox.Show("El servidor no soporta mas juegos");
                                break;
                            case 1:
                                MessageBox.Show("Lobby creado correctamente");
                               
                                Game game;
                                game_name = CreateGamenameBox.Text;
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
                                    CreateGamenameBox.Text = "";
                                    JoinPasswordBox.Text = "";
                                }));
                                
                                break;
                        }
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
                                game_name = JoinGamenameBox.Text;
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
                                    JoinGamenameBox.Text = "";
                                    JoinPasswordBox.Text = "";
                                }));
                                break;

                        }
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

        private void LogOutButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private Game Create_GameForm(string game_name)
        {

            MessageBox.Show("Game logeado correctamente");
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(ip), Convert.ToInt32(port));

            Game game = new Game(username, game_name, userSocket);
            
            return game;
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            string mensaje = "Create/" + CreateGamenameBox.Text + "," + CreatePasswordBox.Text;
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);

            userSocket.Send(msg);
        }

        private void JoinButton_Click(object sender, EventArgs e)
        {
            string mensaje = "Join/" + JoinGamenameBox.Text + "," + JoinPasswordBox.Text;
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);

            userSocket.Send(msg);
        }
    }
}
