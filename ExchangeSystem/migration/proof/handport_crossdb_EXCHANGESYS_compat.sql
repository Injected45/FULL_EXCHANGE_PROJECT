-- =====================================================================================
-- `EXCHANGESYS` compatibility schema
--
-- WHY THIS EXISTS: 90 routines in EXCHANGESYS2026 reference the database `EXCHANGESYS` —
-- the OLD name of this same database. That database DOES NOT EXIST on the SQL Server instance
-- (verified: DB_ID('EXCHANGESYS') IS NULL), so these references are already broken in T-SQL today.
-- After the dbo. strip they convert to `EXCHANGESYS`.<obj>, so we give them a schema to resolve against
-- rather than editing 90 proc bodies (the golden rule: translate, don't rewrite).
--
-- Two objects are referenced:
--
--  1) ERROR_PROC  -- called from 88 CATCH blocks.
--     In T-SQL this was an error LOGGER living in the old DB. Mirrors the shipping migration's decision
--     (README_MYSQL §4): re-create it locally as a routine that RAISES the error to the app.
--     The SIGNAL is essential — without it the EXIT HANDLER swallows the failure silently and the app
--     believes a failed write succeeded.
--
--  2) InternalEx  -- read by InternalEx_SearchForCurrentRecorde2025 (live FROM clause, not commented).
--     `InternalEx` is a real table that exists in EXCHANGESYS2026. Under the DB's former name this was
--     the same table. Exposed here as a VIEW onto the real table so the reference resolves without
--     touching the proc body.
--     >> FLAGGED FOR REVIEW: this RESTORES a proc that is currently broken against SQL Server.
-- =====================================================================================
CREATE DATABASE IF NOT EXISTS `EXCHANGESYS` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE `EXCHANGESYS`;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';

DROP PROCEDURE IF EXISTS ERROR_PROC;
DELIMITER $$
CREATE PROCEDURE `ERROR_PROC`()
BEGIN
  SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'error';
END$$
DELIMITER ;

CREATE OR REPLACE VIEW `InternalEx` AS SELECT * FROM `EXCHANGESYS2026`.`InternalEx`;

-- Also expose ERROR_PROC inside the main schema: a few bodies call it unqualified (bare `ERROR_PROC`),
-- which MySQL resolves against the CURRENT schema, not `EXCHANGESYS`.
USE `EXCHANGESYS2026`;
DROP PROCEDURE IF EXISTS ERROR_PROC;
DELIMITER $$
CREATE PROCEDURE `ERROR_PROC`()
BEGIN
  SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'error';
END$$
DELIMITER ;
