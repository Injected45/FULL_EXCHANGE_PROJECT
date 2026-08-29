-- =====================================================================================
-- Hand-port: GetIdCOde, SalePrice_mo_Value  (both scalar functions, NO Arabic literals)
--
-- Both hit braceless multi-line IF/ELSE + SELECT-assignment patterns that the converter mangles when they
-- compress onto one line. Neither has Arabic string literals, so they are safe to hand-write.
--
-- Faithful translations. Mechanical changes only:
--   @x -> p_x (params) / v_x (locals);  ISNULL -> IFNULL;  SELECT @x = e FROM .. -> SELECT e INTO x FROM ..;
--   "SELECT @x = e" (no FROM) -> SET x = e;  "@x += 1" -> "x = x + 1";  "< =" -> "<=", "> =" -> ">=";
--   braceless IF/ELSE -> IF .. THEN .. ELSE .. END IF;  IF .. BEGIN .. END -> IF .. THEN .. END IF.
-- Logic, order, and every comparison preserved exactly.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

-- ---------------------------------------------------------------------------
DROP FUNCTION IF EXISTS GetIdCOde;
DELIMITER $$
CREATE FUNCTION `GetIdCOde`(`p_perent` DECIMAL(18,0), `p_AccType` INT) RETURNS INT DETERMINISTIC
BEGIN
    DECLARE v_IDCode INT;
    SELECT MAX(IDcode) INTO v_IDCode FROM ACCOUNTSTB WHERE AccParent = p_perent AND ACCTYPE = p_AccType;
    IF v_IDCode IS NULL THEN
        SET v_IDCode = 1;
    ELSE
        SET v_IDCode = v_IDCode + 1;
    END IF;
    RETURN v_IDCode;
END$$
DELIMITER ;

-- ---------------------------------------------------------------------------
DROP FUNCTION IF EXISTS SalePrice_mo_Value;
DELIMITER $$
CREATE FUNCTION `SalePrice_mo_Value`(`p_CountryID` INT, `p_value` INT, `p_Type_ID_int` INT, `p_Type_form` INT)
    RETURNS FLOAT DETERMINISTIC
BEGIN
    DECLARE v_OUtValies      FLOAT;
    DECLARE v_SalePrice       DECIMAL(18,3);
    DECLARE v_maxVAle         DECIMAL(18,3);
    DECLARE v_VuleNu          INT;
    DECLARE v_DicCountfaloe   DECIMAL(18,3);
    DECLARE v_serevesteType_coun INT;

    SELECT IFNULL(SalePrice, 1) INTO v_SalePrice
    FROM NewCurrencyPriceOwnDetailsTb AS a
    INNER JOIN NewCurrencyPricesOwnTb AS b ON a.CPID = b.ID
    INNER JOIN CountiresTb AS c ON b.CountryID = c.ID AND a.CurrencyIDTo = c.DefualtCurrency
    WHERE a.CurrencyIDFrom = 1
      AND b.PriceType = 2
      AND b.AccountType = 3
      AND b.CountryID = p_CountryID;

    SET p_value = p_value * v_SalePrice;
    SELECT COUNT(ID) INTO v_serevesteType_coun FROM CATEGORYTYPESDETAILSTB AS a WHERE a.CATID = p_Type_ID_int;

    IF p_Type_form = 0 THEN
        IF v_serevesteType_coun > 0 THEN
            SELECT a.MaxValue INTO v_maxVAle FROM ExtTraServiceTypeTb AS a WHERE a.ID = p_Type_ID_int;

            IF p_value <= v_maxVAle THEN
                SELECT p_value - CASE WHEN a.RateType = 0 THEN a.DisVal ELSE p_value * a.DisVal END
                  INTO p_value
                FROM CATEGORYTYPESDETAILSTB AS a
                WHERE a.CATID = p_Type_ID_int AND p_value >= a.ValFrom AND p_value <= a.ValTo;
                SET v_OUtValies = p_value;
            END IF;

            IF p_value > v_maxVAle THEN
                SET v_VuleNu = FLOOR(p_value / v_maxVAle);
                SELECT CASE WHEN a.RateType = 0 THEN a.DisVal ELSE v_maxVAle * a.DisVal END
                  INTO v_DicCountfaloe
                FROM CATEGORYTYPESDETAILSTB AS a
                WHERE a.CATID = p_Type_ID_int AND v_maxVAle >= a.ValFrom AND v_maxVAle <= a.ValTo;
                SET v_DicCountfaloe = v_DicCountfaloe * v_VuleNu;
                SET v_maxVAle = v_maxVAle * v_VuleNu;
                SET p_value = p_value - v_maxVAle;
                IF p_value > 0 THEN
                    SELECT p_value - CASE WHEN a.RateType = 0 THEN a.DisVal ELSE p_value * a.DisVal END
                      INTO p_value
                    FROM CATEGORYTYPESDETAILSTB AS a
                    WHERE a.CATID = p_Type_ID_int AND p_value >= a.ValFrom AND p_value <= a.ValTo;
                END IF;
                SET v_OUtValies = (v_maxVAle - v_DicCountfaloe) + p_value;
            END IF;
        END IF;
    END IF;

    RETURN IFNULL(v_OUtValies, p_value);
END$$
DELIMITER ;
