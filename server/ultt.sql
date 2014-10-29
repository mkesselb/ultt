-- MySQL Script generated by MySQL Workbench
-- 08/07/14 09:05:19
-- Model: Social Network    Version: 1.0
SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema ultt
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `ultt` ;
CREATE SCHEMA IF NOT EXISTS `ultt` DEFAULT CHARACTER SET utf16 ;
USE `ultt` ;

-- -----------------------------------------------------
-- Table `ultt`.`user`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ultt`.`user` ;

CREATE TABLE IF NOT EXISTS `ultt`.`user` (
  `user_id` MEDIUMINT(8) NOT NULL AUTO_INCREMENT,
  `token` MEDIUMINT(5) NOT NULL,
  `username` VARCHAR(45) NOT NULL,
  `password` VARCHAR(45) NOT NULL,
  `name_first` VARCHAR(45) NOT NULL,
  `name_last` VARCHAR(45) NOT NULL,
  `email_id` VARCHAR(100) NOT NULL,
  `picture` VARCHAR(255) NULL DEFAULT '/web/image/default.jpg',
  `created_at` TIMESTAMP NOT NULL,
  `school_id` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`user_id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_unicode_ci;


-- -----------------------------------------------------
-- Table `ultt`.`subject`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ultt`.`subject` ;

CREATE TABLE IF NOT EXISTS `ultt`.`subject` (
  `subject_id` INT NOT NULL,
  `subject_name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`subject_id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `ultt`.`tasktype`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ultt`.`tasktype` ;

CREATE TABLE IF NOT EXISTS `ultt`.`tasktype` (
  `tasktype_id` INT NOT NULL,
  `type_name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`tasktype_id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `ultt`.`task`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ultt`.`task` ;

CREATE TABLE IF NOT EXISTS `ultt`.`task` (
  `task_id` MEDIUMINT(8) NOT NULL AUTO_INCREMENT,
  `taskname` VARCHAR(45) NOT NULL,
  `created_at` TIMESTAMP NOT NULL,
  `rating` SMALLINT(5) NULL,
  `description` VARCHAR(100) NOT NULL,
  `public` TINYINT(1) NOT NULL DEFAULT 0,
  `user_id` MEDIUMINT(8) NOT NULL,
  `data_file` LONGTEXT NOT NULL,
  `subject_id` INT NOT NULL,
  `tasktype_id` INT NOT NULL,
  INDEX `fk_status_reply_user` (`user_id` ASC),
  PRIMARY KEY (`task_id`),
  INDEX `fk_task_subject1_idx` (`subject_id` ASC),
  INDEX `fk_task_tasktype1_idx` (`tasktype_id` ASC),
  CONSTRAINT `fk_status_reply_user`
    FOREIGN KEY (`user_id`)
    REFERENCES `ultt`.`user` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_task_subject1`
    FOREIGN KEY (`subject_id`)
    REFERENCES `ultt`.`subject` (`subject_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_task_tasktype1`
    FOREIGN KEY (`tasktype_id`)
    REFERENCES `ultt`.`tasktype` (`tasktype_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;


-- -----------------------------------------------------
-- Table `ultt`.`message`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ultt`.`message` ;

CREATE TABLE IF NOT EXISTS `ultt`.`message` (
  `message_id` MEDIUMINT(8) NOT NULL AUTO_INCREMENT,
  `message` VARCHAR(500) NOT NULL,
  `created_at` TIMESTAMP NOT NULL,
  `is_read` TINYINT(1) NOT NULL DEFAULT 0,
  `is_spam` TINYINT(1) NOT NULL DEFAULT 0,
  `to` MEDIUMINT(8) NULL,
  `isreply` TINYINT(1) NULL DEFAULT 0,
  `user_id` MEDIUMINT(8) NULL,
  PRIMARY KEY (`message_id`),
  INDEX `fk_message_user` (`user_id` ASC),
  CONSTRAINT `fk_message_user`
    FOREIGN KEY (`user_id`)
    REFERENCES `ultt`.`user` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;


-- -----------------------------------------------------
-- Table `ultt`.`rating`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ultt`.`rating` ;

CREATE TABLE IF NOT EXISTS `ultt`.`rating` (
  `rating_id` MEDIUMINT(8) NOT NULL AUTO_INCREMENT,
  `flag` TINYINT(1) NULL DEFAULT 1,
  `created_at` TIMESTAMP NULL,
  `user_id` MEDIUMINT(8) NOT NULL,
  `task_id` MEDIUMINT(8) NOT NULL,
  PRIMARY KEY (`rating_id`),
  INDEX `fk_rating_user1_idx` (`user_id` ASC),
  INDEX `fk_rating_task1_idx` (`task_id` ASC),
  CONSTRAINT `fk_rating_user1`
    FOREIGN KEY (`user_id`)
    REFERENCES `ultt`.`user` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_rating_task1`
    FOREIGN KEY (`task_id`)
    REFERENCES `ultt`.`task` (`task_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;


-- -----------------------------------------------------
-- Table `ultt`.`notification`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ultt`.`notification` ;

CREATE TABLE IF NOT EXISTS `ultt`.`notification` (
  `notification_id` MEDIUMINT(8) NOT NULL AUTO_INCREMENT,
  `msg` VARCHAR(255) NULL,
  `type` SMALLINT(5) NULL,
  `privacy` TINYINT(3) NOT NULL DEFAULT 0,
  `created_at` TIMESTAMP NULL,
  `user_id` MEDIUMINT(8) NULL,
  PRIMARY KEY (`notification_id`),
  INDEX `fk_activity_user` (`user_id` ASC),
  CONSTRAINT `fk_activity_user`
    FOREIGN KEY (`user_id`)
    REFERENCES `ultt`.`user` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;


-- -----------------------------------------------------
-- Table `ultt`.`comment`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ultt`.`comment` ;

CREATE TABLE IF NOT EXISTS `ultt`.`comment` (
  `comment_id` MEDIUMINT(8) NOT NULL AUTO_INCREMENT,
  `message` VARCHAR(255) NULL,
  `created_at` TIMESTAMP NULL,
  `user_id` MEDIUMINT(8) NOT NULL,
  `task_id` MEDIUMINT(8) NOT NULL,
  PRIMARY KEY (`comment_id`),
  INDEX `fk_comment_user1_idx` (`user_id` ASC),
  INDEX `fk_comment_task1_idx` (`task_id` ASC),
  CONSTRAINT `fk_comment_user1`
    FOREIGN KEY (`user_id`)
    REFERENCES `ultt`.`user` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_comment_task1`
    FOREIGN KEY (`task_id`)
    REFERENCES `ultt`.`task` (`task_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;


-- -----------------------------------------------------
-- Table `ultt`.`class`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ultt`.`class` ;

CREATE TABLE IF NOT EXISTS `ultt`.`class` (
  `class_id` INT NOT NULL AUTO_INCREMENT,
  `classname` VARCHAR(45) NOT NULL,
  `privacy` TINYINT(3) NULL,
  `user_id` MEDIUMINT(8) NOT NULL COMMENT 'user_id = id of the user, which created the class',
  `school_year` YEAR NOT NULL,
  `classcode` VARCHAR(45) NOT NULL,
  `subject_id` INT NOT NULL,
  `deleted` TINYINT(1) NOT NULL,
  PRIMARY KEY (`class_id`),
  INDEX `fk_class_user1_idx` (`user_id` ASC),
  INDEX `fk_class_subject1_idx` (`subject_id` ASC),
  CONSTRAINT `fk_class_user1`
    FOREIGN KEY (`user_id`)
    REFERENCES `ultt`.`user` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_class_subject1`
    FOREIGN KEY (`subject_id`)
    REFERENCES `ultt`.`subject` (`subject_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `ultt`.`user_is_in_class`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ultt`.`user_is_in_class` ;

CREATE TABLE IF NOT EXISTS `ultt`.`user_is_in_class` (
  `user_id` MEDIUMINT(8) NOT NULL,
  `class_id` INT NOT NULL,
  `accepted` TINYINT(1) NOT NULL,
  PRIMARY KEY (`user_id`, `class_id`),
  INDEX `fk_user_has_class_class1_idx` (`class_id` ASC),
  INDEX `fk_user_has_class_user1_idx` (`user_id` ASC),
  CONSTRAINT `fk_user_has_class_user1`
    FOREIGN KEY (`user_id`)
    REFERENCES `ultt`.`user` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_user_has_class_class1`
    FOREIGN KEY (`class_id`)
    REFERENCES `ultt`.`class` (`class_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_unicode_ci;


-- -----------------------------------------------------
-- Table `ultt`.`forum_thread`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ultt`.`forum_thread` ;

CREATE TABLE IF NOT EXISTS `ultt`.`forum_thread` (
  `forum_thread_id` INT NOT NULL AUTO_INCREMENT,
  `threadname` VARCHAR(45) NOT NULL,
  `created_at` TIMESTAMP NULL,
  `user_id` MEDIUMINT(8) NOT NULL,
  `class_id` INT NOT NULL,
  PRIMARY KEY (`forum_thread_id`),
  INDEX `fk_forum_thread_user1_idx` (`user_id` ASC),
  INDEX `fk_forum_thread_class1_idx` (`class_id` ASC),
  CONSTRAINT `fk_forum_thread_user1`
    FOREIGN KEY (`user_id`)
    REFERENCES `ultt`.`user` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_forum_thread_class1`
    FOREIGN KEY (`class_id`)
    REFERENCES `ultt`.`class` (`class_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `ultt`.`forum_post`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ultt`.`forum_post` ;

CREATE TABLE IF NOT EXISTS `ultt`.`forum_post` (
  `forum_post_id` INT NOT NULL AUTO_INCREMENT,
  `created_at` TIMESTAMP NULL,
  `text` MEDIUMTEXT NOT NULL,
  `forum_thread_id` INT NOT NULL,
  `user_id` MEDIUMINT(8) NOT NULL,
  PRIMARY KEY (`forum_post_id`),
  INDEX `fk_forum_post_forum_thread1_idx` (`forum_thread_id` ASC),
  INDEX `fk_forum_post_user1_idx` (`user_id` ASC),
  CONSTRAINT `fk_forum_post_forum_thread1`
    FOREIGN KEY (`forum_thread_id`)
    REFERENCES `ultt`.`forum_thread` (`forum_thread_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_forum_post_user1`
    FOREIGN KEY (`user_id`)
    REFERENCES `ultt`.`user` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `ultt`.`class_topic`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ultt`.`class_topic` ;

CREATE TABLE IF NOT EXISTS `ultt`.`class_topic` (
  `class_topic_id` INT NOT NULL AUTO_INCREMENT,
  `class_id` INT NOT NULL,
  `topic_name` VARCHAR(45) NOT NULL,
  `deleted` TINYINT(1) NOT NULL,
  PRIMARY KEY (`class_topic_id`),
  INDEX `fk_class_topic_class1_idx` (`class_id` ASC),
  CONSTRAINT `fk_class_topic_class1`
    FOREIGN KEY (`class_id`)
    REFERENCES `ultt`.`class` (`class_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `ultt`.`task_for_class`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ultt`.`task_for_class` ;

CREATE TABLE IF NOT EXISTS `ultt`.`task_for_class` (
  `class_id` INT NOT NULL,
  `task_id` MEDIUMINT(8) NOT NULL,
  `assign_time` TIMESTAMP NULL,
  `obligatory` TINYINT(1) NOT NULL,
  `deadline` DATE NOT NULL COMMENT 'Zeitpunkt bis zu welchem diese Task gemacht werden soll.',
  `max_attempts` SMALLINT NOT NULL COMMENT 'Anzahl der Versuche',
  `task_for_class_id` MEDIUMINT NOT NULL AUTO_INCREMENT,
  `class_topic_id` INT NOT NULL,
  PRIMARY KEY (`task_for_class_id`),
  INDEX `fk_class_has_task_task1_idx` (`task_id` ASC),
  INDEX `fk_class_has_task_class1_idx` (`class_id` ASC),
  INDEX `fk_task_for_class_class_topic1_idx` (`class_topic_id` ASC),
  CONSTRAINT `fk_class_has_task_class1`
    FOREIGN KEY (`class_id`)
    REFERENCES `ultt`.`class` (`class_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_class_has_task_task1`
    FOREIGN KEY (`task_id`)
    REFERENCES `ultt`.`task` (`task_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_task_for_class_class_topic1`
    FOREIGN KEY (`class_topic_id`)
    REFERENCES `ultt`.`class_topic` (`class_topic_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `ultt`.`user_fulfill_task`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ultt`.`user_fulfill_task` ;

CREATE TABLE IF NOT EXISTS `ultt`.`user_fulfill_task` (
  `user_fulfill_task_id` MEDIUMINT NOT NULL AUTO_INCREMENT,
  `user_id` MEDIUMINT(8) NOT NULL,
  `fulfill_time` TIMESTAMP NOT NULL,
  `results` MEDIUMTEXT NULL,
  `task_for_class_id` MEDIUMINT NOT NULL,
  PRIMARY KEY (`user_fulfill_task_id`, `user_id`),
  INDEX `fk_user_has_task_user1_idx` (`user_id` ASC),
  INDEX `fk_user_fulfill_task_task_for_class1_idx` (`task_for_class_id` ASC),
  CONSTRAINT `fk_user_has_task_user1`
    FOREIGN KEY (`user_id`)
    REFERENCES `ultt`.`user` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_user_fulfill_task_task_for_class1`
    FOREIGN KEY (`task_for_class_id`)
    REFERENCES `ultt`.`task_for_class` (`task_for_class_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_unicode_ci;


-- -----------------------------------------------------
-- Table `ultt`.`wiki`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ultt`.`wiki` ;

CREATE TABLE IF NOT EXISTS `ultt`.`wiki` (
  `wiki_id` INT NOT NULL,
  `wikiname` VARCHAR(45) NOT NULL,
  `class_topic_id` INT NOT NULL,
  PRIMARY KEY (`wiki_id`),
  INDEX `fk_wiki_class_topic1_idx` (`class_topic_id` ASC),
  CONSTRAINT `fk_wiki_class_topic1`
    FOREIGN KEY (`class_topic_id`)
    REFERENCES `ultt`.`class_topic` (`class_topic_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `ultt`.`wiki_entry`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ultt`.`wiki_entry` ;

CREATE TABLE IF NOT EXISTS `ultt`.`wiki_entry` (
  `wiki_entry_id` INT NOT NULL,
  `wiki_entry_title` VARCHAR(45) NOT NULL,
  `text` LONGTEXT NOT NULL,
  `wiki_id` INT NOT NULL,
  PRIMARY KEY (`wiki_entry_id`),
  INDEX `fk_wiki_entry_wiki1_idx` (`wiki_id` ASC),
  CONSTRAINT `fk_wiki_entry_wiki1`
    FOREIGN KEY (`wiki_id`)
    REFERENCES `ultt`.`wiki` (`wiki_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `ultt`.`wiki_entry_picture`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ultt`.`wiki_entry_picture` ;

CREATE TABLE IF NOT EXISTS `ultt`.`wiki_entry_picture` (
  `wiki_entry_picture_id` INT NOT NULL,
  `picture_title` VARCHAR(45) NOT NULL,
  `picture` VARCHAR(300) NOT NULL,
  `wiki_entry_id` INT NOT NULL,
  PRIMARY KEY (`wiki_entry_picture_id`),
  INDEX `fk_wiki_entry_picture_wiki_entry1_idx` (`wiki_entry_id` ASC),
  CONSTRAINT `fk_wiki_entry_picture_wiki_entry1`
    FOREIGN KEY (`wiki_entry_id`)
    REFERENCES `ultt`.`wiki_entry` (`wiki_entry_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `ultt`.`user_worked_on_wiki_entry`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ultt`.`user_worked_on_wiki_entry` ;

CREATE TABLE IF NOT EXISTS `ultt`.`user_worked_on_wiki_entry` (
  `user_id` MEDIUMINT(8) NOT NULL,
  `wiki_entry_id` INT NOT NULL,
  `worked_on_time` TIMESTAMP NOT NULL,
  PRIMARY KEY (`user_id`, `wiki_entry_id`),
  INDEX `fk_user_has_wiki_entry_wiki_entry1_idx` (`wiki_entry_id` ASC),
  INDEX `fk_user_has_wiki_entry_user1_idx` (`user_id` ASC),
  CONSTRAINT `fk_user_has_wiki_entry_user1`
    FOREIGN KEY (`user_id`)
    REFERENCES `ultt`.`user` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_user_has_wiki_entry_wiki_entry1`
    FOREIGN KEY (`wiki_entry_id`)
    REFERENCES `ultt`.`wiki_entry` (`wiki_entry_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_unicode_ci;


-- -----------------------------------------------------
-- Table `ultt`.`linked_tasks`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ultt`.`linked_tasks` ;

CREATE TABLE IF NOT EXISTS `ultt`.`linked_tasks` (
  `user_id` MEDIUMINT(8) NOT NULL,
  `task_id` MEDIUMINT(8) NOT NULL,
  PRIMARY KEY (`user_id`, `task_id`),
  INDEX `fk_user_has_task_task1_idx` (`task_id` ASC),
  INDEX `fk_user_has_task_user2_idx` (`user_id` ASC),
  CONSTRAINT `fk_user_has_task_user2`
    FOREIGN KEY (`user_id`)
    REFERENCES `ultt`.`user` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_user_has_task_task1`
    FOREIGN KEY (`task_id`)
    REFERENCES `ultt`.`task` (`task_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_unicode_ci;


-- -----------------------------------------------------
-- Table `ultt`.`linked_entry`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ultt`.`linked_entry` ;

CREATE TABLE IF NOT EXISTS `ultt`.`linked_entry` (
  `wiki_entry_id` INT NOT NULL,
  `wiki_entry_id1` INT NOT NULL,
  PRIMARY KEY (`wiki_entry_id`, `wiki_entry_id1`),
  INDEX `fk_wiki_entry_has_wiki_entry_wiki_entry2_idx` (`wiki_entry_id1` ASC),
  INDEX `fk_wiki_entry_has_wiki_entry_wiki_entry1_idx` (`wiki_entry_id` ASC),
  CONSTRAINT `fk_wiki_entry_has_wiki_entry_wiki_entry1`
    FOREIGN KEY (`wiki_entry_id`)
    REFERENCES `ultt`.`wiki_entry` (`wiki_entry_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_wiki_entry_has_wiki_entry_wiki_entry2`
    FOREIGN KEY (`wiki_entry_id1`)
    REFERENCES `ultt`.`wiki_entry` (`wiki_entry_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
