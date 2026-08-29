-- =====================================================================================
-- Hand-port: SalaryCalculationTb_MoneyCard. Read-only. MariaDB 10.4 has no OUTER APPLY (LATERAL). Its two
-- correlated APPLYs are rewritten as inline correlated subqueries:
--   acc      (SUM(Credit)-SUM(Debit) for the emp-account in that salary month)  -> a scalar subquery
--   lastMove (TOP 1 year/month of the last movement, ORDER BY InsertDate DESC)  -> two scalar subqueries
--            (Y and M), each LIMIT 1 — a correlated subquery returns one column, so the two columns
--            become two subqueries with the SAME filter/order (same row -> same Y and M). No-match -> NULL,
--            exactly as the OUTER (left) APPLY produced.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

DROP PROCEDURE IF EXISTS `SalaryCalculationTb_MoneyCard`;
DELIMITER $$
CREATE PROCEDURE `SalaryCalculationTb_MoneyCard`(IN `p_EmpID` INT)
BEGIN


    SELECT
            a.SalaryVal,

            CASE a.SALARYMONTH                      WHEN 1                        THEN                        'يناير'                      WHEN 2                        THEN                        'فبراير'                      WHEN 3                        THEN                        'مارس'                      WHEN 4                        THEN                        'ابريل'                      WHEN 5                        THEN                        'مايو'                      WHEN 6                        THEN                        'يونيو'                      WHEN 7                        THEN                        'يوليو'                      WHEN 8                        THEN                        'اغسطس'                      WHEN 9                        THEN                        'سبتمبر'                      WHEN 10                        THEN                        'اكتوبر'                      WHEN 11                        THEN                        'نوفمبر'                      WHEN 12                        THEN                        'ديسمبر'              END                                          AS SALARYMONTH,
      case when       CAST(b.EndofEmpDate AS CHAR) is null then ':حالة الموظف' else ':تاريخ إخلاء الطرف' END as EmpStatus,
      case when       CAST(b.EndofEmpDate AS CHAR) is null then 'مستمر' else CAST(b.EndofEmpDate AS CHAR) END as EndofEmpDate,
            a.SALARYEAR,
            b.EMPNAME,
            b.NATNUMBER,
            c.BName,
            d.ECNAME,
            b.CODE,
            b.PassportNo,
            b.PHONE1,
            DATE_FORMAT(b.EMPDATE, '%Y-%m-%d')              AS EMPDATE,

            a.SALARYTOTAL,


            CASE                      WHEN a.IsIndivdual = 1                        THEN                        a.DayNumber                      ELSE                      30              END                                          AS `عدد الأيام`,


            IFNULL(e.TotalLeaveDiscount, 0)              AS `خصم إجازة`,


            FORMAT(IFNULL((SELECT SUM(s.Credit) - SUM(s.Debit) FROM ExSyAccounts_AccSafeActivityTb s WHERE s.AccIDFrom = b.AccID AND YEAR(s.InsertDate) = a.SALARYEAR AND MONTH(s.InsertDate) = a.SALARYMONTH), 0), 3) AS `صافي الحساب`,


            FORMAT(CASE                                             WHEN b.EndofEmpDate IS NOT NULL                          THEN                        CASE                                                               WHEN a.SALARYEAR = (SELECT YEAR(s.InsertDate) FROM ExSyAccounts_AccSafeActivityTb s WHERE s.AccIDFrom = b.AccID ORDER BY s.InsertDate DESC LIMIT 1)                                  AND a.SALARYMONTH = (SELECT MONTH(s.InsertDate) FROM ExSyAccounts_AccSafeActivityTb s WHERE s.AccIDFrom = b.AccID ORDER BY s.InsertDate DESC LIMIT 1)                                  THEN                                  IFNULL((SELECT SUM(s.Credit) - SUM(s.Debit) FROM ExSyAccounts_AccSafeActivityTb s WHERE s.AccIDFrom = b.AccID AND YEAR(s.InsertDate) = a.SALARYEAR AND MONTH(s.InsertDate) = a.SALARYMONTH), 0)                                ELSE                                a.SALARYTOTAL                        END                                             ELSE                      a.SALARYTOTAL                END, 3)                                            AS `صافي الشهر`,

            FORMAT(a.ConstanceVal + a.BONUSVAL, 0)    AS BONUSVAL,
            FORMAT(a.DiscountsVal, 0)                 AS DiscountsVal,
            FORMAT(a.AdvancePaymentDisc, 0)           AS AdvancePaymentDisc,

            FORMAT(a.SalaryVal + a.ConstanceVal + a.BONUSVAL
            - a.DiscountsVal - IFNULL(e.TotalLeaveDiscount, 0) - a.AdvancePaymentDisc, 0)                                            AS SALARYTOTAL_CALC,

            CASE                      WHEN a.Notes = ''                        THEN                        'لا يوجد'                      ELSE                      a.Notes              END                                          AS Notes

    FROM
            SalaryCalculationTb a
        INNER JOIN
          EmployeeTb b
            ON a.EMPID = b.ID
        LEFT JOIN
          CoBranch c
            ON b.BranchID = c.ID
        LEFT JOIN
          EmployeeClassificationTb d
            ON b.CLASSIFICATION = d.ID


        LEFT JOIN
          (   SELECT
                      EMPID,
                      YEAR(InsertDate) AS Y,
                      MONTH(InsertDate) AS M,
                      SUM(DiscountVal) AS TotalLeaveDiscount
              FROM
                      LeaveTB
              GROUP BY
                      EMPID,
                      YEAR(InsertDate),
                      MONTH(InsertDate)) e
            ON e.EMPID = a.EMPID
              AND e.Y = a.SALARYEAR
              AND e.M = a.SALARYMONTH


        


        

    WHERE
            a.EMPID = p_EmpID

    ORDER BY
            a.SALARYEAR,
            a.SALARYMONTH;
  END
$$
DELIMITER ;
