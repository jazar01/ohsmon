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
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;
\c ohsmonitor
CREATE SEQUENCE public."MonitorItems_RecordID_seq"
    INCREMENT 1
    START 17
    MINVALUE 1
    MAXVALUE 2147483647
    CACHE 1;

ALTER SEQUENCE public."MonitorItems_RecordID_seq"
    OWNER TO monroot;
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