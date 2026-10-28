-- Import thu cong qua phpMyAdmin (tab SQL) neu muon. Ung dung cung tu tao neu chua co.
CREATE DATABASE IF NOT EXISTS calculator_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE calculator_db;

CREATE TABLE IF NOT EXISTS History (
    Id         BIGINT AUTO_INCREMENT PRIMARY KEY,
    Expression VARCHAR(255) NOT NULL,
    Result     DOUBLE       NOT NULL,
    CreatedAt  DATETIME     NOT NULL
);
