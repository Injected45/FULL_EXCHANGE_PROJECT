-- =====================================================================================
-- Hand-port: AccountsTb_LoadSafeToTransfer
--
-- Why the converter could not do it: the T-SQL select list is
--     Select AccID, AccName, * From AccountsTb ...
-- MySQL only accepts a bare "*" as the FIRST select_expr; anywhere else it is a syntax error.
-- The fix is the qualified form "AccountsTb.*", which MySQL DOES accept mid-list and which expands
-- to exactly the same 38 columns in the same order. Result-set shape is therefore unchanged:
-- AccID, AccName, then all 38 columns of AccountsTb (AccID and AccName appear twice, as in T-SQL —
-- ADO.NET de-duplicates the repeated names itself, so the app sees what it always saw).
--
-- Other mechanical changes only:
--   SET NOCOUNT/XACT_ABORT      -> dropped (no MySQL equivalent, no result-set effect)
--   BEGIN TRANSACTION / COMMIT  -> START TRANSACTION / COMMIT
--
-- The duplicated "LEFT(AccParent,5)=10104 OR LEFT(AccParent,5)=10104" is kept verbatim: it is
-- redundant in the source too, and removing it would be a behaviour change I cannot justify.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

DROP PROCEDURE IF EXISTS `AccountsTb_LoadSafeToTransfer`;
DELIMITER $$
CREATE PROCEDURE `AccountsTb_LoadSafeToTransfer`(IN `p_BranchID` INT)
BEGIN
    START TRANSACTION;

    SELECT AccID, AccName, AccountsTb.*
    FROM AccountsTb
    WHERE BranchID = p_BranchID
      AND (LEFT(AccParent, 5) = 10104 OR LEFT(AccParent, 5) = 10104)
      AND Accline = 4;

    COMMIT;
END$$
DELIMITER ;
