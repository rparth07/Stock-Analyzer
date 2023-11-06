delete from dbo.BhavCopyInfo where Date='2023-10-31';


select * from dbo.BhavCopyInfo where Date > '2023-10-30' and Date <= '2023-10-31' and CompanyId='6738454A-FE2C-411F-3867-08DBC103D682';

select * from Company where Symbol = '20MICRONS';

select * from FilterResult where CalculationDate='2023-10-31';

select CalculationDate, Count(Id) from FilterResult group by CalculationDate order by CalculationDate;
'2023-10-27 12:52:16.0000000';

select Max(CalculationDate) from FilterResult;
select Max(Date) from BhavCopyInfo;
select Max(DealDate) from BulkDeal;