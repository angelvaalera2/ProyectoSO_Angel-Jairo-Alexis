using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;


namespace WindowsFormsApplication1
{
    public partial class Launcher : Form
    {
        Socket server;
        Thread atender;

        delegate void DelegadoParaPonerTexto(string texto);


        public Launcher()
        {
            InitializeComponent();
            //CheckForIllegalCrossThreadCalls = false; //Necesario para que los elementos de los formularios puedan ser
            //accedidos desde threads diferentes a los que los crearon
        }

        private void Form1_Load(object sender, EventArgs e)
        {

           
        }



        private void AtenderServidor()
        {
            while (true)
            {
                //Recibimos mensaje del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                string[] serverResponse = Encoding.ASCII.GetString(msg2).Split('/');
                switch (Convert.ToInt32(serverResponse[0]))
                {
                    case 1:
                        switch (Convert.ToInt32(serverResponse[1]))
                        {
                            case -1:
                                MessageBox.Show("Usuario borrado");
                                User_Box.Enabled = true;
                                Password_Box.Enabled = true;
                                LogIn_Button.Text = "Log In";
                                break;
                            case 0:
                                break;
                            case 1:
                                switch (Convert.ToInt32(serverResponse[2]))
                                {
                                    case 0:
                                        MessageBox.Show("Usuario ya existente");
                                        break;
                                    case 1:
                                        MessageBox.Show("Usuario creado correctamente");
                                        break;
                                }
                                break;
                            case 2:
                                switch (Convert.ToInt32(serverResponse[2]))
                                {
                                    case 0:
                                        switch (Convert.ToInt32(serverResponse[3]))
                                        {
                                            case -1:
                                                MessageBox.Show("Usuario no existente");
                                                break;
                                            case 0:
                                                MessageBox.Show("Wrong Password");
                                                break;
                                            case 1:
                                                MessageBox.Show("Usuario logeado correctamente");
                                                //UserLog_Button.Invoke(new Action(() => UserLog_Button.Text = "Log Out"));

                                                break;
                                        }
                                        break;
                                    case 1:
                                        MessageBox.Show("Usuario borrado correctamente");
                                        break;
                                    case 2:
                                        MessageBox.Show("LeaderBoard:\n" + serverResponse[3].Replace(",", "\n"));
                                        break;

                                }
                                break;

                        }
                        break;


                    case 2:
                        switch (Convert.ToInt32(serverResponse[1]))
                        {
                            case 0:
                                MessageBox.Show("Usuario o contraseña incorrectos");
                                break;
                            case 1:
                                MessageBox.Show("Usuario logeado correctamente");
                                User_Box.Enabled = false;
                                Password_Box.Enabled = false;
                                LogIn_Button.Visible = false;
                                LogOut_Button.Visible = true;

                                break;
                        }
                        break;
                }


            }
        }


                    


        private void button1_Click(object sender, EventArgs e)
        {
            //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
            //al que deseamos conectarnos
            IPAddress direc = IPAddress.Parse("192.168.56.102");
            IPEndPoint ipep = new IPEndPoint(direc, 9050);
            

            //Creamos el socket 
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep);//Intentamos conectar el socket
                this.BackColor = Color.Green;
                MessageBox.Show("Conectado");
                //pongo en marcha el thread que atenderá los mensajes del servidor
                ThreadStart ts = delegate { AtenderServidor(); };
                atender = new Thread(ts);
                atender.Start();

            }
            catch (SocketException ex)
            {
                //Si hay excepcion imprimimos error y salimos del programa con return 
                MessageBox.Show("No he podido conectar con el servidor");
                return;
            }

          

        }

     
        private void button3_Click(object sender, EventArgs e)
        {
            //Mensaje de desconexión
            string mensaje = "0/";
        
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            // Nos desconectamos
            atender.Abort();
            this.BackColor = Color.Gray;
            server.Shutdown(SocketShutdown.Both);
            server.Close();


        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Mensaje de desconexión
            string mensaje = "0/";

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            // Nos desconectamos
            atender.Abort();
            this.BackColor = Color.Gray;
            server.Shutdown(SocketShutdown.Both);
            server.Close();

        }


        private void LogIn_Button_Click(object sender, EventArgs e)
        {
            if (LogIn_Button.Text == "Log In")
            {
                string mensaje = "1/2/0/" + User_Box.Text + "," + Password_Box.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);

                server.Send(msg);
            }
            else
            {
                User_Box.Enabled = true;
                Password_Box.Enabled = true;
            }

        }

        private void LogOut_Button_Click(object sender, EventArgs e)
        {
            User_Box.Enabled = true;
            Password_Box.Enabled = true;
            LogOut_Button.Visible = false;

        }

        private void LeaderBoard_Button_Click(object sender, EventArgs e)
        {
            string mensaje = "1/2/2";
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);

            server.Send(msg);
        }

        private void UserSign_Click(object sender, EventArgs e)
        {
            string mensaje = "1/1/" + User_Box.Text + "," + Password_Box.Text;
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);

            server.Send(msg);

        }
    }
}
