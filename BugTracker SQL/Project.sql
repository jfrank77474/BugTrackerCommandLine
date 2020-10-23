CREATE TABLE Project (
	id INT NOT NULL AUTO_INCREMENT,
	company_id INT NOT NULL,
	project_name VARCHAR(100) NULL,
	epic_json JSON NULL,
	bug_json JSON NULL,
	story_json JSON NULL,
	is_active BOOLEAN DEFAULT FALSE,
	PRIMARY KEY (id)
);