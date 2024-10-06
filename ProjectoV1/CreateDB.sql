DROP DATABASE IF EXISTS BoomWords;
CREATE DATABASE BoomWords;
USE BoomWords;

CREATE TABLE Users (
	id INT AUTO_INCREMENT,
	user_name VARCHAR(255),
	password VARCHAR(255),
	score INT DEFAULT 0,
	PRIMARY KEY(id)
);

CREATE TABLE Games (
	id INT AUTO_INCREMENT,
	lobby_name VARCHAR(255),
	password VARCHAR(255),
	PRIMARY KEY(id)
);

CREATE TABLE Players (
	id_user INT,
	id_game INT,
	FOREIGN KEY (id_user) REFERENCES Users(id),
	FOREIGN KEY (id_game) REFERENCES Games(id)
);

INSERT INTO Users (user_name, password, score) VALUES 
('player1', 'pass123', 100),
('player2', 'secretpass', 150),
('player3', 'mypassword', 200);

INSERT INTO Games (lobby_name, password) VALUES 
('Lobby A', 'lobbyPass1'),
('Lobby B', 'lobbyPass2'),
('Lobby C', 'lobbyPass3');

INSERT INTO Players (id_user, id_game) VALUES 
(1, 1),
(2, 1),
(3, 2);
