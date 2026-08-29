-- =====================================================================================
-- Hand-port: ZRPT_SalaryCalc_LoadToCalculate
--
-- Why the converter could not do it: the select list is
--     SELECT ROW_NUMBER() OVER (..) AS '#', *, c.BName AS BName, CASE .. END AS Phone
-- MySQL accepts a bare "*" only as the FIRST select_expr. Here "*" sits in the middle AND the
-- query joins TWO objects, so it expands to VW_SALARYCALCLAST's 20 columns followed by CoBranch's
-- 40 — the qualified rewrite "A.*, C.*" reproduces exactly that, in that order. Getting this wrong
-- (e.g. writing only A.*) would silently drop 40 columns from the salary sheet.
--
-- Mechanical changes only:
--   *                            -> A.*, C.*      (same 60 columns, same order)
--   'alias' (single-quoted)      -> `alias`
--   C.Mobile1 + '-' + C.Mobile2  -> CONCAT(C.Mobile1, '-', C.Mobile2)
--        STRING concatenation — MySQL's "+" would coerce both phone numbers to numbers and return
--        arithmetic garbage.
--   [dbo].[VW_SALARYCALCLAST]    -> VW_SALARYCALCLAST
--   BEGIN TRANSACTION / COMMIT   -> START TRANSACTION / COMMIT
--   SET NOCOUNT / XACT_ABORT     -> dropped (no MySQL equivalent, no result-set effect)
--
-- The CASE ladder (both mobiles empty -> the hard-coded pair, one empty -> the other, else joined
-- with '-'), the NOT EXISTS anti-join against SalaryCalculationTb, and the ORDER BY are unchanged.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

DROP PROCEDURE IF EXISTS `ZRPT_SalaryCalc_LoadToCalculate`;
DELIMITER $$
CREATE PROCEDURE `ZRPT_SalaryCalc_LoadToCalculate`(IN `p_SALARYMONTH` TINYINT UNSIGNED, IN `p_SALARYEAR` INT)
BEGIN
    START TRANSACTION;

    SELECT
            ROW_NUMBER() OVER (ORDER BY A.ID ASC) AS `#`,
            A.*,
            C.*,
            C.BName                               AS BName,
            CASE
                    WHEN C.Mobile1 = ''
                     AND C.Mobile2 = ''
                        THEN '0924903885-0915511445'
                    WHEN C.Mobile1 = ''
                        THEN C.Mobile2
                    WHEN C.Mobile2 = ''
                        THEN C.Mobile1
                    ELSE CONCAT(C.Mobile1, '-', C.Mobile2)
            END                                   AS Phone
    FROM
            VW_SALARYCALCLAST AS A
        LEFT JOIN
            CoBranch AS C
                ON A.BranchID = C.ID
    WHERE
            NOT EXISTS (SELECT
                                b.EMPID
                        FROM
                                SalaryCalculationTb AS b
                        WHERE
                                b.EMPID = A.ID
                                AND b.SALARYMONTH = p_SALARYMONTH
                                AND b.SALARYEAR = p_SALARYEAR)
    ORDER BY
            A.ID;

    COMMIT;
END$$
DELIMITER ;
