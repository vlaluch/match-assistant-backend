CREATE DATABASE IF NOT EXISTS `match_assistant` /*!40100 DEFAULT CHARACTER SET utf8 COLLATE utf8_unicode_ci */;
USE `match_assistant`;

CREATE TABLE IF NOT EXISTS `games` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(50) NOT NULL,
  `Date` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=39 DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `participant_states` (
  `Id` int(11) NOT NULL,
  `Name` varchar(10) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `game_participants` (
  `GameId` int(11) NOT NULL,
  `Name` varchar(50) NOT NULL,
  `StateId` int(11) NOT NULL,
  `Count` int(4) NOT NULL,
  KEY `game_participants_GameFK` (`GameId`),
  KEY `game_participants_StateFK` (`StateId`),
  CONSTRAINT `game_participants_GameFK` FOREIGN KEY (`GameId`) REFERENCES `games` (`Id`),
  CONSTRAINT `game_participants_StateFK` FOREIGN KEY (`StateId`) REFERENCES `participant_states` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `users` (
  `Id` int(11) NOT NULL,
  `Name` varchar(50) NOT NULL,
  `UserName` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `chats` (
  `Id` bigint(30) NOT NULL,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `users_chats` (
  `UserId` int(11) NOT NULL,
  `ChatId` bigint(30) NOT NULL,
  PRIMARY KEY (`UserId`,`ChatId`),
  KEY `users_chats_UserFK` (`UserId`),
  KEY `users_chats_ChatFK` (`ChatId`),
  CONSTRAINT `users_chats_ChatFK` FOREIGN KEY (`ChatId`) REFERENCES `chats` (`Id`),
  CONSTRAINT `users_chats_UserFK` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `player_positions` (
  `Id` int(11) NOT NULL,
  `Name` varchar(15) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `players` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `Rating` int(11) DEFAULT NULL,
  `PositionId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `players_PositionFK` (`PositionId`),
  CONSTRAINT `players_PositionFK` FOREIGN KEY (`PositionId`) REFERENCES `player_positions` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
