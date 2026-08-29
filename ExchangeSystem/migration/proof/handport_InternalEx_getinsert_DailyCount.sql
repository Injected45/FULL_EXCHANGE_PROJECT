-- =====================================================================================
-- Hand-port: InternalEx_getinsert_DailyCount
--
-- The migrator SKIPS every proc containing sp_executesql, so this one was absent entirely rather than
-- appearing in a fail list.
--
-- This IS real dynamic SQL — the TABLE and three COLUMN names are chosen at runtime from @TypeISint —
-- so it becomes MySQL's PREPARE / EXECUTE / DEALLOCATE over a CONCAT-built string, the same mechanism:
--   @TypeISint = 0 -> InternalEx : a.DeliveryPlace / a.OverallVal       / BBRANCHID
--   @TypeISint = 1 -> ExternalEx : a.CityIDTo      / a.CurrRecievedVal  / RecievedBranchID
-- (all referenced columns verified against information_schema before writing this).
--
-- Mechanical changes only:
--   #Tbl                          -> TEMPORARY TABLE tmp_Tbl
--   SELECT .. INTO #Tbl           -> CREATE TEMPORARY TABLE tmp_Tbl AS SELECT ..
--   NVARCHAR(n) / NVARCHAR(MAX)   -> VARCHAR(n) / LONGTEXT
--   AVG(CAST(x AS FLOAT))         -> AVG(x * 1.0e0)   (forces DOUBLE, as T-SQL's FLOAT cast does;
--                                    plain AVG() on an INT returns DECIMAL in MySQL)
--   EXEC sp_executesql @SQL       -> PREPARE .. EXECUTE .. DEALLOCATE PREPARE
--
-- TWO conversions that needed checking rather than guessing:
--
--   DATEPART(WEEK, a.InsertDate)  -> WEEK(a.InsertDate, 0)
--     These do NOT return the same number: SQL Server counts Jan 1 as week 1, MySQL mode 0 calls that
--     week 0, so the labels differ by exactly one (verified for 2026: SQL Server 1,1,2,25 vs MySQL
--     0,0,1,24 for Jan 1 / Jan 3 / Jan 4 / Jun 15). What matters is that BOTH break the year at the
--     same Sunday boundaries, so the GROUP BY buckets are identical — and TransferWeek is only ever
--     grouped on, never returned to the caller, so the differing label is invisible.
--
--   FORMAT(x, '0.##') + '%'       -> CONCAT(TRIM(TRAILING '.' FROM TRIM(TRAILING '0' FROM FORMAT(x,2))), '%')
--     T-SQL's '0.##' means "up to 2 decimals, trailing zeros dropped, always at least one integer
--     digit". MySQL's FORMAT(x,2) always emits exactly 2 decimals, so the trailing zeros are trimmed
--     back off; the '.' left behind by a whole number is then trimmed too ('100.00'->'100', '12.50'->
--     '12.5', '0.50'->'0.5'). The percentage can never reach 1000, so FORMAT's thousands separator
--     never appears.
--
-- ONE GUARD ADDED: if @TypeISint is neither 0 nor 1 the four name variables stay NULL. T-SQL's
-- sp_executesql with a NULL batch is a no-op; MySQL's PREPARE FROM NULL raises an error. The IF below
-- keeps the T-SQL behaviour (empty result) instead of failing.
--
-- The final result set — including the source's DUPLICATE ct.CityName column — its ORDER BY, and every
-- aggregate are unchanged.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

DROP PROCEDURE IF EXISTS `InternalEx_getinsert_DailyCount`;
DELIMITER $$
CREATE PROCEDURE `InternalEx_getinsert_DailyCount`(IN `p_TypeISint` INT, IN `p_BBRANCHID` INT)
BEGIN
    DECLARE v_SourceTable       VARCHAR(50);
    DECLARE v_JoinField         VARCHAR(50);
    DECLARE v_OverallField      VARCHAR(50);
    DECLARE v_RecievedBranchID  VARCHAR(450);
    DECLARE v_TotalAllTransfers INT;

    IF p_TypeISint = 0 THEN
        SET v_SourceTable      = 'InternalEx';
        SET v_JoinField        = 'a.DeliveryPlace';
        SET v_OverallField     = 'a.OverallVal';
        SET v_RecievedBranchID = 'BBRANCHID';
    ELSEIF p_TypeISint = 1 THEN
        SET v_SourceTable      = 'ExternalEx';
        SET v_JoinField        = 'a.CityIDTo';
        SET v_OverallField     = 'a.CurrRecievedVal';
        SET v_RecievedBranchID = 'RecievedBranchID';
    END IF;

    -- بناء الجدول المؤقت وتحميل البيانات
    DROP TEMPORARY TABLE IF EXISTS tmp_TransfersWithCity;
    CREATE TEMPORARY TABLE tmp_TransfersWithCity (
        ID            INT PRIMARY KEY,
        InsertDate    DATETIME,
        CityName      VARCHAR(100),
        OverallVal    DECIMAL(18,2),
        ExVal         DECIMAL(18,2),
        TransferDay   DATE,
        TransferYear  INT,
        TransferMonth INT,
        TransferWeek  INT,
        CiteID        INT
    );

    IF v_SourceTable IS NOT NULL THEN
        SET @SQL_DC = CONCAT(
            'INSERT INTO tmp_TransfersWithCity (ID, InsertDate, CityName, OverallVal, ExVal, TransferDay, TransferYear, TransferMonth, TransferWeek, CiteID) SELECT ',
            'a.ID, a.InsertDate, b.CityName, ', v_OverallField, ', a.ExVal, ',
            'CAST(a.InsertDate AS DATE), YEAR(a.InsertDate), MONTH(a.InsertDate), WEEK(a.InsertDate, 0), b.ID ',
            'FROM ', v_SourceTable, ' AS a ',
            'INNER JOIN CitiesTb AS b ON ', v_JoinField, ' = b.ID ',
            'WHERE a.Type_Moble = 1 ',
            'AND (', p_BBRANCHID, ' = 0 OR ', v_RecievedBranchID, ' = ', p_BBRANCHID, ')');

        PREPARE __dc_stmt FROM @SQL_DC;
        EXECUTE __dc_stmt;
        DEALLOCATE PREPARE __dc_stmt;
    END IF;

    SELECT COUNT(*) INTO v_TotalAllTransfers FROM tmp_TransfersWithCity;

    -- إنشاء مجاميع المدينة
    DROP TEMPORARY TABLE IF EXISTS tmp_CityTotals;
    CREATE TEMPORARY TABLE tmp_CityTotals AS
        SELECT
            CityName,
            COUNT(*)        AS TotalTransfers,
            SUM(OverallVal) AS TotalOverallVal,
            SUM(ExVal)      AS TotalExVal,
            CiteID
        FROM tmp_TransfersWithCity
        GROUP BY CityName, CiteID;

    -- المتوسط اليومي
    DROP TEMPORARY TABLE IF EXISTS tmp_DailyAvg;
    CREATE TEMPORARY TABLE tmp_DailyAvg AS
        SELECT CityName, AVG(DailyCount * 1.0e0) AS DailyAvg
        FROM (SELECT CityName, TransferDay, COUNT(*) AS DailyCount
              FROM tmp_TransfersWithCity
              GROUP BY CityName, TransferDay) AS DailySub
        GROUP BY CityName;

    -- المتوسط الأسبوعي
    DROP TEMPORARY TABLE IF EXISTS tmp_WeeklyAvg;
    CREATE TEMPORARY TABLE tmp_WeeklyAvg AS
        SELECT CityName, AVG(WeeklyCount * 1.0e0) AS WeeklyAvg
        FROM (SELECT CityName, TransferYear, TransferWeek, COUNT(*) AS WeeklyCount
              FROM tmp_TransfersWithCity
              GROUP BY CityName, TransferYear, TransferWeek) AS WeeklySub
        GROUP BY CityName;

    -- المتوسط الشهري
    DROP TEMPORARY TABLE IF EXISTS tmp_MonthlyAvg;
    CREATE TEMPORARY TABLE tmp_MonthlyAvg AS
        SELECT CityName, AVG(MonthlyCount * 1.0e0) AS MonthlyAvg
        FROM (SELECT CityName, TransferYear, TransferMonth, COUNT(*) AS MonthlyCount
              FROM tmp_TransfersWithCity
              GROUP BY CityName, TransferYear, TransferMonth) AS MonthlySub
        GROUP BY CityName;

    -- المتوسط السنوي
    DROP TEMPORARY TABLE IF EXISTS tmp_YearlyAvg;
    CREATE TEMPORARY TABLE tmp_YearlyAvg AS
        SELECT CityName, AVG(YearlyCount * 1.0e0) AS YearlyAvg
        FROM (SELECT CityName, TransferYear, COUNT(*) AS YearlyCount
              FROM tmp_TransfersWithCity
              GROUP BY CityName, TransferYear) AS YearlySub
        GROUP BY CityName;

    -- إخراج النتائج
    SELECT
        ct.CityName,
        ct.TotalTransfers,
        ct.TotalOverallVal,
        ct.TotalExVal,
        ct.CityName,
        ct.CiteID,
        CASE
            WHEN v_TotalAllTransfers = 0 THEN '0%'
            ELSE CONCAT(TRIM(TRAILING '.' FROM
                        TRIM(TRAILING '0' FROM
                        FORMAT(ct.TotalTransfers * 100.0 / v_TotalAllTransfers, 2))), '%')
        END               AS PercentageOfTotal,
        da.DailyAvg       AS DailyCount,
        wa.WeeklyAvg      AS WeeklyCount,
        ma.MonthlyAvg     AS MonthlyCount,
        ya.YearlyAvg      AS YearlyCount
    FROM tmp_CityTotals ct
    LEFT JOIN tmp_DailyAvg   da ON da.CityName = ct.CityName
    LEFT JOIN tmp_WeeklyAvg  wa ON wa.CityName = ct.CityName
    LEFT JOIN tmp_MonthlyAvg ma ON ma.CityName = ct.CityName
    LEFT JOIN tmp_YearlyAvg  ya ON ya.CityName = ct.CityName
    ORDER BY ct.TotalTransfers DESC;

    -- تنظيف الجداول المؤقتة
    DROP TEMPORARY TABLE IF EXISTS tmp_TransfersWithCity;
    DROP TEMPORARY TABLE IF EXISTS tmp_CityTotals;
    DROP TEMPORARY TABLE IF EXISTS tmp_DailyAvg;
    DROP TEMPORARY TABLE IF EXISTS tmp_WeeklyAvg;
    DROP TEMPORARY TABLE IF EXISTS tmp_MonthlyAvg;
    DROP TEMPORARY TABLE IF EXISTS tmp_YearlyAvg;
END$$
DELIMITER ;
