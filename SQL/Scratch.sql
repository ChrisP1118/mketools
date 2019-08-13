/*
truncate table Properties
truncate table Locations
truncate table Addresses
truncate table Streets
truncate table DispatchCalls
truncate table Crimes
*/


select count(*) from Properties
select count(*) from Parcels
select count(*) from Addresses
select count(*) from Streets
select count(*) from DispatchCalls
select count(*) from Crimes

 

select top 5 Point.STAsText(), * 
from Crimes
where POint is not null

--select * from Streets

--select * from Properties where STREET = 'WEBSTER'

/*
select * from DispatchCalls
order by [ReportedDateTime] desc
*/
 
/*
exec sp_executesql N'SET NOCOUNT ON;
INSERT INTO [Properties] ([TAXKEY], [AIR_CONDIT], [ANGLE], [ATTIC], [BASEMENT], [BATHS], [BEDROOMS], [BI_VIOL], [BLDG_AREA], [BLDG_TYPE], [CHG_NR], [CHK_DIGIT], [CONVEY_DAT], [CONVEY_FEE], [CONVEY_TYP], [CORNER_LOT], [C_A_CLASS], [C_A_EXM_IM], [C_A_EXM_LA], [C_A_EXM_TO], [C_A_EXM_TY], [C_A_IMPRV], [C_A_LAND], [C_A_SYMBOL], [C_A_TOTAL], [Centroid], [DIV_DROP], [DIV_ORG], [DPW_SANITA], [EXM_ACREAG], [EXM_PER_CT], [EXM_PER__1], [FIREPLACE], [GEO_ALDER], [GEO_ALDER_], [GEO_BI_MAI], [GEO_BLOCK], [GEO_FIRE], [GEO_POLICE], [GEO_TRACT], [GEO_ZIP_CO], [HIST_CODE], [HOUSE_NR_H], [HOUSE_NR_L], [HOUSE_NR_S], [LAND_USE], [LAND_USE_G], [LAST_NAME_], [LAST_VALUE], [LOT_AREA], [NEIGHBORHO], [NR_ROOMS], [NR_STORIES], [NR_UNITS], [OWNER_CITY], [OWNER_MAIL], [OWNER_NAME], [OWNER_NA_1], [OWNER_NA_2], [OWNER_ZIP], [OWN_OCPD], [PARCEL_TYP], [PARKING_SP], [PARKING_TY], [PLAT_PAGE], [POWDER_ROO], [P_A_CLASS], [P_A_EXM_IM], [P_A_EXM_LA], [P_A_EXM_TO], [P_A_EXM_TY], [P_A_IMPRV], [P_A_LAND], [P_A_SYMBOL], [P_A_TOTAL], [Parcel], [RAZE_STATU], [REASON_FOR], [SDIR], [STREET], [STTYPE], [SUB_ACCT], [SWIM_POOL], [TAX_DELQ], [TAX_RATE_C], [YR_ASSMT], [YR_BUILT], [ZONING])
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20, @p21, @p22, @p23, @p24, @p25, @p26, @p27, @p28, @p29, @p30, @p31, @p32, @p33, @p34, @p35, @p36, @p37, @p38, @p39, @p40, @p41, @p42, @p43, @p44, @p45, @p46, @p47, @p48, @p49, @p50, @p51, @p52, @p53, @p54, @p55, @p56, @p57, @p58, @p59, @p60, @p61, @p62, @p63, @p64, @p65, @p66, @p67, @p68, @p69, @p70, @p71, @p72, @p73, @p74, @p75, @p76, @p77, @p78, @p79, @p80, @p81, @p82, @p83, @p84, @p85, @p86, @p87);
',N'@p0 nvarchar(10),@p1 nvarchar(3),@p2 real,@p3 nvarchar(1),@p4 nvarchar(1),@p5 int,@p6 int,@p7 nvarchar(4),@p8 int,@p9 nvarchar(9),@p10 nvarchar(6),@p11 nvarchar(1),@p12 datetime2(7),@p13 real,@p14 nvarchar(2),@p15 
nvarchar(1),@p16 nvarchar(1),@p17 int,@p18 int,@p19 int,@p20 nvarchar(3),@p21 int,@p22 int,@p23 nvarchar(1),@p24 int,@p25 varbinary(22),@p26 int,@p27 int,@p28 nvarchar(2),@p29 real,@p30 real,@p31 real,@p32 nvarchar(1),@p33 
nvarchar(2),@p34 nvarchar(2),@p35 nvarchar(2),@p36 nvarchar(4),@p37 nvarchar(2),@p38 nvarchar(2),@p39 nvarchar(6),@p40 nvarchar(9),@p41 nvarchar(1),@p42 int,@p43 int,@p44 nvarchar(3),@p45 nvarchar(4),@p46 nvarchar(2),@p47 
datetime2(7),@p48 datetime2(7),@p49 int,@p50 nvarchar(4),@p51 nvarchar(4),@p52 real,@p53 int,@p54 nvarchar(23),@p55 nvarchar(28),@p56 nvarchar(28),@p57 nvarchar(28),@p58 nvarchar(28),@p59 nvarchar(9),@p60 nvarchar(1),@p61 real,@p62 real,@p63 nvarchar(2),@p64 nvarchar(5),@p65 int,@p66 nvarchar(1),@p67 int,@p68 int,@p69 int,@p70 nvarchar(3),@p71 int,@p72 int,@p73 nvarchar(1),@p74 int,@p75 varbinary(192),@p76 nvarchar(1),@p77 nvarchar(3),@p78 nvarchar(1),@p79 nvarchar(18),@p80 nvarchar(2),@p81 nvarchar(1),@p82 nvarchar(1),@p83 int,@p84 nvarchar(2),@p85 nvarchar(4),@p86 nvarchar(4),@p87 nvarchar(7)',@p0=N'7219999110',@p1=N'0',@p2=0,@p3=N'N',@p4=N'N',@p5=0,@p6=0,@p7=N'XXXX',@p8=288,@p9=N'',@p10=N'',@p11=N'',@p12='2017-02-28 00:00:00',@p13=0,@p14=N'WD',@p15=N'',@p16=N'4',@p17=0,@p18=0,@p19=0,@p20=N'',@p21=3834400,@p22=165600,@p23=N'',@p24=4000000,@p25=0xE6100000010CCAFFD9DDEA764540AFFFBE21E5F855C0,@p26=0,@p27=0,@p28=N'S2',@p29=0,@p30=0,@p31=0,@p32=N'0',@p33=N'13',@p34=N'13',@p35=N'0',@p36=N'4000',@p37=N'4',@p38=N'6',@p39=N'160100',@p40=N'53154',@p41=N'',@p42=1701,@p43=1701,@p44=N'',@p45=N'5170',@p46=N'5',@p47='2017-04-25 00:00:00',@p48='2019-06-03 00:00:00',@p49=0,@p50=N'6411',@p51=N'0',@p52=1,@p53=2,@p54=N'DALLAS, TX',@p55=N'2702 LOVE FIELD DR HDQ-7FM',@p56=N'MKE FUEL COMPANY LLC',@p57=N'',@p58=N'',@p59=N'75235',@p60=N'',@p61=0,@p62=0,@p63=N'',@p64=N'72101',@p65=0,@p66=N'4',@p67=0,@p68=0,@p69=0,@p70=N'',@p71=3834400,@p72=165600,@p73=N'',@p74=4000000,@p75=0xE610000001040A000000ACC7D5940477454011C20D50D2F855C08F35A88F04774540A4E95205D6F855C0EBE060F305774540F159E7B0E7F855C0F12799F50577454027D4C668F3F855C0F14B4285FD76454039A71670F5F855C0D580F3D6CF7645401D06C48AF6F855C0B9DD1CC8CF764540C209EF50D6F855C0C83B80A8E4764540CE42897DD4F855C03943FEBBE47645404DA81E19D5F855C0ACC7D5940477454011C20D50D2F855C001000000020000000001000000FFFFFFFF0000000003,@p76=N'9',@p77=N'Est',@p78=N'E',@p79=N'COLLEGE',@p80=N'AV',@p81=N'0',@p82=N'',@p83=99999,@p84=N'40',@p85=N'2019',@p86=N'1973',@p87=N'IL1'
*/

select Centroid.STAsText(), Outline.STAsText(), *
from Locations
where taxkey in (
	'3190455000',	-- 2100 E Webster Pl
	'3921261000',	-- 200 E Wells Ave
	'4220009000'	-- 1 Brewers Way
)

select *
from Addresses
where --dir = 'E'
	street = 'BREWERS'
	--and hse_nbr = 200

-- 1000 meters around 2100 E. Webster Pl.
declare @g geometry

select @g = Outline
from Locations
where taxkey = '3190455000'

select l.Outline.STDistance(@g) / 4326, *
from Locations l
	join Properties p on l.TAXKEY = p.TAXKEY
where l.Outline.STDistance(@g) < 0.005