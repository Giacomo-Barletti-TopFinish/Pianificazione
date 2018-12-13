select * from ditta1.usr_prd_fasi
where (DATAINIZIO >= to_date('10/12/2018', 'DD/MM/YYYY') and DATAINIZIO <= to_date('11/12/2018', 'DD/MM/YYYY')) OR
(DATAFINE >= to_date('10/12/2018', 'DD/MM/YYYY') and DATAFINE <= to_date('11/12/2018', 'DD/MM/YYYY'))



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

select * from pianificazione_log order by idlog desc


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

