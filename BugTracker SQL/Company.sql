CREATE TABLE Company (
	id INT NOT NULL AUTO_INCREMENT,
	company_name varchar(100) NULL,
	is_active BOOLEAN DEFAULT FALSE,
	PRIMARY KEY (id)
);
