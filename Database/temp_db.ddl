#@(#) script.ddl

DROP TABLE IF EXISTS BENCHMARK;

CREATE TABLE BENCHMARK
(
	id integer NOT NULL AUTO_INCREMENT,
	date DATE NOT NULL,
	process varchar (255) NOT NULL,
	cpu double precision NOT NULL,
	disk double precision NOT NULL,
	ram double precision NOT NULL,
	energy double precision NOT NULL,
	PRIMARY KEY(id)
);