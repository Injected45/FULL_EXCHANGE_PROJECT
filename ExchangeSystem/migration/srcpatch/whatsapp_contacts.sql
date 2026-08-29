CREATE PROC [dbo].[whatsapp_contacts]
  @phone_number AS NVARCHAR(MAX)
AS
  BEGIN
    SET NOCOUNT ON;
    -- 🔹 نطبّق دالة NormalizePhone على البراميتر
    SET @phone_number = dbo.NormalizePhone(@phone_number);

    SELECT
            a.phone_number
    FROM
            -- The WhatsApp contacts table was migrated into MySQL under the name `whatsapp_contacts_shipping`
            -- (SQL Server calls it `whatsapp_contacts` in the same rhalla2026Teset DB). Without this the
            -- converted proc referenced a non-existent `rhalla2026Teset.whatsapp_contacts` and every call
            -- raised "Table 'rhalla2026teset.whatsapp_contacts' doesn't exist" (WatsapChick.vb swallowed it,
            -- so the WhatsApp-contact check silently always returned False). The proc is SQL SECURITY DEFINER,
            -- so it reads this cross-DB table under the definer's grants.
            rhalla2026Teset.[db_owner].[whatsapp_contacts_shipping] a
    WHERE
            dbo.NormalizePhone(a.phone_number) = @phone_number;
  END
