CREATE TABLE IF NOT EXISTS "Benchmarks"
(
	process varchar (255) NOT NULL,
	cpu double precision NOT NULL,
	disk double precision NOT NULL,
	ram double precision NOT NULL,
	energy double precision NOT NULL,
	id integer NOT NULL, date DATE NOT NULL, ip VARCHAR(15) NOT NULL DEFAULT '0.0.0.0',
	PRIMARY KEY(id)
);
CREATE TABLE Users (
    id INTEGER NOT NULL,
    email TEXT NOT NULL UNIQUE,
    password TEXT NOT NULL,
    premiumEndDate DATE NOT NULL DEFAULT '1900-01-01',
    PRIMARY KEY(id)
);
CREATE TABLE Apps(version varchar (10) NOT NULL, dllink varchar(255) NOT NULL, date DATE NOT NULL DEFAULT CURRENT_TIMESTAMP, PRIMARY KEY(version));
CREATE TABLE AuthTokens(
token TEXT NOT NULL UNIQUE,
id INTEGER NOT NULL, userID INTEGER NOT NULL REFERENCES User(id), validBefore DATETIME NOT NULL,
PRIMARY KEY(id)
);
CREATE TABLE ConfirmationTokens(
email TEXT NOT NULL UNIQUE,
token varchar(255) NOT NULL,
password TEXT NOT NULL,
expiration_time DATETIME NOT NULL,
PRIMARY KEY(token));
/* bytecode(addr,opcode,p1,p2,p3,p4,p5,comment,subprog) */;
/* completion(candidate) */;
/* dbstat(name,path,pageno,pagetype,ncell,payload,unused,mx_payload,pgoffset,pgsize) */;
/* fsdir(name,mode,mtime,data) */;
/* fts3tokenize(input,token,start,"end",position) */;
/* generate_series(value) */;
/* json_each("key",value,type,atom,id,parent,fullkey,path) */;
/* json_tree("key",value,type,atom,id,parent,fullkey,path) */;
/* pragma_database_list(seq,name,file) */;
/* pragma_module_list(name) */;
/* sqlite_dbdata(pgno,cell,field,value) */;
/* sqlite_dbpage(pgno,data) */;
/* sqlite_dbptr(pgno,child) */;
/* sqlite_stmt(sql,ncol,ro,busy,nscan,nsort,naidx,nstep,reprep,run,mem) */;
/* tables_used(type,schema,name,wr,subprog) */;
/* zipfile(name,mode,mtime,sz,rawdata,data,method) */;
