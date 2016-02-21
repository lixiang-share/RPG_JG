-- MySQL dump 10.13  Distrib 5.6.21, for Win32 (x86)
--
-- Host: localhost    Database: jg_rpg
-- ------------------------------------------------------
-- Server version	5.6.21-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `tb_role`
--

DROP TABLE IF EXISTS `tb_role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tb_role` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `ownerId` int(11) NOT NULL,
  `roleId` varchar(10) DEFAULT NULL,
  `name` varchar(20) NOT NULL DEFAULT '',
  `level` int(11) NOT NULL DEFAULT '0',
  `gender` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tb_role`
--

LOCK TABLES `tb_role` WRITE;
/*!40000 ALTER TABLE `tb_role` DISABLE KEYS */;
INSERT INTO `tb_role` VALUES (1,1,'boy','bbbb',1,0),(2,1,'girl','bbbb',1,0),(3,24,'boy','awe',1,0),(4,24,'girl','weww',1,0),(5,26,'girl','abcd',1,1),(6,27,'girl','girl1234',1,1),(7,28,'girl','The Girl',1,1),(8,29,'boy','Unname',1,0),(9,30,'boy','Unname',1,0),(10,31,'boy','Unname',1,0),(11,32,'boy','Unname',1,0),(12,33,'boy','Unname',1,0),(13,34,'boy','Unname',1,0),(14,36,'boy','Unname',1,0),(15,37,'boy','Unname',1,0);
/*!40000 ALTER TABLE `tb_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tb_server`
--

DROP TABLE IF EXISTS `tb_server`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tb_server` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(20) NOT NULL,
  `ip` varchar(16) NOT NULL,
  `count` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tb_server`
--

LOCK TABLES `tb_server` WRITE;
/*!40000 ALTER TABLE `tb_server` DISABLE KEYS */;
INSERT INTO `tb_server` VALUES (1,'王者荣耀','127.0.0.1',10),(2,'血战长空','192.168.0.1',20),(3,'英雄征途','180.167.0.1',30),(4,'决战紫禁','156.124.0.1',40);
/*!40000 ALTER TABLE `tb_server` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tb_task`
--

DROP TABLE IF EXISTS `tb_task`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tb_task` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `taskId` int(11) NOT NULL,
  `roleId` int(11) NOT NULL,
  `type` varchar(10) NOT NULL,
  `status` int(11) NOT NULL,
  `goldCount` int(11) NOT NULL DEFAULT '0',
  `diamondCount` int(11) NOT NULL DEFAULT '0',
  `curStage` int(11) NOT NULL DEFAULT '0',
  `totalStage` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tb_task`
--

LOCK TABLES `tb_task` WRITE;
/*!40000 ALTER TABLE `tb_task` DISABLE KEYS */;
INSERT INTO `tb_task` VALUES (1,1001,15,'Main',0,1000,5000,0,4),(2,1002,15,'Reward',0,2000,4000,0,4),(3,1003,15,'Daily',0,3000,2000,0,4),(4,1004,15,'Daily',0,1000,6000,0,4);
/*!40000 ALTER TABLE `tb_task` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tb_user`
--

DROP TABLE IF EXISTS `tb_user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tb_user` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(16) NOT NULL DEFAULT '',
  `pwd` varchar(34) NOT NULL DEFAULT '',
  `phone` char(11) DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=38 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tb_user`
--

LOCK TABLES `tb_user` WRITE;
/*!40000 ALTER TABLE `tb_user` DISABLE KEYS */;
INSERT INTO `tb_user` VALUES (21,'b1','934b535800b1cba8f96a5d72f72f1611','3333'),(22,'aaa','81dc9bdb52d04dc20036dbd8313ed055','1234'),(23,'b2','934b535800b1cba8f96a5d72f72f1611','3333'),(24,'a','b59c67bf196a4758191e42f76670ceba',''),(25,'b4','934b535800b1cba8f96a5d72f72f1611','3333'),(26,'abc','81dc9bdb52d04dc20036dbd8313ed055',''),(27,'aa','827ccb0eea8a706c4c34a16891f84e7b',''),(28,'a3','81dc9bdb52d04dc20036dbd8313ed055',''),(29,'a4','b59c67bf196a4758191e42f76670ceba',''),(30,'a5','b59c67bf196a4758191e42f76670ceba',''),(31,'a6','b59c67bf196a4758191e42f76670ceba',''),(32,'a7','b59c67bf196a4758191e42f76670ceba',''),(33,'b3','b59c67bf196a4758191e42f76670ceba',''),(34,'b5','b59c67bf196a4758191e42f76670ceba',''),(35,'b7','b59c67bf196a4758191e42f76670ceba',''),(36,'c3','b59c67bf196a4758191e42f76670ceba',''),(37,'c5','b59c67bf196a4758191e42f76670ceba','');
/*!40000 ALTER TABLE `tb_user` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2016-02-21 21:33:29
