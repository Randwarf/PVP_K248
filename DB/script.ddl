#@(#) script.ddl

DROP TABLE IF EXISTS PROCESAS;
DROP TABLE IF EXISTS IRANGA;
CREATE TABLE IRANGA
(
	kompanija varchar (255) NOT NULL,
	modelis varchar (255) NOT NULL,
	isnaudojimas double precision NOT NULL,
	kaina double precision NOT NULL,
	id_IRANGA integer NOT NULL,
	PRIMARY KEY(id_IRANGA)
);

CREATE TABLE PROCESAS
(
	programa varchar (255) NOT NULL,
	cpu double precision NOT NULL,
	diskas int NOT NULL,
	ram int NOT NULL,
	laikas int NOT NULL,
	id_PROCESAS integer NOT NULL,
	fk_IRANGAid_IRANGA integer NOT NULL,
	PRIMARY KEY(id_PROCESAS),
	CONSTRAINT naudoja FOREIGN KEY(fk_IRANGAid_IRANGA) REFERENCES IRANGA (id_IRANGA)
);
