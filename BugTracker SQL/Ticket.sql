CREATE TABLE Tickets (
	id INT NOT NULL AUTO_INCREMENT,
	project_id INT NULL,
	title VARCHAR(100) NULL,
	description TEXT NULL,
	comment_id INT NULL,
	worker_id INT NULL,
	owner INT NULL,
	tester INT NULL,
	catagory_id INT NULL,
	type_id INT NULL,
	support_number INT NULL,
	status_id INT NULL,
	start_json JSON NULL,
	stop_json JSON NULL,
	PRIMARY KEY (id)
);