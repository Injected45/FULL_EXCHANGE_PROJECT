-- =====================================================================================
-- Hand-port: GET_Deteels_fOr_taxe
--
-- The migrator SKIPS every proc containing sp_executesql, so this one was absent entirely rather than
-- appearing in a fail list.
--
-- The "dynamic" SQL here is a RED HERRING: @sql is one CONSTANT string. Nothing is interpolated into it
-- — @ConfirmType and @DeliveryPlaceID are passed to sp_executesql as BOUND PARAMETERS, not concatenated.
-- So it needs no PREPARE/EXECUTE at all: the statement becomes an ordinary INSERT .. SELECT that uses the
-- procedure's parameters directly. Simpler and safer than emulating dynamic SQL.
--
-- Mechanical changes only:
--   #InternalEx_GET_Taxi          -> TEMPORARY TABLE tmp_InternalEx_GET_Taxi
--   IF OBJECT_ID('tempdb..#x')..  -> DROP TEMPORARY TABLE IF EXISTS (same intent, idempotent)
--   NVARCHAR(n) / NVARCHAR(MAX)   -> VARCHAR(n) / LONGTEXT
--   ISNULL(x, '')                 -> IFNULL(x, '')
--   ''''  (escaped quote inside the dynamic string) -> a plain ''  empty-string literal
--   b.long                        -> b.`long`   (LONG is a MySQL type keyword; bare it is a syntax error)
--   exec sp_executesql @sql, ..   -> the INSERT itself, with p_ConfirmType / p_DeliveryPlaceID inline
--   TRY/CATCH + EXEC ERROR_PROC   -> DECLARE EXIT HANDLER .. ROLLBACK; CALL ERROR_PROC()
--
-- The INSERT has NO explicit column list, so the mapping is POSITIONAL and is preserved exactly, including
-- one oddity in the source: the 17th temp-table column is named AddCancelReason_ID but positionally
-- receives IFNULL(f.NewCause, '') — the cancel-reason TEXT, not its id. That is what SQL Server stores
-- today and what the caller reads, so it is reproduced unchanged rather than "fixed".
--
-- Result-set shape (SELECT * over the 18 temp-table columns), joins and filters are otherwise unchanged.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

DROP PROCEDURE IF EXISTS `GET_Deteels_fOr_taxe`;
DELIMITER $$
CREATE PROCEDURE `GET_Deteels_fOr_taxe`(IN `p_DeliveryPlaceID` INT, IN `p_ConfirmType` INT)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        CALL ERROR_PROC();
    END;

    START TRANSACTION;

    DROP TEMPORARY TABLE IF EXISTS tmp_InternalEx_GET_Taxi;
    CREATE TEMPORARY TABLE tmp_InternalEx_GET_Taxi (
        Code                            VARCHAR(100),
        AddressDescription              VARCHAR(255),
        RecievedName                    VARCHAR(100),
        RPhone1                         VARCHAR(50),
        InsertDate                      DATE,
        ConfirmType                     INT,
        loge                            VARCHAR(100),
        lat                             VARCHAR(100),
        BName                           VARCHAR(100),
        Latitude_branchID               VARCHAR(100),
        longitude_branchID              VARCHAR(100),
        OverallVal                      DECIMAL(18, 3),
        SName                           VARCHAR(100),
        TaxiValues                      DECIMAL(18, 3),
        BranchRecievedID                VARCHAR(100),
        DriverName                      VARCHAR(100),
        AddCancelReason_ID              VARCHAR(100),
        AddCancelReason_NameFrom_Driver LONGTEXT
    );

    INSERT INTO tmp_InternalEx_GET_Taxi
    SELECT
        a.Code,
        a.Notes AS AddressDescription,
        a.RecievedName,
        a.RPhone1,
        a.InsertDate,
        a.ConfirmType,
        a.loge,
        a.lat,
        b.BName,
        b.lat    AS Latitude_branchID,
        b.`long` AS longitude_branchID,
        a.OverallVal,
        c.SName,
        a.TaxiValues,
        d.BName  AS BranchRecievedID,
        IFNULL(e.DriverName, '') AS DriverName,
        IFNULL(f.NewCause, '')   AS NewCause,
        a.AddCancelReason_NameFrom_Driver
    FROM InternalEx AS a
    INNER JOIN CoBranch           AS b ON a.BranchDeliveredID  = b.ID
    INNER JOIN InternalEx_Stautes AS c ON a.ConfirmType        = c.ConfirmType
    INNER JOIN CoBranch           AS d ON a.BranchRecievedID   = d.ID
    LEFT  JOIN DriversTb          AS e ON a.Driver_ID          = e.ID
    LEFT  JOIN AddCancelReason    AS f ON a.AddCancelReason_ID = f.ID
    WHERE a.ConfirmType       = p_ConfirmType
      AND a.BranchDeliveredID = p_DeliveryPlaceID;

    SELECT * FROM tmp_InternalEx_GET_Taxi;

    DROP TEMPORARY TABLE IF EXISTS tmp_InternalEx_GET_Taxi;

    COMMIT;
END$$
DELIMITER ;
