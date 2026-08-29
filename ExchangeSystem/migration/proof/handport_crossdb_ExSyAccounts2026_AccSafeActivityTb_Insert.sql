-- =====================================================================================
-- Cross-DB proc: ExSyAccounts2026.dbo.AccSafeActivityTb_Insert
-- Reached from EXCHANGESYS2026 through the SYNONYM `ExSyAccounts_AccSafeActivityTb_Insert`
-- (see handport_synonyms.sql, which creates the MySQL shim for that synonym).
--
-- Literal port. Quirks in the T-SQL that are PRESERVED exactly (do NOT "fix" these):
--   * InsertDate is inserted as CONVERT(DATE, GETDATE()) -- the @InsertDate PARAMETER IS IGNORED.
--     Kept as CAST(NOW() AS DATE). p_InsertDate stays in the signature but remains unused.
--   * IsActive is the literal 1 (not a parameter).
--   * The final SELECT returns 20 columns in this exact order -- the VB forms bind by name AND index,
--     so the order is load-bearing. SCOPE_IDENTITY() -> LAST_INSERT_ID().
--
-- PARAMETER DEFAULTS: T-SQL declares @Description=NULL, @IsConfirmed=NULL, @IsCanceled=NULL,
-- @DailyClosed=0, @SafeIDDailyClose=0, @SafeIDMovementType=NULL. MySQL has NO proc parameter defaults,
-- and the app's routing layer supplies NULL for any argument the caller omits. For the two params whose
-- T-SQL default is 0 (not NULL) we re-apply that default with IFNULL so an omitted argument still lands
-- as 0 -- otherwise a NULL would silently replace the 0 the app has always written.
-- =====================================================================================
USE ExSyAccounts2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

DROP PROCEDURE IF EXISTS AccSafeActivityTb_Insert;
DELIMITER $$
CREATE PROCEDURE `AccSafeActivityTb_Insert`(
    IN `p_SafeID`             INT,
    IN `p_Debit`              DECIMAL(12,3),
    IN `p_Credit`             DECIMAL(12,3),
    IN `p_InsertDate`         DATE,
    IN `p_Description`        LONGTEXT,
    IN `p_ISID`               LONGTEXT,
    IN `p_TypeID`             INT,
    IN `p_OperationTypeID`    INT,
    IN `p_AccBranchID`        INT,
    IN `p_AccIDFrom`          INT,
    IN `p_AccIDTo`            INT,
    IN `p_IsConfirmed`        TINYINT(1),
    IN `p_IsCanceled`         INT,
    IN `p_MovementType`       LONGTEXT,
    IN `p_CurrencyID`         INT,
    IN `p_DailyClosed`        TINYINT(1),
    IN `p_SafeIDDailyClose`   INT,
    IN `p_SafeIDMovementType` LONGTEXT
)
proc: BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    -- re-apply the T-SQL parameter defaults that are NOT NULL (see header note)
    SET p_DailyClosed      = IFNULL(p_DailyClosed, 0);
    SET p_SafeIDDailyClose = IFNULL(p_SafeIDDailyClose, 0);

    START TRANSACTION;
    INSERT INTO AccSafeActivityTb
    (SafeID,
     Debit,
     Credit,
     InsertDate,
     Description,
     ISID,
     IsActive,
     TypeID,
     OperationTypeID,
     AccBranchID,
     AccIDFrom,
     AccIDTo,
     IsConfirmed,
     IsCanceled,
     MovementType,
     CurrencyID,
     DailyClosed,
     SafeIDDailyClose,
     SafeIDMovement
    )
    VALUES
    (p_SafeID,
     p_Debit,
     p_Credit,
     CAST(NOW() AS DATE),          -- NOT p_InsertDate: faithful to the T-SQL
     p_Description,
     p_ISID,
     1,
     p_TypeID,
     p_OperationTypeID,
     p_AccBranchID,
     p_AccIDFrom,
     p_AccIDTo,
     p_IsConfirmed,
     p_IsCanceled,
     p_MovementType,
     p_CurrencyID,
     p_DailyClosed,
     p_SafeIDDailyClose,
     p_SafeIDMovementType
    );

    SELECT ID,
           SafeID,
           Debit,
           Credit,
           InsertDate,
           Description,
           ISID,
           IsActive,
           TypeID,
           OperationTypeID,
           AccBranchID,
           AccIDFrom,
           AccIDTo,
           IsConfirmed,
           IsCanceled,
           MovementType,
           CurrencyID,
           DailyClosed,
           SafeIDDailyClose,
           SafeIDMovement
    FROM AccSafeActivityTb
    WHERE (ID = LAST_INSERT_ID());
    COMMIT;
END$$
DELIMITER ;
