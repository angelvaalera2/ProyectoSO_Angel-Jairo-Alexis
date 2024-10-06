//STANDARD LIBS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

//MYSQL LIBS
#include <mysql.h>
#include <mysql/mysql.h>
//#include <my_global.h> necesario Shiva2

//SERVER LIBS
#include <unistd.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <fcntl.h>
#include <netinet/in.h>
#include <pthread.h>

#define MAX_STRING 256
#define MAX_NAME 16
#define MAX_PASSWORD 32
#define MAX_QUERY 256


#define MAX_ROW 32
#define MAX_COLUMN 8


//Estructura necesaria para acceso excluyente
pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;
int contador;
int i;
int sockets[100];



// QUERY FUNCTIONS
typedef struct {
    char data[MAX_ROW][MAX_COLUMN][MAX_STRING];  // Almacenar resultados
    int rows;  // Número de filas devueltas
    int cols;  // Número de columnas por fila
} SelectResult;

SelectResult SelectQuery(MYSQL *conn, char query[MAX_QUERY]) {
    SelectResult result;
    result.rows = 0;
    result.cols = 0;

    int err = mysql_query(conn, query);
    if (err != 0) {
        printf("Error al ejecutar el SELECT: %u %s\n", mysql_errno(conn), mysql_error(conn));
        exit(1);
    }

    MYSQL_RES *res = mysql_store_result(conn);
    if (res == NULL) {
        printf("Error al almacenar el resultado del SELECT: %u %s\n", mysql_errno(conn), mysql_error(conn));
        exit(1);
		
    }

    int num_fields = mysql_num_fields(res);
    if (num_fields > MAX_COLUMN) {
        printf("Advertencia: el número de columnas supera el límite de %d\n", MAX_COLUMN);
        num_fields = MAX_COLUMN;  // Limitar el número de columnas al máximo definido
    }
    result.cols = num_fields;

    MYSQL_ROW row;
    int num_rows = mysql_num_rows(res);
    if (num_rows > MAX_ROW) {
        printf("Advertencia: el número de filas supera el límite de %d\n", MAX_ROW);
        num_rows = MAX_ROW;  // Limitar el número de filas al máximo definido
    }
    result.rows = num_rows;

    // Llenar la estructura con los resultados
    int i = 0;
    while ((row = mysql_fetch_row(res)) && i < MAX_ROW) {
        for (int j = 0; j < num_fields; j++) {
            if (row[j]) {
                strncpy(result.data[i][j], row[j], MAX_STRING - 1);
         
            } else {
                result.data[i][j][0] = '\0';  // Si el campo es NULL, se guarda como cadena vacía
            }
        }
        i++;
    }

    mysql_free_result(res); // Liberar el resultado del SELECT
    return result;
}

void InsertQuery(MYSQL *conn, char query[MAX_QUERY]) {
    int err = mysql_query(conn, query);
    if (err != 0) {
        printf("Error al ejecutar el INSERT: %u %s\n", mysql_errno(conn), mysql_error(conn));
        exit(1);
    }
}

int DeleteQuery(MYSQL *conn, char query[MAX_QUERY]) {
    int err = mysql_query(conn, query);
    if (err != 0) {
        printf("Error al ejecutar el DELETE: %u %s\n", mysql_errno(conn), mysql_error(conn));
        exit(1);
    }
}

// DB REQUESTS

int CreateUser(MYSQL *conn, char user_name [MAX_NAME], char password [MAX_PASSWORD])
{
	char query [MAX_QUERY];
	sprintf(query, "SELECT user_name FROM Users WHERE user_name='%s';", user_name);
	SelectResult result = SelectQuery(conn, query);
	if (result.rows > 0) {
		return 0; // Usuario ya existe
	}
	else
	{
		sprintf(query, "INSERT INTO Users (user_name, password) VALUES ('%s', '%s');", user_name, password);
		InsertQuery(conn, query);
		return 1; // Usuario creado
	}
}

int LoginUser(MYSQL *conn, char user_name [MAX_NAME], char password [MAX_PASSWORD])
{
	char query [MAX_QUERY];
	sprintf(query, "SELECT password FROM Users WHERE user_name='%s';", user_name);
	SelectResult result = SelectQuery(conn, query);
	if (result.rows == 0) {
		return -1; // Usuario no encontrado
	}
	else if (strcmp(result.data[0][0], password) != 0)
	{
		return 0; // Password incorrecto
	}
	else
	{
		return 1; // Password correcto
	}
}

void DeleteUser(MYSQL *conn, char user_name [MAX_NAME])
{
	char query [MAX_QUERY];
	sprintf(query, "DELETE FROM Users WHERE user_name='%s';", user_name);
	DeleteQuery(conn, query);
}

int GetUserScore(MYSQL *conn, char user_name [MAX_NAME]) 
{	
	char query [MAX_QUERY];
	sprintf(query, "SELECT score FROM Users WHERE user_name='%s';", user_name);
	SelectResult result = SelectQuery(conn, query);
	if (result.rows == 0) {
		return -1; // Usuario no encontrado
	}
	else
	{
		return atoi(result.data[0][0]);
	}
}

SelectResult GetLeaderBoard(MYSQL *conn)
{
	char query [MAX_QUERY];
	sprintf(query, "SELECT user_name, score FROM Users ORDER BY score DESC;");
	return SelectQuery(conn, query);
}



int CreateGame(MYSQL *conn, char game_name [MAX_NAME], char password [MAX_PASSWORD])
{
	char query [MAX_QUERY];
	sprintf(query, "SELECT game_name FROM Games WHERE game_name='%s';", game_name);
	SelectResult result = SelectQuery(conn, query);
	if (result.rows > 0) {
		return 0; // Game ya existe
	}
	else
	{
		sprintf(query, "INSERT INTO Games (game_name, password) VALUES ('%s', '%s');", game_name, password);
		InsertQuery(conn, query);
		return 1; // Game creado
	}
}

void DeleteGame(MYSQL *conn, char game_name [MAX_NAME])
{
	char query [MAX_QUERY];
	sprintf(query, "DELETE FROM Games WHERE game_name='%s';", game_name);
	DeleteQuery(conn, query);
	sprintf(query, "DELETE FROM Players WHERE id_game IN (SELECT id FROM Games WHERE game_name='%s');", game_name);
	DeleteQuery(conn, query);
}

int JoinGame(MYSQL *conn, char user_name [MAX_NAME], char game_name [MAX_NAME], char password [MAX_PASSWORD])
{
	char query [MAX_QUERY];
	sprintf(query, "SELECT password FROM Games WHERE game_name='%s';", game_name);
	SelectResult result = SelectQuery(conn, query);
	if (result.rows == 0) {
		return -1; // Lobby no encontrado
	}
	else if (strcmp(result.data[0][0], password) != 0)
	{
		return 0; // Password incorrecto
	}
	else
	{
        sprintf(query, "INSERT INTO Players (id_user, id_game) SELECT Users.id, Games.id FROM Users, Games WHERE Users.user_name='%s' AND Games.game_name='%s';", user_name, game_name);
        InsertQuery(conn, query);
		return 1; // User joined Game
	}
}

void LeaveGame(MYSQL *conn, char user_name [MAX_NAME], char game_name [MAX_NAME])
{
	char query [MAX_QUERY];
	sprintf(query, "DELETE FROM Players WHERE id_user IN (SELECT id FROM Users WHERE user_name='%s') AND id_game IN (SELECT id FROM Games WHERE game_name='%s');", user_name, game_name);
	DeleteQuery(conn, query);
}

SelectResult GetGamePlayers(MYSQL *conn, char game_name [MAX_NAME])
{
	char query [MAX_QUERY];
	sprintf(query, "SELECT Users.user_name, Users.score FROM Users, Players, Games WHERE Users.id=Players.id_user AND Games.id=Players.id_game AND Games.game_name='%s'; ORDER BY Users.score DESC;", game_name);
	return SelectQuery(conn, query);
}


//Client Requests
void UsersRequest(MYSQL *conn, char param[512],char response [512])
{
	SelectResult result;	
	char *user_name;
	char *password;
	char *u = strtok(param, "/");
	int code = atoi (u);
	switch (code) {
		
		case -1: //Delete Users
			user_name = strtok(NULL, "\0");
			DeleteUser(conn, user_name);
			response = "1/-1/1";
			break;
		case 0: //Update User
				
			break;
		case 1: //Create User
			user_name = strtok(NULL, ",");
			password = strtok(NULL, "\0");
			sprintf(response, "1/1/%d",CreateUser(conn, user_name, password));
			break;
		case 2: //Info User
			u = strtok(NULL, "/");
			code = atoi(u);
			switch (code)
			{
				case 0: //Login 
					user_name = strtok(NULL, ",");
					password = strtok(NULL, "\0");
					sprintf(response, "1/2/0/%d", LoginUser(conn, user_name, password));
					break;
				case 1: //Check Score
					user_name = strtok(NULL, "\0");
					int score = GetUserScore(conn, user_name);
					if (score == -1)
					{
						sprintf(response, "1/2/1/-1");
					}
					else
					{
						sprintf(response, "1/2/1/%d", score);
					}
					break;

				case 2: //LeaderBoard
					printf("S\n");
					result= GetLeaderBoard(conn);
					strcpy(response, "1/2/2/");

					for (int i = 0; i < result.rows-1; i++)
					{
						sprintf(u, "%s %s,", result.data[i][0], result.data[i][1]);
						strcat(response, u); 
					}
					sprintf(u, "%s %s", result.data[result.rows-1][0], result.data[result.rows-1][1]); 
					strcat(response, u);  
					break;
				default:
					printf("Request not understood1\n");
					exit(1);
			}
			break;

		// Puedes agregar tantos casos como necesites
		default:
			printf("Request not understood2\n");
	}
}
void GamesRequest(MYSQL *conn, char param [512],char response [512])
{
	SelectResult result;
	char *user_name;
	char *game_name;
	char *password;
	
	char *g = strtok(param, "/");
	int code = atoi (g);
	switch (atoi(strtok(NULL, "/"))) {
		case -1: //Delete Game
			game_name = strtok(NULL, ",");
			DeleteGame(conn, game_name);
			response ="2/-1/1";
			break;
		case 0: //Update Game
			
			break;
		case 1: //Create Game
			game_name = strtok(NULL, ",");
			password = strtok(NULL, ",");
			sprintf(response, "2/1/%d",CreateGame(conn, game_name, password));
			break;
		case 2: //Info Game
			g = strtok(param, "/");
			code = atoi (g);
			switch (atoi(strtok(NULL, "/")))
			{
				case 0: //Join Game
					game_name = strtok(NULL, ",");
					password = strtok(NULL, "\0");
					sprintf(response, "2/2/0/%d", JoinGame(conn, user_name, game_name, password));
					break;
				case 1: //Leave Game
					user_name = strtok(NULL, ",");
					game_name = strtok(NULL, "\0");
					LeaveGame(conn, user_name, game_name);
					response = "2/2/1/1";
					break;
				case 2:
					game_name = strtok(NULL, ",");
					result = GetGamePlayers(conn, game_name);
					response = "2/2/2/";
					for (int i = 0; i < result.rows-1; i++)
					{
						sprintf(password, "%s %s,", result.data[i][0], result.data[i][1]);
						strcat(response, password);  
						
					}
					sprintf(password, "%s %s", result.data[result.rows-1][0], result.data[result.rows-1][1]);
					strcat(response, password);  
					break;


				default:
					printf("Request not understood");
					exit(1);
			}
				

			// Puedes agregar tantos casos como necesites
		default:
			printf("Request not understood");
	}
}

typedef struct {
	void *socket;
	MYSQL *conn;
} ThreadArgs;

void *AtenderCliente (void *args)
{
	ThreadArgs *threadArgs = (ThreadArgs *)args;
	
	//MYSQL *conn = ;
	int sock_conn;
	int *s;
	s= (int *) threadArgs->socket;;
	sock_conn= *s;
	char request[512];
	char response[512];
	int ret;
	

	int terminar =0;
	// Entramos en un bucle para atender todas las peticiones de este cliente
	//hasta que se desconecte
	while (terminar ==0)
	{
		// Ahora recibimos la petici?n
		ret=read(sock_conn,request, sizeof(request));
		printf ("Recibido\n");
		
		// Tenemos que a?adirle la marca de fin de string 
		// para que no escriba lo que hay despues en el buffer
		request[ret]='\0';
		printf ("Peticion: %s\n",request);
		
		char *p = strtok(request, "/");
		int codigo = atoi (p);
		
		if (codigo ==0)
			terminar = 1;
		else if (codigo ==1) //Users Request
		{
			printf("codigo %d\n", codigo);
			p = strtok(NULL, "\0");
			printf("User: %s\n",p);
			UsersRequest(threadArgs->conn, p, response);
			printf ("Respuesta: %s\n", response);
		}
			
		else if (codigo == 2)//Games Request
		{
			printf ("Peticion: %s\n",request);
			p = strtok(NULL, "\0");
			printf("Game: %s\n",p);
			GamesRequest(threadArgs->conn, p, response);
			printf ("Respuesta: %s\n", response);
		}
			
		// Puedes agregar tantos casos como necesites
		write (sock_conn,response, strlen(response));
	}
	// Se acabo el servicio para este cliente
	close(sock_conn); 
	
}







int main(int argc, char *argv[]) {
	//CONEXION CON SQL
	MYSQL *conn;
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	conn = mysql_real_connect (conn, "localhost","root", "mysql", "BoomWords",0, NULL, 0);
	if (conn==NULL) {
		printf ("Error al logear %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}

	//LAUNCH SERVER
	int sock_conn, sock_listen;
	struct sockaddr_in serv_adr;
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
		printf("Error creant socket");

	memset(&serv_adr, 0, sizeof(serv_adr));
	serv_adr.sin_family = AF_INET;
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	serv_adr.sin_port = htons(9050);
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		printf ("Error al bind");
	if (listen(sock_listen, 3) < 0)
		printf("Error en el Listen");
	
	pthread_t thread;
	//START LISTENING
	i = 0;
	contador = 0;
	for(;;){
		printf ("Waiting Request...\n");
		sock_conn = accept(sock_listen, NULL, NULL);
		printf("Connected\n");
		sockets[i] =sock_conn;
		ThreadArgs *args = malloc(sizeof(ThreadArgs));
		args->socket = &sockets[i];
		args->conn = conn;
		pthread_create (&thread, NULL, AtenderCliente, args);
		i++;
	}
	
	
	return 0;
}

