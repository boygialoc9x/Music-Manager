CREATE DATABASE v1_MusicDatabase;

USE v1_MusicDatabase;

DROP TABLE customer;

CREATE TABLE customer
(
	userId INT,
	user_name CHAR(50) NOT NULL,
    password VARCHAR(50) NOT NULL, 
    premium BOOl default 0,
    staff BOOL default 0,
    gmail VARCHAR(70),
    accountStatus bool default 1,
    firstName CHAR(20),
    lastName CHAR(20),
    
	PRIMARY KEY(userId)
);

DROP TABLE songs;

CREATE TABLE songs
(
	songId INT NOT NULL,
    songName CHAR(150),
    length INT,
    lyric TEXT,
    downloadLink TEXT,
    songStatus bool,
    rateAdv FLOAT,
    
    PRIMARY KEY(songId)
);

DROP TABLE rateNcmtSong;

CREATE TABLE rateNcmtSong
(
	userId INT,
    songId INT,
    cmt TEXT,
    rate INT,
    
    FOREIGN KEY(userId) REFERENCES customer(userId),
    FOREIGN KEY(songId) REFERENCES songs(songId),
    PRIMARY KEY(userId,songId)
);

DROP TABLE songReport;

CREATE TABLE songReport
(
	songId INT,
    rpCount INT,
    rpCode INT,
    
    FOREIGN KEY(songId) REFERENCES songs(songId)
);

DROP TABLE artists;

CREATE TABLE artists
(
	artistId INT NOT NULL,
	artistFirstName CHAR(50),
    artistLastName CHAR(50),
    theArtist VARCHAR(50),
    artistStatus bool default 1,
    
    PRIMARY KEY(artistId)
);

DROP TABLE artists_song;

CREATE TABLE artists_song
(
	artistId INT,
	songId INT,
    
    FOREIGN KEY(artistId) REFERENCES artists(artistId),
    FOREIGN KEY(songId) REFERENCES songs(songId),
    PRIMARY KEY(artistId,songId)
);

DROP TABLE categories;

CREATE TABLE categories
(
	categoryId INT NOT NULL,
	categoryName CHAR(50),
    categoryStatus bool default 1,
    
    PRIMARY KEY(categoryId)
);

DROP TABLE categories_song;

CREATE TABLE categories_song
(
	categoryId INT,
	songId INT,
    
    FOREIGN KEY(categoryId) REFERENCES categories(categoryId),
    FOREIGN KEY(songId) REFERENCES songs(songId),
    PRIMARY KEY(categoryId,songId)
);

DROP TABLE artist_categories;

CREATE TABLE artist_categories
(
	categoryId INT NOT NULL,
	artistId INT,
    
    FOREIGN KEY(categoryId) REFERENCES categories(categoryId),
    FOREIGN KEY(artistId) REFERENCES artists(artistId),
    PRIMARY KEY(categoryId,artistId)
);

DROP TABLE playlist;

CREATE TABLE playlist
(
	playlistId INT NOT NULL,
    playlistTitle VARCHAR(150),
    userId INT,
    playlistStatus bool,
    createDate DATE,
    
    PRIMARY KEY(playlistId),
    FOREIGN KEY(userId) REFERENCES customer(userId)
);

DROP TABLE playlist_song;

CREATE TABLE playlist_song
(
	playlistId INT,
    songId INT,
    
    FOREIGN KEY(playlistId) REFERENCES playlist(playlistId),
    FOREIGN KEY(songId) REFERENCES songs(songId),
    PRIMARY KEY(playlistId,songId)
);

DROP TABLE premium_manager;

CREATE TABLE premium_manager
(
	userId INT,
    from_date DATE,
    to_date DATE,
    
    FOREIGN KEY(userId) REFERENCES customer(userId),
    PRIMARY KEY(userId)
);

DROP TABLE orders;

CREATE TABLE orders
(
	orderId INT,
    orderDate DATE,
    userId INT,
    paymentMethod INT,
    total DECIMAL,
    
    PRIMARY KEY(orderId),
    FOREIGN KEY(userId) REFERENCES customer(userId)
);

INSERT INTO customer VALUES (1, 'boygialoc9x', 'sonzai123',1,0,'crackchuoi999@gmail.com',1,'Son','Nguyen');
SELECT * FROM customer;

INSERT INTO songs VALUES (1,'KILL YOURSELF PART III', 123, "", " ",1,0);
SELECT * FROM songs;

UPDATE songs
SET lyric ="It's not fair, I found love", songName = 'LMAO'
WHERE songId= 1;

INSERT INTO categories VALUES (1, 'Country', 1);
INSERT INTO categories VALUES (2, 'EDM', 1);
INSERT INTO categories VALUES (3, 'Hip-hop', 1);
INSERT INTO categories VALUES (4, 'Indie Rock', 1);
INSERT INTO categories VALUES (5, 'Jazz', 1);
INSERT INTO categories VALUES (6, 'Metal', 1);
INSERT INTO categories VALUES (7, 'Oldies', 1);
INSERT INTO categories VALUES (8, 'Pop', 1);
INSERT INTO categories VALUES (9, 'RnB', 1);
INSERT INTO categories VALUES (10, 'Rock', 1);
INSERT INTO categories VALUES (11, 'Techno', 1);
INSERT INTO categories VALUES (12, 'Rap', 1);
SELECT * FROM categories;

INSERT INTO categories_song VALUES (12,1);
INSERT INTO categories_song VALUES (3,1);
INSERT INTO categories_song VALUES (6,1);
SELECT * FROM categories_song;

SELECT s.songName, c.categoryName
FROM songs s INNER JOIN categories_song cs ON s.songId = cs.songId
			 INNER JOIN categories c ON cs.categoryId = c.categoryId
WHERE s.songId = 1;

SELECT * FROM songs;

SELECT user_name
FROM customer
WHERE user_name = "boyg";