-- =====================================================================================
-- Hand-port: eserType_mobile_fill_Daily_transfer_preparer_locukupEdit_proc
--
-- The migrator SKIPS every proc containing sp_executesql (it cannot convert dynamic SQL), which is why
-- this one was absent entirely rather than landing in a fail list.
--
-- Here the "dynamic" SQL is a red herring: @sql is set to one of TWO CONSTANT strings depending on
-- @ISupdate, with no runtime identifier or value interpolation. So it needs no PREPARE/EXECUTE at all —
-- the two branches become two ordinary INSERT statements. Simpler AND safer than emulating dynamic SQL.
--
-- Mechanical changes only:
--   #TableDriversTb_Add_From                  -> TEMPORARY TABLE tmp_TableDriversTb_Add_From
--   IF OBJECT_ID('tempdb..#x') IS NOT NULL DROP -> DROP TEMPORARY TABLE IF EXISTS (idempotent, same intent)
--   NVARCHAR(MAX)                             -> LONGTEXT
--   exec sp_executesql @Sql                   -> the constant INSERT itself
-- Result-set shape, column order and both branch conditions are unchanged.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

DROP PROCEDURE IF EXISTS `eserType_mobile_fill_Daily_transfer_preparer_locukupEdit_proc`;
DELIMITER $$
CREATE PROCEDURE `eserType_mobile_fill_Daily_transfer_preparer_locukupEdit_proc`(IN `p_ISupdate` INT)
BEGIN
    DROP TEMPORARY TABLE IF EXISTS tmp_TableDriversTb_Add_From;
    CREATE TEMPORARY TABLE tmp_TableDriversTb_Add_From (ID BIGINT, NAMe_Type LONGTEXT);

    IF p_ISupdate = 0 THEN
        INSERT INTO tmp_TableDriversTb_Add_From
        SELECT * FROM UeserType_mobile_fill_Daily_transfer_preparer_locukupEdit;
    END IF;

    IF p_ISupdate = 1 THEN
        INSERT INTO tmp_TableDriversTb_Add_From
        SELECT * FROM UeserType_mobile;
    END IF;

    SELECT * FROM tmp_TableDriversTb_Add_From;

    DROP TEMPORARY TABLE IF EXISTS tmp_TableDriversTb_Add_From;
END$$
DELIMITER ;
