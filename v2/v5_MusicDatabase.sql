CREATE DATABASE v5_MusicDatabase;

USE v5_MusicDatabase;

DROP TABLE customer;

CREATE TABLE customer
(
	userId INT NOT NULL AUTO_INCREMENT,
	firstName CHAR(20),
    lastName CHAR(20),
	user_name CHAR(50) NOT NULL,
    password VARCHAR(50) NOT NULL, 
    premium BOOl default 0,
    staff BOOL default 0,
    gmail VARCHAR(70),
    playlistCreated INT default 0,
    accountStatus bool default 1,
    
	PRIMARY KEY(userId,user_name)
);

DROP TABLE songs;

CREATE TABLE songs
(
	songId INT NOT NULL AUTO_INCREMENT,
    songName CHAR(150),
    length INT,
    lyric VARCHAR(5000),
    downloadLink VARCHAR(5000),
    songStatus BOOL default 1,
    rateAdv FLOAT default 0,
    
    PRIMARY KEY(songId)
);

DROP TABLE artists;

CREATE TABLE artists
(
	artistId INT NOT NULL AUTO_INCREMENT,
	artistFirstName CHAR(50),
    artistLastName CHAR(50),
    theArtist VARCHAR(50),
    born DATE,
    artistStatus bool default 1,
    
    PRIMARY KEY(artistId)
);

SELECT theArtist FROM artists WHERE theArtist = '$uicideboy$';

DROP TABLE artists_song;

CREATE TABLE artists_song
(
	artistId INT,
	songId INT,
    artistSongStatus BOOL,
    
    FOREIGN KEY(artistId) REFERENCES artists(artistId),
    FOREIGN KEY(songId) REFERENCES songs(songId),
    PRIMARY KEY(artistId,songId)
);

DROP TABLE categories;

CREATE TABLE categories
(
	categoryId INT NOT NULL AUTO_INCREMENT,
	categoryName CHAR(50),
    categoryStatus bool default 1,
    
    PRIMARY KEY(categoryId)
);

DROP TABLE categories_song;

CREATE TABLE categories_song
(
	categoryId INT,
	songId INT,
    categorySongStatus BOOL,
    
    FOREIGN KEY(categoryId) REFERENCES categories(categoryId),
    FOREIGN KEY(songId) REFERENCES songs(songId),
    PRIMARY KEY(categoryId,songId)
);

DROP TABLE playlist;

CREATE TABLE playlist
(
	playlistId INT NOT NULL AUTO_INCREMENT,
    playlistTitle VARCHAR(150),
    userId INT,
    playlistStatus bool default 1,
    createDate DATE,
    
    PRIMARY KEY(playlistId),
    FOREIGN KEY(userId) REFERENCES customer(userId)
);

DROP TABLE playlist_song;

CREATE TABLE playlist_song
(
	playlistId INT,
    songId INT,
    playlistSongStatus BOOL,
    
    FOREIGN KEY(playlistId) REFERENCES playlist(playlistId),
    FOREIGN KEY(songId) REFERENCES songs(songId),
    PRIMARY KEY(playlistId,songId)
);

DELIMITER //
CREATE PROCEDURE remove_allSongs_and_playlist_and_update_playlistCreated(IN plId INT, uId INT)
BEGIN
	UPDATE playlist SET playlistStatus = false WHERE userId = uId AND playlistId = plId;
    UPDATE playlist_song SET playlistSongStatus = false WHERE playlistId = plId;
    UPDATE customer SET playlistCreated = playlistCreated - 1 WHERE userId = uId;
END //
DELIMITER ;

DELIMITER //
CREATE PROCEDURE recycle_removedPlaylist(IN plId INT, uId INT, plTitle VARCHAR(150), cd DATE)
BEGIN
	UPDATE playlist SET playlistStatus = true, playlistTitle = plTitle, createDate = cd WHERE userId = uId AND playlistId = plId;
    UPDATE customer set playlistCreated = playlistCreated + 1 where userId = uId;
END //
DELIMITER ;



#--------------------------------- NOT USE YET -----------------------------------#


DROP TABLE premium_manager;
# No need now
CREATE TABLE premium_manager
(
	userId INT,
    from_date DATE,
    to_date DATE,
    
    FOREIGN KEY(userId) REFERENCES customer(userId),
    PRIMARY KEY(userId)
);

DROP TABLE orders;
# No need now
CREATE TABLE orders
(
	orderId INT AUTO_INCREMENT,
    orderDate DATE,
    userId INT,
    paymentMethod INT,
    total DECIMAL,
    orderStatus bool default 1,
    
    PRIMARY KEY(orderId),
    FOREIGN KEY(userId) REFERENCES customer(userId)
);

CREATE TABLE helperGmail
(
	gmail VARCHAR(300),
    password VARCHAR(300)
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

DROP TABLE rateNcmtSong;

CREATE TABLE rateNcmtSong
(
	userId INT,
    songId INT,
    cmt VARCHAR(5000),
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
    
    FOREIGN KEY(songId) REFERENCES songs(songId),
    PRIMARY KEY(songId)
);

#--------------------------------- NOT USE YET -----------------------------------#

INSERT INTO helperGmail VALUES ('7z6mSGXEaQbxsx1ahUPyYTBosLUwtHFIKX5I\hPI9J0=', 'l7EJk8rP3+SA84lzJSzCqogyVzn0VNYq+GVqlGuc6RA=');
SELECT * FROM helperGmail;
INSERT INTO customer (user_name, firstName, lastName, password, premium, staff, gmail) VALUES ('user', 'Son','Nguyen','sonzai123', false, false, 'crackchuoi999@gmail.com');
INSERT INTO customer (user_name, firstName, lastName, password, premium, staff, gmail) VALUES ('preuser', 'Son','Nguyen','sonzai123', true, false, 'tson.25.8.2001@gmail.com');
INSERT INTO customer (user_name, firstName, lastName, password, premium, staff, gmail) VALUES ('admin', 'Son','Nguyen','sonzai123', true, true, '');
SELECT * FROM customer;

UPDATE customer SET password = "pas123" WHERE userId = 9;

SELECT * FROM songs;

UPDATE songs
SET lyric ="It's not fair, I found love", songName = 'LMAO'
WHERE songId= 1;

update songs SET songStatus = true;
update categories set categoryName = "The New Test Unit 1" WHERE categoryId = 20;

INSERT INTO songs (songName, length, lyric, downloadLink, songStatus) VALUES ('KILL YOURSELF PART I', 177," "," ",1);
INSERT INTO songs (songName, length, lyric, downloadLink, songStatus) VALUES ('KILL YOURSELF PART II', 157," "," ",1);
INSERT INTO songs (songName, length, lyric, downloadLink, songStatus) VALUES ('KILL YOURSELF PART II', 147," "," ",1);
INSERT INTO songs (songName, length, lyric, downloadLink, songStatus) VALUES ('KILL YOURSELF PART IV', 114," "," ",1);
INSERT INTO songs (songName, length, lyric, downloadLink, songStatus) VALUES ('My Flaws Burn Through My Skin Like Demonic Flames From Hell', 167," "," ",1);
INSERT INTO songs (songName, length, lyric, downloadLink, songStatus) VALUES ('Paris', 107," "," ",1);
INSERT INTO songs (songName, length, lyric, downloadLink, songStatus) VALUES ('Aria Math (Metal Cover)', 493," "," ",1);
INSERT INTO songs (songName, length, lyric, downloadLink, songStatus) VALUES ('Aria Math (SynthWave)', 313," "," ",0);
INSERT INTO songs (songName, length, lyric, downloadLink, songStatus) VALUES ('Gimme Gimme Gimme', 210," "," ",1);
INSERT INTO songs (songName, length, lyric, downloadLink, songStatus) VALUES ('Gimme Love', 226," "," ",1);
INSERT INTO songs (songName, length, lyric, downloadLink, songStatus) VALUES ('Slow Dancin in the Dark', 218," "," ",1);
INSERT INTO songs (songName, length, lyric, downloadLink, songStatus) VALUES ('YEAH RIGHT', 175," "," ",1);
INSERT INTO songs (songName, length, lyric, downloadLink, songStatus) VALUES ('Kiss me more', 208," "," ",1);

INSERT INTO categories (categoryName) VALUES ('Country');
INSERT INTO categories (categoryName) VALUES ('EDM');
INSERT INTO categories (categoryName) VALUES ('Hip-hop');
INSERT INTO categories (categoryName) VALUES ('Indie Rock');
INSERT INTO categories (categoryName) VALUES ('Jazz');
INSERT INTO categories (categoryName) VALUES ('Metal');
INSERT INTO categories (categoryName) VALUES ('Oldies');
INSERT INTO categories (categoryName) VALUES ('Pop');
INSERT INTO categories (categoryName) VALUES ('RnB');
INSERT INTO categories (categoryName) VALUES ('Rock');
INSERT INTO categories (categoryName) VALUES ('Techno');
INSERT INTO categories (categoryName) VALUES ('Rap');

INSERT INTO artists (artistFirstName, artistLastName, theArtist, born) VALUES ('Ruby', 'da Cherry', '$uicideboy$', '1990/04/22');
INSERT INTO artists (artistFirstName, artistLastName, theArtist, born) VALUES ('Scrim', '', '$uicideboy$', '1989/04/11');
INSERT INTO artists (artistFirstName, artistLastName, theArtist, born) VALUES ('Miller', 'George Kusunoki', 'Joji', '1992/09/18');
INSERT INTO artists (artistFirstName, artistLastName, theArtist, born) VALUES ('Daniel', 'Rosenfeld', 'C418', '1989/05/9');
INSERT INTO artists (artistFirstName, artistLastName, theArtist, born) VALUES ('Amalaratna', 'Zandile Dlamini', 'Doja Cat', '1995/10/21');
INSERT INTO artists (artistFirstName, artistLastName, theArtist, born) VALUES ('Solána', 'Imani Rowe', 'SZA', '1990/11/8');

Select * from customer where userId = 2;
update customer set playlistCreated = 2 where userId = 2;
select * from artists;
select * from songs;
select * from users;
select * from playlist_song where playlistId = 6;
INSERT INTO playlist (playlistTitle, userId, createDate) VALUES ('Test Play List', 3 ,'2021/10/13');
Update customer set playlistCreated = playlistCreated + 1 where userId = 3;

DROP PROCEDURE insert_playlist_and_update_playlistCreated;

DELIMITER //
CREATE PROCEDURE insert_playlist_and_update_playlistCreated(IN plTitle VARCHAR(255), uId INT, cDate DATE)
BEGIN
	INSERT INTO playlist (playlistTitle, userId, createDate) VALUES (plTitle, uId ,cDate);
    Update customer set playlistCreated = playlistCreated + 1 where userId = uId;
END //
DELIMITER ;

CALL insert_playlist_and_update_playlistCreated('My new playlist', 1, '2021/10/15');

INSERT INTO playlist_song VALUES(3,1,true);
INSERT INTO playlist_song VALUES(3,2,true);
INSERT INTO playlist_song VALUES(3,3,true);
INSERT INTO playlist_song VALUES(3,4,true);
INSERT INTO playlist_song VALUES(3,7,true);
INSERT INTO playlist_song VALUES(3,8,true);
INSERT INTO playlist_song VALUES(3,9,true);
INSERT INTO playlist_song VALUES(3,15,true);

Update playlist_song set playlistSongStatus = false where playlistId = 1 AND songId = 3;

select * from playlist where userId = 1;
select * from playlist_song where playlistId = 2;

select * from playlist_song where playlistId = 2 AND songId = 3;

SELECT playlistSongStatus FROM playlist_song WHERE playlistId = 3 AND songId = 12;

UPDATE playlist_song SET playlistSongStatus = false WHERE songId = 3;

UPDATE playlist SET playlistTitle = "Updated" WHERE playlistId = 3 AND userId = 1;

UPDATE playlist SET playlistStatus = false WHERE playlistId = 3 AND userId = 1;

SELECT s.songName, s.length, s.songId, s.lyric, s.downloadLink, s.rateAdv, s.songStatus, pls.playlistSongStatus
FROM playlist pl 
INNER JOIN playlist_song pls ON pl.playlistId = pls.playlistId 
INNER JOIN songs s ON pls.songId = s.songId 
WHERE pl.playlistId = 3 AND pls.playlistSongStatus = true
OR pl.playlistId = 4 AND pls.playlistSongStatus = false;

SELECT pl.userId, s.songId, s.songName, s.length, s.songStatus, pls.playlistSongStatus
FROM playlist pl 
INNER JOIN playlist_song pls ON pl.playlistId = pls.playlistId 
INNER JOIN songs s ON pls.songId = s.songId 
WHERE pl.playlistId = 3;

SELECT * FROM playlist WHERE userId = 3 AND playlistStatus = true;
select * from playlist_song where playlistId = 5;
DROP PROCEDURE remove_playlist_and_update_playlistCreated;

DELIMITER //
CREATE PROCEDURE remove_allSongs_and_playlist_and_update_playlistCreated(IN plId INT, uId INT)
BEGIN
	UPDATE playlist SET playlistStatus = false WHERE userId = uId AND playlistId = plId;
    UPDATE playlist_song SET playlistSongStatus = false WHERE playlistId = plId;
    UPDATE customer SET playlistCreated = playlistCreated - 1 WHERE userId = uId;
END //
DELIMITER ;

Call remove_allSongs_and_playlist_and_update_playlistCreated(1, 3);

Select * from customer where userId = 1;

DELIMITER //
CREATE PROCEDURE recycle_removedPlaylist(IN plId INT, uId INT, plTitle VARCHAR(150), cd DATE)
BEGIN
	UPDATE playlist SET playlistStatus = true, playlistTitle = plTitle, createDate = cd WHERE userId = uId AND playlistId = plId;
    UPDATE customer set playlistCreated = playlistCreated + 1 where userId = uId;
END //
DELIMITER ;

CALL recycle_removedPlaylist();

UPDATE playlist SET playlistStatus = true WHERE userId = 1 AND playlistId = 3;
Update customer set playlistCreated = playlistCreated + 1 where userId = 1;
Update playlist_song set playlistSongStatus = false where playlistId = 3;

SELECT s.songName, s.length, s.songId, s.lyric, s.downloadLink, s.songStatus 
FROM pl.playlistId INNER JOIN songs s ON pls.songId = s.songId WHERE pl.playlistId = 1 AND pls.playlistSongStatus = true;

INSERT INTO categories_song VALUES (12,1,true);
INSERT INTO categories_song VALUES (3,1, true);
INSERT INTO categories_song VALUES (6,1, true);

INSERT INTO categories_song VALUES (8,13, true);
INSERT INTO categories_song VALUES (9,13, true);

INSERT INTO artists_song VALUES (1,1,true);
INSERT INTO artists_song VALUES (2,1,true);

INSERT INTO artists_song VALUES (5,13,true);
INSERT INTO artists_song VALUES (6,13,true);

SELECT * FROM categories;
SELECT * FROM songs;
SELECT * FROM artists;
SELECT * FROM categories_song;
SELECT * FROM artists_song;
SELECT * FROM playlist_song;
SELECT * FROM playlist;
SELECT * FROM customer;
update artists set artistFirstName = "Ulvaeus", artistLastName = "Bjorn", born = "1945/4/25" where artistId = 22;
SELECT * FROM artists_song WHERE songId = 14;
SELECT a.artistId, a.theArtist, a.artistFirstName, a.artistLastName, a.born, a.artistStatus FROM songs s INNER JOIN artists_song ats ON s.songId = ats.songId INNER JOIN artists a ON a.artistId = ats.artistId WHERE s.songId = 14 AND ats.artistSongStatus = false;

UPDATE artists SET born = '1994-12-25' WHERE artistId = 14 ;

UPDATE customer SET premium = false WHERE userId = 6;

SELECT s.songName, a.theArtist
FROM songs s INNER JOIN artists_song ats ON s.songId = ats.songId
			 INNER JOIN artists a ON a.artistId = ats.artistId
WHERE s.songId = 1;

SELECT s.songName, c.categoryName
FROM songs s INNER JOIN categories_song cs ON s.songId = cs.songId
			 INNER JOIN categories c ON cs.categoryId = c.categoryId
WHERE s.songId = 1;

UPDATE songs SET songName = 'KILL YOURSELF PART III' WHERE songId = 4;
UPDATE songs SET songName = 'Hihi', length = 100 WHERE songId = 16;

SELECT * FROM songs;

SELECT user_name
FROM customer
WHERE user_name = "boyg";

SELECT *
FROM customer
WHERE user_name = "boygialoc9x";

SELECT * FROM customer WHERE user_name = 'user';
SELECT * FROM artists WHERE artistId = 1;

SELECT * FROM songs;
SELECT songId, songName, length, lyric, downloadLink, songStatus FROM songs;

SELECT a.artistId, a.theArtist FROM songs s INNER JOIN artists_song ats ON s.songId = ats.songId INNER JOIN artists a ON a.artistId = ats.artistId WHERE s.songId = 13 AND ats.artistSongStatus = true;

SELECT MAX(songId) FROM songs;

UPDATE customer SET user_name = "newuser" WHERE userId = 9;
UPDATE customer SET password = "testunit123" WHERE userId = 1;
UPDATE songs
SET downloadLink = "https://drive.google.com/uc?id=1EudBUxwEFu0YtlBMtZaG9NDtKYeXoMl_&export=download"
WHERE songId = 3;

UPDATE songs
SET lyric = "It's not fair, I found love
It made me say, Now get back
You'll never
See daylight if I'm not strong
It just might
It's not fair, I found love
It made me say, Now get back
You'll never
See daylight if I'm not strong
It just might
They figure me a dead motherfucker
But I'm just a motherfucker that want to be dead
Snow Leopard with the lead in his head
Turning me into a sweater
Bitches use me as their fucking bedspread
I be the silhouette of a sunset
Smoke a cigarette while I compress my depression
Stare into the violet fluorescent lights makes me violent
I'm trying to get the highest I can get before I overdose and die
My ribs are nothing but an empty cage
Black hole in my chest
Big bang
Yung Plague on the tip of a wave
In my head, I feel like I'm a guest so I'ma throw it all away
Because when I am dead I will be nothing decomposing in a grave
I'm matter but I don't matter
I can feel my skull shatter from the dull chatter
Brain splattered on the wall
Grey stains won't dissolve
Gonna have to paint it all
It's not fair, I found love
It made me say, Now get back
You'll never
See daylight if I'm not strong
It just might
Always boasting my emotions
On how I'm so fucking broken
Think I'm joking when I'm talking
About blowing my head open
'Til the moment you walk in
And find my body motionless
Wrists slit
Thoughts of Slicky falling in an open pit, shit
Always burn my bridges
'Cause I'd rather fall in ditches
If life's a game of inches
Then my dick has been the biggest
And my goal's to fuck the world
Until that motherfucker's twitching
Lane switchin', same mission
To die and blame my addiction, bitch
It's not fair, I found love
It made me say, Now get back
You'll never
See daylight if I'm not strong
It just might"
WHERE songId = 3;

SELECT * FROM artists WHERE theArtist = '$uicideboy$';

SELECT * FROM customer;