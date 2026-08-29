-- =====================================================================================
-- Hand-port: RibbonPermission_Select. Read-only.
--
-- The T-SQL declares a TABLE VARIABLE ("DECLARE @SID table (id int)"), fills it from RibbonTb and uses it as
-- an IN-list. MySQL has no table variables -> a session TEMPORARY TABLE, which is the same mechanism the
-- converter already uses for TVP parameters (tvp_<name>).
--
-- Translated LITERALLY, on purpose. Note the subquery "Select b.ID from @SID": @SID has no alias `b`, so `b`
-- is an OUTER reference to the LEFT JOIN'd RibbonTb. Both T-SQL and MySQL resolve a correlated outer alias the
-- same way, so keeping the statement verbatim (only @SID -> tmp_SID) preserves that behaviour exactly —
-- no need to reason about whether it simplifies to "a.RPID IN (SELECT ID FROM RibbonTb)".
--
-- NOTE: RibbonPermission and RibbonTb are both EMPTY in this database, so the proc returns no rows either way
-- and could not be diff-verified against SQL Server on data. The literal translation is therefore the safe one.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

DROP PROCEDURE IF EXISTS `RibbonPermission_Select`;
DELIMITER $$
CREATE PROCEDURE `RibbonPermission_Select`(IN `p_ProfileID` INT)
BEGIN
    DROP TEMPORARY TABLE IF EXISTS tmp_SID;
    CREATE TEMPORARY TABLE tmp_SID (id INT);

    START TRANSACTION;

    INSERT INTO tmp_SID (id) SELECT ID FROM RibbonTb;

    SELECT a.ID, a.ProfileID, a.RPID, a.CanShow
    FROM RibbonPermission AS a
    LEFT OUTER JOIN RibbonTb AS b ON a.RPID = b.ID
    WHERE a.RPID IN (SELECT b.ID FROM tmp_SID) AND a.ProfileID = p_ProfileID;

    COMMIT;

    DROP TEMPORARY TABLE IF EXISTS tmp_SID;
END$$
DELIMITER ;
