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
	game_name VARCHAR(255),
	password VARCHAR(255),
	playing int DEFAULT 0,
	PRIMARY KEY(id)
);

CREATE TABLE Players (
	id_user INT,
	id_game INT,
	lives INT DEFAULT 0,
	FOREIGN KEY (id_user) REFERENCES Users(id),
	FOREIGN KEY (id_game) REFERENCES Games(id)
);

CREATE TABLE Dictionary (
	word VARCHAR(255)
);

INSERT INTO Users (user_name, password, score) VALUES 
('alexis', 'a', 100),
('angel', 'a', 150),
('jairo', 'j', 200);

LOAD DATA LOCAL INFILE 'Dictionary.txt'
INTO TABLE Dictionary
LINES TERMINATED BY '\r\n'
(word);

