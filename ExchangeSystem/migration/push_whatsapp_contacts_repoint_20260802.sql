SET NAMES utf8mb4;
SET collation_connection='utf8mb4_unicode_ci';
SET SESSION sql_mode='IGNORE_SPACE,NO_ENGINE_SUBSTITUTION';
DROP PROCEDURE IF EXISTS `whatsapp_contacts`;
DELIMITER //
CREATE PROCEDURE `whatsapp_contacts`(IN `p_phone_number` LONGTEXT)
BEGIN


    SET p_phone_number = NormalizePhone(p_phone_number);

    SELECT
            a.phone_number
    FROM
            rhalla2026Teset.`whatsapp_contacts_shipping` a
    WHERE
            NormalizePhone(a.phone_number) = p_phone_number;
  END//
DELIMITER ;
GRANT EXECUTE ON PROCEDURE `exchangesys2026`.`whatsapp_contacts` TO `exchange_app`@`%`;
FLUSH PRIVILEGES;
