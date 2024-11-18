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
#include <unistd.h>

#define MAX_STRING 32
#define MAX_NAME 16
#define MAX_PASSWORD 32
#define MAX_QUERY 500
#define MAX_SYL 4

#define MAX_ROW 32
#define MAX_COLUMN 8

#define MAX_GAME 10

// STRUCTS
typedef struct {
    char data[MAX_ROW][MAX_COLUMN][MAX_STRING];  // Almacenar resultados
    int rows;  // Número de filas devueltas
    int cols;  // Número de columnas por fila
} SelectResult;

typedef struct {
	char user_name [MAX_NAME];
	int socket;
} UserSocket;

typedef struct {
	UserSocket userSocket [100];
	int total;
} OnlineUsers;

typedef struct Game {
    char game_name[MAX_NAME];
	char host[MAX_NAME];
	char turn_user[MAX_NAME];
	char syllable[MAX_NAME];
    char request_queue[MAX_ROW][MAX_QUERY]; // Cola de peticiones como una lista de cadenas
    int queue_size; // Tamaño actual de la cola
    pthread_mutex_t queue_lock; // Mutex para la cola
    pthread_cond_t queue_cond; // Condición para la cola
} Game;

typedef struct {
	Game gameList[MAX_GAME];
	int total;
} CurrentGames;

//GLOBAL VARIABLES
MYSQL *conn;
//Estructura necesaria para acceso excluyente
pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;
int contador;
int i = 0;
int sockets[256];

OnlineUsers onlineUsers;
CurrentGames currentGames;

int FindUserSocket (char user_name [MAX_NAME])
{
	for (int i = 0; i < onlineUsers.total; i++)
	{
		if (strcmp(onlineUsers.userSocket[i].user_name, user_name) == 0)
		{
			
			return onlineUsers.userSocket[i].socket; 
		}
	}
	return -1;
}

void AddOnlineUser (char user_name [MAX_NAME], int sock_conn)
{
	strcpy(onlineUsers.userSocket[onlineUsers.total].user_name, user_name);
	onlineUsers.userSocket[onlineUsers.total].socket = sock_conn;
	onlineUsers.total++;
}

void DeleteOnlineUser (char user_name [MAX_NAME])
{
	int i = 0;
	while (i < onlineUsers.total && strcmp(onlineUsers.userSocket[i].user_name, user_name) != 0)
	{
		i++;
	}
	for (int j = i; j < onlineUsers.total-1; j++)
	{
		onlineUsers.userSocket[j] = onlineUsers.userSocket[j+1];
		
	}
	onlineUsers.total--;
}

void ResponseUser(char user_name [MAX_NAME], char response [MAX_QUERY])
{
	int sock_conn = FindUserSocket(user_name);
	int ret = write(sock_conn, response, strlen(response));
	if (ret < 0)
	{
		printf("%s-RES not processed: %s\n", user_name, response);
	}
	printf("%s-RES -> %s\n", user_name, response);
}


SelectResult SelectQuery(char query[MAX_QUERY]) {
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

void InsertQuery(char query[MAX_QUERY]) {
    int err = mysql_query(conn, query);
    if (err != 0) {
        printf("Error al ejecutar el INSERT: %u %s\n", mysql_errno(conn), mysql_error(conn));
        exit(1);
    }
}

int DeleteQuery(char query[MAX_QUERY]) {
    int err = mysql_query(conn, query);
    if (err != 0) {
        printf("Error al ejecutar el DELETE: %u %s\n", mysql_errno(conn), mysql_error(conn));
        exit(1);
    }
}

// DB REQUESTS

int CreateUser(char user_name [MAX_NAME], char password [MAX_PASSWORD])
{
	char query [MAX_QUERY];
	sprintf(query, "SELECT user_name FROM Users WHERE user_name='%s';", user_name);
	SelectResult result = SelectQuery(query);
	if (result.rows > 0) {
		return 0; // Usuario ya existe
	}
	else
	{
		sprintf(query, "INSERT INTO Users (user_name, password) VALUES ('%s', '%s');", user_name, password);
		InsertQuery(query);
		return 1; // Usuario creado
	}
}

int LoginUser(char user_name [MAX_NAME], char password [MAX_PASSWORD])
{
	char query [MAX_QUERY];
	sprintf(query, "SELECT password FROM Users WHERE user_name='%s';", user_name);
	SelectResult result = SelectQuery(query);
	if (result.rows == 0) {
		return -1; // Usuario no encontrado
	}
	else if (strcmp(result.data[0][0], password) != 0)
	{
		return 0; // Password incorrecto
	}
	else
	{
		if (FindUserSocket(user_name) != -1)
			return 2;	// Usuario ya conectado
		else if (onlineUsers.total >= 100)
			return 3;	// Servidor lleno
		else
		{
			return 1; // Usuario conectado
		}
			
	}
}

void DeleteUser(char user_name [MAX_NAME])
{
	char query [MAX_QUERY];
	sprintf(query, "DELETE FROM Users WHERE user_name='%s';", user_name);
	DeleteQuery(query);
}

int GetUserScore(char user_name [MAX_NAME]) 
{	
	char query [MAX_QUERY];
	sprintf(query, "SELECT score FROM Users WHERE user_name='%s';", user_name);
	SelectResult result = SelectQuery(query);
	if (result.rows == 0) {
		return -1; // Usuario no encontrado
	}
	else
	{
		return atoi(result.data[0][0]);
	}
}

int GetLeaderBoard(char response [MAX_QUERY])
{
	char query [MAX_QUERY];
	sprintf(query, "SELECT user_name, score FROM Users ORDER BY score DESC;");
	SelectResult result= SelectQuery(query);
	int count = result.rows;

	if (count == 0)
	{
		strcpy(response, "");
		return count;
	}
	
	strcpy(response, "LeaderBoard/");
	
	for (int i = 0; i < result.rows-1; i++)
	{
		sprintf(response, "%s%s,%s|",response, result.data[i][0], result.data[i][1]);
	}

	sprintf(response, "%s%s,%s", response,result.data[result.rows-1][0], result.data[result.rows-1][1]);

	return count;
}

int GetAllGames(char report [MAX_QUERY])
{
	char query [MAX_QUERY];
	sprintf(query, "SELECT Games.game_name, COUNT(Players.id_user), Games.playing FROM Games LEFT JOIN Players ON Games.id = Players.id_game GROUP BY Games.id;");
	SelectResult result = SelectQuery(query);
	int count = result.rows;
	if (count == 0)
	{
		strcpy(report, "");
		return count;
	}

	strcpy(report, "ActiveGames/");
	for (int i = 0; i < result.rows-1; i++)
	{
		sprintf(report, "%s%s,%s,%s|", report, result.data[i][0], result.data[i][1], result.data[i][2]);
	}
	if (result.rows > 0)
		sprintf(report, "%s%s,%s,%s", report, result.data[result.rows-1][0], result.data[result.rows-1][1], result.data[result.rows-1][2]);
	return count;
}



int CreateGame(char user_name [MAX_NAME], char game_name [MAX_NAME], char password [MAX_PASSWORD])
{
	char query [MAX_QUERY];
	sprintf(query, "SELECT game_name FROM Games WHERE game_name='%s';", game_name);
	SelectResult result = SelectQuery(query);
	if (result.rows > 0) 
		return -1; // Game ya existe
	else if (currentGames.total == MAX_NAME)
		return 0; // No hay espacio para más juegos
	else
	{
		sprintf(query, "INSERT INTO Games (game_name, password) VALUES ('%s', '%s');", game_name, password);
		InsertQuery(query);
		sprintf(query, "INSERT INTO Players (id_user, id_game) SELECT Users.id, Games.id FROM Users, Games WHERE Users.user_name='%s' AND Games.game_name='%s';", user_name, game_name);
		InsertQuery(query);

		strcpy(currentGames.gameList[currentGames.total].game_name, game_name);
		strcpy(currentGames.gameList[currentGames.total].host, user_name);
		pthread_mutex_init(&currentGames.gameList[currentGames.total].queue_lock, NULL);
		pthread_cond_init(&currentGames.gameList[currentGames.total].queue_cond, NULL); 
		currentGames.gameList[currentGames.total].queue_size = 0;
		currentGames.total++;
		return 1; // Game creado
	}
}

void DeleteGame(char game_name [MAX_NAME])
{
	char query [MAX_QUERY];
	sprintf(query, "DELETE FROM Games WHERE game_name='%s';", game_name);
	DeleteQuery(query);

	for (int i = 0; i < currentGames.total; i++)
	{
		if (strcmp(currentGames.gameList[i].game_name, game_name) == 0)
		{
			for (int j = i; j < currentGames.total-1; j++)
			{
				currentGames.gameList[j] = currentGames.gameList[j+1];
			}
			currentGames.total--;
			break;
		}
	}
}

int JoinGame(char user_name [MAX_NAME], char game_name [MAX_NAME], char password [MAX_PASSWORD])
{
	char query [MAX_QUERY];
	 sprintf(query, 
        "SELECT Games.password, Games.playing, "
        "CASE WHEN EXISTS (SELECT 1 FROM Players "
        "JOIN Users ON Players.id_user = Users.id "
        "WHERE Users.user_name = '%s' AND Players.id_game = Games.id) THEN 1 ELSE 0 END AS is_user_in_game "
        "FROM Games "
        "WHERE Games.game_name='%s';", 
        user_name, game_name);
	SelectResult result = SelectQuery(query);
	if (result.rows == 0) {
		return -1; // Lobby no encontrado
	}
	else if (strcmp(result.data[0][0], password) != 0)
	{
		return 0; // Password incorrecto
	}
	else if(strcmp(result.data[0][1], "1") == 0)
	{
		return 2; // Juego en progreso
	}
	else if(strcmp(result.data[0][2], "1")==0)
	{
		return 3; //
	}
	else
	{
        sprintf(query, "INSERT INTO Players (id_user, id_game) SELECT Users.id, Games.id FROM Users, Games WHERE Users.user_name='%s' AND Games.game_name='%s';", user_name, game_name);
        InsertQuery(query);
		return 1; // User joined Game
	}
}

void LeaveGame(char user_name [MAX_NAME], char game_name [MAX_NAME])
{
	char query [MAX_QUERY];
	sprintf(query, "DELETE FROM Players WHERE id_user IN (SELECT id FROM Users WHERE user_name='%s') AND id_game IN (SELECT id FROM Games WHERE game_name='%s');", user_name, game_name);
	DeleteQuery(query);
}

void StartGame(char game_name [MAX_NAME], int startLives)
{
	char query [MAX_QUERY];
	sprintf(query, "UPDATE Games SET playing=1 WHERE game_name='%s';", game_name);
	InsertQuery(query);
	sprintf(query, "UPDATE Players JOIN Games ON Games.id = Players.id_game SET lives=%d WHERE Games.game_name='%s';", startLives, game_name);
	InsertQuery(query);
}
void EndGame(char game_name [MAX_NAME])
{
	char query [MAX_QUERY];
	sprintf(query, "UPDATE Games SET playing=0 WHERE game_name='%s';", game_name);
	InsertQuery(query);
	query [MAX_QUERY];
	sprintf(query, "UPDATE Players JOIN Games ON Games.id = Players.id_game SET lives=0 WHERE Games.game_name='%s';", game_name);
	InsertQuery(query);
}

SelectResult GetGamePlayers(char game_name [MAX_NAME], int lives)
{
	char query [MAX_QUERY];
	sprintf(query, "SELECT Users.user_name, Players.lives, Users.score FROM Users, Players, Games WHERE Users.id=Players.id_user AND Games.id=Players.id_game AND Games.game_name='%s' AND Players.Lives >= %d;", game_name, lives);
	return SelectQuery(query);
}

void AddPlayerRequest(char user_name [MAX_NAME], char game_name [MAX_NAME], char request [MAX_QUERY]) {
	struct Game *game;
	for (int i = 0; i < currentGames.total; i++) {
		
        if (strcmp(currentGames.gameList[i].game_name, game_name) == 0) {
            game = &currentGames.gameList[i];
			break;
		}
    }
	char gameRequest [MAX_QUERY];
	gameRequest[MAX_QUERY-1] = '\0';
	sprintf(gameRequest, "%s/%s", user_name, request);
    if (game != NULL) {
        pthread_mutex_lock(&game->queue_lock);  // Bloquear el mutex

		// Agregar nueva solicitud a la cola
		strcpy(game->request_queue[game->queue_size], gameRequest);
		game->queue_size++;

		pthread_cond_signal(&game->queue_cond);  // Señalar que hay un nuevo mensaje
		pthread_mutex_unlock(&game->queue_lock);  // Desbloquear el mutex
    } 
}

void LoadGamePlayers(char game_name [MAX_NAME], char response[MAX_QUERY])
{
	SelectResult infPlayers = GetGamePlayers(game_name, 0);
	char info [MAX_QUERY] = "";
	char player [MAX_QUERY];
	for (int i = 0; i < infPlayers.rows-1; i++)
	{
		sprintf(player, "%s %d %d,", infPlayers.data[i][0], atoi(infPlayers.data[i][1]), atoi(infPlayers.data[i][2]));  //user_name, lives, score
		strcat(info, player);
	}
	sprintf(player, "%s %d %d", infPlayers.data[infPlayers.rows-1][0], atoi(infPlayers.data[infPlayers.rows-1][1]), atoi(infPlayers.data[infPlayers.rows-1][2]));  //user_name, lives, score
	strcat(info, player);
	strcpy(response, info);
}

void RefreshPlayer(char game_name [MAX_NAME], char user_name [MAX_NAME], char response[MAX_QUERY])
{
	char query [MAX_QUERY];
	sprintf(query, "SELECT Players.lives, Users.score FROM Players JOIN Users ON Users.id = Players.id_user JOIN Games ON Games.id = Players.id_game WHERE  Users.user_name = '%s' AND  Games.game_name='%s';", user_name, game_name);
	SelectResult result = SelectQuery(query);
	sprintf(response, "%s %d %d", user_name, atoi(result.data[0][0]), atoi(result.data[0][1]));
}

void ResponseGamePlayers(char game_name[MAX_NAME], char response [MAX_QUERY])
{
	SelectResult result = GetGamePlayers(game_name, 0);
	char game_response[MAX_QUERY];
	strcpy(game_response, response);
	sprintf(response, "Game/%s/%s", game_name, game_response);
	for (int i = 0; i < result.rows; i++)
	{
		char user_name [MAX_NAME];
		strcpy(user_name, result.data[i][0]);
		ResponseUser(user_name, response);
	}
}

void ResponseAllUsers(char response [MAX_QUERY])
{
	for (int i = 0; i < onlineUsers.total; i++)
	{
		ResponseUser(onlineUsers.userSocket[i].user_name, response);
	}
}
//GAME FUNCTIONS
void GenerateSyllable(char silaba[MAX_NAME], int tipo) 
{
    char consonantes [21] = "bcdfghjklmnpqrstvwxyz"; 
    char vocales [5] = "aeiou";  
    char diptongos[12][2] = {"ai", "ei", "oi", "ui", "au", "eu", "ia", "ie", "io", "ua", "ue", "uo"};  
    char aux[MAX_NAME];
    int count = 0;  
    while (count < 20000)
	{ 
        if (tipo == 0) {  
            char consonante = consonantes[rand() % (sizeof(consonantes) / sizeof(consonantes[0]))];
            char vocal = vocales[rand() % (sizeof(vocales) / sizeof(vocales[0]))];
            sprintf(aux, "%c%c", consonante, vocal);  
		}
        else if (tipo == 1) {
            char vocal = vocales[rand() % (sizeof(vocales) / sizeof(vocales[0]))];
            char consonante = consonantes[rand() % (sizeof(consonantes) / sizeof(consonantes[0]))];
            sprintf(aux, "%c%c", vocal, consonante);  
        }
        else if (tipo == 2) {  
            char consonante1 = consonantes[rand() % (sizeof(consonantes) / sizeof(consonantes[0]))];
            char vocal = vocales[rand() % (sizeof(vocales) / sizeof(vocales[0]))];
            char consonante2 = consonantes[rand() % (sizeof(consonantes) / sizeof(consonantes[0]))];
            sprintf(aux, "%c%c%c", consonante1, vocal, consonante2); 
        }
        else {  
            char *diptongo = diptongos[rand() % (sizeof(diptongos) / sizeof(diptongos[0]))];
            sprintf(aux, "%s", diptongo);  
        }

        char query[MAX_QUERY];
        sprintf(query, "SELECT COUNT(word) FROM Dictionary WHERE word LIKE '%%%s%%';", aux);
        SelectResult result = SelectQuery(query);  
		count = atoi(result.data[0][0]); 
    } 
    sprintf(silaba, "%s", aux);
}

int CheckInDictionary(char user_word[MAX_STRING], char syllable [MAX_STRING])
{
	char query [MAX_QUERY];
	sprintf(query, "SELECT word FROM Dictionary WHERE word='%s';", user_word);
	SelectResult result = SelectQuery(query);
	printf("result %d\n", result.rows);	

	if (strcmp(user_word,syllable) != 0 && strstr(user_word, syllable) != NULL && result.rows > 0) //Si  es mayor que la silaba,  contiene la silaba y esta en el diccionario
		return 1;
	else
	 	return 0;
}	

int GetLives(char user_name [MAX_NAME], char game_name [MAX_NAME])
{
	char query [MAX_QUERY];
	sprintf(query, 
	"SELECT Players.lives FROM Players JOIN Users ON Users.id = Players.id_user	JOIN Games ON Games.id = Players.id_game WHERE  Users.user_name = '%s' AND  Games.game_name='%s';", user_name, game_name);
	SelectResult result = SelectQuery(query);
	return atoi(result.data[0][0]);
}
//Inicia las vidas de cada jugador
void UpdateLive(char user_name [MAX_NAME], char game_name [MAX_NAME], int lives)
{
	char query [MAX_QUERY];
	sprintf(query, 
	"UPDATE Players JOIN Users ON Users.id = Players.id_user JOIN Games ON Games.id = Players.id_game SET Players.lives = %d WHERE  Users.user_name = '%s' AND  Games.game_name='%s';", lives, user_name, game_name);
	InsertQuery(query);
}

int NextTurn(char game_name [MAX_NAME], char turn_user [MAX_NAME])
{
	SelectResult result = GetGamePlayers(game_name,	1);
	if (result.rows == 1)
	{
		strcpy(turn_user, result.data[0][0]);
		return 0;
	}
	else if (strcmp(turn_user, "") == 0)
	{
		strcpy(turn_user, result.data[0][0]);
		return 1;
	}

	int turnPos;
	for (int i = 0; i < result.rows; i++)
	{
		if (strcmp(result.data[i][0], turn_user) == 0)
		{
			if (i == result.rows-1)
				turnPos = 0;
			else
				turnPos = i+1;
			break;
		}
	}
	strcpy(turn_user, result.data[turnPos][0]);
	return 1;
}
void KillAllGames(MYSQL *conn) {
	char query[MAX_QUERY];
	sprintf(query, "SELECT Games.game_name FROM Games;");
	SelectResult result = SelectQuery(query);
	int i = 0;
	while (i<result.rows) {
		char game_name[MAX_NAME] = "";
		strcpy(game_name ,result.data[i][0]);
		DeleteGame(game_name);
		i++;
	}
}
//GAME LOOP
void* GameLoop(void* arg) 
{
	srand(time(NULL));
    char* name = (char*)arg;
	char game_name [MAX_NAME];
	strcpy(game_name, name);

	struct Game *game;
	for (int i = 0; i < currentGames.total; i++) 
	{
        if (strcmp(currentGames.gameList[i].game_name, game_name) == 0) {
            game = &currentGames.gameList[i];
			break;
		}
    }
	
    while (1) {
        pthread_mutex_lock(&game->queue_lock);  // Bloquear el mutex
        while (game->queue_size == 0) {
            pthread_cond_wait(&game->queue_cond, &game->queue_lock);  // Esperar por peticiones
        }
		
        // Procesar la primera solicitud en la cola
		
		if (strchr(game->request_queue[0], '\0') == NULL) {
			printf("Error: request_queue[0] no tiene un terminador nulo\n");
		}


		printf("Game:%s-REQ -> %s\n", game_name, game->request_queue[0]);
		
		char *game_user = strtok(game->request_queue[0], "/");
		char *turn_user = game->turn_user;
		char *syllable = game->syllable;
		char *g = strtok(NULL, "/");
		char response [MAX_QUERY] = "";

        if (strcmp(g, "Start")==0 && strcmp(game->host, game_user) == 0) //Start Game
		{
			StartGame(game_name, 3); //No one can join
			sleep(0.1);
			
			turn_user = "";
			SelectResult gamePlayers = GetGamePlayers(game_name, 0);
			for (int i = 0; i < gamePlayers.rows; i++)
			{
				char refreshInfo [MAX_QUERY];
				char refreshResponse [MAX_QUERY];	
				RefreshPlayer(game_name, gamePlayers.data[i][0], refreshInfo);
				sprintf(refreshResponse, "Refresh/%s", refreshInfo);
				ResponseGamePlayers(game_name, refreshResponse);
			}

			sleep(1); 
			char report [MAX_QUERY];
			if (GetAllGames(report) > 0)
				ResponseAllUsers(report);
		}
		else if (strcmp(g, "Run")==0 && strcmp(game->host, game_user) == 0) 
		{
			printf("User %s\n", turn_user);

			if (NextTurn(game_name, game->turn_user) == 1)
			{
				printf("User %s\n", turn_user);
				
				GenerateSyllable(syllable,0);
				char turnResponse [MAX_QUERY];
				sprintf(turnResponse, "Turn/%s/%s", turn_user, syllable);
				ResponseGamePlayers(game_name, turnResponse);
			}
			else
			{
				sprintf(response, "Win/%s", turn_user);
				ResponseGamePlayers(game_name, response);
				EndGame(game_name);
				
				sleep(1); // Añadir esta línea para esperar 1 segundo
				char playersInfo [MAX_QUERY];
				char loadResponse [MAX_QUERY];
				LoadGamePlayers(game_name, playersInfo); 		//Notifies all players of the new player
				sprintf(loadResponse, "Load/%s", playersInfo);
				ResponseGamePlayers(game_name, loadResponse);

				sleep(1); 
				char report [MAX_QUERY];
				if (GetAllGames(report) > 0)
					ResponseAllUsers(report);
			}
		}
		else if (strcmp(g, "Boom") == 0 && strcmp(game->host, game_user) == 0)
		{
			int turnUser_lives = GetLives(turn_user, game_name)-1;	
			printf("User %s Lives %d\n", turn_user, turnUser_lives);
			UpdateLive(turn_user, game_name, turnUser_lives);
			sprintf(response, "Boom/%s %d", turn_user, turnUser_lives);
			ResponseGamePlayers(game_name, response);
		}
		else if (strcmp(g, "Join")==0)
		{
			char playersInfo [MAX_QUERY];
			LoadGamePlayers(game_name, playersInfo); 		//Notifies all players of the new player
			sprintf(response, "Load/%s", playersInfo);
			ResponseGamePlayers(game_name, response);
		}
		
		else if (strcmp(g, "Leave")==0)
		{
			if (strcmp(game_user, game->host) == 0) //host left --> End Game
			{
				EndGame(game_name);
				DeleteGame(game_name);
				sprintf(response, "End"); 
				ResponseGamePlayers(game_name, response);
			}
			else //Notify all players that a player left
			{
				LeaveGame(game_user, game_name);
				char PlayersInfo [MAX_QUERY];
				LoadGamePlayers(game_name, PlayersInfo);  
				sprintf(response, "Load/%s", PlayersInfo);
				ResponseGamePlayers(game_name, response);
			}
		}
		else if (strcmp(g, "Word")==0)
		{
			char *user_word = strtok(NULL, "/");
			printf("Syll %s\n", syllable);
			if (CheckInDictionary(user_word, syllable) == 1)
			{
				sprintf(response, "Word/%s/1", user_word);
				ResponseGamePlayers(game_name, response);

				sleep(0.5); // Añadir esta línea para esperar 1 segundo

				NextTurn(game_name, turn_user);
				GenerateSyllable(syllable,0);
				char turnResponse [MAX_QUERY];	
				sprintf(turnResponse, "Turn/%s/%s", turn_user, syllable);
				ResponseGamePlayers(game_name, turnResponse);
			}
			else
			{
				sprintf(response, "Word/%s/0", user_word); //Wrong word
				ResponseGamePlayers(game_name, response);
			}
		}

		else
		{
			printf("Game:%s-REQ not processed: %s\n", game_name, g);
		}


		// Siguiente Request
		for (int i = 1; i < game->queue_size; i++) {
            strcpy(game->request_queue[i - 1], game->request_queue[i]);
        }
        game->queue_size--;
        
		pthread_mutex_unlock(&game->queue_lock);  // Desbloquear el mutex
	}

    return NULL;
}


//Client Requests

int LaucherRequest(int sock_conn, char request [MAX_QUERY], char user_name [MAX_NAME])
{
	
	char response [MAX_QUERY] = "";
	char *u = strtok(request, "/");
	
		
	if (strcmp(u, "LogIn")==0)
	{
		char *name = strtok(NULL, ",");
		char *password = strtok(NULL, "\0");
		sprintf(response, "%s/%d", u, LoginUser(name, password));
	}

	else if (strcmp(u, "SignIn")==0)
	{
		char *name = strtok(NULL, ",");
		char *password = strtok(NULL, "\0");
		sprintf(response, "%s/%d", u, CreateUser(name, password));
	}
		
	else if (strcmp(u, "LeaderBoard")==0)
	{	
		GetLeaderBoard(response);	
	}

	else if (strcmp(u, "User")==0) //Convert Launcher into User
	{ 			
		char *name = strtok(NULL, ",");
		AddOnlineUser(name, sock_conn);
		printf("L-ACT -> %s connect me\n", name);
		printf("%s-ACT -> I'm connected\n", name);
		strcpy(user_name, name);
	}

	else if (strcmp(u,"Exit")==0)
	{
		printf("L-ACT -> Disconnected\n");
		return 0;
	}
	else
	{
		printf("L-REQ not processed: %s\n", u);
	}

	if (strcmp(response, "") != 0)
	{
		printf("L-RES -> %s\n", response);
		write(sock_conn, response, strlen(response));
	}
	return 1;
}
int UserRequest(char request [MAX_QUERY], char user_name [MAX_NAME])
{
	char response [MAX_QUERY] = "";
	char *u = strtok(request, "/");
	if (strcmp(u, "Create")==0)
	{
		char *game_name = strtok(NULL, ",");
		char *password = strtok(NULL, ",");
		int result = CreateGame(user_name, game_name, password);
		sprintf(response, "%s/%d", u, result);
		ResponseUser(user_name, response);
		strcpy(response, "");
		sleep(1); // Añadir esta línea para esperar 1 segundo

		if (result == 1)
		{
			pthread_t game_thread;
			pthread_create(&game_thread, NULL, GameLoop, (void*)game_name);
			
			char report [MAX_QUERY];
			if (GetAllGames(report) > 0)
				ResponseAllUsers(report);
		}	
	}

	else if (strcmp(u, "Join")==0)
	{
		char *game_name = strtok(NULL, ",");
		char *password = strtok(NULL, ",");
		int result =  JoinGame(user_name, game_name, password);
		sprintf(response, "%s/%d", u, result);
		ResponseUser(user_name, response);
		strcpy(response, "");
		sleep(1); // Añadir esta línea para esperar 1 segundo

		if (result == 1)
		{
			pthread_t game_thread;
			pthread_create(&game_thread, NULL, GameLoop, (void*)game_name);
			
			char report [MAX_QUERY];
			if (GetAllGames(report) > 0)
				ResponseAllUsers(report);
		}	
	}

	else if (strcmp(u, "Leave")==0)
	{
		char *user_name = strtok(NULL, ",");
		char *game_name = strtok(NULL, "\0");
		LeaveGame(user_name, game_name);
	}

	else if (strcmp(u, "LogOut")==0)
	{
		DeleteOnlineUser(user_name);
		printf("%s-ACT -> Logged out\n", user_name);
		
		return 0;
	}

	else if (strcmp(u, "ActiveGames")==0)
	{
		char report [MAX_QUERY];
		if (GetAllGames(report) > 0)
			ResponseAllUsers(report);

		
	}
	
	else if (strcmp(u, "Game") == 0)
	{
		char *game_name = strtok(NULL, "/");
		char *request = strtok(NULL, "\0");
		AddPlayerRequest(user_name, game_name, request);
	}

	else
	{
		printf("%s-REQ not processed: %s\n", user_name, u);
	}
	
	if (strcmp(response, "") != 0)
	{
		ResponseUser(user_name, response);
	}

	return 1;
}



//ATTEND CLIENT
void *AttendClient (void *socket)
{
	int sock_conn;
	int *s;
	s= (int *) socket;
	sock_conn= *s;

	char request[512];	
	char user_name[MAX_NAME] = "";
	int listen = 1;
	// Entramos en un bucle para atender todas las peticiones de este cliente
	//hasta que se desconecte
	
	while (listen == 1)
	{
		// Ahora recibimos la peticion
		int ret=read(sock_conn,request, sizeof(request));
		request[ret]='\0';
		// Tenemos que a?adirle la marca de fin de string 
		// para que no escriba lo que hay despues en el buffer
		if (strcmp(user_name, "") == 0)
		{
			printf ("\nL-REQ: %s\n",request);
			listen = LaucherRequest(sock_conn, request, user_name);
		}
			
		else //Users Request
		{
			printf ("\n%s-REQ: %s\n", user_name, request);
			listen = UserRequest(request, user_name);
		}
	
	}
	// Se acabo el servicio para este cliente
	close(sock_conn); 
}



////////////////MAIN///////////////////////////




int main(int argc, char *argv[]) {
	//CONEXION CON SQL
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	conn = mysql_real_connect (conn, "shiva2.upc.es","root", "mysql", "T1_BBDDjuego",0, NULL, 0);
	if (conn==NULL) {
		printf ("Error al logear %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	KillAllGames(conn);
	//LAUNCH SERVER
	int sock_conn, sock_listen;
	struct sockaddr_in serv_adr;
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
		printf("Error creant socket");

	memset(&serv_adr, 0, sizeof(serv_adr));
	serv_adr.sin_family = AF_INET;
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	serv_adr.sin_port = htons(50050);
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
	{
		printf ("Error al bind");
		exit(1);
	}
		
	if (listen(sock_listen, 3) < 0)
		printf("Error en el Listen");
	
	pthread_t thread;
	
	//START LISTENING
	contador = 0;
	onlineUsers.total = 0;
	currentGames.total = 0;
	printf("--Server ON--\n");
	for(;;){
		sock_conn = accept(sock_listen, NULL, NULL);		
		sockets[i] =sock_conn;
		pthread_create (&thread, NULL, AttendClient, &sockets[i]);
		pthread_detach(thread); 
		i++;
	}
	return 0;
}

