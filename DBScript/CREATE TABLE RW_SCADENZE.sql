CREATE TABLE RW_SCADENZE
(
  IDSCADENZA NUMBER NOT NULL,
   "IDPRDMOVFASE" VARCHAR2(25 BYTE), 
   "DATA" DATE NOT NULL, 
   QTA NUMBER(9) NOT NULL,
  PRIMARY KEY (IDSCADENZA)
);

   CREATE INDEX IDX_RW_SCADENZE_1 ON RW_SCADENZE(IDPRDMOVFASE);