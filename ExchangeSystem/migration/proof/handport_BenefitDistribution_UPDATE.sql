-- =====================================================================================
-- Hand-port: BenefitDistribution_UPDATE
--
-- Why the converter could not do it: T-SQL BRACELESS IF / ELSE IF whose bodies are 20-line UPDATE
-- statements with no BEGIN/END. The converter refuses to guess where such a body ends (guessing
-- would truncate the UPDATE and silently write a partial row), so it is done by hand.
--
-- Mechanical changes only:
--   IF <cond> <stmt> ELSE IF <cond> <stmt>  -> IF .. THEN ..; ELSEIF .. THEN ..; END IF;
--   SELECT @gnm = a.IsMain FROM ..          -> SELECT a.IsMain INTO v_gnm FROM ..
--   dbo.BenefitDistribution.DailyClose = 0  -> BenefitDistribution.DailyClose = 0
--   NVARCHAR(MAX)                           -> LONGTEXT
--   SET NOCOUNT / XACT_ABORT                -> dropped (no MySQL equivalent)
--
-- TWO SOURCE QUIRKS PRESERVED VERBATIM — these look like bugs but changing them would change what
-- production writes, so they are reproduced exactly:
--   * In the @gnm = 1 branch the source assigns "RBVal = RBVal" and "BRType = BRType" — the COLUMN
--     to itself, NOT the @RBVal / @BRType parameters. Those two columns are therefore left
--     unchanged by that branch. (The @gnm = 0 branch does use the parameters.)
--   * If @gnm is NULL (no CoBranch row for @RBID) NEITHER branch runs and nothing is updated.
--     The IF/ELSEIF form keeps that.
--
-- Column list, order and the WHERE ISID = @ISID predicate are otherwise unchanged in both branches.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

DROP PROCEDURE IF EXISTS `BenefitDistribution_UPDATE`;
DELIMITER $$
CREATE PROCEDURE `BenefitDistribution_UPDATE`(
    IN `p_ISID`       LONGTEXT,
    IN `p_InsertDate` DATE,
    IN `p_RBID`       INT,
    IN `p_DBBID`      INT,
    IN `p_MainBID`    INT,
    IN `p_RBVal`      DECIMAL(12, 3),
    IN `p_DBVal`      DECIMAL(12, 3),
    IN `p_MainBVal`   DECIMAL(12, 3),
    IN `p_ISIDType`   INT,
    IN `p_SafeID`     INT,
    IN `p_BRType`     TINYINT UNSIGNED,
    IN `p_DBRType`    TINYINT UNSIGNED,
    IN `p_CurrencyID` INT)
BEGIN
    DECLARE v_gnm INT;

    SELECT
            a.IsMain
    INTO    v_gnm
    FROM
            CoBranch AS a
    WHERE
            ID = p_RBID;

    IF v_gnm = 1 THEN
        UPDATE
                BenefitDistribution
        SET
                ISID = p_ISID,
                InsertDate = p_InsertDate,
                RBID = p_RBID,
                DBBID = p_DBBID,
                RBVal = RBVal,                  -- self-assignment in the source; kept
                DBVal = p_DBVal,
                MainBVal = p_MainBVal,
                ISIDType = p_ISIDType,
                SafeID = p_SafeID,
                IsActive = 1,
                MainBID = p_MainBID,
                BRType = BRType,                -- self-assignment in the source; kept
                DBRType = p_DBRType,
                CurrencyID = p_CurrencyID,
                IsCanceled = 0,
                BenefitDistribution.DailyClose = 0
        WHERE
                ISID = p_ISID;
    ELSEIF v_gnm = 0 THEN
        UPDATE
                BenefitDistribution
        SET
                ISID = p_ISID,
                InsertDate = p_InsertDate,
                RBID = p_RBID,
                DBBID = p_DBBID,
                MainBID = p_MainBID,
                RBVal = p_RBVal,
                DBVal = p_DBVal,
                MainBVal = p_MainBVal,
                ISIDType = p_ISIDType,
                SafeID = p_SafeID,
                IsActive = 1,
                BRType = p_BRType,
                DBRType = p_DBRType,
                CurrencyID = p_CurrencyID,
                IsCanceled = 0,
                BenefitDistribution.DailyClose = 0
        WHERE
                ISID = p_ISID;
    END IF;
END$$
DELIMITER ;
