-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Gazdă: localhost
-- Timp de generare: mai 29, 2023 la 10:31 PM
-- Versiune server: 10.4.27-MariaDB
-- Versiune PHP: 8.1.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Bază de date: `Licența`
--

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `Anunturi`
--

CREATE TABLE `Anunturi` (
  `id_anunturi` int(11) NOT NULL,
  `id_utilizator` int(11) NOT NULL,
  `TitluAnunt` varchar(255) NOT NULL,
  `NumeProdus` varchar(255) NOT NULL,
  `DataPostareAnunt` datetime NOT NULL,
  `Pret` decimal(10,2) NOT NULL,
  `StareAnunt` enum('activ','inactiv') NOT NULL,
  `Promovare` tinyint(1) NOT NULL,
  `id_subcategorie` int(11) NOT NULL,
  `DetaliiAnunt` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Eliminarea datelor din tabel `Anunturi`
--

INSERT INTO `Anunturi` (`id_anunturi`, `id_utilizator`, `TitluAnunt`, `NumeProdus`, `DataPostareAnunt`, `Pret`, `StareAnunt`, `Promovare`, `id_subcategorie`, `DetaliiAnunt`) VALUES
(13, 1, 'Vând Golf 4', 'Golf 4', '2023-05-26 14:40:42', '222000.00', 'activ', 1, 1, NULL),
(14, 1, 'Vând Logan', 'Logan', '2023-05-26 14:41:47', '200000.00', 'activ', 0, 1, NULL);

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `Categorii`
--

CREATE TABLE `Categorii` (
  `id_categorie` int(11) NOT NULL,
  `Nume` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Eliminarea datelor din tabel `Categorii`
--

INSERT INTO `Categorii` (`id_categorie`, `Nume`) VALUES
(1, 'Masini'),
(2, 'Animale'),
(3, 'Electrocasnice'),
(4, 'Laptop-uri, Desktop PC, Telefoane, Tablete'),
(5, 'Haine'),
(6, 'Imobiliare'),
(7, 'Periferice');

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `Evaluari`
--

CREATE TABLE `Evaluari` (
  `id_evaluari` int(11) NOT NULL,
  `id_utilizator_evaluator` int(11) NOT NULL,
  `id_utilizator_evaluat` int(11) NOT NULL,
  `id_anunt` int(11) DEFAULT NULL,
  `DataEvaluare` datetime NOT NULL,
  `Nota` int(11) NOT NULL,
  `Comentariu` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `Favorite`
--

CREATE TABLE `Favorite` (
  `Id_favorite` int(11) NOT NULL,
  `id_utilizator` int(11) NOT NULL,
  `id_anunt` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `Imagini`
--

CREATE TABLE `Imagini` (
  `id_imagini` int(11) NOT NULL,
  `id_anunt` int(11) NOT NULL,
  `URLImagine` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Eliminarea datelor din tabel `Imagini`
--

INSERT INTO `Imagini` (`id_imagini`, `id_anunt`, `URLImagine`) VALUES
(33, 13, '/Users/catalinflorea/Desktop/Licenta/Website/Website/wwwroot/images/Golf4.webp'),
(34, 13, '/Users/catalinflorea/Desktop/Licenta/Website/Website/wwwroot/images/Golf4p2.webp'),
(35, 14, '/Users/catalinflorea/Desktop/Licenta/Website/Website/wwwroot/images/Dacia-Logan-2017-C01.jpg');

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `ListaNeagra`
--

CREATE TABLE `ListaNeagra` (
  `id_ListaNeagra` int(11) NOT NULL,
  `id_utilizator` int(11) NOT NULL,
  `Motiv` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Eliminarea datelor din tabel `ListaNeagra`
--

INSERT INTO `ListaNeagra` (`id_ListaNeagra`, `id_utilizator`, `Motiv`) VALUES
(1, 2, 'Tentativa Frauda');

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `Mesaje`
--

CREATE TABLE `Mesaje` (
  `id_mesaj` int(11) NOT NULL,
  `id_utilizator_expeditor` int(11) NOT NULL,
  `id_utilizator_destinatar` int(11) NOT NULL,
  `id_anunt` int(11) DEFAULT NULL,
  `DataMesaj` datetime NOT NULL,
  `Subiect` varchar(255) DEFAULT NULL,
  `Continut` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `Orase`
--

CREATE TABLE `Orase` (
  `id_oras` int(10) UNSIGNED NOT NULL,
  `NumeOras` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Eliminarea datelor din tabel `Orase`
--

INSERT INTO `Orase` (`id_oras`, `NumeOras`) VALUES
(1, 'Buzau');

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `Promovari`
--

CREATE TABLE `Promovari` (
  `id_promovari` int(11) NOT NULL,
  `id_anunt` int(11) NOT NULL,
  `TipPromovare` varchar(255) NOT NULL,
  `Durata` int(11) NOT NULL,
  `Suma` decimal(10,0) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `Subcategorii`
--

CREATE TABLE `Subcategorii` (
  `id_subcategorie` int(11) NOT NULL,
  `id_categorie` int(11) NOT NULL,
  `Nume` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Eliminarea datelor din tabel `Subcategorii`
--

INSERT INTO `Subcategorii` (`id_subcategorie`, `id_categorie`, `Nume`) VALUES
(1, 1, 'Break'),
(2, 1, 'Berlina'),
(3, 2, 'De Companie'),
(4, 2, 'Domestice'),
(5, 3, 'Masini de spalat'),
(6, 3, 'De Bucatarie\r\n                                                                              '),
(7, 3, 'Climatizare'),
(8, 3, 'Aragaze, Cupoate, Hote'),
(9, 4, 'Laptop-uri'),
(10, 4, 'PC-uri'),
(11, 4, 'Telefoane'),
(12, 4, 'Tablete'),
(13, 5, 'Tricouri'),
(14, 5, 'Blugi'),
(15, 5, 'Pantaloni'),
(16, 5, 'Costume'),
(17, 5, 'Adidasi'),
(18, 5, 'Geci'),
(19, 6, 'Apartamente'),
(20, 6, 'Garsoniere'),
(21, 6, 'Case'),
(22, 6, 'Terenuri'),
(23, 6, 'Garaje'),
(24, 6, 'Spatii Comerciale'),
(25, 7, 'Tastaturi'),
(26, 7, 'Casti'),
(27, 7, 'Mouse'),
(28, 7, 'Monitoare');

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `Tranzactii`
--

CREATE TABLE `Tranzactii` (
  `id_tranzactii` int(11) NOT NULL,
  `id_utilizator` int(11) NOT NULL,
  `id_anunt` int(11) NOT NULL,
  `Detalii` longtext DEFAULT NULL,
  `DataTranzactie` datetime NOT NULL,
  `Suma` decimal(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `Utilizatori`
--

CREATE TABLE `Utilizatori` (
  `id_utilizator` int(11) NOT NULL,
  `Nume` varchar(255) NOT NULL,
  `Prenume` varchar(255) NOT NULL,
  `Email` varchar(255) NOT NULL,
  `Parola` varchar(255) NOT NULL,
  `NumarTelefon` varchar(20) NOT NULL,
  `DataNasterii` date NOT NULL,
  `Adresa` varchar(255) DEFAULT NULL,
  `id_oras` int(10) UNSIGNED DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Eliminarea datelor din tabel `Utilizatori`
--

INSERT INTO `Utilizatori` (`id_utilizator`, `Nume`, `Prenume`, `Email`, `Parola`, `NumarTelefon`, `DataNasterii`, `Adresa`, `id_oras`) VALUES
(1, 'Florea', 'Catalin-Ionut', 'fci.floreacatalinionut@gmail.com', 'parola', '0770437244', '2000-11-21', 'Dorobanti 2', 1),
(2, 'Alexandru', 'Alin', 'Alexandru.Alin@gmail.com', 'alin', '0770437246', '1999-10-10', 'Strada Marghiloman, nr14', 1);

--
-- Indexuri pentru tabele eliminate
--

--
-- Indexuri pentru tabele `Anunturi`
--
ALTER TABLE `Anunturi`
  ADD PRIMARY KEY (`id_anunturi`),
  ADD KEY `id_utilizator` (`id_utilizator`) USING BTREE,
  ADD KEY `id_subcategorie` (`id_subcategorie`);

--
-- Indexuri pentru tabele `Categorii`
--
ALTER TABLE `Categorii`
  ADD PRIMARY KEY (`id_categorie`);

--
-- Indexuri pentru tabele `Evaluari`
--
ALTER TABLE `Evaluari`
  ADD PRIMARY KEY (`id_evaluari`),
  ADD KEY `id_utilizator_evaluator` (`id_utilizator_evaluator`),
  ADD KEY `id_utilizator_evaluat` (`id_utilizator_evaluat`),
  ADD KEY `id_anunt` (`id_anunt`);

--
-- Indexuri pentru tabele `Favorite`
--
ALTER TABLE `Favorite`
  ADD PRIMARY KEY (`Id_favorite`),
  ADD KEY `id_anunt` (`id_anunt`),
  ADD KEY `id_utilizator` (`id_utilizator`) USING BTREE;

--
-- Indexuri pentru tabele `Imagini`
--
ALTER TABLE `Imagini`
  ADD PRIMARY KEY (`id_imagini`),
  ADD KEY `id_anunt` (`id_anunt`);

--
-- Indexuri pentru tabele `ListaNeagra`
--
ALTER TABLE `ListaNeagra`
  ADD PRIMARY KEY (`id_ListaNeagra`),
  ADD KEY `id_utilizator` (`id_utilizator`);

--
-- Indexuri pentru tabele `Mesaje`
--
ALTER TABLE `Mesaje`
  ADD PRIMARY KEY (`id_mesaj`),
  ADD KEY `id_utilizator_expeditor` (`id_utilizator_expeditor`),
  ADD KEY `id_utilizator_destinatar` (`id_utilizator_destinatar`),
  ADD KEY `id_anunt` (`id_anunt`);

--
-- Indexuri pentru tabele `Orase`
--
ALTER TABLE `Orase`
  ADD PRIMARY KEY (`id_oras`);

--
-- Indexuri pentru tabele `Promovari`
--
ALTER TABLE `Promovari`
  ADD PRIMARY KEY (`id_promovari`),
  ADD KEY `id_anunt` (`id_anunt`);

--
-- Indexuri pentru tabele `Subcategorii`
--
ALTER TABLE `Subcategorii`
  ADD PRIMARY KEY (`id_subcategorie`),
  ADD KEY `id_categorie` (`id_categorie`);

--
-- Indexuri pentru tabele `Tranzactii`
--
ALTER TABLE `Tranzactii`
  ADD PRIMARY KEY (`id_tranzactii`),
  ADD KEY `id_utilizator` (`id_utilizator`),
  ADD KEY `id_anunt` (`id_anunt`);

--
-- Indexuri pentru tabele `Utilizatori`
--
ALTER TABLE `Utilizatori`
  ADD PRIMARY KEY (`id_utilizator`),
  ADD KEY `fk_Utilizatori_Orase` (`id_oras`);

--
-- AUTO_INCREMENT pentru tabele eliminate
--

--
-- AUTO_INCREMENT pentru tabele `Anunturi`
--
ALTER TABLE `Anunturi`
  MODIFY `id_anunturi` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT pentru tabele `Categorii`
--
ALTER TABLE `Categorii`
  MODIFY `id_categorie` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT pentru tabele `Evaluari`
--
ALTER TABLE `Evaluari`
  MODIFY `id_evaluari` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pentru tabele `Favorite`
--
ALTER TABLE `Favorite`
  MODIFY `Id_favorite` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pentru tabele `Imagini`
--
ALTER TABLE `Imagini`
  MODIFY `id_imagini` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=36;

--
-- AUTO_INCREMENT pentru tabele `ListaNeagra`
--
ALTER TABLE `ListaNeagra`
  MODIFY `id_ListaNeagra` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT pentru tabele `Mesaje`
--
ALTER TABLE `Mesaje`
  MODIFY `id_mesaj` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT pentru tabele `Orase`
--
ALTER TABLE `Orase`
  MODIFY `id_oras` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT pentru tabele `Promovari`
--
ALTER TABLE `Promovari`
  MODIFY `id_promovari` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT pentru tabele `Subcategorii`
--
ALTER TABLE `Subcategorii`
  MODIFY `id_subcategorie` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=29;

--
-- AUTO_INCREMENT pentru tabele `Tranzactii`
--
ALTER TABLE `Tranzactii`
  MODIFY `id_tranzactii` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pentru tabele `Utilizatori`
--
ALTER TABLE `Utilizatori`
  MODIFY `id_utilizator` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Constrângeri pentru tabele eliminate
--

--
-- Constrângeri pentru tabele `Anunturi`
--
ALTER TABLE `Anunturi`
  ADD CONSTRAINT `anunturi_ibfk_1` FOREIGN KEY (`id_utilizator`) REFERENCES `Utilizatori` (`id_utilizator`),
  ADD CONSTRAINT `anunturi_ibfk_2` FOREIGN KEY (`id_subcategorie`) REFERENCES `Subcategorii` (`id_subcategorie`);

--
-- Constrângeri pentru tabele `Evaluari`
--
ALTER TABLE `Evaluari`
  ADD CONSTRAINT `evaluari_ibfk_1` FOREIGN KEY (`id_utilizator_evaluator`) REFERENCES `Utilizatori` (`id_utilizator`),
  ADD CONSTRAINT `evaluari_ibfk_2` FOREIGN KEY (`id_utilizator_evaluat`) REFERENCES `Utilizatori` (`id_utilizator`),
  ADD CONSTRAINT `evaluari_ibfk_3` FOREIGN KEY (`id_anunt`) REFERENCES `Anunturi` (`id_anunturi`);

--
-- Constrângeri pentru tabele `Favorite`
--
ALTER TABLE `Favorite`
  ADD CONSTRAINT `favorite_ibfk_1` FOREIGN KEY (`id_utilizator`) REFERENCES `Utilizatori` (`id_utilizator`),
  ADD CONSTRAINT `favorite_ibfk_2` FOREIGN KEY (`id_anunt`) REFERENCES `Anunturi` (`id_anunturi`);

--
-- Constrângeri pentru tabele `Imagini`
--
ALTER TABLE `Imagini`
  ADD CONSTRAINT `imagini_ibfk_1` FOREIGN KEY (`id_anunt`) REFERENCES `Anunturi` (`id_anunturi`);

--
-- Constrângeri pentru tabele `ListaNeagra`
--
ALTER TABLE `ListaNeagra`
  ADD CONSTRAINT `listaneagra_ibfk_1` FOREIGN KEY (`id_utilizator`) REFERENCES `Utilizatori` (`id_utilizator`);

--
-- Constrângeri pentru tabele `Mesaje`
--
ALTER TABLE `Mesaje`
  ADD CONSTRAINT `mesaje_ibfk_1` FOREIGN KEY (`id_utilizator_expeditor`) REFERENCES `Utilizatori` (`id_utilizator`),
  ADD CONSTRAINT `mesaje_ibfk_2` FOREIGN KEY (`id_utilizator_destinatar`) REFERENCES `Utilizatori` (`id_utilizator`),
  ADD CONSTRAINT `mesaje_ibfk_3` FOREIGN KEY (`id_anunt`) REFERENCES `Anunturi` (`id_anunturi`);

--
-- Constrângeri pentru tabele `Promovari`
--
ALTER TABLE `Promovari`
  ADD CONSTRAINT `promovari_ibfk_1` FOREIGN KEY (`id_anunt`) REFERENCES `Anunturi` (`id_anunturi`);

--
-- Constrângeri pentru tabele `Subcategorii`
--
ALTER TABLE `Subcategorii`
  ADD CONSTRAINT `subcategorii_ibfk_1` FOREIGN KEY (`id_categorie`) REFERENCES `Categorii` (`id_categorie`);

--
-- Constrângeri pentru tabele `Tranzactii`
--
ALTER TABLE `Tranzactii`
  ADD CONSTRAINT `tranzactii_ibfk_1` FOREIGN KEY (`id_utilizator`) REFERENCES `Utilizatori` (`id_utilizator`),
  ADD CONSTRAINT `tranzactii_ibfk_2` FOREIGN KEY (`id_anunt`) REFERENCES `Anunturi` (`id_anunturi`);

--
-- Constrângeri pentru tabele `Utilizatori`
--
ALTER TABLE `Utilizatori`
  ADD CONSTRAINT `fk_Utilizatori_Orase` FOREIGN KEY (`id_oras`) REFERENCES `Orase` (`id_oras`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
