-- =====================================================================================
-- Hand-port: GET_Total_Currency_CustomersTb_PROC
--
-- Why the converter could not do it: the proc is a thin wrapper —
--     SELECT * FROM dbo.GET_Total_Currency_CustomersTb(@Type, @AccBranchID)
-- — over a MULTI-STATEMENT table-valued function (RETURNS @TMB TABLE .. INSERT INTO @TMB .. RETURN).
-- MySQL has no multi-statement TVF and no table-valued function callable in FROM at all. The
-- converter can only inline INLINE TVFs (a single RETURN(SELECT ..)), so this one was left out.
--
-- The TVF is called from exactly this one place, so its body is flattened into the proc: the
-- table variable @TMB becomes a TEMPORARY TABLE with the SAME column types (this matters — the
-- declared types are an implicit CAST: CODE is INT and the three money columns are NVARCHAR(MAX),
-- i.e. the FORMAT() strings, not numbers), the IF-branches are kept as-is, and the wrapper's
-- "SELECT * FROM <tvf>" becomes "SELECT * FROM <temp table>".
--
-- Mechanical changes only:
--   @TMB TABLE(..)                     -> TEMPORARY TABLE tmp_GET_Total_Currency_CustomersTb(..)
--   NVARCHAR(MAX)                      -> LONGTEXT
--   ISNULL(x, y)                       -> IFNULL(x, y)
--   FORMAT(x, 'N3', 'en-us')           -> FORMAT(x, 3)
--        T-SQL 'N3' = 3 decimals WITH thousands separators; MySQL FORMAT(x,3) is the same
--        rendering, and 'en-us' is MySQL's default locale — so the strings match character for
--        character (e.g. "1,234.567"). NOT the REPLACE(..,',','') form used for 'F3' elsewhere.
--   [dbo].X / dbo.X                    -> X
--
-- Two source quirks preserved deliberately:
--   * The "IF @Type = 2" block is nested INSIDE the "IF @Type = 1" block, so it is unreachable in
--     SQL Server too (@Type cannot be both 1 and 2). It is reproduced in the same dead position
--     rather than promoted — promoting it would ADD behaviour that production never executes.
--   * "INNER JOIN ExSyAccounts_AccSafeActivityTb AS b ON a.ID = 1" is a deliberate cross join in
--     the source; kept verbatim.
--
-- The unqualified SUM(Debit) references are unambiguous in every branch (of the three joined
-- tables only ExSyAccounts[Currency]_AccSafeActivityTb has a Debit column — verified against
-- information_schema), so MySQL resolves them to the same column SQL Server did.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

DROP PROCEDURE IF EXISTS `GET_Total_Currency_CustomersTb_PROC`;
DELIMITER $$
CREATE PROCEDURE `GET_Total_Currency_CustomersTb_PROC`(IN `p_Type` INT, IN `p_AccBranchID` INT)
BEGIN
    DROP TEMPORARY TABLE IF EXISTS tmp_GET_Total_Currency_CustomersTb;
    CREATE TEMPORARY TABLE tmp_GET_Total_Currency_CustomersTb
    (
        CODE     INT,
        CuName   LONGTEXT,
        AccTotal LONGTEXT,
        Debit    LONGTEXT,
        Credit   LONGTEXT
    );

    -- p_Type -- عميل أو موظف
    IF p_Type = 0 THEN
        INSERT INTO tmp_GET_Total_Currency_CustomersTb
            SELECT
                    a.ID                                                            AS CODE,
                    a.CuName,
                    FORMAT(IFNULL(SUM(b.Debit), 0.000) - IFNULL(SUM(b.Credit), 0.000), 3) AS AccTotal,
                    FORMAT(IFNULL(SUM(b.Credit), 0.000), 3)                         AS Debit,
                    FORMAT(IFNULL(SUM(Debit), 0.000), 3)                            AS Credit
            FROM
                    CurrencyMainTb AS a
                INNER JOIN
                    ExSyAccountsCurrency_AccSafeActivityTb AS b
                        ON a.ID = b.CurrencyID
                INNER JOIN
                    CustomersTb AS c
                        ON b.AccIDFrom = c.AccID
            WHERE
                    a.IsActive = 1
                    AND b.IsActive = 1
                    AND b.AccBranchID = p_AccBranchID
            GROUP BY
                    a.CuName,
                    a.ID
            UNION
            SELECT
                    a.ID,
                    a.CuName,
                    FORMAT(IFNULL(SUM(b.Credit), 0.000) - IFNULL(SUM(b.Debit), 0.000), 3) AS AccTotal,
                    FORMAT(IFNULL(SUM(Debit), 0.000), 3)                            AS Debit,
                    FORMAT(IFNULL(SUM(b.Credit), 0.000), 3)                         AS Credit
            FROM
                    CurrencyMainTb AS a
                INNER JOIN
                    ExSyAccounts_AccSafeActivityTb AS b
                        ON a.ID = 1
                INNER JOIN
                    CustomersTb AS c
                        ON b.AccIDFrom = c.AccID
            WHERE
                    a.IsActive = 1
                    AND b.IsActive = 1
                    AND b.AccBranchID = p_AccBranchID
                    AND (b.CurrencyID = 1 OR b.CurrencyID = 13)
            GROUP BY
                    a.CuName,
                    a.ID;
    END IF;

    IF p_Type = 1 THEN
        INSERT INTO tmp_GET_Total_Currency_CustomersTb
            SELECT
                    a.ID                                                            AS CODE,
                    a.CuName,
                    FORMAT(IFNULL(SUM(b.Debit), 0.000) - IFNULL(SUM(b.Credit), 0.000), 3) AS AccTotal,
                    FORMAT(IFNULL(SUM(b.Credit), 0.000), 3)                         AS Debit,
                    FORMAT(IFNULL(SUM(b.Debit), 0.000), 3)                          AS Credit
            FROM
                    CurrencyMainTb AS a
                INNER JOIN
                    ExSyAccountsCurrency_AccSafeActivityTb AS b
                        ON a.ID = b.CurrencyID
                INNER JOIN
                    AccountsTb AS c
                        ON b.AccIDFrom = c.AccID
                INNER JOIN
                    EmployeeTb AS d
                        ON c.AccID = d.AccID
            WHERE
                    a.IsActive = 1
                    AND b.IsActive = 1
                    AND b.AccBranchID = p_AccBranchID
            GROUP BY
                    a.CuName,
                    a.ID
            UNION
            SELECT
                    a.ID                                                            AS CODE,
                    a.CuName,
                    FORMAT(IFNULL(SUM(b.Credit), 0.000) - IFNULL(SUM(b.Debit), 0.000), 3) AS AccTotal,
                    FORMAT(IFNULL(SUM(b.Debit), 0.000), 3)                          AS Debit,
                    FORMAT(IFNULL(SUM(b.Credit), 0.000), 3)                         AS Credit
            FROM
                    CurrencyMainTb AS a
                INNER JOIN
                    ExSyAccounts_AccSafeActivityTb AS b
                        ON a.ID = 1
                INNER JOIN
                    AccountsTb AS c
                        ON b.AccIDFrom = c.AccID
                INNER JOIN
                    EmployeeTb AS d
                        ON c.AccID = d.AccID
            WHERE
                    a.IsActive = 1
                    AND b.IsActive = 1
                    AND b.AccBranchID = p_AccBranchID
                    AND (b.CurrencyID = 1 OR b.CurrencyID = 13)
            GROUP BY
                    a.CuName,
                    a.ID;

        -- unreachable in the source too (nested inside IF @Type = 1); kept in position
        IF p_Type = 2 THEN
            INSERT INTO tmp_GET_Total_Currency_CustomersTb
                SELECT
                        a.ID                                                            AS CODE,
                        a.CuName,
                        FORMAT(IFNULL(SUM(b.Credit), 0.000) - IFNULL(SUM(b.Debit), 0.000), 3) AS AccTotal,
                        FORMAT(IFNULL(SUM(b.Debit), 0.000), 3)                          AS Debit,
                        FORMAT(IFNULL(SUM(b.Credit), 0.000), 3)                         AS Credit
                FROM
                        CurrencyMainTb AS a
                    INNER JOIN
                        ExSyAccountsCurrency_AccSafeActivityTb AS b
                            ON a.ID = b.CurrencyID
                    INNER JOIN
                        AccountsTb AS c
                            ON b.AccIDFrom = c.AccID
                    INNER JOIN
                        EmployeeTb AS d
                            ON c.AccID = d.AccID
                WHERE
                        a.IsActive = 1
                        AND b.IsActive = 1
                        AND b.AccBranchID = p_AccBranchID
                GROUP BY
                        a.CuName,
                        a.ID
                UNION
                SELECT
                        a.ID                                                            AS CODE,
                        a.CuName,
                        FORMAT(IFNULL(SUM(b.Credit), 0.000) - IFNULL(SUM(b.Debit), 0.000), 3) AS AccTotal,
                        FORMAT(IFNULL(SUM(b.Debit), 0.000), 3)                          AS Debit,
                        FORMAT(IFNULL(SUM(b.Credit), 0.000), 3)                         AS Credit
                FROM
                        CurrencyMainTb AS a
                    INNER JOIN
                        ExSyAccounts_AccSafeActivityTb AS b
                            ON a.ID = 1
                    INNER JOIN
                        AccountsTb AS c
                            ON b.AccIDFrom = c.AccID
                    INNER JOIN
                        EmployeeTb AS d
                            ON c.AccID = d.AccID
                WHERE
                        a.IsActive = 1
                        AND b.IsActive = 1
                        AND b.AccBranchID = p_AccBranchID
                        AND (b.CurrencyID = 1 OR b.CurrencyID = 13)
                GROUP BY
                        a.CuName,
                        a.ID
                UNION
                SELECT
                        a.ID                                                            AS CODE,
                        a.CuName,
                        FORMAT(IFNULL(SUM(b.Credit), 0.000) - IFNULL(SUM(b.Debit), 0.000), 3) AS AccTotal,
                        FORMAT(IFNULL(SUM(Debit), 0.000), 3)                            AS Debit,
                        FORMAT(IFNULL(SUM(b.Credit), 0.000), 3)                         AS Credit
                FROM
                        CurrencyMainTb AS a
                    INNER JOIN
                        ExSyAccountsCurrency_AccSafeActivityTb AS b
                            ON a.ID = b.CurrencyID
                    INNER JOIN
                        CustomersTb AS c
                            ON b.AccIDFrom = c.AccID
                WHERE
                        a.IsActive = 1
                        AND b.IsActive = 1
                        AND b.AccBranchID = p_AccBranchID
                GROUP BY
                        a.CuName,
                        a.ID
                UNION
                SELECT
                        a.ID,
                        a.CuName,
                        FORMAT(IFNULL(SUM(b.Credit), 0.000) - IFNULL(SUM(b.Debit), 0.000), 3) AS AccTotal,
                        FORMAT(IFNULL(SUM(Debit), 0.000), 3)                            AS Debit,
                        FORMAT(IFNULL(SUM(b.Credit), 0.000), 3)                         AS Credit
                FROM
                        CurrencyMainTb AS a
                    INNER JOIN
                        ExSyAccounts_AccSafeActivityTb AS b
                            ON a.ID = 1
                    INNER JOIN
                        CustomersTb AS c
                            ON b.AccIDFrom = c.AccID
                WHERE
                        a.IsActive = 1
                        AND b.IsActive = 1
                        AND b.AccBranchID = p_AccBranchID
                        AND (b.CurrencyID = 1 OR b.CurrencyID = 13)
                GROUP BY
                        a.CuName,
                        a.ID;
        END IF;
    END IF;

    SELECT * FROM tmp_GET_Total_Currency_CustomersTb;

    DROP TEMPORARY TABLE IF EXISTS tmp_GET_Total_Currency_CustomersTb;
END$$
DELIMITER ;
