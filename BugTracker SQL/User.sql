CREATE TABLE Users (
	id INT NOT NULL AUTO_INCREMENT,
	user_name VARCHAR(20) NULL,
	display_name VARCHAR(50) NULL,
	email VARCHAR(100) NULL,
	password VARCHAR(100) NULL,
	default_active_project INT NULL,
	is_active BOOLEAN DEFAULT FALSE,
	PRIMARY KEY (id)
);