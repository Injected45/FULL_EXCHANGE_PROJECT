-- =====================================================================================
-- Hand-port: the two DBCC CHECKIDENT (identity reseed) utility procs. No MySQL equivalent of DBCC, and
-- ALTER TABLE .. AUTO_INCREMENT = N takes a LITERAL (a variable is not allowed), so the value is built with
-- CONCAT and run via PREPARE/EXECUTE. T-SQL "DBCC CHECKIDENT(t, RESEED, @m)" sets the identity to @m so the
-- NEXT insert is @m+1 — MySQL "AUTO_INCREMENT = N" makes the next value N, hence N = @m+1. No Arabic literals.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

-- ---------------------------------------------------------------------------
DROP PROCEDURE IF EXISTS `AddCancelReason_ReseedCounter`;
DELIMITER $$
CREATE PROCEDURE `AddCancelReason_ReseedCounter`()
BEGIN
    DECLARE v_max INT;
    SELECT MAX(ID) INTO v_max FROM AddCancelReason;
    SET @__reseed_sql = CONCAT('ALTER TABLE `AddCancelReason` AUTO_INCREMENT = ', IFNULL(v_max, 0) + 1);
    PREPARE __reseed_stmt FROM @__reseed_sql;
    EXECUTE __reseed_stmt;
    DEALLOCATE PREPARE __reseed_stmt;
END$$
DELIMITER ;

-- ---------------------------------------------------------------------------
DROP PROCEDURE IF EXISTS `ResedAnyTable`;
DELIMITER $$
CREATE PROCEDURE `ResedAnyTable`()
BEGIN
    DECLARE v_max INT;
    SELECT MAX(ID) FROM BranchRatesTb;                 -- bare SELECT: returns the max as a result set (as T-SQL did)
    SELECT MAX(ID) INTO v_max FROM BranchRatesTb;
    IF v_max IS NULL THEN                               -- check when max is returned as null
        SET v_max = 0;
    END IF;
    SET @__reseed_sql = CONCAT('ALTER TABLE `BranchRatesTb` AUTO_INCREMENT = ', v_max + 1);
    PREPARE __reseed_stmt FROM @__reseed_sql;
    EXECUTE __reseed_stmt;
    DEALLOCATE PREPARE __reseed_stmt;
END$$
DELIMITER ;
