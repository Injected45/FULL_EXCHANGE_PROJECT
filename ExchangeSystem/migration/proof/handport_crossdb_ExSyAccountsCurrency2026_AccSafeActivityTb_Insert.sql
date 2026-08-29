-- =====================================================================================
-- Cross-DB proc: ExSyAccountsCurrency2026.dbo.AccSafeActivityTb_Insert
-- Called cross-schema by EXCHANGESYS2026 procs: NewCurrencyBuyandSale_Insert, NewCurrenciesSaleTB_Insert
-- (their bodies become `ExSyAccountsCurrency2026.AccSafeActivityTb_Insert` after the dbo. strip).
--
-- Literal port. Notes:
--   * The T-SQL trailing SELECT is fully commented out -> this proc returns NO result set. Kept that way.
--   * IsActive is a hardcoded literal 1 in the T-SQL VALUES list -> kept as 1 (NOT a parameter).
--   * Target column is `SafeIDMovement`, fed by param @SafeIDMovementType -> name mismatch is in the
--     original; preserved exactly.
--   * SET XACT_ABORT ON + BEGIN TRAN/COMMIT with no CATCH -> EXIT HANDLER that rolls back and re-signals,
--     which is the MySQL equivalent of XACT_ABORT's "abort the whole transaction on error" (README §4).
-- =====================================================================================
USE ExSyAccountsCurrency2026;
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

    START TRANSACTION;
    INSERT INTO AccSafeActivityTbCurrency
        (
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
        )
    VALUES
        (
          p_SafeID, p_Debit, p_Credit, p_InsertDate, p_Description, p_ISID, 1, p_TypeID, p_OperationTypeID,
          p_AccBranchID, p_AccIDFrom, p_AccIDTo, p_IsConfirmed, p_IsCanceled, p_MovementType, p_CurrencyID,
          p_DailyClosed, p_SafeIDDailyClose, p_SafeIDMovementType
        );
    COMMIT;
END$$
DELIMITER ;
