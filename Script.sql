create database Projetoasp;

use Projetoasp;

Create table usuarios(
	id int primary key auto_increment,
    Nome varchar(100),
    Email varchar(50),
    Senha varchar(50)
);

Create table produtos(
	id int primary key auto_increment,
	Nome varchar(100),
    descricao varchar(100),
    preco decimal(10,2),
    quantidade int
);