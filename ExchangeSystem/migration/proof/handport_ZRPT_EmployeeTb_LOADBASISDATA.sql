-- =====================================================================================
-- Hand-port: ZRPT_EmployeeTb_LOADBASISDATA
--
-- Why the converter could not do it: the proc uses T-SQL's BRACELESS IF, i.e.
--     IF @BranchID <> 0
--         SELECT ... FROM ... ORDER BY ...      -- a 20-line statement, no BEGIN/END
-- The converter deliberately refuses to wrap a braceless-IF body that spans multiple lines: it
-- cannot tell where the statement ends without a parser, and guessing would TRUNCATE the SELECT.
-- So it is done by hand here, where the extent is obvious.
--
-- Mechanical changes only:
--   IF <cond> <stmt>                -> IF <cond> THEN <stmt>; END IF;
--   '0'+'1'+'0'+CONVERT(NVARCHAR,a.ID) -> CONCAT('0','1','0', CAST(a.ID AS CHAR))
--        This is STRING concatenation in T-SQL (all operands are strings/converted to string), so
--        it must become CONCAT — MySQL's "+" would do ARITHMETIC and silently return a number.
--   'alias' (Arabic, single-quoted)  -> `alias` (backticked)  — identical column names
--   [CLASSIFICATION]                 -> CLASSIFICATION
--   BEGIN TRAN / COMMIT              -> START TRANSACTION / COMMIT
--   SET NOCOUNT / XACT_ABORT         -> dropped (no MySQL equivalent, no result-set effect)
--
-- ROW_NUMBER() OVER (ORDER BY ..) is supported by MariaDB 10.2+, so it is kept as-is.
-- Both branches, their WHERE clauses and the ORDER BY are unchanged.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

DROP PROCEDURE IF EXISTS `ZRPT_EmployeeTb_LOADBASISDATA`;
DELIMITER $$
CREATE PROCEDURE `ZRPT_EmployeeTb_LOADBASISDATA`(IN `p_BranchID` INT)
BEGIN
    START TRANSACTION;

    IF p_BranchID <> 0 THEN
        SELECT
                ROW_NUMBER() OVER (ORDER BY a.ID ASC)              AS `#`,
                CONCAT('0', '1', '0', CAST(a.ID AS CHAR))          AS `الرمز`,
                a.EMPNAME                                          AS `الاسم`,
                b.BName                                            AS `الفرع`,
                c.ECNAME                                           AS `التصنيف`
        FROM
                EmployeeTb AS a
            LEFT OUTER JOIN
                CoBranch AS b
                    ON a.BranchID = b.ID
            LEFT OUTER JOIN
                EmployeeClassificationTb AS c
                    ON a.CLASSIFICATION = c.ID
        WHERE
                a.IsActive = 1
                AND a.BranchID = p_BranchID
        ORDER BY
                a.ID ASC;
    END IF;

    IF p_BranchID = 0 THEN
        SELECT
                ROW_NUMBER() OVER (ORDER BY a.ID ASC)              AS `#`,
                CONCAT('0', '1', '0', CAST(a.ID AS CHAR))          AS `الرمز`,
                a.EMPNAME                                          AS `الاسم`,
                b.BName                                            AS `الفرع`,
                c.ECNAME                                           AS `التصنيف`
        FROM
                EmployeeTb AS a
            LEFT OUTER JOIN
                CoBranch AS b
                    ON a.BranchID = b.ID
            LEFT OUTER JOIN
                EmployeeClassificationTb AS c
                    ON a.CLASSIFICATION = c.ID
        WHERE
                a.IsActive = 1
        ORDER BY
                a.ID ASC;
    END IF;

    COMMIT;
END$$
DELIMITER ;
