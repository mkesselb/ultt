-- phpMyAdmin SQL Dump
-- version 4.1.12
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Erstellungszeit: 07. Jan 2015 um 17:14
-- Server Version: 5.6.16
-- PHP-Version: 5.5.11

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Datenbank: `ultt`
--

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `class`
--

CREATE TABLE IF NOT EXISTS `class` (
  `class_id` int(11) NOT NULL AUTO_INCREMENT,
  `classname` varchar(45) NOT NULL,
  `privacy` tinyint(3) DEFAULT NULL,
  `user_id` mediumint(8) NOT NULL COMMENT 'user_id = id of the user, which created the class',
  `school_year` year(4) NOT NULL,
  `classcode` varchar(45) NOT NULL,
  `subject_id` int(11) NOT NULL,
  `deleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`class_id`),
  KEY `fk_class_user1_idx` (`user_id`),
  KEY `fk_class_subject1_idx` (`subject_id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf16 AUTO_INCREMENT=18 ;

--
-- Daten für Tabelle `class`
--

INSERT INTO `class` (`class_id`, `classname`, `privacy`, `user_id`, `school_year`, `classcode`, `subject_id`, `deleted`) VALUES
(1, 'class1', NULL, 1, 2014, 'gqDqOP0tXI', 2, 1),
(2, 'class2', NULL, 1, 2014, '2x5CfT11zp', 3, 1),
(4, 'class1', NULL, 2, 2014, 'KwQidTreYB', 1, 0),
(5, 'new class', NULL, 1, 2014, 'uuNR47hXt2', 2, 1),
(6, 'klasse grg', NULL, 1, 0000, '49WwpOTxvz', 1, 1),
(7, 'klasse grg', NULL, 1, 0000, 'BJ8TVcqjtK', 1, 1),
(8, 'klasse', NULL, 1, 2014, 'TSiVQKE7cw', 1, 0),
(9, 'klasse', NULL, 1, 2014, 'h0dPuUFxeH', 1, 1),
(12, 'class1', NULL, 1, 2015, 'mC2bFn3pj9', 1, 0),
(13, 'class1', NULL, 1, 2015, 'NDxUazjw2t', 1, 1),
(14, 'english class', NULL, 1, 2014, 'KJISzC9o3p', 1, 0),
(15, 'deutsch klasse 1', NULL, 1, 2014, 'V8EjKWBvt6', 3, 0),
(16, 'a', NULL, 1, 2001, '5Yfv0FNgkx', 6, 1),
(17, 'ывавыа', NULL, 1, 0000, 'v1FoQN7iMN', 6, 1);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `class_topic`
--

CREATE TABLE IF NOT EXISTS `class_topic` (
  `class_topic_id` int(11) NOT NULL AUTO_INCREMENT,
  `class_id` int(11) NOT NULL,
  `topic_name` varchar(45) NOT NULL,
  `deleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`class_topic_id`),
  KEY `fk_class_topic_class1_idx` (`class_id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf16 AUTO_INCREMENT=21 ;

--
-- Daten für Tabelle `class_topic`
--

INSERT INTO `class_topic` (`class_topic_id`, `class_id`, `topic_name`, `deleted`) VALUES
(1, 1, 'topic1', 0),
(2, 1, 'topic2', 1),
(3, 1, 'topic3', 0),
(4, 2, 'topic4', 0),
(5, 4, 'topic1', 0),
(6, 4, 'topic12', 1),
(7, 4, 'topic123', 0),
(8, 4, 'topic1234', 0),
(9, 4, 'topic2', 0),
(10, 4, 'topic3', 1),
(11, 5, 'topic1', 0),
(12, 8, '', 1),
(13, 8, '', 1),
(14, 8, '', 1),
(15, 14, 'task topic 1', 0),
(16, 8, '', 1),
(17, 12, '', 0),
(18, 15, '', 0),
(19, 15, '', 1),
(20, 15, '', 1);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `comment`
--

CREATE TABLE IF NOT EXISTS `comment` (
  `comment_id` mediumint(8) NOT NULL AUTO_INCREMENT,
  `message` varchar(255) DEFAULT NULL,
  `created_at` timestamp NULL DEFAULT NULL,
  `user_id` mediumint(8) NOT NULL,
  `task_id` mediumint(8) NOT NULL,
  PRIMARY KEY (`comment_id`),
  KEY `fk_comment_user1_idx` (`user_id`),
  KEY `fk_comment_task1_idx` (`task_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `forum_post`
--

CREATE TABLE IF NOT EXISTS `forum_post` (
  `forum_post_id` int(11) NOT NULL AUTO_INCREMENT,
  `created_at` timestamp NULL DEFAULT NULL,
  `text` mediumtext NOT NULL,
  `forum_thread_id` int(11) NOT NULL,
  `user_id` mediumint(8) NOT NULL,
  PRIMARY KEY (`forum_post_id`),
  KEY `fk_forum_post_forum_thread1_idx` (`forum_thread_id`),
  KEY `fk_forum_post_user1_idx` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf16 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `forum_thread`
--

CREATE TABLE IF NOT EXISTS `forum_thread` (
  `forum_thread_id` int(11) NOT NULL AUTO_INCREMENT,
  `threadname` varchar(45) NOT NULL,
  `created_at` timestamp NULL DEFAULT NULL,
  `user_id` mediumint(8) NOT NULL,
  `class_id` int(11) NOT NULL,
  PRIMARY KEY (`forum_thread_id`),
  KEY `fk_forum_thread_user1_idx` (`user_id`),
  KEY `fk_forum_thread_class1_idx` (`class_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf16 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `linked_entry`
--

CREATE TABLE IF NOT EXISTS `linked_entry` (
  `wiki_entry_id` int(11) NOT NULL,
  `wiki_entry_id1` int(11) NOT NULL,
  PRIMARY KEY (`wiki_entry_id`,`wiki_entry_id1`),
  KEY `fk_wiki_entry_has_wiki_entry_wiki_entry2_idx` (`wiki_entry_id1`),
  KEY `fk_wiki_entry_has_wiki_entry_wiki_entry1_idx` (`wiki_entry_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf16;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `linked_tasks`
--

CREATE TABLE IF NOT EXISTS `linked_tasks` (
  `user_id` mediumint(8) NOT NULL,
  `task_id` mediumint(8) NOT NULL,
  PRIMARY KEY (`user_id`,`task_id`),
  KEY `fk_user_has_task_task1_idx` (`task_id`),
  KEY `fk_user_has_task_user2_idx` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `message`
--

CREATE TABLE IF NOT EXISTS `message` (
  `message_id` mediumint(8) NOT NULL AUTO_INCREMENT,
  `message` varchar(500) NOT NULL,
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `is_read` tinyint(1) NOT NULL DEFAULT '0',
  `is_spam` tinyint(1) NOT NULL DEFAULT '0',
  `to` mediumint(8) DEFAULT NULL,
  `isreply` tinyint(1) DEFAULT '0',
  `user_id` mediumint(8) DEFAULT NULL,
  PRIMARY KEY (`message_id`),
  KEY `fk_message_user` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `notification`
--

CREATE TABLE IF NOT EXISTS `notification` (
  `notification_id` mediumint(8) NOT NULL AUTO_INCREMENT,
  `msg` varchar(255) DEFAULT NULL,
  `type` smallint(5) DEFAULT NULL,
  `privacy` tinyint(3) NOT NULL DEFAULT '0',
  `created_at` timestamp NULL DEFAULT NULL,
  `user_id` mediumint(8) DEFAULT NULL,
  PRIMARY KEY (`notification_id`),
  KEY `fk_activity_user` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `rating`
--

CREATE TABLE IF NOT EXISTS `rating` (
  `rating_id` mediumint(8) NOT NULL AUTO_INCREMENT,
  `flag` tinyint(1) DEFAULT '1',
  `created_at` timestamp NULL DEFAULT NULL,
  `user_id` mediumint(8) NOT NULL,
  `task_id` mediumint(8) NOT NULL,
  PRIMARY KEY (`rating_id`),
  KEY `fk_rating_user1_idx` (`user_id`),
  KEY `fk_rating_task1_idx` (`task_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `subject`
--

CREATE TABLE IF NOT EXISTS `subject` (
  `subject_id` int(11) NOT NULL,
  `subject_name` varchar(45) NOT NULL,
  PRIMARY KEY (`subject_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf16;

--
-- Daten für Tabelle `subject`
--

INSERT INTO `subject` (`subject_id`, `subject_name`) VALUES
(1, 'Informatik'),
(2, 'Mathematik'),
(3, 'Deutsch'),
(4, 'Englisch'),
(5, 'Französisch'),
(6, 'Spanisch'),
(7, 'Biologie'),
(8, 'Geographie');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `task`
--

CREATE TABLE IF NOT EXISTS `task` (
  `task_id` mediumint(8) NOT NULL AUTO_INCREMENT,
  `taskname` varchar(45) NOT NULL,
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `rating` smallint(5) DEFAULT NULL,
  `description` varchar(100) NOT NULL,
  `public` tinyint(1) NOT NULL DEFAULT '0',
  `user_id` mediumint(8) NOT NULL,
  `data_file` longtext NOT NULL,
  `subject_id` int(11) NOT NULL,
  `tasktype_id` int(11) NOT NULL,
  `deleted` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`task_id`),
  KEY `fk_status_reply_user` (`user_id`),
  KEY `fk_task_subject1_idx` (`subject_id`),
  KEY `fk_task_tasktype1_idx` (`tasktype_id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=12 ;

--
-- Daten für Tabelle `task`
--

INSERT INTO `task` (`task_id`, `taskname`, `created_at`, `rating`, `description`, `public`, `user_id`, `data_file`, `subject_id`, `tasktype_id`, `deleted`) VALUES
(1, 'task1', '2015-01-07 13:20:39', NULL, '', 0, 1, '0,n1,m1\n1,n2,m2\n2,ab,cd\n3,n3,m3\n', 1, 1, 0),
(2, 'task1', '2014-12-03 20:49:59', NULL, '', 0, 2, '0,Neue Frage,Neue Antwort,0,Neue Antwort,0,sfsdfadfsdfsdf,0,Neue Antwort,0\n', 3, 3, 0),
(3, 'Taskname', '2014-11-19 21:29:56', NULL, '', 0, 2, '', 1, 1, 0),
(5, 'taskCat', '2014-12-29 10:43:07', NULL, '', 0, 1, '0,a1,aa,aaa\n1,b2,bb,bbb,bbbb\n', 3, 2, 0),
(6, 'taskQuiz', '2014-12-28 12:22:25', NULL, '', 0, 1, '0,,,0,,0,,0\n1,,,0,,0\n', 3, 3, 0),
(7, 'task2', '2014-12-29 21:30:07', NULL, '', 0, 1, '0,frage1,a1-r,1,a2-f,0,a3-r,1\n1,frage2,a2-f,0,aw-r,1,a3-r,0,Neue Antwort4,0\n', 3, 3, 0),
(8, '1task', '2014-12-28 10:44:59', NULL, '', 1, 1, '', 1, 1, 1),
(9, 'biology task', '2014-12-28 12:18:10', NULL, '', 1, 1, '', 7, 2, 0),
(10, 'task kategorie', '2014-12-29 10:30:58', NULL, '', 1, 1, '0,cat1,1,11\n1,cat2,2,22,222\n', 5, 2, 0),
(11, 'deutsch kategorie aufgabe 1', '2014-12-29 21:23:58', NULL, '', 1, 1, '0,schriftsteller,goethe,tolstoi\n1,epochen,barock,romatik,sturm und drang\n', 3, 2, 0);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `tasktype`
--

CREATE TABLE IF NOT EXISTS `tasktype` (
  `tasktype_id` int(11) NOT NULL,
  `type_name` varchar(45) NOT NULL,
  PRIMARY KEY (`tasktype_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf16;

--
-- Daten für Tabelle `tasktype`
--

INSERT INTO `tasktype` (`tasktype_id`, `type_name`) VALUES
(1, 'Zuordnung'),
(2, 'Kategorie'),
(3, 'Quiz'),
(4, 'Sequenz');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `task_for_class`
--

CREATE TABLE IF NOT EXISTS `task_for_class` (
  `class_id` int(11) NOT NULL,
  `task_id` mediumint(8) NOT NULL,
  `assign_time` timestamp NULL DEFAULT NULL,
  `obligatory` tinyint(1) NOT NULL,
  `deadline` datetime NOT NULL COMMENT 'Zeitpunkt bis zu welchem diese Task gemacht werden soll.',
  `max_attempts` smallint(6) NOT NULL COMMENT 'Anzahl der Versuche',
  `task_for_class_id` mediumint(9) NOT NULL AUTO_INCREMENT,
  `class_topic_id` int(11) NOT NULL,
  `deleted` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`task_for_class_id`),
  KEY `fk_class_has_task_task1_idx` (`task_id`),
  KEY `fk_class_has_task_class1_idx` (`class_id`),
  KEY `fk_task_for_class_class_topic1_idx` (`class_topic_id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf16 AUTO_INCREMENT=15 ;

--
-- Daten für Tabelle `task_for_class`
--

INSERT INTO `task_for_class` (`class_id`, `task_id`, `assign_time`, `obligatory`, `deadline`, `max_attempts`, `task_for_class_id`, `class_topic_id`, `deleted`) VALUES
(1, 1, NULL, 0, '2014-11-05 12:13:00', 3, 7, 1, 0),
(8, 6, NULL, 1, '0000-00-00 00:00:00', 1, 8, 13, 0),
(8, 1, NULL, 1, '0000-00-00 00:00:00', 1, 9, 13, 0),
(4, 5, '2014-12-15 23:00:00', 1, '2014-12-25 00:00:00', 10, 10, 5, 0),
(14, 5, '2014-12-15 23:00:00', 1, '2014-12-25 00:00:00', 10, 11, 15, 0),
(15, 11, NULL, 0, '0000-00-00 00:00:00', 1, 12, 18, 0),
(15, 11, NULL, 0, '0000-00-00 00:00:00', 1, 13, 18, 0),
(15, 11, NULL, 0, '0000-00-00 00:00:00', 1, 14, 18, 0);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `user`
--

CREATE TABLE IF NOT EXISTS `user` (
  `user_id` mediumint(8) NOT NULL AUTO_INCREMENT,
  `token` mediumint(5) NOT NULL,
  `username` varchar(45) COLLATE utf8_unicode_ci NOT NULL,
  `password` varchar(45) COLLATE utf8_unicode_ci NOT NULL,
  `name_first` varchar(45) COLLATE utf8_unicode_ci NOT NULL,
  `name_last` varchar(45) COLLATE utf8_unicode_ci NOT NULL,
  `email_id` varchar(100) COLLATE utf8_unicode_ci NOT NULL,
  `picture` varchar(255) COLLATE utf8_unicode_ci DEFAULT '/web/image/default.jpg',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `school_id` varchar(45) COLLATE utf8_unicode_ci NOT NULL,
  PRIMARY KEY (`user_id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci AUTO_INCREMENT=3 ;

--
-- Daten für Tabelle `user`
--

INSERT INTO `user` (`user_id`, `token`, `username`, `password`, `name_first`, `name_last`, `email_id`, `picture`, `created_at`, `school_id`) VALUES
(1, 0, 'pasan', 'password', 'anastasiya', 'pasynkava', 'metal@lika', '/web/image/default.jpg', '2014-10-29 22:17:11', 'htl'),
(2, 0, 'mk', '123456', 'm', 'k', 'm@k', '/web/image/default.jpg', '2014-11-19 20:47:25', 'MK');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `user_fulfill_task`
--

CREATE TABLE IF NOT EXISTS `user_fulfill_task` (
  `user_fulfill_task_id` mediumint(9) NOT NULL AUTO_INCREMENT,
  `user_id` mediumint(8) NOT NULL,
  `fulfill_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `results` mediumtext COLLATE utf8_unicode_ci,
  `task_for_class_id` mediumint(9) NOT NULL,
  PRIMARY KEY (`user_fulfill_task_id`,`user_id`),
  KEY `fk_user_has_task_user1_idx` (`user_id`),
  KEY `fk_user_fulfill_task_task_for_class1_idx` (`task_for_class_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `user_is_in_class`
--

CREATE TABLE IF NOT EXISTS `user_is_in_class` (
  `user_id` mediumint(8) NOT NULL,
  `class_id` int(11) NOT NULL,
  `accepted` tinyint(1) NOT NULL,
  PRIMARY KEY (`user_id`,`class_id`),
  KEY `fk_user_has_class_class1_idx` (`class_id`),
  KEY `fk_user_has_class_user1_idx` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `user_worked_on_wiki_entry`
--

CREATE TABLE IF NOT EXISTS `user_worked_on_wiki_entry` (
  `user_id` mediumint(8) NOT NULL,
  `wiki_entry_id` int(11) NOT NULL,
  `worked_on_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`user_id`,`wiki_entry_id`),
  KEY `fk_user_has_wiki_entry_wiki_entry1_idx` (`wiki_entry_id`),
  KEY `fk_user_has_wiki_entry_user1_idx` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `wiki`
--

CREATE TABLE IF NOT EXISTS `wiki` (
  `wiki_id` int(11) NOT NULL,
  `wikiname` varchar(45) NOT NULL,
  `class_topic_id` int(11) NOT NULL,
  PRIMARY KEY (`wiki_id`),
  KEY `fk_wiki_class_topic1_idx` (`class_topic_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf16;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `wiki_entry`
--

CREATE TABLE IF NOT EXISTS `wiki_entry` (
  `wiki_entry_id` int(11) NOT NULL,
  `wiki_entry_title` varchar(45) NOT NULL,
  `text` longtext NOT NULL,
  `wiki_id` int(11) NOT NULL,
  PRIMARY KEY (`wiki_entry_id`),
  KEY `fk_wiki_entry_wiki1_idx` (`wiki_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf16;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `wiki_entry_picture`
--

CREATE TABLE IF NOT EXISTS `wiki_entry_picture` (
  `wiki_entry_picture_id` int(11) NOT NULL,
  `picture_title` varchar(45) NOT NULL,
  `picture` varchar(300) NOT NULL,
  `wiki_entry_id` int(11) NOT NULL,
  PRIMARY KEY (`wiki_entry_picture_id`),
  KEY `fk_wiki_entry_picture_wiki_entry1_idx` (`wiki_entry_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf16;

--
-- Constraints der exportierten Tabellen
--

--
-- Constraints der Tabelle `class`
--
ALTER TABLE `class`
  ADD CONSTRAINT `fk_class_subject1` FOREIGN KEY (`subject_id`) REFERENCES `subject` (`subject_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_class_user1` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Constraints der Tabelle `class_topic`
--
ALTER TABLE `class_topic`
  ADD CONSTRAINT `fk_class_topic_class1` FOREIGN KEY (`class_id`) REFERENCES `class` (`class_id`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Constraints der Tabelle `comment`
--
ALTER TABLE `comment`
  ADD CONSTRAINT `fk_comment_task1` FOREIGN KEY (`task_id`) REFERENCES `task` (`task_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_comment_user1` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Constraints der Tabelle `forum_post`
--
ALTER TABLE `forum_post`
  ADD CONSTRAINT `fk_forum_post_forum_thread1` FOREIGN KEY (`forum_thread_id`) REFERENCES `forum_thread` (`forum_thread_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_forum_post_user1` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Constraints der Tabelle `forum_thread`
--
ALTER TABLE `forum_thread`
  ADD CONSTRAINT `fk_forum_thread_class1` FOREIGN KEY (`class_id`) REFERENCES `class` (`class_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_forum_thread_user1` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Constraints der Tabelle `linked_entry`
--
ALTER TABLE `linked_entry`
  ADD CONSTRAINT `fk_wiki_entry_has_wiki_entry_wiki_entry1` FOREIGN KEY (`wiki_entry_id`) REFERENCES `wiki_entry` (`wiki_entry_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_wiki_entry_has_wiki_entry_wiki_entry2` FOREIGN KEY (`wiki_entry_id1`) REFERENCES `wiki_entry` (`wiki_entry_id`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Constraints der Tabelle `linked_tasks`
--
ALTER TABLE `linked_tasks`
  ADD CONSTRAINT `fk_user_has_task_task1` FOREIGN KEY (`task_id`) REFERENCES `task` (`task_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_user_has_task_user2` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Constraints der Tabelle `message`
--
ALTER TABLE `message`
  ADD CONSTRAINT `fk_message_user` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Constraints der Tabelle `notification`
--
ALTER TABLE `notification`
  ADD CONSTRAINT `fk_activity_user` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Constraints der Tabelle `rating`
--
ALTER TABLE `rating`
  ADD CONSTRAINT `fk_rating_task1` FOREIGN KEY (`task_id`) REFERENCES `task` (`task_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_rating_user1` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Constraints der Tabelle `task`
--
ALTER TABLE `task`
  ADD CONSTRAINT `fk_status_reply_user` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_task_subject1` FOREIGN KEY (`subject_id`) REFERENCES `subject` (`subject_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_task_tasktype1` FOREIGN KEY (`tasktype_id`) REFERENCES `tasktype` (`tasktype_id`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Constraints der Tabelle `task_for_class`
--
ALTER TABLE `task_for_class`
  ADD CONSTRAINT `fk_class_has_task_class1` FOREIGN KEY (`class_id`) REFERENCES `class` (`class_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_class_has_task_task1` FOREIGN KEY (`task_id`) REFERENCES `task` (`task_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_task_for_class_class_topic1` FOREIGN KEY (`class_topic_id`) REFERENCES `class_topic` (`class_topic_id`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Constraints der Tabelle `user_fulfill_task`
--
ALTER TABLE `user_fulfill_task`
  ADD CONSTRAINT `fk_user_fulfill_task_task_for_class1` FOREIGN KEY (`task_for_class_id`) REFERENCES `task_for_class` (`task_for_class_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_user_has_task_user1` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Constraints der Tabelle `user_is_in_class`
--
ALTER TABLE `user_is_in_class`
  ADD CONSTRAINT `fk_user_has_class_class1` FOREIGN KEY (`class_id`) REFERENCES `class` (`class_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_user_has_class_user1` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Constraints der Tabelle `user_worked_on_wiki_entry`
--
ALTER TABLE `user_worked_on_wiki_entry`
  ADD CONSTRAINT `fk_user_has_wiki_entry_user1` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_user_has_wiki_entry_wiki_entry1` FOREIGN KEY (`wiki_entry_id`) REFERENCES `wiki_entry` (`wiki_entry_id`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Constraints der Tabelle `wiki`
--
ALTER TABLE `wiki`
  ADD CONSTRAINT `fk_wiki_class_topic1` FOREIGN KEY (`class_topic_id`) REFERENCES `class_topic` (`class_topic_id`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Constraints der Tabelle `wiki_entry`
--
ALTER TABLE `wiki_entry`
  ADD CONSTRAINT `fk_wiki_entry_wiki1` FOREIGN KEY (`wiki_id`) REFERENCES `wiki` (`wiki_id`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Constraints der Tabelle `wiki_entry_picture`
--
ALTER TABLE `wiki_entry_picture`
  ADD CONSTRAINT `fk_wiki_entry_picture_wiki_entry1` FOREIGN KEY (`wiki_entry_id`) REFERENCES `wiki_entry` (`wiki_entry_id`) ON DELETE NO ACTION ON UPDATE NO ACTION;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
