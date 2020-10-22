select top 1 PP.payroll_process_id 
from payroll_process PP
       Inner join payroll_period PPER on (PPER.payroll_period_id = PP.payroll_period_id)
where PPER.code_payroll_process_group_cd = 'Biweekly'
order by PP.payroll_process_id desc;

select top 1 PP.payroll_process_id 
from payroll_process PP
       Inner join payroll_period PPER on (PPER.payroll_period_id = PP.payroll_period_id)
where PPER.code_payroll_process_group_cd = 'Biweekly' and PP.processed_date is not null
order by PP.processed_date desc;

SELECT * FROM payroll_process WHERE payroll_process_id = 365;
SELECT * FROM payroll_period WHERE code_payroll_process_group_cd = 'Biweekly';

