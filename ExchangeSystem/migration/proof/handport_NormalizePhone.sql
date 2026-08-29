-- =====================================================================================
-- Hand-port: NormalizePhone   (strips the country dialling prefix / leading zero)
--
-- WHY IT CANNOT BE AUTO-CONVERTED — a chained braceless IF / ELSE IF:
--
--     IF @phone LIKE '218%'
--         SET @clean = ...
--     ELSE IF @phone LIKE '20%'
--         SET @clean = ...
--     ...
--     ELSE
--         SET @clean = @phone
--
-- MySQL spells the chain as one word, ELSEIF, and the WHOLE chain closes with a single END IF.
-- The converter handles a braceless IF by emitting "... END IF;" for it, which closes the IF before the
-- next ELSEIF is reached — so the chain has to be scoped as one unit, by hand. (I tried a blanket
-- "ELSE IF" -> "ELSEIF" rewrite in the converter; it broke functions that were already converting cleanly,
-- so it was reverted. See STATUS.md.)
--
-- Order of the branches is load-bearing and is preserved EXACTLY: '218' is tested before '20', and the
-- bare '0%' rule comes last. Reordering would change which prefix wins.
-- LEN -> CHAR_LENGTH. The LIKE patterns use only '%', which MySQL supports unchanged.
-- =====================================================================================
USE EXCHANGESYS2026;
SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';

DROP FUNCTION IF EXISTS NormalizePhone;
DELIMITER $$
CREATE FUNCTION `NormalizePhone`(`p_phone` VARCHAR(50)) RETURNS VARCHAR(50) DETERMINISTIC
BEGIN
    DECLARE v_clean VARCHAR(50);

    -- ليبيا 218
    IF p_phone LIKE '218%' THEN
        SET v_clean = SUBSTRING(p_phone, 4, CHAR_LENGTH(p_phone));

    -- مصر 20
    ELSEIF p_phone LIKE '20%' THEN
        SET v_clean = SUBSTRING(p_phone, 3, CHAR_LENGTH(p_phone));

    -- السعودية 966
    ELSEIF p_phone LIKE '966%' THEN
        SET v_clean = SUBSTRING(p_phone, 4, CHAR_LENGTH(p_phone));

    -- العراق 964
    ELSEIF p_phone LIKE '964%' THEN
        SET v_clean = SUBSTRING(p_phone, 4, CHAR_LENGTH(p_phone));

    -- أي رقم يبدأ بـ 0 → نشيل الصفر
    ELSEIF p_phone LIKE '0%' THEN
        SET v_clean = SUBSTRING(p_phone, 2, CHAR_LENGTH(p_phone));

    ELSE
        SET v_clean = p_phone;
    END IF;

    RETURN v_clean;
END$$
DELIMITER ;
