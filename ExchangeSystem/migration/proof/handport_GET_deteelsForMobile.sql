-- =====================================================================================
-- Hand-port: GET_deteelsForMobile   (genuine dynamic SQL -> PREPARE/EXECUTE)
--
-- The migrator SKIPS every proc containing sp_executesql, so this one was absent entirely rather than
-- appearing in a fail list.
--
-- This IS real dynamic SQL: the TABLE and several COLUMN names are chosen at runtime from @type, so it
-- cannot be flattened into a static statement the way eserType_mobile_fill could. sp_executesql therefore
-- becomes MySQL's PREPARE / EXECUTE / DEALLOCATE on a CONCAT-built string - the same mechanism.
--
--   @type = 0 -> InternalEx : DeliveryPlace / OverallVal      / SPhone1 / BBRANCHID
--   @type = 1 -> ExternalEx : CityIDTo      / CurrRecievedVal / Phone1  / RecievedBranchID
-- (all 11 referenced columns verified to exist on the matching table before writing this)
--
-- Mechanical changes only:
--   #InternalEx_tip        -> TEMPORARY TABLE tmp_InternalEx_tip
--   nvarchar(450)          -> VARCHAR(450);  nvarchar(max) -> LONGTEXT
--   time(7)                -> TIME(6)        (MariaDB's maximum fractional-seconds precision is 6)
--   TRY/CATCH + ERROR_PROC -> DECLARE EXIT HANDLER ... ROLLBACK; CALL ERROR_PROC()
--   EXEC sp_executesql @s  -> PREPARE .. EXECUTE .. DEALLOCATE PREPARE
--
-- One deliberate FIX: the T-SQL concatenation produced "... = 5and  a.Type_Moble ..." (no space before
-- "and", because the literal starts with 'and'). SQL Server tolerates a number glued to a keyword;
-- MySQL does not. A space is inserted. Same predicate, just parseable.
--
-- Column order, filters and result-set shape are otherwise unchanged.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

DROP PROCEDURE IF EXISTS `GET_deteelsForMobile`;
DELIMITER $$
CREATE PROCEDURE `GET_deteelsForMobile`(IN `p_type` INT, IN `p_cties` INT, IN `p_branchID` INT)
BEGIN
    DECLARE v_TableNAMe        VARCHAR(450);
    DECLARE v_CtiNAMe          VARCHAR(450);
    DECLARE v_OverallVal       VARCHAR(450);
    DECLARE v_Phone1           VARCHAR(450);
    DECLARE v_RecievedBranchID VARCHAR(450);
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        CALL ERROR_PROC();
    END;

    IF p_type = 0 THEN
        SET v_TableNAMe        = 'InternalEx';
        SET v_CtiNAMe          = 'a.DeliveryPlace';
        SET v_OverallVal       = 'a.OverallVal';
        SET v_Phone1           = 'SPhone1';
        SET v_RecievedBranchID = 'BBRANCHID';
    END IF;

    IF p_type = 1 THEN
        SET v_TableNAMe        = 'ExternalEx';
        SET v_CtiNAMe          = 'a.CityIDTo';
        SET v_OverallVal       = 'a.CurrRecievedVal';
        SET v_Phone1           = 'Phone1';
        SET v_RecievedBranchID = 'RecievedBranchID';
    END IF;

    START TRANSACTION;

    DROP TEMPORARY TABLE IF EXISTS tmp_InternalEx_tip;
    CREATE TEMPORARY TABLE tmp_InternalEx_tip (
        ID           INT PRIMARY KEY,
        Code         VARCHAR(450),
        SenderName   VARCHAR(450),
        SPhone1      VARCHAR(450),
        RecievedName VARCHAR(450),
        RPhone1      VARCHAR(450),
        InsertDate   DATE,
        InsertTime   TIME(6),
        CityName     VARCHAR(450),
        OverallVal   DECIMAL(18,2),
        ExVal        DECIMAL(18,2)
    );

    -- The "INSERT INTO <table> (<all columns>) SELECT" prefix is deliberately kept in ONE string literal
    -- rather than split across CONCAT arguments. The static write-path checker (cmp_writes.py) matches the
    -- target table and its column list textually; a list split over two literals is invisible to it, and an
    -- invisible write is exactly what that check exists to prevent. The concatenated result is unchanged.
    SET @SQL_MA = CONCAT(
        'INSERT INTO tmp_InternalEx_tip (ID, Code, SenderName, SPhone1, RecievedName, RPhone1, InsertDate, InsertTime, CityName, OverallVal, ExVal) SELECT ',
        'a.ID, a.Code, a.SenderName, ', v_Phone1, ', a.RecievedName, a.RPhone1, ',
        'a.InsertDate, a.InsertTime, b.CityName, ', v_OverallVal, ', ExVal ',
        'FROM ', v_TableNAMe, ' AS a ',
        'INNER JOIN CitiesTb AS b ON ', v_CtiNAMe, ' = b.ID ',
        'WHERE ', v_CtiNAMe, ' = ', p_cties, ' AND a.Type_Moble = 1 ',
        'AND (', v_RecievedBranchID, ' = ', p_branchID, ' OR ', p_branchID, ' = 0)');

    PREPARE __mob_stmt FROM @SQL_MA;
    EXECUTE __mob_stmt;
    DEALLOCATE PREPARE __mob_stmt;

    SELECT * FROM tmp_InternalEx_tip;

    DROP TEMPORARY TABLE IF EXISTS tmp_InternalEx_tip;

    COMMIT;
END$$
DELIMITER ;
