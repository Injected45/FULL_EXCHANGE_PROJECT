-- =====================================================================================
-- Hand-port: SendTypeTB_Roll_luckbedit
--
-- Why the converter could not do it: this proc's definition is stored with CR-only line
-- terminators, so the converter's line-oriented ';'-insertion pass saw the whole body as ONE
-- line and could not find a single statement boundary.
--
-- Mechanical changes only:
--   #SendTypeTB_TMB                       -> TEMPORARY TABLE tmp_SendTypeTB_TMB
--   IF OBJECT_ID('tempdb..#x') IS NOT NULL DROP  -> DROP TEMPORARY TABLE IF EXISTS (same intent)
--   NVARCHAR(100)                         -> VARCHAR(100)
--   DECLARE @a BIT, @b BIT, @c BIT        -> three separate DECLAREs (MySQL has no multi-declare)
--   BIT                                   -> TINYINT(1)   (T-SQL BIT is 0/1; MySQL BIT(1) would
--                                            return a b'' string to ADO.NET, TINYINT keeps 0/1)
--   SELECT @x = a.col FROM ..             -> SELECT a.col INTO v_x FROM ..
--   TRY/CATCH + exec dbo.ERROR_PROC       -> DECLARE EXIT HANDLER .. ROLLBACK; CALL ERROR_PROC()
--
-- Result-set shape (SELECT * FROM the temp table: SID, SName) and all three OR-branch conditions
-- are unchanged.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

DROP PROCEDURE IF EXISTS `SendTypeTB_Roll_luckbedit`;
DELIMITER $$
CREATE PROCEDURE `SendTypeTB_Roll_luckbedit`(IN `p_ScreenID` INT, IN `p_UeserID` INT)
BEGIN
    DECLARE v_can_banck  TINYINT(1);
    DECLARE v_can_cash   TINYINT(1);
    DECLARE v_can_Acount TINYINT(1);
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        CALL ERROR_PROC();
    END;

    START TRANSACTION;

    DROP TEMPORARY TABLE IF EXISTS tmp_SendTypeTB_TMB;
    CREATE TEMPORARY TABLE tmp_SendTypeTB_TMB
    (
        SID   INT PRIMARY KEY,
        SName VARCHAR(100)
    );

    SELECT
        a.can_banck,
        a.can_cash,
        a.can_Acount
    INTO v_can_banck, v_can_cash, v_can_Acount
    FROM FrmScreensTb_Details_UESIRID AS a
    WHERE a.ScreenID = p_ScreenID AND a.UeserID = p_UeserID;

    INSERT INTO tmp_SendTypeTB_TMB (SID, SName)
    SELECT s.SID, s.SName
    FROM SendTypeTB s
    WHERE (s.SID = 2 AND v_can_banck  = 1)
       OR (s.SID = 0 AND v_can_cash   = 1)
       OR (s.SID = 1 AND v_can_Acount = 1);

    SELECT * FROM tmp_SendTypeTB_TMB;

    DROP TEMPORARY TABLE IF EXISTS tmp_SendTypeTB_TMB;

    COMMIT;
END$$
DELIMITER ;
