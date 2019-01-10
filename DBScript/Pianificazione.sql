select * from ditta1.usr_prd_fasi
where DATAINIZIO <= to_date('11/12/2018', 'DD/MM/YYYY') AND
DATAFINE >= to_date('10/12/2018', 'DD/MM/YYYY') 
--1991

select mf.idprdmovfase,fa.* from pianificazione_fase fa
left join usr_prd_movfasi mf on fa.idprdfase = mf.idprdfase
where fa.DATAINIZIO <= to_date('10/01/2019', 'DD/MM/YYYY') AND
fa.DATAFINE >= to_date('21/12/2018', 'DD/MM/YYYY') 
and fa.stato = 'PIANIFICATO'


select distinct idlanciod from ditta1.usr_prd_fasi
where (DATAINIZIO >= to_date('10/12/2018', 'DD/MM/YYYY') and DATAINIZIO <= to_date('11/12/2018', 'DD/MM/YYYY')) OR
(DATAFINE >= to_date('10/12/2018', 'DD/MM/YYYY') and DATAFINE <= to_date('11/12/2018', 'DD/MM/YYYY'))
-- 105

select * from ditta1.usr_prd_fasi fas1
where idlanciod in (select distinct idlanciod from ditta1.usr_prd_fasi
where (DATAINIZIO >= to_date('1/12/2018', 'DD/MM/YYYY') and DATAINIZIO <= to_date('11/12/2018', 'DD/MM/YYYY')) OR
(DATAFINE >= to_date('1/12/2018', 'DD/MM/YYYY') and DATAFINE <= to_date('11/12/2018', 'DD/MM/YYYY')))

select * from ditta1.usr_prd_lanciod where datacr >to_date('01/01/2018', 'DD/MM/YYYY')


select * from ditta1.usr_prd_fasi
where (DATAINIZIO >= to_date('11/12/2018', 'DD/MM/YYYY') and DATAINIZIO <= to_date('11/12/2018', 'DD/MM/YYYY')) OR
(DATAFINE >= to_date('11/12/2018', 'DD/MM/YYYY') and DATAFINE <= to_date('11/12/2018', 'DD/MM/YYYY'))
and qtadater >0


select * from ditta1.usr_prd_fasi where idlanciod = '0000000000000000000051910' order by datainizio

select odl.idprdmovfase, fas.* from ditta1.usr_prd_fasi fas 
left join ditta1.usr_prd_movfasi odl on odl.idprdfase = fas.idprdfase
where idlanciod = '0000000000000000000051910' order by fas.datainizio

select * from ditta1.usr_prd_movfasi where idprdfase = '0000000000000000000403373' order by datamovfase

select * from ditta1.usr_prd_movfasi where idprdfase = '0000000000000000000403374' order by datamovfase

select * from ditta1.usr_prd_fasi where idprdfasepadre = '0000000000000000000403373' order by datainizio


select * from ditta1.usr_prd_lanciod where nomecommessa like '%12724'
0000000000000000000056711   PRO/2018/0000017330
                            PRO/2018/0000012724
select * from ditta1.usr_prd_lanciod where nomecommessa = 'PRO/2018/0000012724'

select * from ditta1.usr_prd_lanciod where idlanciod='0000000000000000000044473'

                            
select * from ditta1.usr_prd_fasi where idlanciod = '0000000000000000000044473'

select mf.* from ditta1.usr_prd_movfasi mf 
inner join ditta1.usr_prd_fasi fa on fa.idprdfase = mf.idprdfase
where fa.idlanciod = '0000000000000000000044473'

select mf.* from ditta1.usr_prd_movfasi mf 
inner join ditta1.usr_prd_fasi fa on fa.idprdfase = mf.idprdfase
inner join ditta1.usr_prd_lanciod la on fa.idlanciod = la.idlanciod
where la.nomecommessa = 'PRO/2018/0000012724'
order by mf.idprdfase, mf.idprdmovfase


select * from ditta1.usr_prd_fasi where idprdfase = '0000000000000000000353949'

select * from odl_aperti

select tab.* from (
select 'MP' as azienda , m.* from ditta1.usr_prd_movfasi m where qtadater > 0
union all 
select 'TF' as azienda,m.* from ditta2.usr_prd_movfasi m where qtadater > 0
)  tab
order by tab.idprdfase

create OR REPLACE view USR_PRD_FASI AS
select 'MP' as azienda , m.* from ditta1.usr_prd_fasi m 
union all 
select 'TF' as azienda,m.* from ditta2.usr_prd_fasi m 

create OR REPLACE view USR_PRD_MOVFASI AS
select 'MP' as azienda , m.* from ditta1.usr_prd_MOVfasi m 
union all 
select 'TF' as azienda,m.* from ditta2.usr_prd_MOVfasi m 

create OR REPLACE view USR_PRD_LANCIOD AS
select 'MP' as azienda , m.* from ditta1.usr_prd_LANCIOD m 
union all 
select 'TF' as azienda,m.* from ditta2.usr_prd_LANCIOD m 

SELECT COUNT(*) FROM USR_PRD_MOVFASI WHERE QTADATER > 0

SELECT COUNT(DISTINCT FA.IDPRDFASE) FROM USR_PRD_FASI FA
INNER JOIN USR_PRD_MOVFASI MF ON MF.IDPRDFASE = FA.IDPRDFASE
WHERE MF.QTADATER > 0

SELECT COUNT(DISTINCT LA.IDLANCIOD) FROM USR_PRD_LANCIOD LA 
INNER JOIN USR_PRD_FASI FA ON FA.IDLANCIOD = LA.IDLANCIOD
INNER JOIN USR_PRD_MOVFASI MF ON MF.IDPRDFASE = FA.IDPRDFASE
WHERE MF.QTADATER > 0

select sysdate from dual

SELECT * FROM USR_PRD_MOVFASI WHERE QTADATER > 0

SELECT FA.* FROM USR_PRD_FASI FA
INNER JOIN USR_PRD_MOVFASI MF ON MF.IDPRDFASE = FA.IDPRDFASE
WHERE MF.QTADATER > 0
ORDER BY FA.IDPRDFASE

SELECT LA.* FROM USR_PRD_LANCIOD LA 
INNER JOIN USR_PRD_FASI FA ON FA.IDLANCIOD = LA.IDLANCIOD
INNER JOIN USR_PRD_MOVFASI MF ON MF.IDPRDFASE = FA.IDPRDFASE
WHERE MF.QTADATER > 0

SELECT IDPRDFASE, COUNT(*) FROM USR_PRD_MOVFASI
GROUP BY IDPRDFASE
ORDER BY IDPRDFASE
HAVING COUNT(*) > 1

SELECT * FROM USR_PRD_MOVFASI WHERE IDPRDFASE = '0000000000000000000078871'

SELECT * FROM USR_PRD_FASI WHERE IDPRDFASE = '0000000000000000000078871'

SELECT * FROM USR_PRD_FASI WHERE IDPRDFASEPADRE = '0000000000000000000078871'
SELECT * FROM USR_PRD_MOVFASI WHERE IDPRDFASE = '0000000000000000000078877'

select idlanciod,count(*) from usr_prd_fasi group by idlanciod

select * from usr_prd_fasi where idlanciod = '0000000000000000000059778' order by idprdfase

TRUNCATE TABLE PIANIFICAZIONE_LOG;

  SELECT TO_CHAR(SYSDATE, 'YYYY-MM-DD HH24:MI:SS') FROM dual;

select TO_CHAR(l.data, 'DD-MON-YYYY HH24:MI:SS'),
 l.* from pianificazione_log l order by idlog desc

select * from pianificazione_log  where tipo = 'ERRORE'


select distinct tipo from pianificazione_log

select * from pianificazione_lancio 

select * from pianificazione_fase 
Elaborazione IDLANCIOD: 0000000000000000000056796


select * from usr_prd_fasi where idlanciod = '0000000000000000000056796'

select * from usr_prd_movfasi mf 
inner join usr_prd_fasi fa on fa.idprdfase = mf.idprdfase
where fa.idlanciod = '0000000000000000000056796'

TRUNCATE TABLE PIANIFICAZIONE_LANCIO;
TRUNCATE TABLE PIANIFICAZIONE_FASE;
TRUNCATE TABLE PIANIFICAZIONE_LOG;

select * from pianificazione_fase where idlancio = 52507 order by idfase

select * from pianificazione_fase 
where stato <>'CHIUSO' AND codiceclifo = 'GALVA' and ((DATAINIZIO >= to_date('10/12/2018', 'DD/MM/YYYY') and DATAINIZIO <= to_date('11/12/2018', 'DD/MM/YYYY')) OR
(DATAFINE >= to_date('10/12/2018', 'DD/MM/YYYY') and DATAFINE <= to_date('11/12/2018', 'DD/MM/YYYY')))

select * from usr_prd_fasi where idprdfase = '0000000000000000000358630'

select * from usr_prd_movfasi where idprdfase = '0000000000000000000358630'

select * from monitor_scheduler order by idscheduler desc


select la.idmagazz,la.datacommessa, la.nomecommessa,pf.modello modellofinito,ar.modello,fa.stato,
fa.idfase,fa.codiceclifo,fa.datainizio, fa.datafine, fa.qta, fa.qtadater, fa.qtater
from PIANIFICAZIONE_FASE fa
inner join pianificazione_lancio la on la.idlancio = fa.idlancio
inner join es_diba db on db.idarticolo = fa.idmagazz
inner join gruppo.magazz ar on ar.idmagazz=fa.idmagazz
inner join gruppo.magazz pf on pf.idmagazz=db.idprodottofinito
where ((DATAINIZIO >= to_date('10/12/2018', 'DD/MM/YYYY') and DATAINIZIO <= to_date('16/12/2018', 'DD/MM/YYYY')) OR
(DATAFINE >= to_date('10/12/2018', 'DD/MM/YYYY') and DATAFINE <= to_date('16/10/2018', 'DD/MM/YYYY')))
and nomecommessa = 'STC/2018/0000016334'

SELECT * FROM BOLLE_VENDITA
WHERE 
    DATDOC >= TO_DATE(?,'DD-MM-YYYY')
AND DATDOC<= TO_DATE(?,'DD-MM-YYYY')

select * from es_diba

TRUNCATE TABLE PIANIFICAZIONE_LOG;

select * from pianificazione_log order by idlog desc;

select * from monitor_scheduler where ESEGUITA = 'N';

select distinct tipo from pianificazione_log ;

select fa.azienda,la.idmagazz,fa.idmagazz idmagazz_fase,la.datacommessa, la.nomecommessa,pf.modello modelloLancio,ar.modello,fa.stato,
fa.idfase,fa.codiceclifo,trim(cli.ragionesoc), fa.datainizio, fa.datafine, fa.qta, fa.qtadater, fa.qtater
from PIANIFICAZIONE_FASE fa
inner join pianificazione_lancio la on la.idlancio = fa.idlancio
inner join gruppo.magazz ar on ar.idmagazz=fa.idmagazz
inner join gruppo.magazz pf on pf.idmagazz=la.idmagazz
inner join gruppo.clifo cli on cli.codice = fa.codiceclifo
where ((DATAINIZIO >= to_date('10/12/2018', 'DD/MM/YYYY') and DATAINIZIO <= to_date('16/12/2018', 'DD/MM/YYYY')) OR
(DATAFINE >= to_date('10/12/2018', 'DD/MM/YYYY') and DATAFINE <= to_date('16/10/2018', 'DD/MM/YYYY')))
and nomecommessa = 'STC/2018/0000016334'


SELECT * FROM PIANIFICAZIONE_FASE 
WHERE ( DATAINIZIO <= to_date('13/12/2018 23:59:59','DD/MM/YYYY HH24:MI:SS') AND DATAINIZIO >= to_date('11/12/2018 00:00:01','DD/MM/YYYY HH24:MI:SS') ) 
OR ( DATAFINE <= to_date('13/12/2018 23:59:59','DD/MM/YYYY HH24:MI:SS') AND DATAFINE >= to_date('11/12/2018 00:00:01','DD/MM/YYYY HH24:MI:SS') )
order by idlancio


select * from ditta2.usr_prd_lanciod where idlanciod = '0000000000000000000025994'
select * from ditta2.usr_prd_fasi where idprdfase = '0000000000000000000223619'
select * from ditta2.usr_prd_movfasi where idprdfase = '0000000000000000000223619'
union all
select * from ditta2.usr_prd_movfasi where idprdfase = '0000000000000000000223618'

select * from ditta2.usr_prd_fasi where idlanciod = '0000000000000000000025994'


select * from pianificazione_lancio where idlanciod = '0000000000000000000025994'
37213

select * from pianificazione_fase where idlancio in( 37213,37173)

select * from pianificazione_lancio where nomecommessa like '%18576%'


select fa.* from pianificazione_fase fa
inner join pianificazione_lancio la on la.idlancio = fa.idlancio
where la.nomecommessa like '%19043%'
order by fa.idlancio, fa.idfase

select * from pianificazione_fase where idlancio = 76868

select codiceclifo,count(*) from pianificazione_fase where idlancio = 76868
group by codiceclifo

ord
select * from pianificazione_lancio where idlancio in (76868)
0000000000000000000060293

select * from ditta2.usr_prd_flusso_movfasi where idprdmovfase = '0000000000000000000263521'

select * from ditta2.usr_prd_lanciod pl
inner join pianificazione_lancio la on la.idlanciod = pl.idlanciod
where la.idlancio in (76868)

select * from usr_prd_fasi where idlanciod = '0000000000000000000060293'



select mf.idprdmovfase,fa.* from pianificazione_fase fa
left join usr_prd_movfasi mf on fa.idprdfase = mf.idprdfase AND FA.STATO = 'APERTO'
where fa.DATAINIZIO <= to_date('10/01/2019', 'DD/MM/YYYY') AND
fa.DATAFINE >= to_date('27/12/2018', 'DD/MM/YYYY') 
and fa.stato = 'PIANIFICATO'




select * from usr_prd_movfasi where idprdmovfase = '0000000000000000000626491'
0000000000000000000464319


select fa.azienda,la.idmagazz,fa.idmagazz idmagazz_fase,la.datacommessa, la.nomecommessa,pf.modello modelloLancio,ar.modello,fa.stato,
fa.idfase,fa.codiceclifo as reparto,trim(cli.ragionesoc), fa.datainizio, fa.datafine, 
nvl(mf.qta,fa.qta) qta, 
nvl(mf.qtadater,fa.qtadater) qtadater, 
nvl(mf.qtater,fa.qtater) qtater, 
nvl(mf.qtaann,fa.qtaann) qtaann, 
fa.idfasepadre, fa.idlancio,
climf.codice as lavorante,trim(climf.ragionesoc),mf.qta as qta_odl,mf.qtadater as qtadater_odl, mf.qtater as qtater_odl, mf.qtaann as qtaann_odl
from PIANIFICAZIONE_FASE fa
inner join pianificazione_lancio la on la.idlancio = fa.idlancio
inner join gruppo.magazz ar on ar.idmagazz=fa.idmagazz
inner join gruppo.magazz pf on pf.idmagazz=la.idmagazz
inner join gruppo.clifo cli on cli.codice = fa.codiceclifo 
left join usr_prd_movfasi mf on fa.idprdfase = mf.idprdfase AND FA.STATO = 'APERTO'
left join gruppo.clifo climf on climf.codice = mf.codiceclifo 
order by idfase

where fa.DATAINIZIO <= to_date(?, 'DD/MM/YYYY') AND 
fa.DATAFINE >= to_date(?, 'DD/MM/YYYY') order by fa.idlancio, fa.idfase

select TO_CHAR(l.data, 'DD-MON-YYYY HH24:MI:SS'),
 l.* from pianificazione_log l 
 where tipo = 'END' or tipo = 'START'
 order by idlog desc

select * from pianificazione_fase where idfase = 39629

select * from pianificazione_lancio where idlancio = 39624

select * from usr_prd_movfasi where idprdmovfase = '0000000000000000000320746'

select * from usr_prd_fasi where idprdfase = '0000000000000000000432697'

select * from usr_prd_movfasi where idprdfase = '0000000000000000000432697'

select * from usr_prd_fasi where idlanciod = '0000000000000000000055559'

select * from usr_prd_fasi fa
inner join usr_prd_movfasi mf on mf.idprdfase = fa.idprdfase
where idlanciod = '0000000000000000000034165'


TRUNCATE TABLE PIANIFICAZIONE_ODL;

TRUNCATE TABLE pianificazione_log;


select * from pianificazione_log order by idlog desc;


SELECT DISTINCT TF.* FROM USR_PRD_FASI TF 
                            INNER JOIN USR_PRD_LANCIOD LA ON TF.IDLANCIOD = LA.IDLANCIOD
                            INNER JOIN USR_PRD_FASI FA ON FA.IDLANCIOD = LA.IDLANCIOD
                            INNER JOIN USR_PRD_MOVFASI MF ON MF.IDPRDFASE = FA.IDPRDFASE
                            WHERE MF.QTADATER > 0
                            


SELECT * FROM PIANIFICAZIONE_ODL ORDER BY idlanciod,IDPRDMOVFASE, ATTENDIBILITA 

select * from usr_prd_fasi where idprdfase = '0000000000000000000416478'
select * from usr_prd_fasi where idprdfase = '0000000000000000000416512'

select * from usr_prd_fasi where idprdfasepadre = '0000000000000000000416478'

select count(*),idprdfasepadre from usr_prd_fasi group by idprdfasepadre having count(*) > 3

select * from ditta1.usr_prd_rdiba where idmagazz = 0000077693

select * from es_diba where idarticolo = 0000077693