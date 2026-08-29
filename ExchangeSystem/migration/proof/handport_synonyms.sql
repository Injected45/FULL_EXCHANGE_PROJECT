-- =====================================================================================
-- SQL Server SYNONYMS -> MySQL shims
--
-- EXCHANGESYS2026 defines 9 SYNONYMs (sys.synonyms). They are the system's REAL cross-database
-- mechanism: a proc says `FROM dbo.ExSyAccounts_AccSafeActivityTb` and SQL Server silently resolves it
-- to [ExSyAccounts2026].[dbo].[AccSafeActivityTb]. Because the 3-part name never appears in the proc
-- body, a text scan of sys.sql_modules does NOT reveal these dependencies -- they are invisible until a
-- converted routine fails at runtime with "Table 'a' doesn't exist" (MySQL reports the ALIAS when the
-- base table is missing, which is a very misleading error).
--
-- MySQL has no synonyms. Two shim kinds, both keeping the ORIGINAL synonym name so no proc body changes:
--   * TABLE synonym -> updatable VIEW (a single-table `SELECT *` view supports INSERT/UPDATE/DELETE in
--     MySQL, and LAST_INSERT_ID() propagates, so the write paths keep working).
--   * PROC  synonym -> wrapper PROCEDURE that CALLs the real proc with the same parameter list.
--
-- DANGLING SYNONYM (flagged, deliberately NOT created):
--   GEt_DriversTb -> [ShippingTransportSystem2024Teset].[dbo].[DriversTb]
--   That database DOES NOT EXIST on the SQL Server instance, so this synonym is already broken and the
--   2 routines referencing it are broken in production today. Creating a shim would invent behavior.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';

-- ---- TABLE synonyms -> updatable views -------------------------------------------------
-- 3 synonyms all point at the SAME table (ExSyAccounts2026.AccSafeActivityTb)
CREATE OR REPLACE VIEW `ExSyAccounts_AccSafeActivityTb`  AS SELECT * FROM `ExSyAccounts2026`.`AccSafeActivityTb`;
CREATE OR REPLACE VIEW `EX24AccSafeActivityTb`           AS SELECT * FROM `ExSyAccounts2026`.`AccSafeActivityTb`;
CREATE OR REPLACE VIEW `cr_2024_CurrencyMovementsTable`  AS SELECT * FROM `ExSyAccounts2026`.`AccSafeActivityTb`;

CREATE OR REPLACE VIEW `ExSyAccountsCurrency_AccSafeActivityTb`
    AS SELECT * FROM `ExSyAccountsCurrency2026`.`AccSafeActivityTbCurrency`;
CREATE OR REPLACE VIEW `ExSyAccountsCurrency_CurrencymovementPruse`
    AS SELECT * FROM `ExSyAccountsCurrency2026`.`CurrencymovementPruse`;

-- self-referencing synonym: [EXCHANGESYS2026].[dbo].[CurrencyMainTb] is a LOCAL table
CREATE OR REPLACE VIEW `EXCHANGESYS_CurrencyMainTb` AS SELECT * FROM `EXCHANGESYS2026`.`CurrencyMainTb`;

-- ---- PROCEDURE synonyms -> wrapper procs ------------------------------------------------
DELIMITER $$

DROP PROCEDURE IF EXISTS `ExSyAccounts_AccSafeActivityTb_Insert`$$
CREATE PROCEDURE `ExSyAccounts_AccSafeActivityTb_Insert`(
    IN `p_SafeID` INT, IN `p_Debit` DECIMAL(12,3), IN `p_Credit` DECIMAL(12,3), IN `p_InsertDate` DATE,
    IN `p_Description` LONGTEXT, IN `p_ISID` LONGTEXT, IN `p_TypeID` INT, IN `p_OperationTypeID` INT,
    IN `p_AccBranchID` INT, IN `p_AccIDFrom` INT, IN `p_AccIDTo` INT, IN `p_IsConfirmed` TINYINT(1),
    IN `p_IsCanceled` INT, IN `p_MovementType` LONGTEXT, IN `p_CurrencyID` INT, IN `p_DailyClosed` TINYINT(1),
    IN `p_SafeIDDailyClose` INT, IN `p_SafeIDMovementType` LONGTEXT
)
BEGIN
    CALL `ExSyAccounts2026`.`AccSafeActivityTb_Insert`(
        p_SafeID, p_Debit, p_Credit, p_InsertDate, p_Description, p_ISID, p_TypeID, p_OperationTypeID,
        p_AccBranchID, p_AccIDFrom, p_AccIDTo, p_IsConfirmed, p_IsCanceled, p_MovementType, p_CurrencyID,
        p_DailyClosed, p_SafeIDDailyClose, p_SafeIDMovementType);
END$$

-- NOTE the synonym's name is misspelled "_Inert" in SQL Server. Keep the typo: proc bodies call it by
-- that exact name (README: match existing names, including existing typos).
DROP PROCEDURE IF EXISTS `ExSyAccountsCurrency_AccSafeActivityTb_Inert`$$
CREATE PROCEDURE `ExSyAccountsCurrency_AccSafeActivityTb_Inert`(
    IN `p_SafeID` INT, IN `p_Debit` DECIMAL(12,3), IN `p_Credit` DECIMAL(12,3), IN `p_InsertDate` DATE,
    IN `p_Description` LONGTEXT, IN `p_ISID` LONGTEXT, IN `p_TypeID` INT, IN `p_OperationTypeID` INT,
    IN `p_AccBranchID` INT, IN `p_AccIDFrom` INT, IN `p_AccIDTo` INT, IN `p_IsConfirmed` TINYINT(1),
    IN `p_IsCanceled` INT, IN `p_MovementType` LONGTEXT, IN `p_CurrencyID` INT, IN `p_DailyClosed` TINYINT(1),
    IN `p_SafeIDDailyClose` INT, IN `p_SafeIDMovementType` LONGTEXT
)
BEGIN
    CALL `ExSyAccountsCurrency2026`.`AccSafeActivityTb_Insert`(
        p_SafeID, p_Debit, p_Credit, p_InsertDate, p_Description, p_ISID, p_TypeID, p_OperationTypeID,
        p_AccBranchID, p_AccIDFrom, p_AccIDTo, p_IsConfirmed, p_IsCanceled, p_MovementType, p_CurrencyID,
        p_DailyClosed, p_SafeIDDailyClose, p_SafeIDMovementType);
END$$

DELIMITER ;
