-- Postgres SQL 10
-- from command prompt:
--   psql -U Postgres -f c:\data\projects\ohsmon\ohsmon\dbscript.sql
--      note: user Postgres password was set at installation.  JRA used "Orasi"
\c ohsmonitor

-- DROP TABLE public."MonitorItems";
-- \c postgres
-- DROP DATABASE ohsmonitor;
-- DROP USER monroot;

CREATE USER monroot WITH
  LOGIN
  SUPERUSER
  INHERIT
  CREATEDB
  CREATEROLE
  REPLICATION;
CREATE DATABASE ohsmonitor
    WITH 
    OWNER = monroot
    ENCODING = 'UTF8'
    LC_COLLATE = 'English_United States.1252'
    LC_CTYPE = 'English_United States.1252'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;
\c xohsmonitor
CREATE TABLE public."MonitorItems"
(
    "RecordID" integer NOT NULL DEFAULT nextval('"MonitorItems_RecordID_seq"'::regclass),
    "ClientID" text COLLATE pg_catalog."default",
    "Date" date NOT NULL,
    "Memo" text COLLATE pg_catalog."default",
    "ResponseTime" oid NOT NULL,
    "Time" interval NOT NULL,
    "Type" text COLLATE pg_catalog."default",
    CONSTRAINT "PK_MonitorItems" PRIMARY KEY ("RecordID")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."MonitorItems"
    OWNER to monroot;