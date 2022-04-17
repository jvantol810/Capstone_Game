-- phpMyAdmin SQL Dump
-- version 5.0.4
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Apr 17, 2022 at 04:55 AM
-- Server version: 5.7.24
-- PHP Version: 8.0.1

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `csc483capstone`
--

-- --------------------------------------------------------

--
-- Table structure for table `accounts`
--

CREATE TABLE `accounts` (
  `playerid` int(8) NOT NULL,
  `username` varchar(20) NOT NULL,
  `hash` varchar(100) NOT NULL,
  `salt` varchar(50) NOT NULL,
  `totalruns` int(8) NOT NULL DEFAULT '0',
  `currenttime` int(10) NOT NULL DEFAULT '0',
  `currency` int(10) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `accounts`
--

INSERT INTO `accounts` (`playerid`, `username`, `hash`, `salt`, `totalruns`, `currenttime`, `currency`) VALUES
(1, 'testuser', '$5$rounds=5000$steamedhamstestu$XrDNass4TleimnUjRBAi/UQoQIBKWOn.Wy8mwsXuvGA', '$5$rounds=5000$steamedhamstestuser$', 0, 0, 1500),
(2, 'testuser2', '$5$rounds=5000$steamedhamstestu$gtBEskHAV5PFRrUAz03kOObdO55AqBHocWDYHoinlMB', '$5$rounds=5000$steamedhamstestuser2$', 0, 0, 0),
(3, 'testuser3', '$5$rounds=5000$steamedhamstestu$ler4f8SWvCWR/GTgTM0rZUxdEDs/2cfmxLQccuy7LMA', '$5$rounds=5000$steamedhamstestuser3$', 0, 0, 0),
(4, 'testuser4', '$5$rounds=5000$steamedhamstestu$NggD8jqlh7njs7KQqg9PpHQJc8V4TRwm4jmo4dbNUTA', '$5$rounds=5000$steamedhamstestuser4$', 0, 0, 150),
(5, 'testuser5', '$5$rounds=5000$steamedhamstestu$ZWQFn1JHwFVq8gjjQGdZPac7lgz8GFVhFzyg8bppmk0', '$5$rounds=5000$steamedhamstestuser5$', 0, 0, 0),
(6, 'testuser7', '$5$rounds=5000$steamedhamstestu$P5/mnhMPSD07k/V.CyBgEIkQqUEU2qH0vn5X28iIdYD', '$5$rounds=5000$steamedhamstestuser7$', 0, 0, 0),
(7, 'testacct', '$5$rounds=5000$steamedhamstesta$Vyt8tciEO1a5xek0eOPjKNK4P3hEW0R8/PSQzzN2k99', '$5$rounds=5000$steamedhamstestacct$', 0, 0, 3150),
(8, 'newuserlol', '$5$rounds=5000$steamedhamsnewus$QVM5wiykiuDlVbTWpF4AzX0IgpRiBxbZlZcXd1uZAo1', '$5$rounds=5000$steamedhamsnewuserlol$', 0, 0, 0),
(9, 'newtest1', '$5$rounds=5000$steamedhamsnewte$m2xlZju9f5OviffCPtoV1bW.2vINoc6d2yJLouCpGgA', '$5$rounds=5000$steamedhamsnewtest1$', 0, 0, 0),
(10, 'newtest2', '$5$rounds=5000$steamedhamsnewte$m2xlZju9f5OviffCPtoV1bW.2vINoc6d2yJLouCpGgA', '$5$rounds=5000$steamedhamsnewtest2$', 0, 0, 0);

-- --------------------------------------------------------

--
-- Table structure for table `cashshop_purchases`
--

CREATE TABLE `cashshop_purchases` (
  `playerid` int(8) NOT NULL,
  `itemid` varchar(4) NOT NULL,
  `price` int(5) NOT NULL,
  `date` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `cashshop_purchases`
--

INSERT INTO `cashshop_purchases` (`playerid`, `itemid`, `price`, `date`) VALUES
(12345, 'H001', 150, '2022-04-08'),
(12345, 'H002', 150, '2022-04-08'),
(12345, 'H333', 11, '2022-04-10'),
(1, 'H001', 150, '2022-04-10'),
(2, 'H001', 150, '2022-04-10'),
(3, 'H001', 150, '2022-04-10'),
(4, 'H001', 150, '2022-04-10'),
(7, 'H001', 150, '2022-04-10');

-- --------------------------------------------------------

--
-- Table structure for table `leaderboard`
--

CREATE TABLE `leaderboard` (
  `playerid` int(8) NOT NULL,
  `username` varchar(20) NOT NULL,
  `time` int(10) NOT NULL,
  `levelscompleted` int(4) NOT NULL,
  `count` int(8) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `leaderboard`
--

INSERT INTO `leaderboard` (`playerid`, `username`, `time`, `levelscompleted`, `count`) VALUES
(1, 'testuser', 9, 4, 0),
(4, 'testuser4', 11, 1, 0),
(4, 'testuser4', 12, 1, 0),
(4, 'testuser4', 14, 3, 0),
(1337, 'extraeffort', 2000, 17, 0),
(1337, 'extraeffort', 3000, 17, 0);

-- --------------------------------------------------------

--
-- Table structure for table `useraccounts`
--

CREATE TABLE `useraccounts` (
  `playerid` int(8) NOT NULL,
  `username` varchar(20) NOT NULL,
  `hash` varchar(100) NOT NULL,
  `salt` varchar(50) NOT NULL,
  `totalruns` int(8) NOT NULL,
  `currenttime` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `accounts`
--
ALTER TABLE `accounts`
  ADD PRIMARY KEY (`playerid`),
  ADD UNIQUE KEY `username` (`username`);

--
-- Indexes for table `leaderboard`
--
ALTER TABLE `leaderboard`
  ADD PRIMARY KEY (`playerid`,`username`,`time`,`levelscompleted`) USING BTREE;

--
-- Indexes for table `useraccounts`
--
ALTER TABLE `useraccounts`
  ADD PRIMARY KEY (`playerid`),
  ADD UNIQUE KEY `username` (`username`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `accounts`
--
ALTER TABLE `accounts`
  MODIFY `playerid` int(8) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `useraccounts`
--
ALTER TABLE `useraccounts`
  MODIFY `playerid` int(8) NOT NULL AUTO_INCREMENT;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
