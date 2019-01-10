CREATE SEQUENCE LANCIO_SEQUENCE START WITH 1 INCREMENT BY 1 CACHE 2;

  CREATE TABLE PIANIFICAZIONE_LANCIO 
   (	IDLANCIO NUMBER NOT NULL, 
   AZIENDA VARCHAR2(2) NULL,
	"IDLANCIOT" VARCHAR2(25 BYTE), 
  "IDLANCIOD" VARCHAR2(25 BYTE), 
	"IDMAGAZZ" VARCHAR2(10 BYTE), 
	"IDDIBAMETHID" VARCHAR2(10 BYTE), 
	"VERSION" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"QTALANCIO" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"DATACOMMESSA" DATE, 
	"NOMECOMMESSA" VARCHAR2(50 BYTE), 
	"CODICECLIFO" CHAR(10 BYTE), 
	"DATDOC" DATE, 
	"NUMDOC" VARCHAR2(25 BYTE), 
	"DATARIF" DATE, 
	"RIFERIMENTO" VARCHAR2(100 BYTE), 
	"SEGNALATORE" CHAR(10 BYTE), 
	"RIFERIMENTO_INFRA" VARCHAR2(50 BYTE), 
	"DATARIF_INFRA" DATE, 
	"NRRIGA" VARCHAR2(5 BYTE), 
	"RIFERIMENTORIGA" VARCHAR2(50 BYTE), 
	"DATARIFRIGA" DATE, 
	 PRIMARY KEY (IDLANCIO)
   );
   
  CREATE INDEX IDX_PIA_LANCIO_1 ON PIANIFICAZIONE_LANCIO(IDLANCIOD);
  CREATE INDEX IDX_PIA_LANCIO_2 ON PIANIFICAZIONE_LANCIO(IDMAGAZZ);
  CREATE INDEX IDX_PIA_LANCIO_3 ON PIANIFICAZIONE_LANCIO(NOMECOMMESSA);
         
  CREATE TABLE PIANIFICAZIONE_FASE
   (	
   IDFASE NUMBER NOT NULL, 
   IDLANCIO NUMBER,
   AZIENDA VARCHAR2(2) NULL,
   STATO VARCHAR2(20) NULL,
IDFASEPADRE NUMBER null,
	"IDPRDFASE" VARCHAR2(25 BYTE), 
  	"IDPRDFASEPADRE" VARCHAR2(25 BYTE), 
	"CODICECLIFO" CHAR(10 BYTE), 
	"DATAINIZIO" DATE, 
	"DATAFINE" DATE, 
	"OFFSETTIME" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"LEADTIME" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"IDMAGAZZ" VARCHAR2(10 BYTE), 
	"IDDIBAMETHOD" VARCHAR2(10 BYTE), 
	"VERSION" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"QTA" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"QTAANN" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"QTANET" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"QTAASS" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"QTACON" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"QTATER" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"QTADATER" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"QTAACC" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"QTADAC" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"BARCODE" VARCHAR2(13 BYTE), 
	"QTALAV" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"QTADAPIA" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"QTAPIA" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"QTAACCLE" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"RIFERIMENTO_INFRA" VARCHAR2(50 BYTE), 
	"DATARIF_INFRA" DATE, 
  ATTENDIBILITA NUMBER NULL,
	 PRIMARY KEY (IDFASE));

CREATE INDEX IDX_PIA_FASE_1 ON PIANIFICAZIONE_FASE(IDPRDFASE);
CREATE INDEX IDX_PIA_FASE_2 ON PIANIFICAZIONE_FASE(IDFASEPADRE);
CREATE INDEX IDX_PIA_FASE_3 ON PIANIFICAZIONE_FASE(IDPRDFASEPADRE);

  CREATE TABLE PIANIFICAZIONE_LOG
   (	IDLOG NUMBER GENERATED BY DEFAULT ON NULL AS IDENTITY, 
    DATA DATE NULL,
   TIPO VARCHAR2(20 BYTE) NOT NULL ,
   NOTA VARCHAR2(500 BYTE) NOT NULL ,
   PRIMARY KEY (IDLOG));
   
    CREATE TABLE PIANIFICAZIONE_ODL
   (	
   IDODL NUMBER NOT NULL, 
  "IDLANCIOD" VARCHAR2(25 BYTE), 
  "IDPRDFASE" VARCHAR2(25 BYTE), 
   AZIENDA VARCHAR2(2) NULL,
   STATO VARCHAR2(20) NULL,
	 "IDPRDMOVFASE" VARCHAR2(25 BYTE), 
	"CODICECLIFO" CHAR(10 BYTE), 
	"DATAINIZIO" DATE, 
	"DATAFINE" DATE, 
	"OFFSETTIME" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"LEADTIME" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"IDMAGAZZ" VARCHAR2(10 BYTE), 
	"QTA" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"QTAANN" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"QTANET" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"QTAASS" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"QTACON" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"QTATER" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"QTADATER" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"QTAACC" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"QTADAC" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"BARCODE" VARCHAR2(13 BYTE), 
	"QTALAV" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"QTADAPIA" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"QTAPIA" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"QTAACCLE" FLOAT(126) DEFAULT 0 NOT NULL ENABLE, 
	"RIFERIMENTO_INFRA" VARCHAR2(50 BYTE), 
	"DATARIF_INFRA" DATE, 
  ATTENDIBILITA NUMBER NULL,
	DATA_ELABORAZIONE DATE, 
	 PRIMARY KEY (IDODL));