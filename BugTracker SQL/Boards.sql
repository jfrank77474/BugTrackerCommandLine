CREATE TABLE Boards (
	id INT NOT NULL AUTO_INCREMENT,
	default_boards varchar(50) NULL,
	user_named_boards varchar(50) NULL,
	sort_order INT NULL,
	PRIMARY KEY (id)
);