
-- PostgresSQL

-- Database: ohsmonitor

-- DROP DATABASE ohsmonitor;

CREATE DATABASE ohsmonitor
    WITH 
    OWNER = monroot
    ENCODING = 'UTF8'
    LC_COLLATE = 'English_United States.1252'
    LC_CTYPE = 'English_United States.1252'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;

-- Table: public."MonitorItems"

-- DROP TABLE public."MonitorItems";

CREATE TABLE public."MonitorItems"
(
    "ClientID" text COLLATE pg_catalog."default" NOT NULL,
    "Date" date NOT NULL,
    "Memo" text COLLATE pg_catalog."default",
    "ResponseTime" bigint NOT NULL,
    "Time" interval NOT NULL,
    "Type" text COLLATE pg_catalog."default",
    CONSTRAINT "PK_MonitorItems" PRIMARY KEY ("ClientID")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."MonitorItems"
    OWNER to monroot;