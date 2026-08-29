-- =====================================================================================
-- Hand-port: ExtractNumbers, ExtractLastNumbers, ExtractAccountNumberFromText
--
-- WHY THESE CANNOT BE AUTO-CONVERTED — T-SQL LIKE supports CHARACTER CLASSES:
--
--     IF @char LIKE '[0-9]'            -- T-SQL: "is this one character a digit?"
--
-- MySQL's LIKE has NO character classes. '[0-9]' is a LITERAL four-character string there, so the test
-- would be false for every digit and the functions would silently return ''. The MySQL equivalent of a
-- LIKE character class is REGEXP:
--
--     IF v_char REGEXP '^[0-9]$' THEN  -- anchored so it matches exactly one digit, same as T-SQL
--
-- (An ordinary LIKE with % / _ — as in '%من رقم الحساب%' below — needs no change; only bracket classes do.)
--
-- Other mechanical changes: LEN->CHAR_LENGTH, CHARINDEX->LOCATE (same argument order),
-- '+' string concat -> CONCAT, WHILE..BEGIN/END -> WHILE..DO/END WHILE, and DECLARE defaults that
-- reference a PARAMETER (DECLARE @i INT = LEN(@input)) must become a SET after the declarations,
-- because MySQL only allows a constant in a DECLARE ... DEFAULT.
--
-- Logic, argument order and return values are otherwise unchanged.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

-- ---------------------------------------------------------------------------
DROP FUNCTION IF EXISTS ExtractNumbers;
DELIMITER $$
CREATE FUNCTION `ExtractNumbers`(`p_input` LONGTEXT) RETURNS LONGTEXT DETERMINISTIC
BEGIN
    DECLARE v_result LONGTEXT DEFAULT '';
    DECLARE v_i INT DEFAULT 1;
    DECLARE v_char CHAR(1);

    WHILE v_i <= CHAR_LENGTH(p_input) DO
        SET v_char = SUBSTRING(p_input, v_i, 1);
        IF v_char REGEXP '^[0-9]$' THEN
            SET v_result = CONCAT(v_result, v_char);
        END IF;
        SET v_i = v_i + 1;
    END WHILE;

    RETURN v_result;
END$$
DELIMITER ;

-- ---------------------------------------------------------------------------
-- Walks BACKWARDS from the end of the text, collecting the last @length digits.
DROP FUNCTION IF EXISTS ExtractLastNumbers;
DELIMITER $$
CREATE FUNCTION `ExtractLastNumbers`(`p_input` LONGTEXT, `p_length` INT) RETURNS LONGTEXT DETERMINISTIC
BEGIN
    DECLARE v_numbers LONGTEXT DEFAULT '';
    DECLARE v_i INT;
    DECLARE v_char CHAR(1);
    DECLARE v_foundNumbers INT DEFAULT 0;

    SET v_i = CHAR_LENGTH(p_input);        -- was: DECLARE @i INT = LEN(@input)

    WHILE v_i > 0 AND v_foundNumbers < p_length DO
        SET v_char = SUBSTRING(p_input, v_i, 1);
        IF v_char REGEXP '^[0-9]$' THEN
            SET v_numbers = CONCAT(v_char, v_numbers);   -- NOTE the order: prepend, not append
            SET v_foundNumbers = v_foundNumbers + 1;
        END IF;
        SET v_i = v_i - 1;
    END WHILE;

    RETURN v_numbers;
END$$
DELIMITER ;

-- ---------------------------------------------------------------------------
DROP FUNCTION IF EXISTS ExtractAccountNumberFromText;
DELIMITER $$
CREATE FUNCTION `ExtractAccountNumberFromText`(`p_Text` LONGTEXT) RETURNS VARCHAR(50) DETERMINISTIC
BEGIN
    DECLARE v_Result VARCHAR(50) DEFAULT '';
    DECLARE v_StartPos INT DEFAULT 0;
    DECLARE v_EndPos INT DEFAULT 0;

    IF p_Text LIKE '%من رقم الحساب%' THEN
        SET v_StartPos = LOCATE('من رقم الحساب', p_Text) + CHAR_LENGTH('من رقم الحساب');
        SET v_EndPos   = LOCATE('إلى', p_Text, v_StartPos);

        IF v_EndPos > v_StartPos THEN
            SET v_Result = ExtractNumbers(SUBSTRING(p_Text, v_StartPos, v_EndPos - v_StartPos));
        END IF;
    ELSE
        SET v_Result = ExtractNumbers(p_Text);
    END IF;

    RETURN v_Result;
END$$
DELIMITER ;
