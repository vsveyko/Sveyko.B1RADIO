CREATE DATABASE B1RADIO;
GO

USE B1RADIO;
GO

CREATE TABLE GENRE (
    ID int NOT NULL IDENTITY,
    NAME nvarchar(150) NOT NULL,
    CONSTRAINT PK_GENRE PRIMARY KEY (ID)
);
GO

CREATE TABLE SINGER (
    ID int NOT NULL IDENTITY,
    NAME nvarchar(150) NOT NULL,
    CONSTRAINT PK_SINGER PRIMARY KEY (ID)
);
GO

CREATE TABLE SOUNDTRACK (
    ID int NOT NULL IDENTITY,
    GENRE_ID int NOT NULL,
	SINGER_ID int NOT NULL,
    TITLE nvarchar(max) NOT NULL,
    FILEPATH nvarchar(max) NOT NULL,
    CONSTRAINT PK_SOUNDTRACK PRIMARY KEY (ID),
    CONSTRAINT FK_SOUNDTRACK_GENRE_ID FOREIGN KEY (GENRE_ID) REFERENCES GENRE (ID),
	CONSTRAINT FK_SOUNDTRACK_SINGER_ID FOREIGN KEY (SINGER_ID) REFERENCES SINGER (ID)
);
GO

INSERT INTO GENRE (NAME) VALUES ('Pop');
INSERT INTO GENRE (NAME) VALUES ('Rock');
INSERT INTO GENRE (NAME) VALUES ('Dance');
INSERT INTO GENRE (NAME) VALUES ('Latin');

