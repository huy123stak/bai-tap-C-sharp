-- Import thu cong qua phpMyAdmin (tab SQL) neu muon. Ung dung cung tu tao neu chua co.
CREATE DATABASE IF NOT EXISTS student_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE student_db;

CREATE TABLE IF NOT EXISTS Students (
    Id          INT AUTO_INCREMENT PRIMARY KEY,
    HoTen       VARCHAR(100) NOT NULL,
    Email       VARCHAR(150) NOT NULL UNIQUE,
    Tuoi        INT          NOT NULL,
    SoDienThoai VARCHAR(15)  NOT NULL,
    CreatedAt   DATETIME     NOT NULL
);
